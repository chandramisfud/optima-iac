using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models.Dashboard;
using Repositories.Entities.Report;

namespace Repositories.Repos
{
    public class DashboardApproverRepo : IDashboardApproverRepo
    {
        readonly IConfiguration __config;
        public DashboardApproverRepo(IConfiguration config)
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
        // GetAllChannel
        public async Task<IList<ListingPromoReportingChannelList>> GetChannelList(string userid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __query = new DynamicParameters();

                __query.Add("@userid", userid);
                __query.Add("@attribute", "channel");
                __query.Add("@parent", __parent.AsTableValuedParameter());


                conn.Open();
                var child = await conn.QueryAsync<ListingPromoReportingChannelList>("ip_getattribute_bymapping", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // kpiscoring/approver
        public async Task<KPIScoringApproverDashboardSummary> GetKPIScoringApprover(string userid, DateTime promostart, DateTime promoend)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = new DynamicParameters();
                __query.Add("@userid", userid);
                __query.Add("@promostart", promostart);
                __query.Add("@promoend", promoend);

                var __obj = await conn.QueryMultipleAsync("ip_kpi_scorboard_approver", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);

                var __reskpiApprover1 = __obj.Read<KPIApproverResult1>();
                var __reskpiApprover2 = __obj.Read<KPIApproverResult2>();

                KPIScoringApproverDashboardSummary __res = new()
                {
                    KPIApproverResults1 = __reskpiApprover1.ToList(),
                    KPIApproverResults2 = __reskpiApprover2.ToList()
                };

                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // kpiscoring/standing
        public async Task<KPIScoringStandingDashboardSummary> GetKPIScoringStanding(DateTime create_from, DateTime create_to)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = new DynamicParameters();
                __query.Add("@create_from", create_from);
                __query.Add("@create_to", create_to);

                var __obj = await conn.QueryMultipleAsync("ip_kpi_scorboard_creator_league_standings", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);

                var __reskpiStanding1 = __obj.Read<Result1>();
                var __reskpiStanding2 = __obj.Read<Result2>();
                var __reskpiStanding3 = __obj.Read<Result3>();
                var __reskpiStanding4 = __obj.Read<Result4>();
                var __reskpiStanding5 = __obj.Read<Result5>();

                KPIScoringStandingDashboardSummary __res = new()
                {
                    Result1s = __reskpiStanding1.ToList(),
                    Result2s = __reskpiStanding2.ToList(),
                    Result3s = __reskpiStanding3.ToList(),
                    Result4s = __reskpiStanding4.ToList(),
                    Result5s = __reskpiStanding5.ToList(),

                };
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}