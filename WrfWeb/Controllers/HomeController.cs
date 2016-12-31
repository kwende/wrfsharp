using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WrfWeb.Helpers;
using WrfWeb.Models;

namespace WrfWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            MySQLDatabase db = new MySQLDatabase(
                "Server=asf;Database=adsf;Uid=asdf;Pwd=asdf;");

            PrecipSimulationResults results = db.GetLatestPrecipSimulationResults(); 

            return View(results);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
