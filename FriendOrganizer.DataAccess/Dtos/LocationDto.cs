using System.Collections.Generic;
using Newtonsoft.Json;

namespace FriendOrganizer.DataAccess.Dtos
{
    public class LocationDto
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

    }
    public class Result
    {
        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }
    }
    public class Geometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }
    }
    public class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }
        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
}

