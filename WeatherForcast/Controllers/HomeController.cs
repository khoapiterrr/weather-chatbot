using DataMigration;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WeatherForcast.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}