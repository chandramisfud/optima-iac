using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNPromoDisplayRepo : IDNPromoDisplayRepo
    {
        readonly IConfiguration __config;
        public DNPromoDisplayRepo(IConfiguration config)
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

        // promov3/getbyprimaryid
        public async Task<DNPromoDisplayId> GetDNPromoDisplaybyId(int id)
        {
            try
            {
                DNPromoDisplayId __res = new();
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", id);
                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display_dist]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        var __promoHeader = __obj.Read<DNPromoDisplayIdHeader>().FirstOrDefault();
                        var __regions = __obj.Read<DNPromoDisplayIdRegionRes>().ToList();
                        var __channels = __obj.Read<DNPromoDisplayIdChannelRes>().ToList();
                        var __subChannels = __obj.Read<DNPromoDisplayIdSubChannelRes>().ToList();
                        var __accounts = __obj.Read<DNPromoDisplayIdAccountRes>().ToList();
                        var __subAccounts = __obj.Read<DNPromoDisplayIdSubAccountRes>().ToList();
                        var __brands = __obj.Read<DNPromoDisplayIdBrandRes>().ToList();
                        var __products = __obj.Read<DNPromoDisplayIdProductRes>().ToList();
                        var __activities = __obj.Read<DNPromoDisplayIdActivityRes>().ToList();
                        var __subActivities = __obj.Read<DNPromoDisplayIdSubActivityRes>().ToList();
                        var __attachments = __obj.Read<DNPromoDisplayIdAttachmentRes>().ToList();
                        var __listApprovalStatus = __obj.Read<DNPromoDisplayIdApproval>().ToList();
                        var __mechanisms = __obj.Read<DNPromoDisplayIdMechanismRes>().ToList();
                        var __investment = __obj.Read<DNPromoDisplayIdInvestmentRes>().ToList();
                        var __groupBrand = __obj.Read<object>().ToList();

                        __res.PromoHeader = __promoHeader!;
                        __res.Regions = __regions;
                        __res.Channels = __channels;
                        __res.SubChannels = __subChannels;
                        __res.Accounts = __accounts;
                        __res.SubAccounts = __subAccounts;
                        __res.Brands = __brands;
                        __res.Skus = __products;
                        __res.Activities = __activities;
                        __res.SubActivities = __subActivities;
                        __res.Attachments = __attachments;
                        __res.ListApprovalStatus = __listApprovalStatus;
                        __res.Mechanisms = __mechanisms;
                        __res.Investments = __investment;
                        __res.GroupBrand = __groupBrand;
                    }
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // promov3/display/distributor
        public async Task<DNPromoDisplayDistPaging> GetDNPromoDisplayLandingPage(
            string period,
            int entityId,
            int distributorId,
            int budgetParentId,
            int channelId,
            string profileId,
            bool cancelstatus,
            string keyword,
            string sortColumn,
            string sortDirection = "ASC",
            int pageNum = 0,
            int dataDisplayed = 10)
        {
            try
            {
                DNPromoDisplayDistPaging res = new();
                using (IDbConnection conn = Connection)
                {
                    var startData = pageNum * dataDisplayed;
                    keyword = (keyword) ?? "";

                    var __param = new DynamicParameters();
                    __param.Add("@periode", period);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@userid", profileId);
                    __param.Add("@cancelstatus", cancelstatus);
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@filter", "");
                    __param.Add("@txtsearch", keyword);

                    var __object = await conn.QueryMultipleAsync("ip_promo_v3_list_display_dist", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    var data = __object.Read<DNPromoDisplayDist>().ToList();
                    var count = __object.ReadSingle<DNPromoDisplayRecord>();

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

        //Select Entity
        public async Task<IList<FinPromoDisplayEntityList>> GetEntityList()
        {
            try
            {
                using IDbConnection conn = Connection;

                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<FinPromoDisplayEntityList>(__query);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // promov3/getrecon
        public async Task<DNPromoDisplayPromoRecon> GetPromoReconPromoDisplay(int Id)
        {
            try
            {
                DNPromoDisplayPromoRecon __res = new();
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", Id);
                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display_recon_dist]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        var __promoHeader = __obj.Read<DNPromoDisplayMainData>().FirstOrDefault();
                        var __regions = __obj.Read<DNPromoDisplayRegionRes>().ToList();
                        var __channels = __obj.Read<DNPromoDisplayChannelRes>().ToList();
                        var __subChannels = __obj.Read<DNPromoDisplaySubChannelRes>().ToList();
                        var __accounts = __obj.Read<DNPromoDisplayAccountRes>().ToList();
                        var __subAccounts = __obj.Read<DNPromoDisplaySubAccountRes>().ToList();
                        var __brands = __obj.Read<DNPromoDisplayBrandRes>().ToList();
                        var __products = __obj.Read<DNPromoDisplayProductRes>().ToList();
                        var __activities = __obj.Read<DNPromoDisplayActivityRes>().AsList();
                        var __subActivities = __obj.Read<DNPromoDisplaySubActivityRes>().AsList();
                        var __attachments = __obj.Read<DNPromoDisplayAttachment>().AsList();
                        var __listApprovalStatus = __obj.Read<DNPromoDisplayApprovalRes>().ToList();
                        var __sKPValidation = __obj.Read<DNPromoDisplaySKPValidationRes>().FirstOrDefault();
                        var __investment = __obj.Read<DNPromoDisplayReconInvestmentRes>().AsList();
                        var __mechanisms = __obj.Read<DNPrommoDisplayMechanismRes>().AsList();
                        var __groupBrand = __obj.Read<object>().AsList();

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
                        __res.GroupBrand = __groupBrand;
                    }
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // Get user distributor
        public async Task<DNPromoDisplayGetDistributorId> GetDistributorId(string UserId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"SELECT TOP 1 Id, DistributorId  
                                FROM  tbset_user_distributor WHERE UserId = @UserId AND ISNULL(IsDeleted,0)=0";

                var __obj = await conn.QueryAsync<DNPromoDisplayGetDistributorId>(__query, new { UserId });
                return __obj.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}