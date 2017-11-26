namespace FriendOrganizer.Model
{
    public class Weather
    {
        public string WeatherStateName { get; set; }
        public string WindDirectionCompass { get; set; }
        public string Created { get; set; }
        public string ApplicableDate { get; set; }
        public string MinTemp { get; set; }
        public string MaxTemp { get; set; }
        public string TheTemp { get; set; }
        public string WindSpeed { get; set; }
        public string Humidity { get; set; }
        public string Predictability { get; set; }
    }
}
