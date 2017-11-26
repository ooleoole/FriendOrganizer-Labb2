using System;
using System.Net.Http;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess.Dtos;
using Newtonsoft.Json;

namespace FriendOrganizer.DataAccess
{
    public class LocationDataAccessPoint : HttpClientBase, ILocationDataAccessPoint
    {
        public LocationDataAccessPoint() : base("https://maps.googleapis.com/maps/api/geocode/")
        {
        }
        public async Task<LocationDto> GetLocation(string locationName)
        {
            var path = "json?address=" + locationName + ",+&key=AIzaSyASzXdAHDwRe-J887exNG_h80QcKFlWJYA";

            HttpResponseMessage response;
            try
            {
                response = await HttpClient.GetAsync(path);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("GetLocation failed to conect to it's remote resource", ex);
            }

            response.EnsureSuccessStatusCode();
            var obj = JsonConvert.DeserializeObject<LocationDto>(await response.Content.ReadAsStringAsync());
            return obj;

        }
    }








}
