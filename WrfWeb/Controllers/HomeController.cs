using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WrfWeb.Helpers;
using WrfWeb.Helpers.Database;
using WrfWeb.Models;

namespace WrfWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            MySQLDatabase db = new MySQLDatabase(
                "Server=www.ben-rush.net;Database=asdf;Uid=asdf;Pwd=asdf;");

            PrecipSimulationResults results = db.GetLatestPrecipSimulationResults();

            IndexModel model = new IndexModel();

            model.PrecipSummary = new List<List<object>>();

            List<object> header = new List<object>();
            header.Add("Date"); 
            for(int c=0;c<results.RunIds.Count;c++)
            {
                header.Add(c.ToString()); 
            }
            model.PrecipSummary.Add(header);

            int numberOfRows = results.RunRecords[0].Length; 
            for(int c=0;c< numberOfRows; c++)
            {
                List<object> row = new List<object>();
                row.Add(results.Dates[c]); 

                foreach(float[] runRecord in results.RunRecords)
                {
                    row.Add(runRecord[c]); 
                }

                model.PrecipSummary.Add(row); 
            }

            //model.PrecipSummary.Add(new List<object>)

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
