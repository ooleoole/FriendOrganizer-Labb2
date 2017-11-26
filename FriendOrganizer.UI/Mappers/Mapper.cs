using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FriendOrganizer.DataAccess.Dtos;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Mappers
{
    public static class Mapper
    {
        public static Model.Location LocationDtoToLocation(LocationDto locationDto)
        {
            var loc = locationDto.Results.FirstOrDefault()?.Geometry.Location;
            return new Model.Location
            {
                Latitude = loc?.Lat ?? 0,
                Longitude = loc?.Lng ?? 0

            };
        }

        public static IEnumerable<Weather> WeatherDtoToWeatherList(WeatherDto weatherDto) =>
                    weatherDto.ConsolidatedWeather.Select(ConsolidatedWeatherToWeather).ToList();


        private static Weather ConsolidatedWeatherToWeather(ConsolidatedWeather consolidatedWeather)
        {
            const string noData = "No data";
            return new Weather
            {
                WeatherStateName = consolidatedWeather.WeatherStateName ?? noData,
                WindDirectionCompass = consolidatedWeather.WindDirectionCompass ?? noData,
                Created = consolidatedWeather.Created == new DateTime() ? noData : consolidatedWeather.Created.ToString(""),
                ApplicableDate = consolidatedWeather.ApplicableDate is null ? noData : consolidatedWeather.ApplicableDate,
                MinTemp = consolidatedWeather.MinTemp == null ? noData : Math.Round((double)consolidatedWeather.MinTemp, 1).ToString(CultureInfo.CurrentCulture),
                MaxTemp = consolidatedWeather.MaxTemp == null ? noData : Math.Round((double)consolidatedWeather.MaxTemp, 1).ToString(CultureInfo.CurrentCulture),
                TheTemp = consolidatedWeather.TheTemp == null ? noData : Math.Round((double)consolidatedWeather.TheTemp, 1).ToString(CultureInfo.CurrentCulture),
                WindSpeed = consolidatedWeather.WindSpeed == null ? noData : Math.Round((double)consolidatedWeather.WindSpeed, 1).ToString(CultureInfo.CurrentCulture),
                Humidity = consolidatedWeather.Humidity == 0 ? noData : consolidatedWeather.Humidity.ToString(),
                Predictability = consolidatedWeather.Predictability == 0 ? noData : consolidatedWeather.Predictability.ToString()


            };
        }
    }
}
