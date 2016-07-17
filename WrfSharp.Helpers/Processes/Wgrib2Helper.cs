using System;
using System.Collections.Generic;
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
        public static void FindStartAndEndDatesOnWGribFiles(
            WrfConfiguration config, out DateTime startDate, out DateTime endDate, 
            IProcessLauncher processLauncher, IFileSystem fileSystem)
        {
            string dataDirectory = config.DataDirectory; 
            string[] files = fileSystem.GetFilesInDirectory(dataDirectory);

            // go past the period and the 'f'. ex: .f003. 
            int[] suffixes = files.Select(n => int.Parse(n.Substring(
                n.LastIndexOf('.') + 2))).OrderByDescending(n => n).ToArray();

            string latestFileSuffix = suffixes[0].ToString();
            string firstFileSuffix = suffixes[suffixes.Length - 1].ToString();

            string latestFile = Path.Combine(dataDirectory, 
                files.Where(n => n.EndsWith(latestFileSuffix)).First()); 
            string firstFile = Path.Combine(dataDirectory, 
                files.Where(n => n.EndsWith(firstFileSuffix)).First()); 

            string wgrib2Path = config.WGRIB2FilePath;

            string stdOut = 
                processLauncher.LaunchProcessAndCaptureSTDOUT(wgrib2Path, $"-start_ft {latestFile}"); 

        }
    }
}
