using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForcast.Model
{
    public class WeatherDetail
    {
        public string Image { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public string ShortDesc { get; set; }
        public string Detail { get; set; }
        public string Temp { get; set; }
    }
}