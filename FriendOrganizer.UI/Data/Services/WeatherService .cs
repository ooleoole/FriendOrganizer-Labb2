using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.DataAccess.Dtos;
using FriendOrganizer.DataAccess.Excpetions;

namespace FriendOrganizer.UI.Data.Services
{

    public class WeatherService : IWeatherService
    {
        private readonly IWeatherDataAccessPoint _weatherDataAccessPoint;

        public WeatherService(IWeatherDataAccessPoint weatherDataAccessPoint)
        {
            _weatherDataAccessPoint = weatherDataAccessPoint;
        }

        public async Task<WeatherDto> GetWeather(Model.Location location)
        {
            var woeid = await _weatherDataAccessPoint.GetWeatherId(location);
            try
            {
                return await _weatherDataAccessPoint.GetWeather(woeid);
            }
            catch (WeatherNotFoundException ex)
            {
                ex.Data.Add("locationName", location.Name);
                throw;
            }

        }
    }
}
