using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Configuration;

namespace Repositories.Repos
{
    public class ReminderRepo : IReminderRepo
    {
        readonly IConfiguration __config;
        public ReminderRepo(IConfiguration config)
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
        public async Task<List<ConfigReminderList>> GetListReminder(int remindertype)
        {
            using IDbConnection conn = Connection;
            try
            {
                conn.Open();
                var sql = @"SELECT a.id, a.remindertype, b.name remindertypename, a.category, c.name categoryname, a.description, a.daysfrom, 
                                    a.daysto, a.frequency, a.userinput, a.dateinput, a.useredit, a.dateedit, a.isdeleted, a.deletedby, a.deletedon 
                                FROM tbset_config_reminder a 
                                inner join tbset_config_dropdown b on a.remindertype = b.id AND b.category = 'remindertype'
                                inner join tbset_config_dropdown c on a.category = c.id AND c.category = 'remindercategory'
                                WHERE a.remindertype=@remindertype";
                var result = await conn.QueryAsync<ConfigReminderList>(sql, new
                {
                    ReminderType = remindertype
                });
                return result.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<bool> UpdateReminder(ConfigReminderListUpdate reminder)
        {
            bool res = true;
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    using var transaction = conn.BeginTransaction();
                    foreach (var item in reminder.configList!)
                    {
                        var sql = @"UPDATE [dbo].[tbset_config_reminder]
                                SET
                                    daysfrom=@daysfrom,
                                    daysto=@daysto,
                                    frequency=@frequency,
                                    useredit=@useredit,
                                    dateedit=@dateedit,
                                    ModifiedEmail=@ModifiedEmail
                                WHERE
                                id=@id";
                        await conn.QueryAsync(sql, new
                        {
                            item.id,
                            item.daysfrom,
                            item.daysto,
                            item.frequency,
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