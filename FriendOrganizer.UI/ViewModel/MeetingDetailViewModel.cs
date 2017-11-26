using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using FriendOrganizer.DataAccess.Dtos;
using FriendOrganizer.DataAccess.Excpetions;
using FriendOrganizer.UI.Data.Services;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.Extensions;
using FriendOrganizer.UI.Mappers;
using Location = FriendOrganizer.Model.Location;

namespace FriendOrganizer.UI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private readonly IMeetingRepository _meetingRepository;
        private MeetingWrapper _meeting;
        private Friend _selectedAvailableFriend;
        private Friend _selectedAddedFriend;
        private readonly ILocationService _locationService;
        private readonly IWeatherService _weatherService;
        private ObservableCollection<Weather> _weathers;

        public MeetingWrapper Meeting
        {
            get => _meeting;
            private set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddFriendCommand { get; }

        public ICommand RemoveFriendCommand { get; }
        public ICommand LoadWeatherCommand { get; }
        public ObservableCollection<Friend> AddedFriends { get; }

        public ObservableCollection<Weather> Weathers
        {
            get => _weathers;
            set
            {
                _weathers = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Friend> AvailableFriends { get; }

        public Friend SelectedAvailableFriend
        {
            get => _selectedAvailableFriend;
            set
            {
                _selectedAvailableFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)AddFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public Friend SelectedAddedFriend
        {
            get => _selectedAddedFriend;
            set
            {
                _selectedAddedFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveFriendCommand).RaiseCanExecuteChanged();
            }
        }


        public MeetingDetailViewModel(IEventAggregator eventAggregator,
      IMessageDialogService messageDialogService,
      IMeetingRepository meetingRepository,
      ILocationService locationService,
      IWeatherService weatherService) : base(eventAggregator, messageDialogService)
        {
            _meetingRepository = meetingRepository;
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            _locationService = locationService;
            _weatherService = weatherService;
            AddedFriends = new ObservableCollection<Friend>();
            AvailableFriends = new ObservableCollection<Friend>();
            Weathers = new ObservableCollection<Weather>();
            AddFriendCommand = new DelegateCommand(OnAddFriendExecute, OnAddFriendCanExecute);
            RemoveFriendCommand = new DelegateCommand(OnRemoveFriendExecute, OnRemoveFriendCanExecute);
            LoadWeatherCommand = new DelegateCommand(OnLoadWeatherExecute, OnLoadWeatherCanExecute);
        }

        private bool OnLoadWeatherCanExecute()
        {
            return Meeting.LocationName != string.Empty;
        }

        private async void OnLoadWeatherExecute()
        {
            await SetupWeather();
        }


        public override async Task LoadAsync(int meetingId)
        {
            var meeting = meetingId > 0
              ? await _meetingRepository.GetByIdAsync(meetingId)
              : CreateNewMeeting();

            Id = meetingId;

            InitializeMeeting(meeting);
            if (meeting.LocationName != null)
            {
                await SetupWeather(false);
            }

            SetupPicklist();
        }

        protected override async void OnDeleteExecute()
        {
            var result = await MessageDialogService.ShowOkCancelDialogAsync($"Do you really want to delete the meeting {Meeting.Title}?", "Question");
            if (result != MessageDialogResult.OK) return;
            _meetingRepository.Remove(Meeting.Model);
            await _meetingRepository.SaveAsync();
            RaiseDetailDeletedEvent(Meeting.Id);
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {

            await SetupWeather(false);

            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            Id = Meeting.Id;
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }

        private async Task SetupWeather(bool notifyUserOnFail = true)
        {
            try
            {
                var location = await _locationService.ResolveLocation(Meeting.LocationName);
                await ResolveWeather(location, notifyUserOnFail);
            }
            catch (HttpRequestException)
            {
                if (notifyUserOnFail)
                {
                    await MessageDialogService.ShowInfoDialogAsync($"Failed to connect to weather service. Please check your internet connection");
                }
                Weathers.Clear();
            }

        }

        private async Task ResolveWeather(Location location, bool notifyUserOnFail)
        {

            WeatherDto weather;
            try
            {
                weather = await _weatherService.GetWeather(location);
            }
            catch (WeatherNotFoundException ex)
            {
                if (notifyUserOnFail)
                {
                    await MessageDialogService.ShowInfoDialogAsync($"No weather found for {ex.Data["locationName"]}");
                }
                Weathers = new ObservableCollection<Weather>();
                return;

            }

            Weathers = Mapper.WeatherDtoToWeatherList(weather).ToObservableCollection();


        }
        private async void SetupPicklist()
        {
            var friends = await _meetingRepository.GetAllFriendsAsync();
            var meetingFriendIds = Meeting.Model.Friends.Select(f => f.Id).ToList();
            var addedFriends = friends.Where(f => meetingFriendIds.Contains(f.Id)).OrderBy(f => f.FirstName);
            var availableFriends = friends.Except(addedFriends).OrderBy(f => f.FirstName);

            AddedFriends.Clear();
            AvailableFriends.Clear();
            foreach (var addedFriend in addedFriends)
            {
                AddedFriends.Add(addedFriend);
            }
            foreach (var availableFriend in availableFriends)
            {
                AvailableFriends.Add(availableFriend);
            }

        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _meetingRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(Meeting.Title))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Meeting.Id == 0)
            {
                Meeting.Title = "";
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title = Meeting.Title;
        }

        private void OnRemoveFriendExecute()
        {
            var friendToRemove = SelectedAddedFriend;

            Meeting.Model.Friends.Remove(friendToRemove);
            AddedFriends.Remove(friendToRemove);
            AvailableFriends.Add(friendToRemove);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveFriendCanExecute()
        {
            return SelectedAddedFriend != null;
        }

        private bool OnAddFriendCanExecute()
        {
            return SelectedAvailableFriend != null;
        }

        private void OnAddFriendExecute()
        {
            var friendToAdd = SelectedAvailableFriend;

            Meeting.Model.Friends.Add(friendToAdd);
            AddedFriends.Add(friendToAdd);
            AvailableFriends.Remove(friendToAdd);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private async void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName != nameof(FriendDetailViewModel)) return;
            await _meetingRepository.ReloadFriendAsync(args.Id);

            SetupPicklist();
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName != nameof(FriendDetailViewModel)) return;

            SetupPicklist();
        }
    }
}
