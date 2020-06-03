using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForcast.Model
{
    public class City
    {
        public string CityName { get; set; }
        public string Province { get; set; }
        public string Area { get; set; }
        public string Population { get; set; }
    }

    public class CitiesConfig
    {
        public string Country { get; set; }
        public List<City> Cities { get; set; }
    }
}