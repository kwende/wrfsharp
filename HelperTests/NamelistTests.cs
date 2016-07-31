using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using WrfSharp.Helpers.Namelists;
using WrfSharp.DataStructures;
using WrfSharp.Interfaces;
using HelperTests.Mocks;

namespace HelperTests
{
    [TestClass]
    public class NamelistTests
    {
        [TestMethod]
        public void ParseFileContents()
        {
            string namelistContent = File.ReadAllText("TestFiles/namelist.input");

            Namelist nameList = NamelistParser.ParseFromString(namelistContent);

            Assert.AreEqual(8, nameList.Sections.Count);
            Assert.AreEqual("time_control", nameList["time_control"].Name);
            Assert.AreEqual("run_days", nameList["time_control"]["run_days"].Name); 
            Assert.AreEqual(1, nameList["time_control"].Items[0].Values.Count);
            Assert.AreEqual(125.0, nameList["time_control"]["run_hours"].Values[0]);
            Assert.AreEqual(1, nameList["time_control"]["start_year"].Values.Count);

            string namelistBackToString = NamelistParser.ParseToString(nameList);

            nameList = NamelistParser.ParseFromString(namelistBackToString);

            Assert.AreEqual(8, nameList.Sections.Count);
            Assert.AreEqual("time_control", nameList["time_control"].Name);
            Assert.AreEqual("run_days", nameList["time_control"]["run_days"].Name);
            Assert.AreEqual(1, nameList["time_control"].Items[0].Values.Count);
            Assert.AreEqual(125.0, nameList["time_control"]["run_hours"].Values[0]);
            Assert.AreEqual(1, nameList["time_control"]["start_year"].Values.Count);
        }

        [TestMethod]
        public void ParseFileContentsWithString()
        {
            string namelistContent = File.ReadAllText("TestFiles/wrfnamelist.input");

            Namelist nameList = NamelistParser.ParseFromString(namelistContent);
            string value = nameList["geogrid"]["map_proj"].Values[0].ToString();
            Assert.AreEqual("lambert", value);

            nameList["geogrid"]["map_proj"].Values.Clear();
            nameList["geogrid"]["map_proj"].Values.Add("test");

            string asString = NamelistParser.ParseToString(nameList);

            Assert.IsTrue(asString.Contains("map_proj = 'test'")); 
        }

        [TestMethod]
        public void TestSavingContentInWPSNamelist()
        {
            IFileSystem fileSystem = new MockWPSNamelistFileSystem();
            WrfConfiguration config = new WrfConfiguration();
            config.WRFNamelist = "";
            NamelistHelper.UpdateDatesInWPSNamelist(config, new DateTime(1980, 5, 26),
                new DateTime(1981, 5, 26), fileSystem);

            string result = fileSystem.ReadFileContent("getresult");
            Assert.IsTrue(result.Contains("start_date = '1980-05-26_00:00:00'")); 
        }

        [TestMethod]
        public void TestSavingContentInWrfNamelist()
        {
            IFileSystem fileSystem = new MockWRFNamelistFileSystem();
            WrfConfiguration config = new WrfConfiguration();
            config.WPSNamelist = "";
            NamelistHelper.UpdateDatesInWRFNamelist(config, 
                new DateTime(1980, 5, 26, 1, 0, 0, 0),
                new DateTime(1981, 6, 27, 2, 0, 0, 0), 
                fileSystem);

            string result = fileSystem.ReadFileContent("getresult");

            Assert.IsTrue(result.Contains("start_year = 1980"));
            Assert.IsTrue(result.Contains("start_month = 5"));
            Assert.IsTrue(result.Contains("start_day = 26"));
            Assert.IsTrue(result.Contains("start_hour = 1"));

            Assert.IsTrue(result.Contains("end_year = 1981"));
            Assert.IsTrue(result.Contains("end_month = 6"));
            Assert.IsTrue(result.Contains("end_day = 27"));
            Assert.IsTrue(result.Contains("end_hour = 2"));
        }
    }
}
