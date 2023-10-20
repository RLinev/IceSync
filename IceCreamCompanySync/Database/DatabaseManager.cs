using IceCreamCompanySync.Database.Intefaces;
using System.Data;
using System.Data.SqlClient;

namespace CPanel.DistributionWorker.Manager.Database
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly ILogger<DatabaseManager> _logger;
        private readonly string _connectionString; 
        private readonly string _connectionTimeout;
        public DatabaseManager(IConfiguration config, ILogger<DatabaseManager> logger)
        {
            _connectionString = config.GetValue<string>("ConnectionString");
            _connectionTimeout = config.GetValue<string>("ConnectionTimeOut");
            _logger = logger;
        }
        public bool ExecuteNotQuery(string p_Command, List<SqlParameter> prms)
        {
            bool bSucceed = true;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    _logger.LogInformation($"Trying to connect to database {connection.ConnectionString} and execute stored procedure with name: {p_Command}");
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(p_Command, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = int.Parse(_connectionTimeout);
                        if (prms != null)
                        {
                            foreach (SqlParameter param in prms)
                            {
                                cmd.Parameters.Add(param);
                            }
                        }
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }
                catch (SqlException ex)
                {
                    _logger.LogInformation($"SqlExeption occur while executing. Exeption: {ex.Message}");
                    bSucceed = false;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Error while executing. Exeption: {ex.Message}");
                    bSucceed = false;
                }
                finally
                {
                    _logger.LogInformation($"Execution complease");
                    connection.Close();
                    connection.Dispose();
                }

                return bSucceed;
            }
        }
    }
}