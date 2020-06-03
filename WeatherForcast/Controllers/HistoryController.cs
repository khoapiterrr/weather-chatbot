using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMigration;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForcast.Controllers
{
    public class HistoryController : Controller
    {
        private readonly DataMigrationContext _context;

        public HistoryController(DataMigrationContext context)
        {
            _context = context;
        }

        public IActionResult Crawl()
        {
            return View(_context.ViewWeatherHistoryCrawl.ToList());
        }

        public IActionResult FbApi()
        {
            return View(_context.ViewWeatherHistoryApi.ToList());
        }
    }
}