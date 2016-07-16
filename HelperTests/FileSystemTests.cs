using System;
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
        public void CleanWPSDirectory()
        {
            List<string> filesDeleted = FileSystemHelper.RemoveTempFilesInWPSDirectory(
                new WrfConfiguration(), new MockWPSFileSystem());

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
                new WrfConfiguration(), new MockWRFFileSystem());

            Assert.AreEqual(5, filesDeleted.Count);
            Assert.AreEqual("wrfout_ereere", filesDeleted[0]);
            Assert.AreEqual("wrfrst", filesDeleted[1]);
            Assert.AreEqual("met_em", filesDeleted[2]);
            Assert.AreEqual("test.mp4", filesDeleted[3]);
            Assert.AreEqual("test.png", filesDeleted[4]);
        }
    }
}
