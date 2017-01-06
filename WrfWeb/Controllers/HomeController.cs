﻿using System;
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

            IndexModel model = new IndexModel();

            model.SimulationStartDate = results.SimulationStartDate.AddHours(-5.0); 
            model.PrecipSummary = new List<List<object>>();
            model.TempSummary = new List<List<object>>(); 

            List<object> header = new List<object>();
            header.Add("Date"); 
            for(int c=0;c<results.RunIds.Count;c++)
            {
                header.Add("Physics " + c.ToString()); 
            }
            model.PrecipSummary.Add(header);
            model.TempSummary.Add(header); 

            int numberOfRows = results.PrecipRecords.Count > 0 ? 
                results.PrecipRecords[0].Length : 0; 
            for(int c=0;c< numberOfRows; c++)
            {
                List<object> precipRow = new List<object>();
                precipRow.Add(results.Dates[c].AddHours(-5.0).ToString());  

                foreach(float[] runRecord in results.PrecipRecords)
                {
                    precipRow.Add(runRecord[c]); 
                }

                model.PrecipSummary.Add(precipRow);

                List<object> tempRow = new List<object>();
                tempRow.Add(results.Dates[c].AddHours(-5.0).ToString());

                foreach (float[] tempRecord in results.TempRecords)
                {
                    tempRow.Add(tempRecord[c]);
                }

                model.TempSummary.Add(tempRow);
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
