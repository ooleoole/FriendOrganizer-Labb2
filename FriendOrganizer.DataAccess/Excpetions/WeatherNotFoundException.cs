using System;

namespace FriendOrganizer.DataAccess.Excpetions
{
    public class WeatherNotFoundException : Exception
    {
        public WeatherNotFoundException() { }
        public WeatherNotFoundException(string message) : base(message) { }
        public WeatherNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
