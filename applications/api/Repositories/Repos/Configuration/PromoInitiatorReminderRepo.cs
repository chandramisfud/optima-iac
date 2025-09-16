using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;

namespace Repositories.Repos
{
    public class PromoInitiatorReminderRepo : IPromoInitiatorReminderRepo
    {
        readonly IConfiguration __config;
        public PromoInitiatorReminderRepo(IConfiguration config)
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
        public async Task<IList<ConfigPromoInitiatorReminderList>> ConfigPromoInitiatorReminderList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var result = await conn.QueryAsync<ConfigPromoInitiatorReminderList>("ip_conf_reminder_promo_initiator", commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<bool> UpdateConfigReminderPromoInitiator(ConfigPromoInitiatorReminderListUpdate body)
        {
            bool res = true;
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    using var transaction = conn.BeginTransaction();
                    foreach (var item in body.configList!)
                    {
                        var sql = @"UPDATE [dbo].[tbset_config_reminder]
                                SET
                                    datereminder=@datereminder,
                                    useredit=@useredit,
                                    dateedit=@dateedit,
                                    ModifiedEmail=@ModifiedEmail
                                WHERE
                                id=@id";
                        await conn.QueryAsync(sql, new
                        {
                            item.id,
                            item.datereminder,
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