using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;
using WrfSharp.Interfaces;

namespace WrfSharp.Helpers.Processes
{
    public static class Wgrib2Helper
    {
        private static DateTime GetDateTimeForStdOut(string stdout)
        {
            string[] lines = stdout.Split('\n');
            string dateForFirstLine = lines[0].Substring(lines[0].IndexOf('=') + 1);

            return DateTime.ParseExact(dateForFirstLine, "yyyyMMddHH", CultureInfo.InvariantCulture);
        }

        public static void FindStartAndEndDatesOnWGribFiles(
            WrfConfiguration config, out DateTime startDate, out DateTime endDate, 
            IProcessLauncher processLauncher, IFileSystem fileSystem)
        {
            string dataDirectory = config.DataDirectory; 
            string[] files = fileSystem.GetFilesInDirectory(dataDirectory);

            // go past the period and the 'f'. ex: .f003.
            string[] orderedFiles = files.OrderByDescending(n => 
                int.Parse(n.Substring(n.LastIndexOf('.') + 2))).ToArray();

            string lastFile = orderedFiles[0];
            string firstFile = orderedFiles[orderedFiles.Length - 1];

            lastFile = Path.Combine(dataDirectory, lastFile);
            firstFile = Path.Combine(dataDirectory, firstFile); 

            string wgrib2Path = config.WGRIB2FilePath;

            //352:18979996:start_ft=2016071706
            string stdOut = 
                processLauncher.LaunchProcessAndCaptureSTDOUT(wgrib2Path, $"-start_ft {firstFile}");

            startDate = GetDateTimeForStdOut(stdOut); 

            stdOut =
                processLauncher.LaunchProcessAndCaptureSTDOUT(wgrib2Path, $"-start_ft {lastFile}");

            endDate = GetDateTimeForStdOut(stdOut); 
        }
    }
}
