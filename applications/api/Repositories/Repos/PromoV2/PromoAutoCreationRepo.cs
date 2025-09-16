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
       
        public async Task<promoCreationResult> SetPromoAutoCreation(int period, int category, int distributor,
            int brand, int channel, int subAccount, int subActivity, string profileId, string userEmail)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@period", period);
                    __param.Add("@categoryId", category);
                    __param.Add("@distributorId", distributor);
                    __param.Add("@brandId", brand);
                    __param.Add("@channelId", channel);
                    __param.Add("@subAccountId", subAccount);
                    __param.Add("@subActivityId", subActivity);
                    __param.Add("@createdBy", profileId);
                    __param.Add("@createdEmail", userEmail);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_generate]", __param, 
                        commandType: CommandType.StoredProcedure, commandTimeout: 180);

                    promoCreationResult _res = __resp.Read<promoCreationResult>().First();
                    if (_res.isSendEmail)
                    {
                        _res.dataEmail = __resp.Read<object>().First();
                    }
                    return _res;
              
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoAutoCreationAtribute( string profileId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();                
                    __param.Add("@profileId", profileId);
                

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_generate_attribute_list]", __param,
                        commandType: CommandType.StoredProcedure, commandTimeout: 180);

                    var _res = new 
                    {
                        category = __resp.Read<object>().ToList(),
                        distributor = __resp.Read<object>().ToList(),
                        brand = __resp.Read<object>().ToList(),
                        channel = __resp.Read<object>().ToList(),
                        subAccount = __resp.Read<object>().ToList(),
                        subActivity = __resp.Read<object>().ToList()
                };
                    return _res;

                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}