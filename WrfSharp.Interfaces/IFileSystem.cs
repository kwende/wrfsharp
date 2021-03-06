﻿using System;
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
        bool DirectoryExists(string directory);
        void CreateDirectory(string directory);
        string ReadFileContent(string path);
        void WriteFileContent(string path, string content);
        void ChangeCurrentDirectory(string path);
        void CreateSymLink(string source, string link); 
    }
}
