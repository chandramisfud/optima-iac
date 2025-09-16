using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNUploadFakturRepo : IDNUploadFakturRepo
    {
        readonly IConfiguration __config;
        public DNUploadFakturRepo(IConfiguration config)
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

        public async Task<IList<DNFilter>> DNFilterUploadFaktur(string userid, string status, int entity, string TaxLevel, DataTable dn)
        {
            try
            {
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var result = await conn.QueryAsync<DNFilter>("ip_debetnote_filter",
                new
                {
                    userid = userid,
                    status = status,
                    entity = entity,
                    TaxLevel = TaxLevel,
                    dn = dn.AsTableValuedParameter("TemplateDebetNote")
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<DNUpload>> DNUploadUpdateFP(DataTable dn, string userId)
        {
            try
            {
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var result = await conn.QueryAsync<DNUpload>("ip_dn_update_fp",
                new
                {
                    dn = dn.AsTableValuedParameter("DebetNoteFPType"),
                    userid = userId
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout:180);

                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}