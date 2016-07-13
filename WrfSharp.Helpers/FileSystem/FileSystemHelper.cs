using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;

namespace WrfSharp.Helpers.FileSystem
{
    public static class FileSystemHelper
    {
        public static void DeleteDataDirectory(WrfConfiguration config)
        {
            Directory.Delete(config.DataDirectory, true); 
        }
    }
}
