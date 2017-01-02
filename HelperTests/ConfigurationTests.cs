using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WrfSharp.DataStructures;
using WrfSharp.Helpers.Configuration;

namespace HelperTests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void ParseLatLonRect()
        {
            WrfConfiguration config = new WrfConfiguration();
            config.UpperLeftLatLon = "41.044190,-96.911967";
            config.LowerRightLatLon = "40.525328,-96.467021";

            LatLongRect rect = ConfigurationHelper.ParseLatLongRect(config);
            rect.LowerRightLat = 40.525328f;
            rect.LowerRightLong = -96.467021f;
            rect.UpperLeftLat = 41.044190f;
            rect.UpperLeftLong = -96.911967f;
        }
    }
}
