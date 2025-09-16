using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Repositories.Entities.Report;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class FinMatrixApprovalListingRepository : IFinMatrixApprovalListingRepo
    {
        readonly IConfiguration __config;
        public FinMatrixApprovalListingRepository(IConfiguration config)
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

        public async Task<object> GetMatrixPromoAprovalHistoryList(int category, int entity, int distributor, string userid,
                 int start, int length, string txtSearch, string order, string sort)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                //EP1 2024 #143 
                //__param.Add("@periode", body.periode);
                __param.Add("@category", category);
                __param.Add("@entity", entity);
                __param.Add("@distributor", distributor);
                __param.Add("@userid", userid);
                __param.Add("@start", start);
                __param.Add("@length", length);
                __param.Add("@txtSearch", txtSearch);
                __param.Add("@order", order);
                __param.Add("@sort", sort);


                BaseLP baseLP = new BaseLP();
                var __result = conn.QueryMultiple("ip_matrix_approval_history_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                baseLP = __result.Read<BaseLP>().First();               
                baseLP.Data = __result.Read<object>().ToList();
                return baseLP;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<MatrixApprovalLandingPage> GetMatrixApprovalLandingPage(
            string period,
            int categoryId,
            int entityId,
            int distributorId,
            string search,
            string sortColumn,
            int pageNum = 0,
            int dataDisplayed = 10,
            string sortDirection = "ASC"
            )
        {
            MatrixApprovalLandingPage res = null!;
            using (IDbConnection conn = Connection)
                try
                {
                    var __param = new DynamicParameters();
                    __param.Add("@userid", "0");
                    __param.Add("@periode", period);
                    __param.Add("@category", categoryId);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@start", pageNum);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@order", sortColumn);
                    __param.Add("@sort", sortDirection);
                    __param.Add("@txtSearch", search);


                    var __object = await conn.QueryMultipleAsync("ip_matrix_promo_report_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);

                    res = __object.ReadSingle<MatrixApprovalLandingPage>();

                    var data = __object.Read<MatrixApprovalData>().ToList();

                    res.Data = data;
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
            return res;
        }

        public async Task<IList<MatrixApprovalEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<MatrixApprovalEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<MatrixApprovalDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
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
                var child = await conn.QueryAsync<MatrixApprovalDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<object>> GetCategoryDropdownList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT id, shortDesc, longDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<object>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}