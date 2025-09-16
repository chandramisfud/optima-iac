using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Repositories.Contracts;
using Repositories.Entities.Report;

namespace Repositories.Repos
{
    public partial class FinInvestmentReportRepository : IFinInvestmentReportRepo
    {
        readonly IConfiguration __config;
        public FinInvestmentReportRepository(IConfiguration config)
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

        public async Task<InvestmentReportLandingPage> GetInvestmentReportLandingPage(string search, string sortColumn, 
            string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId,
            int pageNum = 0, int dataDisplayed = 10, string sortDirection = "ASC")
        {
            InvestmentReportLandingPage res = null!;
            using (IDbConnection conn = Connection)
                try
                {
                    var __param = new DynamicParameters();
                    __param.Add("@userid", profileId);
                    __param.Add("@periode", period);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@start", pageNum);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@order", sortColumn);
                    __param.Add("@sort", sortDirection);
                    __param.Add("@txtSearch", search);
                    var __object = await conn.QueryMultipleAsync("ip_investment_report_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);

                    res = __object.ReadSingle<InvestmentReportLandingPage>();

                    var data = __object.Read<InvestmentReportData>().ToList();

                    res.Data = data;
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
            return res;
        }

        public async Task<IList<InvestmentBudgetAllocationList>> GetBudgetAllocationList(string year, int entityId, int distributorId, int budgetParentId, int channelId, string userId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@periode", year);
                __param.Add("@entity", entityId);
                __param.Add("@distributor", distributorId);
                __param.Add("@budgetparent", budgetParentId);
                __param.Add("@channel", channelId);
                __param.Add("@userid", userId);

                var result = await conn.QueryAsync<InvestmentBudgetAllocationList>("ip_budgetallocation_list", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<InvestmentEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<InvestmentEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<InvestmentDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
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
            var child = await conn.QueryAsync<InvestmentDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return child.ToList();
        }

    }
}
