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

        public static void DeleteDataDirectory(WrfConfiguration config, IFileSystem fileSystem)
        {
            fileSystem.DeleteDirectory(config.DataDirectory); 
        }

        public static List<string> RemoveTempFilesInWPSDirectory(WrfConfiguration config, 
            IFileSystem fileSystem)
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
                    }
                }
            }
            return ret; 
        }

        public static List<string> RemoveTempFilesInWRFDirectory(WrfConfiguration config, 
            IFileSystem fileSystem)
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
                    }
                }
                foreach (string suffix in SufixesOfFilesToDeleteFromWRFDirectory)
                {
                    if (file.EndsWith(suffix))
                    {
                        ret.Add(file);
                        fileSystem.DeleteFile(file);
                    }
                }
            }
            return ret; 
        }
    }
}
