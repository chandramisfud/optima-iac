using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Repositories.Repos
{
    public partial class ConfigRepository : IConfigRepository
    {
        readonly IConfiguration __config;
        public ConfigRepository(IConfiguration config)
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

        public async Task<List<Entities.Dtos.Config>> GetConfig(string category)
        {
            List<Entities.Dtos.Config>? result = null;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var sql = @"SELECT id, name, nourut FROM tbset_config_dropdown
                    WHERE category = '{0}'
                    ORDER BY nourut ASC
                    ";
                var __res = await conn.QueryAsync<Entities.Dtos.Config>(String.Format(sql, category));
                result = __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return result;
        }       
    }
}
