using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;
using WrfSharp.Interfaces;

namespace WrfSharp.Db
{
    public class MySQL : IDatabase
    {
        public void SaveRun(DateTime startDate, DateTime simulationStartDate, int westEastDimension, 
            int southNorthDimension, int bottomTopDimension, PhysicsConfigurationProcessed physicsConfiguration, string runId)
        {
            throw new NotImplementedException();
        }

        public void UpdateRun(DateTime simulationEndDate, string runId)
        {
            throw new NotImplementedException();
        }
    }
}
