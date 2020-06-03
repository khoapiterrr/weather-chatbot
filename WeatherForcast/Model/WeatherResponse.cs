using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForcast.Model
{
    public class WeatherResponseApi
    {
        public Coord coord { get; set; }
        public List<weath> weather { get; set; }
        public string Base { get; set; }

        public string timezone { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string cod { get; set; }
        public string dt { get; set; }
        public Sys sys { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
    }

    public class Main
    {
        public float temp { get; set; }
        public float feels_like { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
        public float pressure { get; set; }
        public float humidity { get; set; }
    }

    public class Clouds
    {
        public float all { get; set; }
    }

    public class Wind
    {
        public float speed { get; set; }
        public float deg { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public long id { get; set; }
        public float message { get; set; }
        public string country { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }
    }

    public class weath
    {
        public long id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Coord
    {
        public float lon { get; set; }
        public float lat { get; set; }
    }
}