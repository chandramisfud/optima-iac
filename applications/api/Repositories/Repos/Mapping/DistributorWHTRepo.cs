using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using static Dapper.SqlMapper;

namespace Repositories.Repos
{
    public class DistributorWHTRepo : IDistributorWHTRepo
    {
        readonly IConfiguration __config;
        public DistributorWHTRepo(IConfiguration config)
        {
            __config = config;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }

        public async Task<BaseLP> GetDistributorWHTLP(string keyword, string distributor, 
            string subActivity,  string subAccount, string WHTType,
            int start, int length, string fieldOrder, string sort)
        {
            BaseLP __result = new();
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@txtSearch", keyword);
                __param.Add("@distributor", distributor);
                __param.Add("@subActivity", subActivity);
                __param.Add("@subAccount", subAccount );
                __param.Add("@WHTType", WHTType);

                __param.Add("@start", start);
                __param.Add("@length", length);
                __param.Add("@fieldOrder", fieldOrder);
                __param.Add("@sort", sort);
                var res = await conn.QueryMultipleAsync("ip_distributor_wht_p", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                if (res != null)
                {
                    __result.Data = res.Read<object>().ToList();
                    BaseLPStats stats = res.Read<BaseLPStats>().First();
                    __result.TotalCount = stats.recordsTotal;
                    __result.FilteredCount = stats.recordsFiltered;
                }

            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __result;
        }

        public async Task<bool> UpdateDistributorWHT(int id, string WHTType, string modifiedBy, string modifiedEmail)
        {
            bool __result = false;
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@id", id);
                __param.Add("@WHTType", WHTType);
                __param.Add("@modifBy", modifiedBy);
                __param.Add("@modifEmail", modifiedEmail);
                var res = await conn.ExecuteAsync("ip_distributor_wht_update", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                __result = res > 0;

            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __result;
        }
        public async Task<bool> CreateDistributorWHT(string distributor, string subActivity, string subAccount, 
            string WHTType, string modifiedBy, string modifiedEmail)
        {
            bool __result = false;
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@distributor", distributor);
                __param.Add("@subActivity", subActivity);
                __param.Add("@subAccount", subAccount);
                __param.Add("@WHTType", WHTType);
                __param.Add("@createBy", modifiedBy);
                __param.Add("@createEmail", modifiedEmail);
                var res = await conn.ExecuteAsync("ip_distributor_wht_create", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                __result = res > 0;

            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __result;
        }

        public async Task<List<object>> ImportDistributorWHT(DataTable dt, string createdBy, string createdEmail)
        {
            List<object>? result = new();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    var __res  = await conn.QueryMultipleAsync("ip_distributor_wht_by_import",
                    new
                    {
                        dataType = dt.AsTableValuedParameter("DistributorWHTSet"),
                        createBy = createdBy,
                        createEmail = createdEmail
                    }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    result = __res.Read<object>().ToList();
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return result;
        }

        public async Task<bool> DeleteDistributorWHT(int id, string modifiedBy, string modifiedEmail)
        {
            bool __result = false;
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@id", id);
                __param.Add("@WHTType", "");
                __param.Add("@modifBy", modifiedBy);
                __param.Add("@modifEmail", modifiedEmail);
                var res = await conn.ExecuteAsync("ip_distributor_wht_update", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                __result = res > 0;

            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __result;
        }


        public async Task<object> GetDistributorWHT(int id)
        {
            List<object> __result = new();
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@id", id);
                var res = await conn.QueryAsync("ip_distributor_wht", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                __result = res.ToList();

            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __result;
        }

        /// <summary>
        ///  run query string result list string
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IList<string>> RunQueryString(string qry)
        {            
            try
            {
                using IDbConnection conn = Connection;
                var res = await conn.QueryAsync<string>(qry, commandTimeout: 180);
                return res.ToList();

            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}