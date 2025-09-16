using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using Repositories.Entities.Models.Dashboard;

namespace Repositories.Repos
{
    public class DashboardMainRepo : IDashboardMainRepo
    {
        readonly IConfiguration __config;
        public DashboardMainRepo(IConfiguration config)
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

        public async Task<IEnumerable<DashboardMasterbyAccess>> GetAllAccountByAccess(string userid, int channelid)
        {
            try
            {
                IEnumerable<DashboardMasterbyAccess>? __values = null;
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@userid", userid);
                    __query.Add("@channelid", channelid);
                    __values = await conn.QueryAsync<DashboardMasterbyAccess>("[dbo].[ip_get_account_by_access]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                }
                return __values;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<IEnumerable<DashboardMasterbyAccess>> GetAllChannelByAccess(string userId)
        {
            try
            {
                IEnumerable<DashboardMasterbyAccess>? __values = null;
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@userid", userId);
                    __values = await conn.QueryAsync<DashboardMasterbyAccess>("[dbo].[ip_get_channel_by_access]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                }
                return __values;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BudgetUsage> GetBudgetUsage(DateTime periode, int entityId, string channel, string account, int subcategoryid, string userid, string viewmode)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@periode", periode);
                __param.Add("@userid", userid);
                __param.Add("@entity", entityId);
                __param.Add("@channel", channel);
                __param.Add("@account", account);
                __param.Add("@subcategory", subcategoryid);
                __param.Add("@viewmode", viewmode);

                var result = await conn.QueryAsync<BudgetUsage>("ip_dashboard_spend", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DashboardMainDto> GetDashboardMain(DateTime period, int entityId, int[] channelId, int[] accountId, int[] categoryId, int subcategoryId, string userid, string viewmode)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __pr = new("AttributeType");
                __pr.Columns.Add("channel");
                foreach (int v in channelId)
                    __pr.Rows.Add(v);

                DataTable __prj = new("AttributeType");
                __prj.Columns.Add("account");
                foreach (int vi in accountId)
                    __prj.Rows.Add(vi);

                DataTable __cat = new("AttributeType");
                __cat.Columns.Add("category");
                foreach (int cat in categoryId)
                    __cat.Rows.Add(cat);

                var __param = new DynamicParameters();
                __param.Add("@periode", period);
                __param.Add("@userid", userid);
                __param.Add("@entity", entityId);
                __param.Add("@channel", __pr.AsTableValuedParameter());
                __param.Add("@account", __prj.AsTableValuedParameter());
                __param.Add("@category", __cat.AsTableValuedParameter());
                __param.Add("@subcategory", subcategoryId);
                __param.Add("@viewmode", viewmode);

                var result = await conn.QueryAsync<DashboardMainDto>("ip_dashboard_main", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DashboardTrendDto>> GetDashboardTrend(DateTime period, int entityId, string userid, int[] channelId, int[] accountId)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __pr = new("AttributeType");
                __pr.Columns.Add("channel");
                foreach (int v in channelId)
                    __pr.Rows.Add(v);

                DataTable __prj = new("AttributeType");
                __prj.Columns.Add("account");
                foreach (int vi in accountId)
                    __prj.Rows.Add(vi);
                var __param = new DynamicParameters();
                __param.Add("@periode", period);
                __param.Add("@userid", userid);
                __param.Add("@entity", entityId);
                __param.Add("@channel", __pr.AsTableValuedParameter());
                __param.Add("@account", __prj.AsTableValuedParameter());

                var result = await conn.QueryAsync<DashboardTrendDto>("ip_dashboard_trend_promo", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetDistributorList(int budgetid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __query = new DynamicParameters();

                __query.Add("@budgetid", budgetid);
                __query.Add("@attribute", "distributor");
                __query.Add("@parent", __parent.AsTableValuedParameter());

                conn.Open();
                var child = await conn.QueryAsync<BaseDropDownList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNListbyPromoIdDto>> GetDNListbyPromoId(int promoId)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@promoid", promoId);

            var result = await conn.QueryAsync<DNListbyPromoIdDto>("ip_debetnote_list_by_promoid", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return result.ToList();
        }

        public async Task<DNMonitoring> GetDNMonitoring(DateTime period, int entityId, string channel, string account, string paymentstatus, string userid, int distibutorId, int dnpromo)
        {
            DNMonitoring __dn = new();
            using IDbConnection conn = Connection;
            try
            {
                var __query = new DynamicParameters();
                __query.Add("@periode", period);
                __query.Add("@entity", entityId);
                __query.Add("@channel", channel);
                __query.Add("@account", account);
                __query.Add("@paymentstatus", paymentstatus);
                __query.Add("@userid", userid);
                __query.Add("@distributorid", distibutorId);
                __query.Add("@dnpromo", dnpromo);

                using (var __re = await conn.QueryMultipleAsync("[dbo].[ip_dashboard_dn]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180))
                {

                    DNMonitoring __result = new()
                    {
                        chart = new List<BarChart>()
                    };
                    foreach (BarChart r in __re.Read<BarChart>())
                        __result.chart.Add(new BarChart()
                        {
                            days = r.days,
                            SGM = r.SGM,
                            NIS = r.NIS,
                            NMN = r.NMN

                        });
                    __result.DN = __re.Read<DNMonitoringHeader>().FirstOrDefault();

                    __result.filter_channel = new List<FilterChannel>();
                    foreach (FilterChannel r in __re.Read<FilterChannel>())
                        __result.filter_channel.Add(new FilterChannel()
                        {
                            ChannelId = r.ChannelId,
                            ChannelDesc = r.ChannelDesc

                        });

                    __result.filter_account = new List<FilterAccount>();
                    foreach (FilterAccount r in __re.Read<FilterAccount>())
                        __result.filter_account.Add(new FilterAccount()
                        {
                            AccountId = r.AccountId,
                            AccountDesc = r.AccountDesc

                        });

                    __result.filter_status = new List<MasterStatus>();
                    foreach (MasterStatus sa in __re.Read<MasterStatus>())
                        __result.filter_status.Add(new MasterStatus()
                        {

                            StatusCode = sa.StatusCode,
                            StatusDesc = sa.StatusDesc
                        });


                    __dn = __result;
                }
                return __dn;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetDNOverBudgetToBeSettled(string userId,
             string sortColumn, string sortDirection, int length = 10, int start = 0, string? txtSearch = null)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@userid", userId);
            __param.Add("@length", length);
            __param.Add("@start", start);
            __param.Add("@txtSearch", txtSearch);
            __param.Add("@SortColumn", sortColumn);
            __param.Add("@SortDirection", sortDirection);

            var __res = await conn.QueryMultipleAsync("ip_dnoverbudget_tobesettled", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            List<object> __data = __res.Read<object>().ToList();

            var res = __res.ReadSingle<BaseLP2>();
            res.Data = __data.Cast<object>().ToList();
            return res;
        }

        public async Task<DropdownList> GetDropDownList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var sql = "select * from tbset_usergroup; select * from tbset_userlevel; select * from tbmst_distributor; select * from tbmst_principal; select * from tbmst_category";
                var __obj = await conn.QueryMultipleAsync(sql);

                var __resuserGroups = __obj.Read<UserGroupforDashboard>();
                var __resuserLevels = __obj.Read<UserLevelforDashboard>();
                var __resdistributors = __obj.Read<DistributorforDashboard>();
                var __resprincipals = __obj.Read<EntityforDashboard>();
                var __rescategories = __obj.Read<CategoryforDashboard>();

                DropdownList __res = new()
                {
                    UserGroup = __resuserGroups.ToList(),
                    UserLevel = __resuserLevels.ToList(),
                    Distributor = __resdistributors.ToList(),
                    Entity = __resprincipals.ToList(),
                    Category = __rescategories.ToList()
                };
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<Notifications> GetNotification(string userid)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@userid", userid);

                var result = await conn.QueryAsync<Notifications>("ip_dashboard_user_notifications_new", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<decimal> GetOnTimePromoApproval(DateTime periode, string viewmode)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@periode", periode);
                __param.Add("@viewmode", viewmode);
                var result = await conn.QueryAsync<decimal>("ip_dashboard_promo_approval", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<decimal> GetOnTimePromoSubmission(DateTime periode, string viewmode)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@periode", periode);
                __param.Add("@viewmode", viewmode);
                var result = await conn.QueryAsync<decimal>("ip_dashboard_promo_submission", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> Search(string refID, string userId)
        {
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                //var sql = @"SELECT a.Id, a.RefId, a.CategoryId, 
                //            b.ShortDesc [CategoryShortDesc], b.LongDesc [CategoryLongDesc],
                //            'Promo' as tipe 
                //            FROM tbtrx_promo AS a
                //            LEFT JOIN tbmst_category b ON b.Id = a.CategoryId
                //            WHERE a.refid LIKE '%{0}%'
                //            UNION ALL
                //            SELECT c.Id, c.RefId, NULL AS [CategoryId], 
                //            NULL AS [CategoryShortDesc], NULL AS [CategoryLongDesc], 
                //            'Debet Note' as tipe FROM tbtrx_debetnote AS c
                //            WHERE c.refid LIKE '%{0}%'";
                var __param = new DynamicParameters();
                __param.Add("@refid", refID);
                __param.Add("@userid", userId);
                return await conn.QueryAsync<object>("ip_search", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}