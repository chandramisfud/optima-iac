using System.Data;
using Dapper;
using Repositories.Contracts;

namespace Repositories.Repos
{
    public partial class ConfigRepository : IConfigRepository
    {
        public async Task<object> UploadMechanismInput(DataTable data, string fileName, string profile, string email)
        {
            int rowAffected = 0;
            using IDbConnection conn = Connection;
            try
            {
                conn.Open();


                var logId = conn.Query<int>("ip_log_upload",
                new
                {
                    activity = "Mechanism Input Method",
                    filename = fileName,
                    profileId = profile,
                    email = email,
                    logstatus = "success"
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180).First();

                conn.Close();   

                conn.Open();

                rowAffected = conn.Execute("ip_promo_mechanism_input_method_upload",
                new
                {
                    dataSet = data.AsTableValuedParameter()
                }
               , commandType: CommandType.StoredProcedure, commandTimeout: 180);

                conn.Close();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return rowAffected;
        }

        public async Task<object> GetMechanismInput()
        {
            int rowAffected = 0;
            using IDbConnection conn = Connection;
            try
            {
                conn.Open();

                var __res = await conn.QueryAsync("ip_promo_mechanism_input_method_list",
                commandType: CommandType.StoredProcedure, commandTimeout: 180);

                conn.Close();

                return __res;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return rowAffected;
        }
    }
}