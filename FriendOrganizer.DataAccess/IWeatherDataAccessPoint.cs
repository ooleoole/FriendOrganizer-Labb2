using System.Threading.Tasks;
using FriendOrganizer.DataAccess.Dtos;

namespace FriendOrganizer.DataAccess
{
    public interface IWeatherDataAccessPoint
    {
        Task<int> GetWeatherId(Model.Location location);
        Task<WeatherDto> GetWeather(int woeid);
    }
}