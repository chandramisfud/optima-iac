using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Repositories.Entities;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Model;

namespace Repositories.Repos
{
    public partial class BudgetHistoryRepository : IBudgetHistoryRepository
    {
        readonly IConfiguration __config;
        public BudgetHistoryRepository(IConfiguration config)
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

        public async Task<BaseLP> GetBudgetHistoryLandingPage(string year, int entity, int distributor, int budgetParent, int channel, string profileId,
            string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            BaseLP? res = null;
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@periode", year);
                __param.Add("@entity", entity);
                __param.Add("@distributor", distributor);
                __param.Add("@budgetparent", budgetParent);
                __param.Add("@channel", channel);
                __param.Add("@userid", profileId);

                __param.Add("@txtSearch", keyword);
                __param.Add("@sort", sortDirection);
                __param.Add("@order", sortColumn);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                var __res = await conn.QueryMultipleAsync("ip_budget_historical_movement_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                res = __res.ReadSingle<BaseLP>();
                res.Data = __res.Read<BudgetHistoryDto>().Cast<object>().ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<IEnumerable<BaseDropDownList>> GetAllEntity()
        {
            IEnumerable<BaseDropDownList> principals;

            using (IDbConnection conn = Connection)
            {

                var sql = "Select * from tbmst_principal where ISNULL(IsDeleted, 0) = 0";

                principals = await conn.QueryAsync<BaseDropDownList>(sql);
            }
            return principals;
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

    }
}
