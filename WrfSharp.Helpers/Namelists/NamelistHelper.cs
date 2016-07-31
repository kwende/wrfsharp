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
        public static void UpdateStartEndDatesInWRFNamelist(WrfConfiguration config, 
            DateTime startDate, DateTime endDate, IFileSystem fileSystem)
        {
            string wrfNamelistPath = config.WPSNamelist;
            string wrfNamelistContent = fileSystem.ReadFileContent(wrfNamelistPath);

            Namelist nameList = NamelistParser.ParseFromString(wrfNamelistContent);
            nameList["share"]["start_date"].Values = new List<object>(
                new string[] { startDate.ToString("yyyy-MM-dd_HH:mm:ss") });
            nameList["share"]["end_date"].Values = new List<object>(
                new string[] { endDate.ToString("yyyy-MM-dd_HH:mm:ss") });

            string updatedContent = NamelistParser.ParseToString(nameList);
            fileSystem.WriteFileContent(wrfNamelistPath, updatedContent); 
        }
    }
}
