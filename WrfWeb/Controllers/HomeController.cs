using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WrfWeb.Helpers;
using WrfWeb.Helpers.Database;
using WrfWeb.Models;
using WrfWeb.Helpers.Configuration;

namespace WrfWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string connectionString = ConfigurationReader.ReadDatabaseConnectionString(); 

            MySQLDatabase db = new MySQLDatabase(connectionString);

            SimulationResults results = db.GetLatestSimulationResults();
            RunState runState = db.GetRunState(); 

            IndexModel model = new IndexModel();

            model.SimulationStartDate = results.SimulationStartDate.AddHours(-5.0); 
            model.PrecipSummary = new List<List<object>>();
            model.TempSummary = new List<List<object>>();
            model.SnowDepths = new List<List<object>>();
            model.WindSpeeds = new List<List<object>>();
            model.SurfacePressures = new List<List<object>>();
            model.RunIds = results.RunIds;
            model.CurrentRunState = runState.StateText;
            model.LastCheckinDate = runState.LastCheckinDate; 

            List<object> header = new List<object>();
            header.Add("Date"); 
            for(int c=0;c<results.RunIds.Count;c++)
            {
                header.Add("Physics " + c.ToString()); 
            }
            model.PrecipSummary.Add(header);
            model.TempSummary.Add(header);
            model.SnowDepths.Add(header);
            model.WindSpeeds.Add(header);
            model.SurfacePressures.Add(header); 

            int numberOfRows = results.PrecipRecords.Count > 0 ? 
                results.PrecipRecords[0].Length : 0; 
            for(int c=0;c< numberOfRows; c++)
            {
                List<object> precipRows = new List<object>();
                precipRows.Add(results.Dates[c].AddHours(-5.0).ToString());  

                foreach(float[] row in results.PrecipRecords)
                {
                    precipRows.Add(row[c]); 
                }

                model.PrecipSummary.Add(precipRows);

                List<object> tempRows = new List<object>();
                tempRows.Add(results.Dates[c].AddHours(-5.0).ToString());

                foreach (float[] row in results.TempRecords)
                {
                    tempRows.Add(row[c]);
                }

                model.TempSummary.Add(tempRows);

                List<object> snowRows = new List<object>();
                snowRows.Add(results.Dates[c].AddHours(-5.0).ToString());

                foreach (float[] row in results.SnowDepths)
                {
                    snowRows.Add(row[c] * 39.3701f);
                }

                model.SnowDepths.Add(snowRows);

                List<object> windRows = new List<object>();
                windRows.Add(results.Dates[c].AddHours(-5.0).ToString()); 

                foreach(float[] row in results.WindSpeeds)
                {
                    windRows.Add(row[c]); 
                }

                model.WindSpeeds.Add(windRows);

                List<object> pressureRows = new List<object>();
                pressureRows.Add(results.Dates[c].AddHours(-5.0).ToString());

                foreach (float[] row in results.SurfacePressure)
                {
                    pressureRows.Add(row[c]);
                }

                model.SurfacePressures.Add(pressureRows);
            }

            //model.PrecipSummary.Add(new List<object>)

            return View(model);
        }

        public IActionResult RunDetails(string runId)
        {
            return View();
        }
    }
}
