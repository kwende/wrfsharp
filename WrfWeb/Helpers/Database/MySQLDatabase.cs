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

        public PrecipSimulationResults GetLatestPrecipSimulationResults()
        {
            PrecipSimulationResults ret = new PrecipSimulationResults();
            ret.RunIds = new List<string>();
            ret.RunRecords = new List<float[]>();
            ret.Dates = new List<DateTime>(); 

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

                        ret.RunRecords.Add(averages.ToArray()); 
                    }
                }
            }

            return ret; 
        }
    }
}
