using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Entities;
//using static Dapper.SqlMapper;
using Repositories.Contracts;
using Repositories.Entities.BudgetAllocation;

namespace Repositories.Repos
{
    public class FinPromoApprovalReminderRepository : IFinPromoApprovalReminderRepository
    {
        readonly IConfiguration _config;
        public FinPromoApprovalReminderRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public async Task<PromoApprovalReminderRegularSend> GetPromoApprovalReminderRegularSend()
        {
            PromoApprovalReminderRegularSend res = new();
            try
            {
                using IDbConnection conn = Connection;

                var result = await conn.QueryMultipleAsync("ip_tools_promo_approval_reminder_regular_send", commandType: CommandType.StoredProcedure, commandTimeout: 180);
                res.email = result.Read<string>().ToList()!;
                res.data = result.Read<PromoApprovalReminder>().ToList();
                res.gap = result.Read<PromoApprovalInvestmentGap>().FirstOrDefault()!;
                res.lsPromo = result.Read<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
        public async Task<PromoApprovalReminderResp> GetPromoApprovalReminder(string year, string month, string month2)
        {
            PromoApprovalReminderResp result = new();
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@yperiod", year);
                __param.Add("@mperiod1", month);
                __param.Add("@mperiod2", month2);


                var __res = await conn.QueryMultipleAsync("ip_tools_promo_approval_reminder", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                result.data = __res.Read<PromoApprovalReminder>().ToList();
                result.gap = __res.Read<PromoApprovalInvestmentGap>().FirstOrDefault()!;
                result.lsPromo = __res.Read<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return result;
        }
        public async Task<PromoApprovalReminderSetting> GetPromoApprovalReminderSettingById(int id)
        {
            PromoApprovalReminderSetting res = new();
            try
            {
                using IDbConnection conn = Connection;

                string qry = @"SELECT *
                                    FROM tbset_tools_promo_approval_reminder
                                    WHERE id =" + id;

                var __result = await conn.QueryAsync<PromoApprovalReminderSetting>(qry);
                res = __result.FirstOrDefault()!;
                // get email
                qry = @"SELECT *
                            FROM tbset_tools_promo_approval_reminder_email where id=" + id;

                var __res = await conn.QueryAsync<PromoApprovalReminderConfigEmail>(qry);
                res.configEmail = __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<object> GetPromoApprovalReminderManualEmailConfig()
        {
            List<PromoApprovalReminderConfigEmail> res = null!;
            try
            {
                using IDbConnection conn = Connection;
                // get email
                string qry = @"SELECT *
                            FROM tbset_tools_promo_approval_reminder_email_send";

                var __res = await conn.QueryAsync<PromoApprovalReminderConfigEmail>(qry);
                res = __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<bool> UpdatePromoApprovalReminderSetting(int id, int dt1, int dt2, bool eod, bool autoRun,
            List<PromoApprovalReminderConfigEmail> configEmail, string userId, string userEmail)
        {
            bool res = false;
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                await InsertHistory(id);
                using IDbConnection conn = Connection;

                string qry = @"UPDATE tbset_tools_promo_approval_reminder
                        SET dt1={0}, dt2={1}, eod='{2}', autoRun = '{3}',
                        modifiedBy = '{4}', modifiedOn='{6}', modifiedEmail='{5}'
                        WHERE id =" + id;
                qry = String.Format(qry, dt1, dt2, eod, autoRun, userId, userEmail, TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone));
                var result = await conn.ExecuteAsync(qry);
                res = result > 0;
                if (res)
                {
                    await UpdatePromoApprovalReminderConfigEmail(id, configEmail, userId, userEmail);
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
        public async Task<bool> UpdatePromoApprovalReminderConfigEmail(int id,
            List<PromoApprovalReminderConfigEmail> configEmail, string userId, string userEmail)
        {
            bool res = false;
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                using IDbConnection conn = Connection;
                // insert history of data tobe deleted
                string qry = @"INSERT INTO tbhis_tools_promo_approval_reminder_email 
                            (ParentId, email, username, userGroupName, statusName, ModifiedOn, ModifiedBy, ModifiedEmail, Action  )
                        SELECT id, email, username, userGroupName, statusName,  '{2}', '{0}', '{1}', 'Delete'
                        FROM tbset_tools_promo_approval_reminder_email
                        WHERE id=" + id + ";";
                qry = String.Format(qry, userId, userEmail, TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone));
                qry += @"DELETE tbset_tools_promo_approval_reminder_email; ";
                if (configEmail.Where(x => !string.IsNullOrEmpty(x.email)).Any())
                {
                    qry += "INSERT INTO tbset_tools_promo_approval_reminder_email " +
                        "(id, email, username, userGroupName, statusName, ModifiedOn, ModifiedBy, ModifiedEmail  ) " +
                        "VALUES ";
                    int i = 0;
                    foreach (var item in configEmail)
                    {
                        if (!String.IsNullOrEmpty(item.email))
                        {
                            qry += String.Format(" ({0}, '{1}', '{2}', '{3}', '{4}', '{7}', '{5}', '{6}') ",
                                id, item.email, item.userName, item.userGroupName, item.statusName, userId, userEmail, TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone));
                            i++;
                            if (i < configEmail.Count)
                            {
                                qry += ",";
                            }
                        }
                    }
                    //closure
                    qry += ";";
                }
                var result = await conn.ExecuteAsync(qry);
                res = result > 0;
                // insert history of data inserted
                qry = @"INSERT INTO tbhis_tools_promo_approval_reminder_email 
                            (ParentId, email, username, userGroupName, statusName, ModifiedOn, ModifiedBy, ModifiedEmail, Action  )
                        SELECT id, email, username, userGroupName, statusName, '{2}', '{0}', '{1}', 'Insert'
                        FROM tbset_tools_promo_approval_reminder_email
                        WHERE id=" + id + ";";
                qry = String.Format(qry, userId, userEmail, TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone));
                var result2 = await conn.ExecuteAsync(qry);
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<List<object>> GetUserList(string usergroupid, int userlevel, int isdeleted)
        {
            using IDbConnection conn = Connection;
            var sqlWhere = "";
            var sqlselect = @"SELECT a.id, a.email, a.username userName, b.usergroupname userGroupName, c.levelname levelName, 
                            case when isdeleted=0 then 'active' else 'inActive' end as statusName
                            FROM tbset_user a 
                            left join tbset_usergroup b on a.usergroupid = b.usergroupid
                            left join tbset_userlevel c on a.userlevel = c.userlevel";
            if (usergroupid != "all")
            {
                sqlWhere = " where b.usergroupid=@usergroupid";
            }
            else
            {
                sqlWhere = "";
            }

            if (userlevel != 0)
            {
                if (sqlWhere == "")
                {
                    sqlWhere = " where c.userlevel=@userlevel";
                }
                else
                {
                    sqlWhere += " and c.userlevel=@userlevel";
                }
            }
            if (isdeleted != 0)
            {
                if (isdeleted == 2)
                {
                    isdeleted = 0;
                }
                if (sqlWhere == "")
                {
                    sqlWhere = " where isdeleted=@isdeleted";
                }
                else
                {
                    sqlWhere += " and isdeleted=@isdeleted";
                }
            }
            var sql = sqlselect + sqlWhere;
            var result = await conn.QueryAsync<object>(sql, new { UserGroupId = usergroupid, UserLevel = userlevel, IsDeleted = isdeleted });
            return result.ToList();
        }

        public async Task<bool> UpdatePromoApprovalReminderManualEmailConfig(
           List<PromoApprovalReminderConfigEmail> configEmail, string userId, string userEmail)
        {
            bool res = false;
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {

                using IDbConnection conn = Connection;
                // insert history of data tobe deleted
                string qry = @"INSERT INTO tbhis_tools_promo_approval_reminder_email_send 
                            (email, username, userGroupName, statusName, ModifiedOn, ModifiedBy, ModifiedEmail, Action  )
                        SELECT email, username, userGroupName, statusName, '{2}', '{0}', '{1}', 'Delete'
                        FROM tbset_tools_promo_approval_reminder_email_send;";
                qry = String.Format(qry, userId, userEmail, TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone));
                qry += @"DELETE tbset_tools_promo_approval_reminder_email_send; ";
                if (configEmail.Where(x => !string.IsNullOrEmpty(x.email)).Any())
                {
                    qry += "INSERT INTO tbset_tools_promo_approval_reminder_email_send " +
                        "(email, username, userGroupName, statusName, ModifiedOn, ModifiedBy, ModifiedEmail  ) " +
                        "VALUES ";
                    int i = 0;
                    foreach (var item in configEmail)
                    {
                        if (!String.IsNullOrEmpty(item.email))
                        {
                            qry += String.Format(" ('{0}', '{1}', '{2}', '{3}',  '{6}', '{4}', '{5}') ",
                                item.email, item.userName, item.userGroupName, item.statusName, userId, userEmail, TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone));
                            i++;
                            if (i < configEmail.Count)
                            {
                                qry += ",";
                            }
                        }
                    }
                    //closure
                    qry += ";";
                }
                var result = await conn.ExecuteAsync(qry);
                res = result > 0;
                // insert history of data inserted
                qry = @"INSERT INTO tbhis_tools_promo_approval_reminder_email_send 
                            (email, username, userGroupName, statusName, ModifiedOn, ModifiedBy, ModifiedEmail, Action  )
                        SELECT email, username, userGroupName, statusName, ModifiedOn, ModifiedBy, ModifiedEmail, 'Insert'
                        FROM tbset_tools_promo_approval_reminder_email_send";
                var result2 = await conn.ExecuteAsync(qry);
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
        private async Task InsertHistory(int id)
        {
            using IDbConnection conn = Connection;
            string qry = "INSERT INTO tbhis_tools_promo_approval_reminder " +
                "(ParentId, Dt1, Dt2, EOD, autorun, email, " +
                "CreateOn , CreateBy , CreatedEmail,  " +
                "ModifiedOn , ModifiedBy, ModifiedEmail, " +
                "Action) " +
                "SELECT " +
                "id, Dt1, Dt2, EOD, autorun, email, " +
                "CreateOn , CreateBy , CreatedEmail,  " +
                "ModifiedOn , ModifiedBy, ModifiedEmail, " +
                "'UPDATE' " +
                " FROM tbset_tools_promo_approval_reminder " +
                " WHERE id=" + id;
            var __result = await conn.ExecuteAsync(qry);
        }

    }
}
