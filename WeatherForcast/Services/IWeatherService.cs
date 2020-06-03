using System.Threading.Tasks;
using WeatherForcast.Model;

namespace WeatherForcast.Services
{
    public interface IWeatherService
    {
        WeatherForecaster CrawlDataFromLink(string link);

        Task<WeatherResponseApi> GetWeatherFromCity(string cityName);
    }
}