using System.Threading.Tasks;
using FriendOrganizer.DataAccess.Dtos;

namespace FriendOrganizer.UI.Data.Services
{
    public interface IWeatherService
    {
        Task<WeatherDto> GetWeather(Model.Location location);
    }
}