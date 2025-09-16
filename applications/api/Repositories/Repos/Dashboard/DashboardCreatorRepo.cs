using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models.Dashboard;

namespace Repositories.Repos
{
    public class DashboardCreatorRepo : IDashboardCreatorRepo
    {
        readonly IConfiguration __config;
        public DashboardCreatorRepo(IConfiguration config)
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
        // kpiscoring/dashboard
        public async Task<IList<KPIScoringDashboard_DashboardSummary>> GetKPIScoringDashboard(DateTime create_from, DateTime create_to, string userid, bool period_monitoring, DateTime date_monitoring)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@create_from", create_from);
                __param.Add("@create_to", create_to);
                __param.Add("@userid", userid);
                __param.Add("@period_monitoring", period_monitoring);
                __param.Add("@date_monitoring", date_monitoring);
                var result = await conn.QueryAsync<KPIScoringDashboard_DashboardSummary>("ip_kpi_scorboard", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // kpiscoring/league
        public async Task<KPIScoringLeagueDashboardSummary> GetKPIScoringLeagues(DateTime create_from, DateTime create_to, string ChannelDesc)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = new DynamicParameters();
                __query.Add("@create_from", create_from);
                __query.Add("@create_to", create_to);
                __query.Add("@ChannelDesc", ChannelDesc);

                var __obj = await conn.QueryMultipleAsync("ip_kpi_scorboard_creator_league", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);

                var __resOverallScores = __obj.Read<OverallScore>();
                var __resChannelDesc = __obj.Read<ChannelDescription>();

                KPIScoringLeagueDashboardSummary __res = new()
                {
                    OverallScores = __resOverallScores.ToList(),
                    ChannelDescriptions = __resChannelDesc.ToList()
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