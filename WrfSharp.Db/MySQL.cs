﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.DataStructures;
using WrfSharp.Interfaces;

namespace WrfSharp.Db
{
    public class MySQL : IDatabase
    {
        private MySqlConnection _connection; 
        public string Server { get; private set; }
        public string Database { get; private set; }

        public static MySQL OpenConnection(string server, string userName, 
            string password, string database)
        {
            MySQL ret = new MySQL(server, database);
            ret.OpenConnection(userName, password);

            return ret; 
        }

        private MySQL(string server, string database)
        {
            Server = server;
            Database = database; 
        }

        private void OpenConnection(string userName, string password)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = Server;
            builder.Database = Database;
            builder.UserID = userName;
            builder.Password = password; 

            _connection = new MySqlConnection();
            _connection.ConnectionString = builder.ToString();
            _connection.Open(); 
        }

        public void SaveRun(DateTime startDate, DateTime simulationStartDate, float westEastDimension,
            float southNorthDimension, float bottomTopDimension, 
            PhysicsConfigurationProcessed physics, string runId)
        {
            using (MySqlCommand cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "insert into Runs (RunStartDate, SimulationStartDate, WestEastDimension, SouthNorthDimension, " +
                    "BottomTopDimension, Mp_physics, Ra_lw_physics, Ra_sw_physics, Sf_sfclay_physics, Sf_surface_physics, Bl_pbl_physics, Bldt, Cu_physics, " +
                    "Cudt, Isfflx, Ifsnow, Icloud, Surface_input_source, Num_soil_layers, Sf_urban_physics, RunId) values " +
                    "(@RunStartDate, @SimulationStartDate, @WestEastDimension, @SouthNorthDimension, " +
                    "@BottomTopDimension, @Mp_physics, @Ra_lw_physics, @Ra_sw_physics, @Sf_sfclay_physics, @Sf_surface_physics, @Bl_pbl_physics, @Bldt, @Cu_physics, " +
                    "@Cudt, @Isfflx, @Ifsnow, @Icloud, @Surface_input_source, @Num_soil_layers, @Sf_urban_physics, @RunId)";

                cmd.CommandTimeout = 60; 

                cmd.Parameters.AddWithValue("RunStartDate", startDate);
                cmd.Parameters.AddWithValue("SimulationStartDate", simulationStartDate);
                cmd.Parameters.AddWithValue("WestEastDimension", westEastDimension);
                cmd.Parameters.AddWithValue("SouthNorthDimension", southNorthDimension);
                cmd.Parameters.AddWithValue("BottomTopDimension", bottomTopDimension);
                cmd.Parameters.AddWithValue("Mp_physics", physics.MpPhysics);
                cmd.Parameters.AddWithValue("Ra_lw_physics", physics.RaLwPhysics);
                cmd.Parameters.AddWithValue("Ra_sw_physics", physics.RaSwPhysics);
                cmd.Parameters.AddWithValue("Sf_sfclay_physics", physics.SfSfClayPhysics);
                cmd.Parameters.AddWithValue("Sf_surface_physics", physics.SfSurfacePhysics);
                cmd.Parameters.AddWithValue("Bl_pbl_physics", physics.BlPblPhysics);
                cmd.Parameters.AddWithValue("Bldt", physics.Bldt);
                cmd.Parameters.AddWithValue("Cu_physics", physics.CuPhysics);
                cmd.Parameters.AddWithValue("Cudt", physics.Cudt);
                cmd.Parameters.AddWithValue("Isfflx", physics.IsFflx);
                cmd.Parameters.AddWithValue("Ifsnow", physics.IfSnow);
                cmd.Parameters.AddWithValue("Icloud", physics.ICloud);
                cmd.Parameters.AddWithValue("Surface_input_source", physics.SurfaceInputSource);
                cmd.Parameters.AddWithValue("Num_soil_layers", physics.NumSoilLayers);
                cmd.Parameters.AddWithValue("Sf_urban_physics", physics.SfUrbanPhysics);
                cmd.Parameters.AddWithValue("RunId", runId);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateRun(DateTime simulationEndDate, string runId)
        {
        }
    }
}
