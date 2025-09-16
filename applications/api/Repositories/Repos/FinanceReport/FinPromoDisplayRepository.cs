using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class FinPromoDisplayRepository : IFinPromoDisplayRepo
    {
        readonly IConfiguration __config;
        public FinPromoDisplayRepository(IConfiguration config)
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
        public async Task<IList<FinPromoDisplayDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
        {
            try
            {
                using IDbConnection conn = Connection;
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __query = new DynamicParameters();

                __query.Add("@budgetid", budgetid);
                __query.Add("@attribute", "distributor");
                __query.Add("@parent", __parent.AsTableValuedParameter());


                conn.Open();
                var child = await conn.QueryAsync<FinPromoDisplayDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();

            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

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

        public async Task<FinPromoDisplayLandingPage> GetFinPromoDisplayLandingPage(string period,
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
                FinPromoDisplayLandingPage res = new();
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

                    var __object = await conn.QueryMultipleAsync("ip_promo_v3_list_display_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    var data = __object.Read<FinPromoDisplayData>().ToList();
                    var count = __object.ReadSingle<FinPromoDisplayRecord>();
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

        public async Task<PromoDisplayList> GetPromoDisplaybyId(int id)
        {
            try
            {
                PromoDisplayList __res = new();
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", id);
                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        var __promoHeader = __obj.Read<PromoDisplayData>().FirstOrDefault();
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
                        var __mechanisms = __obj.Read<MechanismData>().ToList();
                        var __investment = __obj.Read<PromoReconInvestmentData>().ToList();
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

        public async Task<PromoRecon> GetPromoReconPromoDisplay(int Id)
        {
            try
            {
                PromoRecon __res = new();
                List<PromoRecon> __promoRecon = new();
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", Id);

                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display_recon]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
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

        /// <summary>
        /// promo V2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> GetPromoDisplaypdfbyId(int id, string profile="")
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);
                    __param.Add("@profile", profile);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data_display_recon]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var _promoRecon = new
                    {
                        promo = __resp.ReadSingleOrDefault<object>(),
                        region = __resp.Read<object>().ToList(),
                        sku = __resp.Read<object>().ToList(),
                        attachment = __resp.Read<object>().ToList(),
                        promoStatus = __resp.Read<object>().ToList(),
                        mechanism = __resp.Read<object>().ToList(),
                        prevMechanism = __resp.Read<object>().ToList(),
                        workflow = __resp.Read<object>().ToList()
                    };
                    var _promoCreation = new
                    {
                        promo = __resp.ReadSingleOrDefault<object>(),
                        region = __resp.Read<object>().ToList(),
                        sku = __resp.Read<object>().ToList(),
                        attachment = __resp.Read<object>().ToList(),
                        promoStatus = __resp.Read<object>().ToList(),
                        mechanism = __resp.Read<object>().ToList(),
                        workflow = __resp.Read<object>().ToList()
                    };

                    var dataPromoCreation = _promoCreation.promo;
                    var result = new Object();
                    if (dataPromoCreation == null)
                    {
                        result = new
                        {
                            promoCreation = _promoRecon,
                            promoRecon = dataPromoCreation
                        };
                    }
                    else
                    {
                        result = new
                        {
                            promoCreation = _promoCreation,
                            promoRecon = _promoRecon
                        };
                    }

                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}
