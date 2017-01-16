using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;

namespace WrfSharp.Interfaces
{
    public interface IDatabase
    {
        void SaveRun(DateTime startDate, DateTime endDate, DateTime simulationStartDate,
            float westEastDimension, float southNorthDimension, float bottomTopDimension,
            PhysicsConfigurationProcessed physicsConfiguration, string runId);
        void SaveVariableRecord(string runId, VariableRecord[] records);
        bool TestConnection();
        void Checkin(string state, DateTime checkinDate); 
    }
}
