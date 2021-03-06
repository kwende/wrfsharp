﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace HelperTests.Mocks
{
    public class MockWRFFileSystem : IFileSystem
    {
        public void ChangeCurrentDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void CreateDirectory(string dataDirectory)
        {
            throw new NotImplementedException();
        }

        public void CreateSymLink(string source, string link)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string directory)
        {
            return; 
        }

        public void DeleteFile(string file)
        {
            return; 
        }

        public bool DirectoryExists(string dataDirectory)
        {
            throw new NotImplementedException();
        }

        public string[] GetFilesInDirectory(string directory)
        {
            //{ "wrfout", "wrfrst", "met_em" };
            //{ ".mp4", ".png", };
            return new string[]
            {
                "wrfout_ereere",
                "wrf",
                "wrfrst",
                "wrfsrt",
                "met_em",
                "metmetmet",
                "test.mp4",
                "testmp4",
                "test.png",
                "testpng"
            }; 
        }

        public string ReadFileContent(string path)
        {
            throw new NotImplementedException();
        }

        public void WriteFileContent(string path, string content)
        {
            throw new NotImplementedException();
        }
    }
}
