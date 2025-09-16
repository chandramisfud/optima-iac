using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNUploadRepo : IDNUploadRepo
    {
        readonly IConfiguration __config;
        public DNUploadRepo(IConfiguration config)
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

        public async Task<DNUploadReturn> DNUpload(DataTable dn, string userId)
        {
            DNUploadReturn res = new();
            try
            {
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var result = await conn.QueryMultipleAsync("ip_dn_create_by_import",
                     new
                     {
                         dn = dn.AsTableValuedParameter("DebetNoteType"),
                         userId = userId
                     },
                commandTimeout: 180, commandType: CommandType.StoredProcedure);
                res.data = result.Read<DNUpload>().ToList();
                res.totalRecord = result.ReadSingle<DNUploadRecordTotal>();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<IList<DNFilter>> DNUploadFilter(string userid, string status, int entity, string TaxLevel, DataTable dn)
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
    }
}