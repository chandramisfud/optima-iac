using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNVATExpiredChecklistRepo : IDNVATExpiredChecklistRepo
    {
        readonly IConfiguration __config;
        public DNVATExpiredChecklistRepo(IConfiguration config)
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

        // debetnote/vatexpriedupdate
        public async Task DNVATExpiredUpdate(string userid, int id, int VATExpired)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@userid", userid);
                __param.Add("@id", id);
                __param.Add("@VATExpired", VATExpired);

                await conn.ExecuteAsync("ip_dn_vat_expired_update", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // Select Distributor
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

        // Select Entity
        public async Task<IList<DNCreationEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<DNCreationEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // debetnote/vatexpired/list/
        public async Task<object> GetVATExpiredList(string status, string userid, int entityId, int distributorId, 
            string TaxLevel,
            string sortColumn, string sortDirection, int length = 10, int start = 0, string? txtSearch = null)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@status", status);
                __param.Add("@userid", userid);
                __param.Add("@entityid", entityId);
                __param.Add("@distributorid", distributorId);
                __param.Add("@TaxLevel", TaxLevel);
                __param.Add("@length", length);
                __param.Add("@start", start);
                __param.Add("@txtSearch", txtSearch);
                __param.Add("@SortColumn", sortColumn);
                __param.Add("@SortDirection", sortDirection);


                var __res = await conn.QueryMultipleAsync("ip_debetnote_list_for_vatexpired", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                List<object> __data = __res.Read<object>().ToList();

                var res = __res.ReadSingle<BaseLP2>();
                res.Data = __data.Cast<object>().ToList();
                return res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}