using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNMultiPrintRepo : IDNMultiPrintRepo
    {
        readonly IConfiguration __config;
        public DNMultiPrintRepo(IConfiguration config)
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
        // debetnote/print   
        public async Task<DNPrint> DNPrint(int id)
        {
            try
            {
                List<DNPrint> __dn = new();
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@id", id);

                using (var __re = await conn.QueryMultipleAsync("ip_debetnote_print", __param, commandType: CommandType.StoredProcedure, commandTimeout:180))
                {
                    DNPrint __result = new();
                    __result = __re.Read<DNPrint>().FirstOrDefault()!;
                    __dn.Add(__result);
                }
                return __dn.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // promo/getPromoForDn/
        public async Task<IList<PromoforDN>> GetApprovedPromoforDN(string period, int entity, int channel, int account, string userid)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@periode", period);
                __param.Add("@entity", entity);
                __param.Add("@channel", channel);
                __param.Add("@account", account);
                __param.Add("@userid", userid);

                var result = await conn.QueryAsync<PromoforDN>("ip_promo_approved_list_for_dn", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // debetnote/list
        public async Task<DNMultiPrintLandingPage> GetDNListLandingPage(string period, int entityId, int distributorId, int channelId, int accountId, string profileId, bool isdnmanual, string search, string sortColumn, int pageNum = 0, int dataDisplayed = 10, string sortDirection = "ASC")
        {
            try
            {
                DNMultiPrintLandingPage res = new();
                using IDbConnection conn = Connection;
                var strData = pageNum * dataDisplayed;
                search = (search) ?? "";
                var __param = new DynamicParameters();
                __param.Add("@periode", period);
                __param.Add("@entity", entityId);
                __param.Add("@distributor", distributorId);
                __param.Add("@channel", channelId);
                __param.Add("@account", accountId);
                __param.Add("@userid", profileId);
                __param.Add("@isdnmanual", isdnmanual);
                __param.Add("@start", strData);
                __param.Add("@length", dataDisplayed);
                __param.Add("@filter", "");
                __param.Add("@txtsearch", search);

                var __object = await conn.QueryMultipleAsync("ip_debetnote_list_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                {
                    var data = __object.Read<DNMultiprintList>().ToList();
                    var count = __object.ReadSingle<DNMultiPrintRecordTotal>();

                    res.totalCount = count.recordsTotal;
                    res.filteredCount = count.recordsFiltered;

                    res.Data = data;

                }
                return res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // Select SubAccount
        public async Task<IList<SubAccountforDNMultiprint>> GetSubAccountList()
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"select id, LongDesc 
                                from tbmst_subaccount
                                where isnull(IsDelete, 0) = 0";
                var __res = await conn.QueryAsync<SubAccountforDNMultiprint>(__query);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // user/getbyprimarykey
        public async Task<UserForDNMultiPrint> GetUserById(int userId)
        {
            UserForDNMultiPrint? __res = null;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var sql = @"SELECT * FROM [dbo].[tbset_user_login]
                        WHERE
                        id='{0}'";
                var res = await conn.QueryAsync<UserForDNMultiPrint>(String.Format(sql, userId));
                if (res.Any())
                {
                    __res = res.First();
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __res!;

        }
    }
}