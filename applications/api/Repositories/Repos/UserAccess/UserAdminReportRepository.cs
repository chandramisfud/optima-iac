using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security.Principal;
using System.Threading.Channels;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using Repositories.Entities.UserAccess;

namespace Repositories.Repos
{
    public class UserAdminReportRepository : IUserAdminReportRepository
    {
        readonly IConfiguration __config;
        public UserAdminReportRepository(IConfiguration config)
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
        public async Task<UserAdminReportLandingPage> GetUserAdminReportLandingPage(string keyword, int dataDisplayed = 100, int pageNum = 0)
        {
            UserAdminReportLandingPage res = new();
            using (IDbConnection conn = Connection)
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@userid", "0");
                __param.Add("@start", pageNum);
                __param.Add("@length", dataDisplayed);
                __param.Add("@filter", "");
                __param.Add("@txtSearch", keyword);


                var __object = await conn.QueryMultipleAsync("ip_user_adm_report_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                var data = __object.Read<UserAdminReportData>().ToList();

                var count = __object.ReadSingle<UseradminRecordCount>();

                res.totalCount = count.recordsTotal;
                res.filteredCount = count.recordsFiltered;

                res.Data = data;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
    }
}
