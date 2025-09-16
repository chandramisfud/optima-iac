using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNOverBudgetRepo : IDNOverBudgetRepo
    {
        readonly IConfiguration __config;
        public DNOverBudgetRepo(IConfiguration config)
        {
            __config = config;
        }
        //   public IDbConnection Connection => throw new NotImplementedException();
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }

        // debetnote/dnupdateoverbudget
        public async Task DNUpdateOverBudget(int PromoId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@PromoId", PromoId);
                await conn.ExecuteAsync("ip_dn_update_overbudget", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // debetnote/listrefreshoverbudget
        public async Task<IList<DNOverBudgetList>> GetDNOverBudgetList(string periode, int entityId, int distributorId, string channelId, string accountId, string userid, bool isdnmanual)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@periode", "");
                __param.Add("@entity", 0);
                __param.Add("@distributor", 0);
                __param.Add("@channel", 0);
                __param.Add("@account", 0);
                __param.Add("@userid", 0);
                __param.Add("@isdnmanual", true);

                var result = await conn.QueryAsync<DNOverBudgetList>("ip_dn_list_refresh_overbudget", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}