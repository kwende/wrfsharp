using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;
using WrfSharp.Interfaces;

namespace WrfSharp.Helpers.Namelists
{
    public static class NamelistHelper
    {
        public static void UpdateDatesInWPSNamelist(WrfConfiguration config, 
            DateTime startDate, DateTime endDate, IFileSystem fileSystem)
        {
            string wpsNamelistPath = config.WPSNamelist;
            string wpsNamelistContent = fileSystem.ReadFileContent(wpsNamelistPath);

            Namelist nameList = NamelistParser.ParseFromString(wpsNamelistContent);
            nameList["share"]["start_date"].Values = new List<object>(
                new string[] { startDate.ToString("yyyy-MM-dd_HH:mm:ss") });
            nameList["share"]["end_date"].Values = new List<object>(
                new string[] { endDate.ToString("yyyy-MM-dd_HH:mm:ss") });

            string updatedContent = NamelistParser.ParseToString(nameList);
            fileSystem.WriteFileContent(wpsNamelistPath, updatedContent); 
        }

        public static void UpdateDatesInWRFNamelist(WrfConfiguration config, 
            DateTime startDate, DateTime endDate, IFileSystem fileSystem)
        {
            string wrfNamelistPath = config.WRFNamelist;
            string wrfNamelistContent = fileSystem.ReadFileContent(wrfNamelistPath);

            Namelist nameList = NamelistParser.ParseFromString(wrfNamelistContent);

            nameList["time_control"]["start_year"].Values.Clear(); 
            nameList["time_control"]["start_month"].Values.Clear();
            nameList["time_control"]["start_day"].Values.Clear();
            nameList["time_control"]["start_hour"].Values.Clear();
            nameList["time_control"]["end_year"].Values.Clear();
            nameList["time_control"]["end_month"].Values.Clear();
            nameList["time_control"]["end_day"].Values.Clear();
            nameList["time_control"]["end_hour"].Values.Clear();

            nameList["time_control"]["start_year"].Values.Add(startDate.Year);
            nameList["time_control"]["start_month"].Values.Add(startDate.Month);
            nameList["time_control"]["start_day"].Values.Add(startDate.Day);
            nameList["time_control"]["start_hour"].Values.Add(startDate.Hour);

            nameList["time_control"]["end_year"].Values.Add(endDate.Year);
            nameList["time_control"]["end_month"].Values.Add(endDate.Month);
            nameList["time_control"]["end_day"].Values.Add(endDate.Day);
            nameList["time_control"]["end_hour"].Values.Add(endDate.Hour);

            string updatedContent = NamelistParser.ParseToString(nameList);
            fileSystem.WriteFileContent(wrfNamelistPath, updatedContent); 
        }
    }
}
