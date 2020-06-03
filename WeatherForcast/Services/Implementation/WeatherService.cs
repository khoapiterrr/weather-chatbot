using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherForcast.Model;
using Entity;

namespace WeatherForcast.Services.Implementation
{
    public class WeatherService : IWeatherService
    {
        private readonly string _weatherApiKey;

        public WeatherService()
        {
            _weatherApiKey = "23916b46baafe10baf75a899f2197cec";
        }

        public WeatherForecaster CrawlDataFromLink(string link)
        {
            link = string.IsNullOrEmpty(link) ? "https://forecast.weather.gov/MapClick.php?lat=37.7772&lon=-105.5168#.XrUr2mgzaHv" : link;
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring!"), Cron.Minutely);

            var urlbase = "https://forecast.weather.gov/";
            //Kiêm tra đường dẫn
            if (!link.Contains(urlbase))
            {
                return null;
            }
            var objReturn = new WeatherForecaster();
            var htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
            };

            try
            {
                var document = htmlWeb.Load(link);
                var weatherDetail = document.DocumentNode.Descendants("div").FirstOrDefault(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "current_conditions_detail").Descendants("tr").ToList();
                var weatherDetail1 = document.DocumentNode.SelectSingleNode("//div[@id=current_conditions_detail]");
                foreach (var item in weatherDetail)
                {
                    var td = item.ChildNodes.Where(x => x.Name == "td").ToList();
                    objReturn.WeatherDetail.Add(td[0].InnerText.ToString(), td[1].InnerText.ToString());
                }
                //< !--Graphic and temperatures -->
                var temperatures = document.DocumentNode.Descendants("div").FirstOrDefault(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "current_conditions-summary").ChildNodes.ToList();
                try
                {
                    objReturn.Image = urlbase + temperatures.FirstOrDefault(node => node.Attributes.Contains("src")).Attributes["src"].Value;
                }
                catch (Exception)
                {
                }

                objReturn.TemperatureF = temperatures.FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "myforecast-current-lrg").InnerText;
                objReturn.TemperatureC = temperatures.FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "myforecast-current-sm").InnerText;
                //dia chi
                var addr = document.DocumentNode.Descendants("div").FirstOrDefault(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "getfcst-head");

                //weather
                var listForecaseShort = document.DocumentNode.Descendants("ul").FirstOrDefault(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "seven-day-forecast-list").ChildNodes.ToList();
                foreach (var item in listForecaseShort)
                {
                    var data = item.ChildNodes.FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "tombstone-container");
                    var obj = new WeatherDetail();
                    obj.Time = data.ChildNodes.FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "period-name").InnerText;
                    obj.ShortDesc = data.ChildNodes.FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "short-desc").InnerText;
                    obj.Temp = data.ChildNodes.FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "short-desc").NextSibling.InnerText;
                    obj.Image = urlbase + data.ChildNodes.Descendants("img").FirstOrDefault(node => node.Attributes.Contains("src")).Attributes["src"].Value;
                    obj.Description = data.ChildNodes.Descendants("img").FirstOrDefault(node => node.Attributes.Contains("alt")).Attributes["alt"].Value;
                    objReturn.Weathers.Add(obj);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return objReturn;
        }

        public async Task<WeatherResponseApi> GetWeatherFromCity(string cityName)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={_weatherApiKey}&units=metric&lang=vi");
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    WeatherResponseApi data = JsonConvert.DeserializeObject<WeatherResponseApi>(apiResponse);
                    return data;
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}