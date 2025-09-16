using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Report;

namespace Repositories.Repos
{
    public partial class AccrualReportRepository : IAccrualReportRepo
    {
        readonly IConfiguration __config;
        public AccrualReportRepository(IConfiguration config)
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

        public async Task<AccrualReportLandingPage> GetAccrualReportLandingPage(int download, string period, int entityId, int distributorId, 
            int budgetParentId, int channelId, int grpBrandId, string profileId, string closingDate,
            string search, string sortColumn, int pageNum = 0, int dataDisplayed = 10, string sortDirection = "ASC")
        {
            AccrualReportLandingPage res = null!;
            using (IDbConnection conn = Connection)
                try
                {
                    var __param = new DynamicParameters();
                    __param.Add("@download", download);
                    __param.Add("@userid", profileId);
                    __param.Add("@periode", period);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@closingdt", closingDate);
                    __param.Add("@start", pageNum);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@order", sortColumn);
                    __param.Add("@sort", sortDirection);
                    __param.Add("@txtSearch", search);
                    __param.Add("@grpBrandId", grpBrandId);


                    var __object = await conn.QueryMultipleAsync("ip_promo_accrual_report_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);

                    res = __object.ReadSingle<AccrualReportLandingPage>();

                    var data = __object.Read<object>().ToList();

                    res.Data = data;
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
            return res;
        }

        public async Task<IList<AccrualEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<AccrualEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetGroupBrandList(int entity)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT id grpBrandId, longdesc grBrandDesc 
                    FROM tbmst_brand_group
                    WHERE PrincipalId=@pEntity AND ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<object>(__query, new { pEntity=entity });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}