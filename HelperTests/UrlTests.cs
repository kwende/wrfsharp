using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WrfSharp.Helpers.Web;

namespace HelperTests
{
    [TestClass]
    public class UrlTests
    {
        [TestMethod]
        public void JoinUrls()
        {
            const string Expected = "http://www.google.com/test"; 

            string url = UrlHelper.Join("http://www.google.com", "/test");
            Assert.AreEqual(Expected, url);

            url = UrlHelper.Join("http://www.google.com/", "test");
            Assert.AreEqual(Expected, url);

            url = UrlHelper.Join("http://www.google.com/", "/test");
            Assert.AreEqual(Expected, url);

            url = UrlHelper.Join("http://www.google.com", "test");
            Assert.AreEqual(Expected, url);

            url = UrlHelper.Join(@"http:\\www.google.com\", @"\test");
            Assert.AreEqual(Expected, url);
        }
    }
}
