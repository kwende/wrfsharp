using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WrfSharp.Helpers.Web;
using HelperTests.Mocks;

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
                "C:\\local\\directory",
                new MockDownloader());

            Assert.AreEqual("C:\\local\\directory\\test.bmp", localPath);

            string localPath2 = DownloadHelper.DownloadFile(
                "http://www.blah.com/hello/world/test.bmp",
                "C:\\local\\directory\\",
                new MockDownloader());

            Assert.AreEqual("C:\\local\\directory\\test.bmp", localPath2);
        }
    }
}
