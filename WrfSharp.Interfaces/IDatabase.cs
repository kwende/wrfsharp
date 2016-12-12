﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;

namespace WrfSharp.Interfaces
{
    public interface IDatabase
    {
        void SaveRun(DateTime startDate, DateTime simulationStartDate,
            float westEastDimension, float southNorthDimension, float bottomTopDimension,
            PhysicsConfigurationProcessed physicsConfiguration, string runId);

        void UpdateRun(DateTime simulationEndDate, string runId); 
    }
}
