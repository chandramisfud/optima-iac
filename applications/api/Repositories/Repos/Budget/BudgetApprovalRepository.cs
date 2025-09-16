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
    public partial class BudgetApprovalRepository : IBudgetApprovalRepository
    {
        readonly IConfiguration __config;
        public BudgetApprovalRepository(IConfiguration config)
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

        public async Task<List<BudgetApprovalView>> GetBudgetApprovalLandingPage(string year, int entity, int distributor, int BudgetParent, int channel, string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@periode", year);
                __param.Add("@entity", entity);
                __param.Add("@distributor", distributor);
                __param.Add("@budgetparent", BudgetParent);
                __param.Add("@channel", channel);
                __param.Add("@userid", profileId);

                var result = await conn.QueryAsync<BudgetApprovalView>("ip_budgetallocation_list_approval", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
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

        public async Task<IEnumerable<BaseDropDownList>> GetAllChannel()
        {
            IEnumerable<BaseDropDownList> principals;
            using IDbConnection conn = Connection;
            try
            {
                var sql = "SELECT * FROM tbmst_channel WHERE ISNULL(IsDelete, 0) = 0";

                principals = await conn.QueryAsync<BaseDropDownList>(sql);
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return principals;
        }

        public async Task<bool> BudgetApproval(BudgetApprovalApproveDto budgetApprove)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@budgetid", budgetApprove.budgetId);
                __param.Add("@status", budgetApprove.statsuApproval);
                __param.Add("@notes", budgetApprove.notes);
                __param.Add("@userid", budgetApprove.profileId);

                var result = await conn.ExecuteAsync("ip_approval_budget", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result > 0;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<bool> BudgetUnapproval(BudgetApprovalApproveDto budgetApprove)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@budgetid", budgetApprove.budgetId);
                __param.Add("@status", budgetApprove.statsuApproval);
                __param.Add("@notes", budgetApprove.notes);
                __param.Add("@userid", budgetApprove.profileId);

                var result = await conn.ExecuteAsync("ip_approval_budget", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result > 0;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

    }
}
