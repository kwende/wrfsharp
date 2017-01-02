using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;

namespace WrfSharp.Helpers.Configuration
{
    public static class ConfigurationHelper
    {
        public static LatLongRect ParseLatLongRect(WrfConfiguration config)
        {
            string[] upperLeftLatLon = config.UpperLeftLatLon.Split(',');
            string[] lowerRightLatLon = config.LowerRightLatLon.Split(',');

            return new LatLongRect
            {
                 UpperLeftLat = float.Parse(upperLeftLatLon[0]),
                 LowerRightLat = float.Parse(lowerRightLatLon[0]),
                 LowerRightLong = float.Parse(lowerRightLatLon[1]),
                 UpperLeftLong = float.Parse(upperLeftLatLon[1]),
            }; 
        }
    }
}
