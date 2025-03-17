using System.Collections.Generic;

namespace DefaultNamespace
{
    
    [System.Serializable]
    public class WeatherResponse
    {
        public WeatherProperties properties;
    }

    [System.Serializable]
    public class WeatherProperties
    {
        public WeatherPeriod[] periods;
    }

    [System.Serializable]
    public class WeatherPeriod
    {
        public string name;
        public int temperature;
    }
}