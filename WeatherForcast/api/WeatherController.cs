using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherForcast.Model;
using WeatherForcast.Services;

namespace WeatherForcast.api
{
    [Route("api/")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _service;

        public WeatherController(IWeatherService service)
        {
            _service = service;
        }

        [HttpPost("GetDataFromLink")]
        public IActionResult GetDataFromLink(FomatParam link)
        {
            var data = _service.CrawlDataFromLink(link.Link);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }
    }
}