﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WrfSharp.Helpers.FileSystem;
using WrfSharp.DataStructures;
using HelperTests.Mocks;
using System.Collections.Generic;

namespace HelperTests
{
    [TestClass]
    public class FileSystemTests
    {
        [TestMethod]
        public void CreateEmSymlinks()
        {
            WrfConfiguration config = new WrfConfiguration();
            config.WPSDirectory = "/home/brush/Downloads/wrf/WPS";
            config.WRFDirectory = "/home/brush/Downloads/wrf/WRFV3/test/em_real";

            MockWPSFileSystemWithMetEmFiles fs = new MockWPSFileSystemWithMetEmFiles(); 
            FileSystemHelper.CreateMetEmSymlinksInRealDirectory(config, fs);

            Assert.AreEqual(129, fs.SymLinksCreated.Count); 
            foreach(string symlink in fs.SymLinksCreated)
            {
                Assert.IsTrue(symlink.Contains("met_em")); 
            }

            return;  
        }

        [TestMethod]
        public void CleanWPSDirectory()
        {
            List<string> filesDeleted = FileSystemHelper.RemoveTempFilesInWPSDirectory(
                new WrfConfiguration(), new MockWPSFileSystem(), new MockLogger());

            Assert.AreEqual(4, filesDeleted.Count);
            Assert.AreEqual("FILE:test", filesDeleted[0]);
            Assert.AreEqual("PFILE:adfadfs", filesDeleted[1]);
            Assert.AreEqual("GRIBFILE_", filesDeleted[2]);
            Assert.AreEqual("met_em", filesDeleted[3]);
        }

        [TestMethod]
        public void CleanWRFDirectory()
        {
            List<string> filesDeleted = FileSystemHelper.RemoveTempFilesInWRFDirectory(
                new WrfConfiguration(), new MockWRFFileSystem(), new MockLogger());

            Assert.AreEqual(5, filesDeleted.Count);
            Assert.AreEqual("wrfout_ereere", filesDeleted[0]);
            Assert.AreEqual("wrfrst", filesDeleted[1]);
            Assert.AreEqual("met_em", filesDeleted[2]);
            Assert.AreEqual("test.mp4", filesDeleted[3]);
            Assert.AreEqual("test.png", filesDeleted[4]);
        }

        [TestMethod]
        public void FindFirstAndLastGribFile()
        {

        }
    }
}
