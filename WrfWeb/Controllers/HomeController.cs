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
                "connection");

            PrecipSimulationResults results = db.GetLatestPrecipSimulationResults(); 

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
