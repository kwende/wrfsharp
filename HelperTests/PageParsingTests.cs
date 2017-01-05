using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using WrfSharp.Helpers.Web;
using System.Collections.Generic;

namespace HelperTests
{
    [TestClass]
    public class PageParsingTests
    {
        [TestMethod]
        public void ProdGFSDirectoriesPage()
        {
            string pageContent = File.ReadAllText("TestFiles/ProdDirectoryHtml.txt");

            string directoryName = 
                PageParsingHelper.FindDirectoryNameForSecondToLastGFSEntry(pageContent);

            Assert.AreEqual("gfs.2016071518", directoryName);

            directoryName =
                PageParsingHelper.FindDirectoryNameForLatestGFSEntry(pageContent);

            Assert.AreEqual("gfs.2016071600", directoryName);
        }

        [TestMethod]
        public void ProdGFSItemsPage()
        {
            string pageContent = File.ReadAllText("TestFiles/ProdItemsHtml.txt");

            List<string> items =
                PageParsingHelper.FindAllGFSOneDegreePGRB2Files(pageContent);

            Assert.AreEqual("gfs.t18z.pgrb2.1p00.f000", items[0]);
            Assert.AreEqual("gfs.t18z.pgrb2.1p00.f384", items[items.Count - 1]); 
        }
    }
}
