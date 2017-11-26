using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.DataAccess.Dtos;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Mappers;
using Location = FriendOrganizer.Model.Location;

namespace FriendOrganizer.UI.Data.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepostitory _locationRepository;
        private readonly ILocationDataAccessPoint _locationDataAccsessPoint;

        public LocationService(ILocationRepostitory locationRepostitory,
            ILocationDataAccessPoint locationDataAccessPoint)
        {
            _locationRepository = locationRepostitory;
            _locationDataAccsessPoint = locationDataAccessPoint;
        }

        public async Task<Location> ResolveLocation(string locationName)
        {
            var persistedLocation = await _locationRepository.GetByName(locationName);

            if (persistedLocation != null &&
                (persistedLocation.Longitude != 0.0 || persistedLocation.Latitude != 0.0)) return persistedLocation;

            var newLocation = await _locationDataAccsessPoint.GetLocation(locationName);

            if (persistedLocation == null)
            {
                persistedLocation = await PersistLocation(locationName, newLocation);
            }
            else
            {
                persistedLocation = UpdatePersistedLocation(locationName, persistedLocation, newLocation);
            }

            return persistedLocation;
        }

        private static Location UpdatePersistedLocation(string locationName, Location persistedLocation, LocationDto location)
        {
            persistedLocation = Mapper.LocationDtoToLocation(location);
            persistedLocation.Name = locationName;
            return persistedLocation;
        }

        private async Task<Location> PersistLocation(string locationName, LocationDto location)
        {
            var persistedLocation = Mapper.LocationDtoToLocation(location);
            persistedLocation.Name = locationName;
            _locationRepository.Add(persistedLocation);
            await _locationRepository.SaveAsync();
            return persistedLocation;
        }
    }
}
