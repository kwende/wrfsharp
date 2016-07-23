using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace WrfSharp.Runner.Implementations
{
    public class FileSystem : IFileSystem
    {
        public void CreateDirectory(string directory)
        {
            Directory.CreateDirectory(directory); 
        }

        public void DeleteDirectory(string directory)
        {
            Directory.Delete(directory, true); 
        }

        public void DeleteFile(string file)
        {
            File.Delete(file); 
        }

        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(directory); 
        }

        public string[] GetFilesInDirectory(string directory)
        {
            return Directory.GetFiles(directory); 
        }
    }
}
