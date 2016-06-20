using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using WrfSharp.Helpers.Namelists;
using WrfSharp.DataStructures;

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

            Assert.AreEqual(5, nameList.Sections.Count);
            Assert.AreEqual("time_control", nameList["time_control"].Name);
            Assert.AreEqual("run_days", nameList["time_control"]["run_days"].Name); 
            Assert.AreEqual(1, nameList["time_control"].Items[0].Values.Count);
            Assert.AreEqual(12, nameList["time_control"]["run_hours"].Values[0]);
            Assert.AreEqual(3, nameList["time_control"]["start_year"].Values.Count);

            string namelistBackToString = NamelistParser.ParseToString(nameList); 
        }
    }
}
