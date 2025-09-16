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

namespace Repositories.Repos
{
    public class FinDocumentCompletenessRepository : IFinDocumentCompletenessRepo
    {
        readonly IConfiguration __config;
        public FinDocumentCompletenessRepository(IConfiguration config)
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

        public async Task<DocumentCompletenessLandingPage> GetDocumentCompletenessLandingPage(int entityId, int distributorId, string profileId, string status, string taxLevel,
             string search, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10)
        {
            DocumentCompletenessLandingPage res = new();
            using (IDbConnection conn = Connection)
                try
                {
                    var __param = new DynamicParameters();
                    __param.Add("@entityid", entityId);
                    __param.Add("@distributorid", distributorId);
                    __param.Add("@userid", profileId);
                    __param.Add("@status", status);
                    __param.Add("@TaxLevel", taxLevel);
                    __param.Add("@start", pageNum);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@order", sortColumn);
                    __param.Add("@sort", sortDirection);
                    __param.Add("@txtSearch", search);
                    var __object = await conn.QueryMultipleAsync("ip_debetnote_list_doc_completeness_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    res = __object.ReadSingle<DocumentCompletenessLandingPage>();
                    var data = __object.Read<DocumentCompletenessData>().ToList();
                    res.Data = data;
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
            return res;
        }

        public async Task<IList<DocumentCompletenessEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<DocumentCompletenessEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DocumentCompletenessDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
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
                var child = await conn.QueryAsync<DocumentCompletenessDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

    }
}
