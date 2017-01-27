using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using WrfSharp.DataStructures;
using WrfSharp.Db;
using WrfSharp.Helpers.Configuration;
using WrfSharp.Helpers.Database;
using WrfSharp.Helpers.FileSystem;
using WrfSharp.Helpers.Namelists;
using WrfSharp.Helpers.Process;
using WrfSharp.Helpers.Processes;
using WrfSharp.Helpers.Web;
using WrfSharp.Interfaces;
using WrfSharp.NetCDF;
using WrfSharp.Runner.Implementations;

namespace WrfSharp.Runner
{
    class Program
    {
        static Timer _timer; 

        static WrfConfiguration LoadConfigurationFromAppSettings(ILogger logger)
        {
            WrfConfiguration config = new WrfConfiguration();

            Type configType = typeof(WrfConfiguration);
            string[] keys = ConfigurationManager.AppSettings.AllKeys;

            foreach (PropertyInfo prop in configType.GetProperties())
            {
                if (keys.Any(n => n == prop.Name))
                {
                    string key = prop.Name;
                    string value = ConfigurationManager.AppSettings[key];
                    logger.LogLine("\t" + key + " : " + value);

                    if(prop.PropertyType == typeof(int))
                    {
                        prop.SetValue(config, int.Parse(value));
                    }
                    else if(prop.PropertyType == typeof(bool))
                    {
                        prop.SetValue(config, bool.Parse(value));
                    }
                    else
                    {
                        prop.SetValue(config, value);
                    }
                }
            }

            return config;
        }

        static void RecurseAdd(List<PhysicsConfigurationProcessed> list, PhysicsConfiguration sourceConfig, 
            List<PropertyInfo> multipleProps, int level, Type physicsConfigurationType)
        {
            if(level < multipleProps.Count)
            {
                PropertyInfo readProp = multipleProps[level];
                PropertyInfo writeProp = physicsConfigurationType.GetProperty(readProp.Name); 

                string[] vals = (readProp.GetValue(sourceConfig) as string).Split(',').Select(n => n.Trim()).ToArray();
                int numberSame = list.Count / vals.Length; 

                for(int valueIndex=0; valueIndex < vals.Length; valueIndex++)
                {
                    int startIndex = valueIndex * numberSame;
                    int value = int.Parse(vals[valueIndex]);

                    List<PhysicsConfigurationProcessed> toSendDown = 
                        new List<PhysicsConfigurationProcessed>(numberSame); 
                    for(int i=startIndex;i<numberSame+startIndex;i++)
                    {
                        PhysicsConfigurationProcessed settingNow = list[i];
                        writeProp.SetValue(settingNow, value);
                        toSendDown.Add(settingNow); 
                    }

                    RecurseAdd(toSendDown, sourceConfig, multipleProps, 
                        level + 1, physicsConfigurationType); 
                }
            }
        }

        static List<PhysicsConfigurationProcessed> LoadPhysicsConfigurationsFromConfiguration()
        {
            Type physicsConfigType = typeof(PhysicsConfiguration);
            Type physicsConfigProcessedType = typeof(PhysicsConfigurationProcessed);

            Configuration obj = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            PhysicsConfigurationSection section = (PhysicsConfigurationSection)obj.GetSection("physics");

            List<PhysicsConfigurationProcessed> ret = new List<PhysicsConfigurationProcessed>();

            foreach (PhysicsConfiguration configuration in section.PhysicsConfigurations)
            {
                int total = 1;
                List<PropertyInfo> multipleProps = new List<PropertyInfo>();
                List<PropertyInfo> singleProps = new List<PropertyInfo>(); 
                foreach (PropertyInfo prop in physicsConfigType.GetProperties())
                {
                    string val = prop.GetValue(configuration) as string;
                    if (val != null)
                    {
                        int numberOfCommas = (val as string).Count(n => n == ',');
                        if(numberOfCommas>0)
                        {
                            multipleProps.Add(prop); 
                        }
                        else
                        {
                            singleProps.Add(prop); 
                        }
                        total *= (numberOfCommas + 1);
                    }
                }

                Type physicsConfigurationProcessedType = typeof(PhysicsConfigurationProcessed); 

                List<PhysicsConfigurationProcessed> toAdd = new List<PhysicsConfigurationProcessed>(total);
                for (int c = 0; c < total; c++)
                {
                    PhysicsConfigurationProcessed config = new PhysicsConfigurationProcessed();
                    foreach(PropertyInfo singleProp in singleProps)
                    {
                        if(singleProp.Name.ToLower() != "name")
                        {
                            string value = singleProp.GetValue(configuration) as string;
                            if (value != null)
                            {
                                physicsConfigProcessedType.GetProperty(singleProp.Name).SetValue(config, int.Parse(value));
                            }
                        }
                    }

                    toAdd.Add(config);
                }

                RecurseAdd(toAdd, configuration, multipleProps, 0, physicsConfigurationProcessedType);

                ret.AddRange(toAdd); 
            }

            return ret;
        }

        static void PrepStage(IFileSystem iFileSystem, INetwork iDownloader,
            ILogger iLogger, IProcessLauncher iProcess, WrfConfiguration config)
        {
            iLogger.Log("Cleaning data directory...");
            FileSystemHelper.CleanDataDirectory(config, iFileSystem);
            iLogger.LogLine("...done");

            iLogger.LogLine("Downloading GFS product page...");
            string productPageContent = DownloadHelper.DownloadString(
                config.GFSProductUrl, iDownloader);
            iLogger.LogLine("...done");

            iLogger.Log("Finding GFS products to use...");

            string gfsProductDirectory = PageParsingHelper.FindDirectoryNameForLatestGFSEntry(
                productPageContent);
            string gfsProductUrl = UrlHelper.Join(config.GFSProductUrl, gfsProductDirectory);
            string pageContent = DownloadHelper.DownloadString(
                gfsProductUrl, iDownloader);
            List<string> productsToDownload = new List<string>();

            for(;;)
            {
                productsToDownload = PageParsingHelper.FindAllGFSOneDegreePGRB2Files(pageContent);
                if(productsToDownload.Count != 93 && config.ForceLatestGFSData)
                {
                    iLogger.Log($"...forcing latest GFS data, not enough yet ({productsToDownload.Count})...");
                    Thread.Sleep(1000 * 60);
                    pageContent = DownloadHelper.DownloadString(
                        gfsProductUrl, iDownloader);
                }
                else if(productsToDownload.Count != 93 && !config.ForceLatestGFSData)
                {
                    iLogger.Log($"...falling back to previous run, incorrect asset count of...{productsToDownload.Count}");
                    gfsProductDirectory = PageParsingHelper.FindDirectoryNameForSecondToLastGFSEntry(
                        productPageContent);
                    gfsProductUrl = UrlHelper.Join(config.GFSProductUrl, gfsProductDirectory);
                    pageContent = DownloadHelper.DownloadString(
                        gfsProductUrl, iDownloader);
                    productsToDownload =
                        PageParsingHelper.FindAllGFSOneDegreePGRB2Files(pageContent);
                    break; 
                }
                else
                {
                    iLogger.Log($"...the latest is ready!...");
                    break; 
                }
            }
            iLogger.LogLine($"...done. Found {productsToDownload.Count} items at {gfsProductUrl}.");

            iLogger.LogLine("Downloading the products...");
            DownloadHelper.DownloadGFSProductsToDataDirectory(gfsProductUrl, productsToDownload,
                config, iDownloader, iLogger);
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

            iLogger.LogLine($"Updating the start/end dates in the {config.WPSNamelist} file.");
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

        static void ComputeStage(IFileSystem iFileSystem, ILogger iLogger,
            IProcessLauncher iProcess, IEnvironment iEnvironment, IDatabase iDatabase,
            WrfConfiguration config, PhysicsConfigurationProcessed physicsConfig)
        {
            DateTime runStartTime = DateTime.Now;
            string runId = Guid.NewGuid().ToString().Replace("-", "");

            iLogger.LogLine("Updating physics parameters...");
            NamelistHelper.UpdatePhysicsParameters(config, physicsConfig, iFileSystem);
            iLogger.LogLine("...done");

            iLogger.LogLine("Changing directory to real directory...");
            FileSystemHelper.SetCurrentDirectoryToWRFDirectory(config, iFileSystem);
            iLogger.LogLine("...done");

            iLogger.LogLine("Launching real.exe...");
            ProcessHelper.MpiRunRealExecutable(config, iProcess);
            iLogger.LogLine("...done");

            iLogger.LogLine($"Launching wrf.exe against mp_physics={physicsConfig.MpPhysics}");
            ProcessHelper.MpiRunWrfExecutable(config, iProcess);
            iLogger.LogLine("...done");

            iLogger.LogLine("Locating the WrfOut file...");
            string wrfOutFile =
                FileSystemHelper.RetrievePathToWrfOutFile(config, iFileSystem);
            iLogger.LogLine($"...found at {wrfOutFile}.");

            iLogger.LogLine("Retrieving scripts to run...");
            string[] scripts = FileSystemHelper.RetrieveNclScriptsToRun(config, iFileSystem);
            iLogger.LogLine($"...found {scripts.Length} scripts: {string.Join(",", scripts)}");

            //string wrfOutFile = @"C:\Users\Ben\Desktop\wrfout"; 

            DateTime runEndTime = DateTime.Now;
            INetCDFReader netCdfReader = new NetCDFReader(wrfOutFile);

            DatabaseHelper.CreateRunRecord(netCdfReader, iDatabase,
                runStartTime, runEndTime, runId);

            LatLongRect latLongRect = ConfigurationHelper.ParseLatLongRect(config); 

            DatabaseHelper.RecordVariables(netCdfReader, iDatabase,
                runId, latLongRect.UpperLeftLong, latLongRect.LowerRightLong, 
                latLongRect.LowerRightLat, latLongRect.UpperLeftLat);

            foreach (string script in scripts)
            {
                iLogger.LogLine($"Launching NCL against {script}...");
                ProcessHelper.NclRunScript(config, iProcess, script, wrfOutFile);
                iLogger.LogLine("...done");

                ProcessHelper.MakeVideoWithFFMPEG(config, iProcess, script, runId);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                IFileSystem iFileSystem = new FileSystem();
                INetwork iDownloader = new Downloader();
                ILogger iLogger = new Logger("/home/brush/wrf/log.txt");
                IProcessLauncher iProcess = new ProcessLauncher();
                IEnvironment iEnvironment = new WrfSharp.Runner.Implementations.Environment();

                ProcessLock lockFile = ProcessLock.TryLock();
                if (lockFile != null)
                {
                    using (lockFile)
                    {
                        string connectionString =
                            ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                        IDatabase iDatabase = MySQL.OpenConnection(connectionString);

                        iLogger.Log("Testing DB connectivity...");
                        if (!iDatabase.TestConnection())
                        {
                            iLogger.LogLine("....Connection failed. Check connection string.");
                        }
                        else
                        {
                            iLogger.LogLine("....Connection succeeded.");
                        }

                        List<PhysicsConfigurationProcessed> physicsConfigs = LoadPhysicsConfigurationsFromConfiguration();
                        iLogger.LogLine($"Loading configuration...");
                        WrfConfiguration config = LoadConfigurationFromAppSettings(iLogger);
                        iLogger.LogLine("...done");

                        string stateText = "Prepping"; 

                        _timer = new Timer(delegate (object state)
                        {
                            iDatabase.Checkin(stateText, DateTime.Now);
                        }, null, 0, 1000 * 60);

                        if (args.Length > 0 && args[0].ToLower() == "nodownload")
                        {
                            iLogger.LogLine("Downloading of new data skipped...");
                        }
                        else
                        {
                            PrepStage(iFileSystem, iDownloader, iLogger, iProcess, config);
                        }

                        Random rand = new Random();
                        physicsConfigs = physicsConfigs.OrderBy(m => rand.Next()).ToList();

                        // infinity if -1
                        if (config.MaximumNumberOfRuns == -1) config.MaximumNumberOfRuns = int.MaxValue;

                        stateText = "Computing"; 

                        for (int c = 0; c < config.MaximumNumberOfRuns && c < physicsConfigs.Count; c++)
                        {
                            PhysicsConfigurationProcessed physicsConfig = physicsConfigs[c];
                            ComputeStage(iFileSystem, iLogger, iProcess,
                                iEnvironment, iDatabase, config, physicsConfig);
                        }

                        iDatabase.Checkin("Done", DateTime.Now);
                    }
                }
                else
                {
                    //iLogger.LogLine("WrfSharp appears to be running already, or port 666 is in use.");
                }
            }
            catch(Exception ex)
            {
                File.AppendAllText("crash.txt", ex.ToString()); 
            }
           
        }
    }
}
