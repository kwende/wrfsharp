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
    public static class ProcessHelper
    {
        private static DateTime GetDateTimeForStdOut(string stdout)
        {
            string[] lines = stdout.Split('\n');
            string dateForFirstLine = lines[0].Substring(lines[0].IndexOf('=') + 1);

            return DateTime.ParseExact(dateForFirstLine, "yyyyMMddHH", CultureInfo.InvariantCulture);
        }

        public static void NclRunScript(WrfConfiguration config, IProcessLauncher processLauncher, 
            string scriptName, string pathToWrfOutFile)
        {
            string scriptPath = Path.Combine(config.ScriptsDirectory, scriptName);
            string nclExecutablePath = config.NCLPath;
            string arguments = $"{scriptPath} netcdfFile=\\\"{pathToWrfOutFile}\\\"";

            Console.WriteLine("arguments = " + arguments); 
            processLauncher.LaunchProcess(nclExecutablePath, arguments, false); 
        }

        public static void MpiRunRealExecutable(WrfConfiguration config, IProcessLauncher processLauncher)
        {
            string mpiRunPath = config.MpiRunPath;
            string realExecutablePath = config.RealExecutablePath; 
            processLauncher.LaunchProcess(mpiRunPath, $"--allow-run-as-root -np 1 {realExecutablePath}", false); 
        }

        public static void MpiRunWrfExecutable(WrfConfiguration config, IProcessLauncher processLauncher)
        {
            string mpiRunPath = config.MpiRunPath;
            string wrfExecutablePath = config.WrfExecutablePath; 
            processLauncher.LaunchProcess(mpiRunPath, $"--allow-run-as-root -np 1 {wrfExecutablePath}", false);
        }

        public static void UseWgrib2ToFindStartAndEndDatesOnWGribFiles(
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

        public static void UseGeogridToProcessTerrestrialData(WrfConfiguration config,
            IProcessLauncher processLauncher)
        {
            string geogrid = config.GeogridFilePath;
            processLauncher.LaunchProcess(geogrid, "", false);
        }

        public static void UseLinkGribToCreateSymbolicLinks(WrfConfiguration config,
            IProcessLauncher processLauncher)
        {
            string csh = config.CSHFilePath;
            string linkGrib = config.LinkGribCsh;
            string dataDirectory = config.DataDirectory;

            processLauncher.LaunchProcess(csh, $"-c \"{linkGrib} {dataDirectory}\"", true);
        }

        public static void UseUngribToUnpackageGRIBFiles(WrfConfiguration config,
            IProcessLauncher processLauncher)
        {
            string ungribPath = config.UngribFilePath;
            processLauncher.LaunchProcess(ungribPath, "", false); 
        }

        public static void UseMetgridToHorizontallyInterpolateData(WrfConfiguration config, 
            IProcessLauncher processLauncher)
        {
            string metgridPath = config.MetgridFilePath;
            processLauncher.LaunchProcess(metgridPath, "", false); 
        }

        public static void MakeVideoWithFFMPEG(WrfConfiguration config, 
            IProcessLauncher iProcess, string script)
        {
            string scriptFileName = script.Substring(script.LastIndexOf('/') + 1);
            scriptFileName = scriptFileName.Substring(0, scriptFileName.IndexOf('.'));
            scriptFileName = scriptFileName.Replace("wrf_", "plt_"); 

            string wrfDirectory = config.WRFDirectory;

            string ffmpegPath = config.FFMPEGPath;
            iProcess.LaunchProcess(ffmpegPath, 
                $"-r 4 -i {scriptFileName}.000%03d.png -c:v libx264 -pix_fmt yuv420p {scriptFileName}.mp4", 
                false); 
        }
    }
}
