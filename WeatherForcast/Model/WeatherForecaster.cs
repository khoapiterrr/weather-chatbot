using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForcast.Model
{
    public class WeatherForecaster
    {
        public string Address { get; set; }
        public float latitude { get; set; }
        public float Longitude { get; set; }
        public string TemperatureC { get; set; }
        public string TemperatureF { get; set; }
        public string Image { get; set; }

        ///// <summary>
        ///// ĐỘ ẩm
        ///// </summary>
        //public string Humidity { get; set; }

        ///// <summary>
        ///// Gió
        ///// </summary>
        //public string WindSpeed { get; set; }

        //public string Barometer { get; set; }
        //public string Dewpoint { get; set; }
        //public string Visibility { get; set; }
        //public string WindChill { get; set; }
        //public string LastUpdate { get; set; }
        public Dictionary<string, string> WeatherDetail { get; set; }

        public List<WeatherDetail> Weathers { get; set; }

        public WeatherForecaster()
        {
            WeatherDetail = new Dictionary<string, string>();
            Weathers = new List<WeatherDetail>();
        }
    }
}