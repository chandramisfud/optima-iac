using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.Dashboard;

namespace Repositories.Repos
{
    public class DashboardPromoCalendarRepo : IDashboardPromoCalendarRepo
    {
        readonly IConfiguration __config;
        public DashboardPromoCalendarRepo(IConfiguration config)
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

        public async Task<IList<SubCategorytoCategory>> GetCategoryListforDashboardPromoCalendar()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<SubCategorytoCategory>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IEnumerable<DashboardMasterbyAccess>> GetDashboardMasterAccountbyAccesses(string userid, int channelid)
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

        public async Task<IEnumerable<DashboardMasterbyAccess>> GetDashboardMasterChannelbyAccesses(string userId)
        {
            try
            {
                IEnumerable<DashboardMasterbyAccess> __values = null!;
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

        public async Task<IList<DashboardCalendarDto>> GetDashboarPromodCalendar(string userId, int promoPlanId, string activity_desc)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@userid", userId);
                __param.Add("@promoplanid", promoPlanId);
                __param.Add("@activity_desc", activity_desc);

                var __res = await conn.QueryAsync<DashboardCalendarDto>("ip_dashboard_promo_calendar", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return __res.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<EntityForMechanism>> GetEntityForDashboardPromoCalendar()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<EntityForMechanism>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IEnumerable<SubCategoryforDashboardPromoCalendar>> GetSubCategoryforDashboardPromoCalendars(string userid)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = new DynamicParameters();
                __query.Add("@userid", userid);
                var __res = await conn.QueryAsync<SubCategoryforDashboardPromoCalendar>("[dbo].[ip_select_subcategory_byuser]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}