using Mono.Unix;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;
using WrfSharp.Helpers.FileSystem;
using WrfSharp.Helpers.Namelists;
using WrfSharp.Helpers.Processes;
using WrfSharp.Helpers.Web;
using WrfSharp.Interfaces;
using WrfSharp.Runner.Implementations;

namespace WrfSharp.Runner
{
    class Program
    {
        static WrfConfiguration LoadConfigurationFromAppSettings(ILogger logger)
        {
            WrfConfiguration config = new WrfConfiguration();

            Type configType = typeof(WrfConfiguration);
            string[] keys = ConfigurationManager.AppSettings.AllKeys;

            foreach(PropertyInfo prop in configType.GetProperties())
            {
                if(keys.Any(n=>n==prop.Name))
                {
                    string key = prop.Name; 
                    string value = ConfigurationManager.AppSettings[key];
                    logger.LogLine("\t" + key + " : " + value); 
                    prop.SetValue(config, value); 
                }
            }

            return config; 
        }

        static void Main(string[] args)
        {
            IFileSystem iFileSystem = new FileSystem();
            IDownloader iDownloader = new Downloader();
            ILogger iLogger = new Logger(null);
            IProcessLauncher iProcess = new ProcessLauncher();

            iLogger.LogLine("Loading configuration...");
            WrfConfiguration config = LoadConfigurationFromAppSettings(iLogger);
            iLogger.LogLine("...done");

            iLogger.Log("Cleaning data directory...");
            //FileSystemHelper.CleanDataDirectory(config, iFileSystem);
            iLogger.LogLine("...done");

            iLogger.LogLine("Downloading GFS product page...");
            string productPageContent = DownloadHelper.DownloadString(
                config.GFSProductUrl, iDownloader);
            iLogger.LogLine("...done");

            iLogger.LogLine("Finding GFS product url to use...");
            string gfsProductDirectory = PageParsingHelper.FindDirectoryNameForSecondToLastGFSEntry(
                productPageContent);
            string gfsProductUrl = UrlHelper.Join(config.GFSProductUrl, gfsProductDirectory);
            iLogger.LogLine($"...done. It'll be '{gfsProductUrl}'");

            iLogger.LogLine("Finding GFS products on page...");
            string pageContent = DownloadHelper.DownloadString(
                gfsProductUrl, iDownloader);
            List<string> productsToDownload =
                PageParsingHelper.FindAllGFSOneDegreePGRB2Files(pageContent);
            iLogger.LogLine($"...found {productsToDownload.Count} items.");

            iLogger.LogLine("Downloading the products...");
            //DownloadHelper.DownloadGFSProductsToDataDirectory(gfsProductUrl, productsToDownload,
            //    config, iDownloader, iLogger);
            iLogger.LogLine("...done");

            iLogger.LogLine("Cleaning intermediary files before run...");
            FileSystemHelper.RemoveTempFilesInWPSDirectory(config, iFileSystem, iLogger);
            FileSystemHelper.RemoveTempFilesInWRFDirectory(config, iFileSystem, iLogger);
            iLogger.LogLine("...done");

            iLogger.LogLine("Finding first and last GFS files that were downloaded...");
            DateTime startDate, endDate;
            ProcessHelper.UseWgrib2ToFindStartAndEndDatesOnWGribFiles(config, out startDate, out endDate,
                iProcess, iFileSystem);
            iLogger.LogLine($"...done. First grib file is {startDate}, and last is {endDate}");

            iLogger.LogLine("Updating the start/end dates in the WPS namelist.config file.");
            NamelistHelper.UpdateDatesInWPSNamelist(config,
                startDate, endDate, iFileSystem);
            iLogger.LogLine("...done");

            iLogger.LogLine("Updating the start/end dates in the WRF namelist.config file.");
            NamelistHelper.UpdateDatesInWRFNamelist(config,
                startDate, endDate, iFileSystem);
            iLogger.LogLine("...done");

            iLogger.LogLine("Setting current working directory to WPS directory...");
            FileSystemHelper.SetCurrentDirectoryToWPSDirectory(config, iFileSystem);
            iLogger.LogLine("...done");

            iLogger.LogLine("Launching geogrid.exe");
            ProcessHelper.UseGeogridToProcessTerrestrialData(config, iProcess);
            iLogger.LogLine("...done");

            iLogger.LogLine("Setting up symlinks through CSH script...");
            ProcessHelper.UseLinkGribToCreateSymbolicLinks(config, iProcess);
            iLogger.LogLine("...done");

            iLogger.LogLine("Using ungrib to unpackage GRIB files....");
            ProcessHelper.UseUngribToUnpackageGRIBFiles(config, iProcess);
            iLogger.LogLine("...done");

            iLogger.LogLine("Use metrgrid to horizontally interpolate data...");
            ProcessHelper.UseMetgridToHorizontallyInterpolateData(config, iProcess);
            iLogger.LogLine("...done");

            iLogger.LogLine("Creating symlinks in Real directory...");
            FileSystemHelper.CreateMetEmSymlinksInRealDirectory(config, iFileSystem);
            iLogger.LogLine("...done");
        }
    }
}
