using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;
using WrfSharp.Helpers.NetCDF;
using WrfSharp.Interfaces;

namespace WrfSharp.Helpers.Database
{
    public static class DatabaseHelper
    {
        public static void CreateRunRecord(INetCDFReader iNetCdf, IDatabase iDatabase, 
            DateTime startDate, DateTime endDate, string runId)
        {
            DateTime simulationStartDate = NetCDFReaderHelper.GetSimulationDate(iNetCdf);

            float westEast = 0, southNorth = 0, bottomTop = 0;
            NetCDFReaderHelper.ReadGridDimensions(iNetCdf, out westEast, out southNorth, out bottomTop);

            PhysicsConfigurationProcessed physics = 
                NetCDFReaderHelper.ReadPhysicsSettings(iNetCdf);

            iDatabase.SaveRun(startDate, endDate, simulationStartDate,
                westEast, southNorth, bottomTop,
                physics, runId); 
        }
    }
}
