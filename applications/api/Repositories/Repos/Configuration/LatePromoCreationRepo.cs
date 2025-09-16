using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;
using LatePromoCreation = Repositories.Entities.Configuration.LatePromoCreation;

namespace Repositories.Repos
{
    public class LatePromoCreationRepo : ILatePromoCreationRepo
    {
        readonly IConfiguration __config;
        public LatePromoCreationRepo(IConfiguration config)
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
        public async Task<IList<ConfigLatePromoCreation>> GetConfigLatePromo()
        {
            using IDbConnection conn = Connection;
            try
            {
                var result = await conn.QueryAsync<ConfigLatePromoCreation>("ip_conf_latepromocreation", commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<bool> UpdateLatePromoCreation(LatePromoCreation latePromoCreation)
        {
            bool res = true;
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    using var transaction = conn.BeginTransaction();
                    foreach (var item in latePromoCreation.configList!)
                    {
                        var sql = @"UPDATE [dbo].[tbset_config_reminder]
                                SET
                                    daysfrom=@daysfrom,
                                    useredit=@useredit,
                                    dateedit=@dateedit,
                                    ModifiedEmail=@ModifiedEmail
                                WHERE
                                id=@id";
                        await conn.QueryAsync(sql, new
                        {
                            item.id,
                            item.daysfrom,
                            item.useredit,
                            item.dateedit,
                            item.ModifiedEmail
                        }, transaction);
                    }
                    transaction.Commit();
                }
                return res;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

    }
}