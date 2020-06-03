using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMigration;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForcast.Controllers
{
    public class TestDbController : Controller
    {
        private readonly DataMigrationContext _context;

        public TestDbController(DataMigrationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.MesBotEntities.ToList());
        }
    }
}