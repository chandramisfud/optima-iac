using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class FinPromoApprovalAgingRepository : IFinPromoApprovalAgingRepo
    {
        readonly IConfiguration __config;
        public FinPromoApprovalAgingRepository(IConfiguration config)
        {
            __config = config;
        }
        //   public IDbConnection Connection => throw new NotImplementedException();
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }

        public async Task<IList<FinPromoApprovalAgingDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
        {
            try
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
                var child = await conn.QueryAsync<FinPromoApprovalAgingDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<FinPromoApprovalAgingEntityList>> GetEntityList()
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<FinPromoApprovalAgingEntityList>(__query);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<FinPromoApprovalAgingLandingPage> GetFinPromoApprovalAgingLandingPage(string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId, string search, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10)
        {
            try
            {
                FinPromoApprovalAgingLandingPage res = null!;
                using (IDbConnection conn = Connection)
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


                    var __object = await conn.QueryMultipleAsync("ip_promo_aging_approval_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);

                    res = __object.ReadSingle<FinPromoApprovalAgingLandingPage>();

                    var data = __object.Read<FinPromoApprovalAgingData>().ToList();

                    res.Data = data;
                }
                return res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}