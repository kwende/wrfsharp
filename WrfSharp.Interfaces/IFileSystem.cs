using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.Interfaces
{
    public interface IFileSystem
    {
        void DeleteFile(string file);
        void DeleteDirectory(string directory);
        string[] GetFilesInDirectory(string directory); 
    }
}
