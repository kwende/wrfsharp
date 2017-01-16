using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WrfWeb.Helpers.Database
{
    public class MySQLDatabase
    {
        private string _connectionString; 

        public MySQLDatabase(string connectionString)
        {
            _connectionString = connectionString; 
        }

        public RunState GetRunState()
        {
            RunState state = new RunState(); 
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * from wrf.ProcessState;";
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            state.StateText = reader["State"] as string;
                            state.LastCheckinDate = (DateTime)reader["CheckinDate"];
                        }
                    }
                }
            }
            return state; 
        }

        public SimulationResults GetLatestSimulationResults()
        {
            SimulationResults ret = new SimulationResults();
            ret.RunIds = new List<string>();
            ret.PrecipRecords = new List<float[]>();
            ret.Dates = new List<DateTime>();
            ret.TempRecords = new List<float[]>();
            ret.SnowDepths = new List<float[]>();
            ret.WindSpeeds = new List<float[]>();
            ret.SurfacePressure = new List<float[]>(); 

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "wrf.GetLatestRuns";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure; 
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            ret.RunIds.Add(((Guid)reader["RunId"]).ToString().Replace("-",""));

                            if(ret.SimulationStartDate == DateTime.MinValue)
                            {
                                ret.SimulationStartDate = (DateTime)reader["SimulationStartDate"]; 
                            }
                        }
                    }
                }

                bool retrievedDates = false;
                foreach (string runId in ret.RunIds)
                {
                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        List<float> averages = new List<float>();
                        cmd.CommandText = "wrf.GetAveragePrecip";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; 
                        cmd.Parameters.AddWithValue("RunId", runId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                averages.Add((float)(Decimal)reader["AveragePrecip"]);

                                if (!retrievedDates)
                                {
                                    ret.Dates.Add((DateTime)reader["DateTime"]);
                                }
                            }
                            retrievedDates = true;
                        }

                        ret.PrecipRecords.Add(averages.ToArray()); 
                    }

                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        List<float> temps = new List<float>();
                        cmd.CommandText = "wrf.GetAverageTemp";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("RunId", runId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                temps.Add((float)(Decimal)reader["AverageTemp"]); 
                            }
                        }

                        ret.TempRecords.Add(temps.ToArray()); 
                    }

                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        List<float> snowDepths = new List<float>();
                        cmd.CommandText = "wrf.GetAverageSnowDepth";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("RunId", runId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                snowDepths.Add((float)(Decimal)reader["AvgSnowDepth"]); 
                            }
                        }

                        ret.SnowDepths.Add(snowDepths.ToArray()); 
                    }

                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        List<float> windSpeeds = new List<float>();
                        cmd.CommandText = "wrf.GetAverageWindSpeed";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("RunId", runId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                windSpeeds.Add((float)(Double)reader["AvgWindSpeed"]);
                            }
                        }

                        ret.WindSpeeds.Add(windSpeeds.ToArray());
                    }

                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        List<float> surfacePressures = new List<float>();
                        cmd.CommandText = "wrf.GetAverageSurfacePressure";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("RunId", runId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                surfacePressures.Add((float)(Decimal)reader["AvgSurfacePressure"]);
                            }
                        }

                        ret.SurfacePressure.Add(surfacePressures.ToArray());
                    }
                }
            }

            return ret; 
        }
    }
}
