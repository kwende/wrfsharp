using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WrfSharp.Helpers.Processes;
using WrfSharp.DataStructures;
using HelperTests.Mocks;

namespace HelperTests
{
    [TestClass]
    public class ProcessTests
    {
        [TestMethod]
        public void Wgrib2StartEndDates()
        {
            WrfConfiguration config = new WrfConfiguration();
            config.DataDirectory = "/home/brush/Downloads/wrf/data"; 

            DateTime startDate, endDate;
            Wgrib2Helper.FindStartAndEndDatesOnWGribFiles(
                config, out startDate, out endDate,
                new MockProcessLauncherForWgrib2(),
                new MockGFSListingFileSystem());

            Assert.AreEqual("7/17/2016 6:00:00 AM", startDate.ToString());
            Assert.AreEqual("8/1/2016 6:00:00 PM", endDate.ToString()); 
        }
    }
}
