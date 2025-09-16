using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Repositories.Entities;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public partial class PromoCreationV2Repository 
    { 
       
        public async Task<object> GetPromoCancelRequestLP(string period, int entity, int distributor,
            int budgetParent, int channel, string profileId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@periode", period);
                    __param.Add("@entity", entity);
                    __param.Add("@distributor", distributor);
                    __param.Add("@budgetParent", budgetParent);
                    __param.Add("@channel", channel);
                    __param.Add("@userid", profileId);

                    var __resp = await conn.QueryAsync<object>("[dbo].[ip_promo_cancel_request_list]", __param, 
                        commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    

                    return __resp;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    
    }
}