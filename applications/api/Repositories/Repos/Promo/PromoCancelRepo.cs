using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts.Promo;
using Repositories.Entities;
using Repositories.Entities.Models.DN;
using System.Data;
using System.Data.SqlClient;

namespace Repositories.Repos
{
    public class PromoCancelRepository : IPromoCancelRepository
    {
        readonly IConfiguration __config;
        public PromoCancelRepository(IConfiguration config)
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
        public async Task<IList<PromoView>> GetPromoCancel(string year, int entity, int distributor, 
            int BudgetParent, int channel, string userid)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@periode", year);
            __param.Add("@entity", entity);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);
            __param.Add("@cancelstatus", true);

            var result = await conn.QueryAsync<PromoView>("ip_promo_list_bystatus", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return result.ToList();
        }

        public async Task<IList<object>> GetPromoCancelRequestLP(string year, int entity, int distributor, 
            int BudgetParent, int channel, string userid)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@periode", year);
            __param.Add("@entity", entity);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);

            var result = await conn.QueryAsync<object>("ip_promo_cancel_request_list", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return result.ToList();
        }

        /// <summary>
        /// to approve: statusCode = 'TP2'
        /// to sendback: statusCode = 'TP3'
        /// </summary>
        /// <param name="promoId"></param>
        /// <param name="statusCode"></param>
        /// <param name="userId"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public async Task<ErrorMessageDto> SetPromoCancelRequest(int promoId, int promoPlanId,  string statusCode, 
            string notes, string userId, string emailId)
        {
            try
            {
                ErrorMessageDto __result = new();
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@promoid", promoId);
                __param.Add("@userid", userId);
                __param.Add("@statuscode", statusCode);
                __param.Add("@ApproverEmail", emailId);


                var result = await conn.QueryAsync<ErrorMessageDto>("ip_promo_cancel_approval", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                __result = result.FirstOrDefault()!;

                if (statusCode == "TP2" && __result.statuscode == 200)
                {
                    // if success canceling promo then cancel the planning
                    var __planparam = new DynamicParameters();
                    __planparam.Add("@promoplanid", promoPlanId);
                    __planparam.Add("@reason", notes);
                    __planparam.Add("@userid", userId);


                    var result2 = await conn.QueryAsync<ErrorMessageDto>("ip_promo_planning_cancel", __planparam, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    __result = result2.FirstOrDefault()!;
                }

                return __result;
            }
            catch (Exception __ex) {  
                throw new Exception(__ex.Message, __ex);
            }
        }
    }
}
