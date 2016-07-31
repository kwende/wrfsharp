using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WrfSharp.Helpers.Web;
using HelperTests.Mocks;
using System.Collections.Generic;
using WrfSharp.DataStructures;

namespace HelperTests
{
    [TestClass]
    public class DownloadTests
    {
        [TestMethod]
        public void DownloadFile()
        {
            string localPath = DownloadHelper.DownloadFile(
                "http://www.blah.com/hello/world/test.bmp", 
                "C:/local/directory",
                new MockDownloader());

            Assert.AreEqual("C:/local/directory/test.bmp", localPath);

            string localPath2 = DownloadHelper.DownloadFile(
                "http://www.blah.com/hello/world/test.bmp",
                "C:/local/directory/",
                new MockDownloader());

            Assert.AreEqual("C:/local/directory/test.bmp", localPath2);
        }

        [TestMethod]
        public void DownloadGFSProductsToDirectory()
        {
            WrfConfiguration config = new WrfConfiguration
            {
                DataDirectory = "/home/brush/Downloads/wrf/data"
            };

            List<string> products = new List<string>();
            products.Add("prod1");
            products.Add("prod2");
            products.Add("prod3");

            List<string> localFiles = DownloadHelper.DownloadGFSProductsToDataDirectory(
                "http://www.ftp.ncep.noaa.gov/data/nccf/com/gfs/prod/gfs.2016072312/",
                products, config, new MockDownloader(), new MockLogger());

            Assert.AreEqual("/home/brush/Downloads/wrf/data/prod1", localFiles[0]);
            Assert.AreEqual("/home/brush/Downloads/wrf/data/prod2", localFiles[1]);
            Assert.AreEqual("/home/brush/Downloads/wrf/data/prod3", localFiles[2]);
        }
    }
}
