using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess.Dtos;
using FriendOrganizer.DataAccess.Excpetions;
using Newtonsoft.Json;

namespace FriendOrganizer.DataAccess
{
    public class WeatherDataAccessPoint : HttpClientBase, IWeatherDataAccessPoint
    {
        public WeatherDataAccessPoint() : base("https://www.metaweather.com/api/")
        {
        }

        public async Task<int> GetWeatherId(Model.Location location)
        {
            var locationIsUnresolved = location.Longitude == 0 && location.Latitude == 0;

            if (locationIsUnresolved) return 0;

            var path =
                $"location/search/?lattlong={location.Latitude.ToString(NumberFormatInfo.InvariantInfo)},{location.Longitude.ToString(NumberFormatInfo.InvariantInfo)}";

            HttpResponseMessage response;
            try
            {
                response = await HttpClient.GetAsync(path);
               
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("GetWeatherId failed to connect to it's remote resource", ex);
            }
            response.EnsureSuccessStatusCode();
            var obj = JsonConvert.DeserializeObject<List<WeatherId>>(await response.Content.ReadAsStringAsync());
            return obj.First().Woeid;



        }



        public async Task<WeatherDto> GetWeather(int woeid)
        {
            var path = $"location/location/{woeid}/";

            HttpResponseMessage response;
            try
            {
                 response = await HttpClient.GetAsync(path);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("GetWeather failed to connect to it's remote resource", ex);
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var ex = new WeatherNotFoundException();
                ex.Data.Add("woeid", woeid);
                throw ex;
            }
            response.EnsureSuccessStatusCode();

            var obj = JsonConvert.DeserializeObject<WeatherDto>(await response.Content.ReadAsStringAsync());
            return obj;
        }

    }
}


