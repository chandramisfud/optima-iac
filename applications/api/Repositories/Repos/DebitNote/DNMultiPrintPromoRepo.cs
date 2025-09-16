using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNMultiPrintPromoRepo : IDNMultiPrintPromoRepo
    {
        readonly IConfiguration __config;
        public DNMultiPrintPromoRepo(IConfiguration config)
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

        public Task<UserProfileDataById> GetById(string id)
        {
            try
            {
                using IDbConnection conn = Connection;
                var userDictionary = new Dictionary<string, UserProfileDataById>();
                string sql = @"SELECT A.[id]
                            ,A.[username]
                            ,A.[email]
                            ,A.[password]
                            ,A.[usergroupid]
                            ,A.[userlevel]
                            ,A.[isLogin]
                            ,A.[lastLogin]
                            ,A.[department]
                            ,A.[jobtitle]
                            ,A.[contactinfo]
                            ,B.[DistributorId]
                            ,A.[registered]
                            ,A.[code]
                            ,A.[password_change]
                            ,A.[token]
                            ,A.[token_date]
                            ,A.[userinput]
                            ,A.[dateinput]
                            ,A.[useredit]
                            ,A.[dateedit]
                            ,A.[isdeleted]
                            ,A.[deletedby]
                            ,A.[deletedon]
                FROM tbset_user AS A LEFT JOIN 
                tbset_user_distributor AS B ON A.id = B.UserId 
                WHERE A.id =@id";
                var list = conn.Query<UserProfileDataById, ListDistributor, UserProfileDataById>(
                sql, (user, distributor) =>
                {

                    if (!userDictionary.TryGetValue(user.id!, out UserProfileDataById? userEntry))
                    {
                        userEntry = user;
                        userEntry.distributorlist = new List<ListDistributor>();
                        userDictionary.Add(userEntry.id!, userEntry);
                    }

                    userEntry.distributorlist!.Add(distributor);
                    return userEntry;
                },
                    new { Id = id },
                    splitOn: "DistributorId")
                    .Distinct()
                    .FirstOrDefault();

                return Task.FromResult(
                    list
                )!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<FinPromoDisplayEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<FinPromoDisplayEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<PromoView>> GetPromoByConditionsByStatus(string period, int entityId, int distributorId, int BudgetParent, int channelId, string userid, bool cancelstatus)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@periode", period);
                __param.Add("@entity", entityId);
                __param.Add("@distributor", distributorId);
                __param.Add("@budgetparent", BudgetParent);
                __param.Add("@channel", channelId);
                __param.Add("@userid", userid);
                __param.Add("@cancelstatus", cancelstatus);

                var result = await conn.QueryAsync<PromoView>("ip_promo_list_bystatus", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<PromoMultiprint>> GetPromoMultiprint(string period, int entityId, int distributorId, int BudgetParent, int channelId, string userid, bool cancelstatus)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@periode", period);
                __param.Add("@entity", entityId);
                __param.Add("@distributor", distributorId);
                __param.Add("@budgetparent", BudgetParent);
                __param.Add("@channel", channelId);
                __param.Add("@userid", userid);
                __param.Add("@cancelstatus", cancelstatus);

                var result = await conn.QueryAsync<PromoMultiprint>("ip_promo_multiprint_dist", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoRecon> GetPromoReconPromoDisplay(int Id)
        {
            try
            {
                PromoRecon __res = new();
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", Id);
                    __query.Add("@LongDesc", 0);
                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_v3_select_recon]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        var __promoHeader = __obj.Read<PromoReconMainData>().FirstOrDefault();
                        var __regions = __obj.Read<PromoRegionRes>().ToList();
                        var __channels = __obj.Read<PromoChannelRes>().ToList();
                        var __subChannels = __obj.Read<PromoSubChannelRes>().ToList();
                        var __accounts = __obj.Read<PromoAccountRes>().ToList();
                        var __subAccounts = __obj.Read<PromoSubAccountRes>().ToList();
                        var __brands = __obj.Read<PromoBrandRes>().ToList();
                        var __products = __obj.Read<PromoProductRes>().ToList();
                        var __activities = __obj.Read<PromoActivityRes>().ToList();
                        var __subActivities = __obj.Read<PromoSubActivityRes>().ToList();
                        var __attachments = __obj.Read<PromoAttachment>().ToList();
                        var __listApprovalStatus = __obj.Read<ApprovalRes>().ToList();
                        var __sKPValidation = __obj.Read<SKPValidation>().FirstOrDefault();
                        var __investment = __obj.Read<PromoReconInvestmentData>().ToList();
                        var __mechanisms = __obj.Read<MechanismData>().ToList();

                        __res.PromoHeader = __promoHeader;
                        __res.Regions = __regions;
                        __res.Channels = __channels;
                        __res.SubChannels = __subChannels;
                        __res.Accounts = __accounts;
                        __res.SubAccounts = __subAccounts;
                        __res.Brands = __brands;
                        __res.Skus = __products;
                        __res.Activity = __activities;
                        __res.SubActivity = __subActivities;
                        __res.Attachments = __attachments;
                        __res.ListApprovalStatus = __listApprovalStatus;
                        __res.SKPValidations = __sKPValidation;
                        __res.Investments = __investment;
                        __res.Mechanisms = __mechanisms;
                    }
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}