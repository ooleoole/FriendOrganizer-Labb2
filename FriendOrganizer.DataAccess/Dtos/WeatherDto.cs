using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FriendOrganizer.DataAccess.Dtos
{

    public class WeatherDto
    {
        [JsonProperty("consolidated_weather")]
        public List<ConsolidatedWeather> ConsolidatedWeather { get; set; }
        [JsonProperty("Time")]
        public DateTime Time { get; set; }

    }
    public class ConsolidatedWeather
    {
        [JsonProperty("weather_state_name")]
        public string WeatherStateName { get; set; }
        [JsonProperty("weather_state_abbr")]
        public string WeatherStateAbbr { get; set; }
        [JsonProperty("wind_direction_compass")]
        public string WindDirectionCompass { get; set; }
        [JsonProperty("created")]
        public DateTime Created { get; set; }
        [JsonProperty("applicable_date")]
        public string ApplicableDate { get; set; }
        [JsonProperty("min_temp")]
        public double? MinTemp { get; set; }
        [JsonProperty("max_temp")]
        public double? MaxTemp { get; set; }
        [JsonProperty("the_temp")]
        public double? TheTemp { get; set; }
        [JsonProperty("wind_speed")]
        public double? WindSpeed { get; set; }
        [JsonProperty("humidity")]
        public int Humidity { get; set; }
        [JsonProperty("visibility")]
        public double? Visibility { get; set; }
        [JsonProperty("predictability")]
        public int Predictability { get; set; }
    }
    public class WeatherId
    {

        [JsonProperty("woeid")]
        public int Woeid { get; set; }

    }

}
