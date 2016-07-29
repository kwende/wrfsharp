using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;
using WrfSharp.Helpers.FileSystem;
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

            iLogger.Log("Loading configuration..."); 
            WrfConfiguration config = LoadConfigurationFromAppSettings(iLogger);
            iLogger.LogLine("...done");

            iLogger.Log("Cleaning data directory..."); 
            //FileSystemHelper.CleanDataDirectory(config, iFileSystem);
            iLogger.LogLine("...done");

            iLogger.Log("Downloading GFS product page...");
            string productPageContent = DownloadHelper.DownloadString(
                config.GFSProductUrl, iDownloader);
            iLogger.LogLine("...done");

            iLogger.Log("Finding GFS product url to use...");
            string gfsProductDirectory = PageParsingHelper.FindDirectoryNameForSecondToLastGFSEntry(
                productPageContent);
            string gfsProductUrl = UrlHelper.Join(config.GFSProductUrl, gfsProductDirectory);
            iLogger.LogLine($"...done. It'll be '{gfsProductUrl}'");

            iLogger.Log("Finding GFS products on page...");
            string pageContent = DownloadHelper.DownloadString(
                gfsProductUrl, iDownloader);
            List<string> productsToDownload = 
                PageParsingHelper.FindAllGFSOneDegreePGRB2Files(pageContent);
            iLogger.LogLine($"...found {productsToDownload.Count} items.");

            iLogger.Log("Downloading the products...");
            //DownloadHelper.DownloadGFSProductsToDataDirectory(gfsProductUrl, productsToDownload,
            //    config, iDownloader, iLogger);
            iLogger.LogLine("...done");

            iLogger.LogLine("Cleaning intermediary files before run...");
            FileSystemHelper.RemoveTempFilesInWPSDirectory(config, iFileSystem, iLogger);
            FileSystemHelper.RemoveTempFilesInWRFDirectory(config, iFileSystem, iLogger); 
            iLogger.LogLine("...done");

            iLogger.LogLine("Finding first and last GFS files that were downloaded...");
            DateTime startDate, endDate;
            Wgrib2Helper.FindStartAndEndDatesOnWGribFiles(config, out startDate, out endDate,
                iProcess, iFileSystem); 
            iLogger.LogLine($"...done. First grib file is {startDate}, and last is {endDate}"); 
        }
    }
}
