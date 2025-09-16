using Dapper;
using System.Data;
using Repositories.Entities;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public partial class FinInvestmentReportRepository
    {
        //public class BudgetSummaryModel
        //{
        //    public List<BrandModel> brands;
        //}
        public async Task<object> GetBudgetSummaryLP(int period, int category, DataTable dtGrpBrand, DataTable dtChannel,
            int start, int length, string txtSearch, string sort, string order)
        {
            using (IDbConnection conn = Connection)
                try
                {
                    var __param = new DynamicParameters();
                    __param.Add("@period", period);
                    __param.Add("@categoryId", category);
                    __param.Add("@channelid", dtChannel.AsTableValuedParameter());
                    __param.Add("@groupBrandId", dtGrpBrand.AsTableValuedParameter());
                    __param.Add("@start", start);
                    __param.Add("@length", length);
                    __param.Add("@order", order);
                    __param.Add("@sort", sort);
                    __param.Add("@txtSearch", txtSearch);
                    var __res = await conn.QueryMultipleAsync("ip_summary_budgeting_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                    var _stat = __res.ReadSingle<dynamic>();
                    var _data = __res.Read<object>().ToList();

                    return new
                    {
                        data = _data,
                        totalCount = _stat.totalCount,
                        filteredCount = _stat.filteredCount
                    };
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
        }
        public async Task<object> GetBudgetSummary(int period, int category, DataTable dtGrpBrand, DataTable dtChannel)
        {
            using (IDbConnection conn = Connection)
                try
                {
                    conn.Open();
                    var __param = new DynamicParameters();
                    __param.Add("@period", period);
                    __param.Add("@categoryId", category);
                    __param.Add("@channelid", dtChannel.AsTableValuedParameter());
                    __param.Add("@groupBrandId", dtGrpBrand.AsTableValuedParameter());

                    var __object = await conn.QueryMultipleAsync("ip_summary_budgeting", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                    List<BudgetSummaryDataModel> byAccount = __object.Read<BudgetSummaryDataModel>().ToList();
                    List<BudgetSummaryDataModel> byChannel = __object.Read<BudgetSummaryDataModel>().ToList();
                    List<BudgetSummaryDataModel> byBrand = __object.Read<BudgetSummaryDataModel>().ToList();

                    List<BrandSummaryModel> tblByBrand = new();
                    //tblByBrand.brands = new List<BrandModel>();

                    foreach (var itemBrand in byBrand)
                    {
                        BrandSummaryModel brands = new BrandSummaryModel();
                        brands.brand = itemBrand.brand;
                        brands.grandTotal = itemBrand;
                        brands.channels = new List<BrandSummaryChannelModel>();
                        foreach (var itemChannel in byChannel.Where(x => x.brand == itemBrand.brand).ToList())
                        {
                            BrandSummaryChannelModel channel = new BrandSummaryChannelModel();
                            channel.channel = itemChannel.channel;
                            channel.accounts = new List<BrandSummaryAccountModel>();
                            foreach (var item in byAccount.
                               Where(x => x.brand == itemBrand.brand && x.channel == itemChannel.channel).ToList())
                            {
                                channel.accounts.Add(new BrandSummaryAccountModel()
                                {
                                    account = item.account,
                                    total = item
                                });
                            }
                            channel.subTotal = itemChannel;
                            brands.channels.Add(channel);
                        }
                        tblByBrand.Add(brands);

                    }
                    conn.Close();
                    
                    conn.Open();
                    __object = await conn.QueryMultipleAsync("ip_summary_budgeting_DC", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                    List<BudgetSummaryDCDataModel> bySubActivityType = __object.Read<BudgetSummaryDCDataModel>().ToList();
                    //List<BudgetSummaryDCDataModel> byDistributor = __object.Read<BudgetSummaryDCDataModel>().ToList();
                    List<BudgetSummaryDCDataModel> byBrandDC = __object.Read<BudgetSummaryDCDataModel>().ToList();

                    List<DCBrandModel> tblByBrandDC = new();

                    foreach (var itemBrand in byBrandDC)
                    {
                        DCBrandModel brands = new DCBrandModel();
                        brands.brand = itemBrand.brand;
                        brands.grandTotal = itemBrand;
                        brands.subActivityType = new List<subActivityTypeModel>();
                        foreach (var itemSubActivity in bySubActivityType.Where(x => x.brand == itemBrand.brand).ToList())
                        {
                            subActivityTypeModel subActivity = new subActivityTypeModel();
                            subActivity.subActivityType = itemSubActivity.subActivityType;
                            subActivity.subTotal = itemSubActivity;
                            brands.subActivityType.Add(subActivity);
                        }
                        tblByBrandDC.Add(brands);

                    }
                    conn.Close();
                    conn.Open();
                    __param = new DynamicParameters();
                    __param.Add("@period", period);

                    var __resp = await conn.QueryMultipleAsync("ip_summary_budgeting_signoff", __param,
                        commandType: CommandType.StoredProcedure, commandTimeout: 180);

                    var tblSign = new
                    {
                        dsDesc = __resp.Read<object>().ToList(),
                        dsType = __resp.Read<object>().ToList(),
                        dsSign = __resp.Read<object>().ToList(),
                    };
                    conn.Close();

                    return new
                    {
                        budgetSign = tblSign,
                        budgetSummary = tblByBrand,
                        budgetSummaryDC = tblByBrandDC
                    };
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
        }
        public async Task<object> GetBudgetSummaryFilter()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var sql = @" /* List Category */
        	                     SELECT id, shortDesc, longDesc
                                 FROM tbmst_category 
                                 where ISNULL(IsDeleted,0) = 0;

                                /* List Channel */
    	                        SELECT DISTINCT a.channelId, a.channelDesc FROM vw_account a
		                        ORDER BY a.ChannelDesc;

                                /* List Account */
	                            SELECT DISTINCT a.channelId, a.channelDesc,  a.accountId, a.accountDesc
	                            FROM vw_account a
	                            ORDER BY a.ChannelDesc,  a.AccountDesc;

	                            /* List group Brand */
	                            SELECT DISTINCT entityId, entityDesc, entityShortDesc, groupBrandId, groupBrandDesc 
                                FROM vw_sku_active ORDER BY entityDesc, groupBrandDesc";

                    using (var __resp = await conn.QueryMultipleAsync(sql, commandTimeout: 180))
                    {
                        var result = new
                        {
                            category = __resp.Read<object>().ToList(),
                            channel = __resp.Read<object>().ToList(),
                            account = __resp.Read<object>().ToList(),
                            grpBrand = __resp.Read<object>().ToList(),
                        };

                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> GetBudgetSummarySignoff(int period)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@period", period);

                    var __resp = await conn.QueryMultipleAsync("ip_summary_budgeting_signoff", __param, 
                        commandType: CommandType.StoredProcedure, commandTimeout: 180);

                    var result = new
                    {
                        dsDesc = __resp.Read<object>().ToList(),
                        dsType = __resp.Read<object>().ToList(),
                        dsSign = __resp.Read<object>().ToList(),
                    };

                    return result;


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> GetBudgetSummaryDC(int period, int category, DataTable dtGrpBrand, DataTable dtChannel)
        {
            using (IDbConnection conn = Connection)
                try
                {
                    var __param = new DynamicParameters();
                    __param.Add("@period", period);
                    __param.Add("@categoryId", category);
                    __param.Add("@channelid", dtChannel.AsTableValuedParameter());
                    __param.Add("@groupBrandId", dtGrpBrand.AsTableValuedParameter());
                    var __object = await conn.QueryMultipleAsync("ip_summary_budgeting_DC", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                    List<BudgetSummaryDCDataModel> bySubActivityType = __object.Read<BudgetSummaryDCDataModel>().ToList();
                    //List<BudgetSummaryDCDataModel> byDistributor = __object.Read<BudgetSummaryDCDataModel>().ToList();
                    List<BudgetSummaryDCDataModel> byBrand = __object.Read<BudgetSummaryDCDataModel>().ToList();

                    List<DCBrandModel> tblByBrand = new();
                    //tblByBrand.brands = new List<BrandModel>();

                    foreach (var itemBrand in byBrand)
                    {
                        DCBrandModel brands = new DCBrandModel();
                        brands.brand = itemBrand.brand;
                        brands.grandTotal = itemBrand;
                        brands.subActivityType = new List<subActivityTypeModel>();
                        foreach (var itemSubActivity in bySubActivityType.Where(x => x.brand == itemBrand.brand).ToList())
                        {
                            subActivityTypeModel subActivity = new subActivityTypeModel();
                            subActivity.subActivityType = itemSubActivity.subActivityType;
                            subActivity.subTotal = itemSubActivity;
                            brands.subActivityType.Add(subActivity);
                        }
                        tblByBrand.Add(brands);

                    }
                    return tblByBrand.ToList();
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
        }
    }
}
