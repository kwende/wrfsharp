using Mono.Unix;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;
using WrfSharp.Interfaces;

namespace WrfSharp.Helpers.FileSystem
{
    public static class FileSystemHelper
    {
        public static string[] PrefixesOfFilesToDeleteFromWPSDirectory = { "FILE:", "PFILE:", "GRIBFILE", "met_em" };
        public static string[] PrefixesOfFilesToDeleteFromWRFDirectory = { "wrfout", "wrfrst", "met_em" };
        public static string[] SufixesOfFilesToDeleteFromWRFDirectory = { ".mp4", ".png", };

        public static void CleanDataDirectory(WrfConfiguration config, IFileSystem fileSystem)
        {
            if(fileSystem.DirectoryExists(config.DataDirectory))
            {
                fileSystem.DeleteDirectory(config.DataDirectory);
            }
            fileSystem.CreateDirectory(config.DataDirectory); 
        }

        public static void SetCurrentDirectoryToWRFDirectory(WrfConfiguration config, IFileSystem fileSystem)
        {
            fileSystem.ChangeCurrentDirectory(config.WRFDirectory);
        }

        public static void SetCurrentDirectoryToWPSDirectory(WrfConfiguration config, IFileSystem fileSystem)
        {
            fileSystem.ChangeCurrentDirectory(config.WPSDirectory); 
        }

        public static List<string> RemoveTempFilesInWPSDirectory(WrfConfiguration config, 
            IFileSystem fileSystem, ILogger iLogger)
        {
            List<string> ret = new List<string>(); 
            string[] files = fileSystem.GetFilesInDirectory(config.WPSDirectory); 
            foreach(string file in files)
            {
                foreach(string prefix in PrefixesOfFilesToDeleteFromWPSDirectory)
                {
                    if(file.StartsWith(prefix))
                    {
                        ret.Add(file);
                        fileSystem.DeleteFile(file);
                        iLogger.LogLine($"\t...Deleted '{file}'");  
                    }
                }
            }
            return ret; 
        }

        public static void CreateMetEmSymlinksInRealDirectory(WrfConfiguration config, IFileSystem fileSystem)
        {
            IEnumerable<string> files = fileSystem.GetFilesInDirectory(
                config.WPSDirectory).Where(m => m.StartsWith("met_em")); 

            foreach(string file in files)
            {
                string link = Path.Combine(config.WRFDirectory, Path.GetFileName(file));
                Console.WriteLine($"Source: {file}, Destination: {link}"); 
                fileSystem.CreateSymLink(file, link);  
            }
        }

        public static List<string> RemoveTempFilesInWRFDirectory(WrfConfiguration config, 
            IFileSystem fileSystem, ILogger iLogger)
        {
            List<string> ret = new List<string>();
            string[] files = fileSystem.GetFilesInDirectory(config.WRFDirectory);
            foreach (string file in files)
            {
                foreach (string prefix in PrefixesOfFilesToDeleteFromWRFDirectory)
                {
                    if (file.StartsWith(prefix))
                    {
                        ret.Add(file);
                        fileSystem.DeleteFile(file);
                        iLogger.LogLine($"\t...deleted file {file}"); 
                    }
                }
                foreach (string suffix in SufixesOfFilesToDeleteFromWRFDirectory)
                {
                    if (file.EndsWith(suffix))
                    {
                        ret.Add(file);
                        fileSystem.DeleteFile(file);
                        iLogger.LogLine($"\t...deleted file {file}");
                    }
                }
            }
            return ret; 
        }
    }
}
