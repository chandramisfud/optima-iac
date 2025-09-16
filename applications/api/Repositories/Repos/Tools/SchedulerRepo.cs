using Dapper;
using Entities;
using MailKit.Net.Imap;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repositories.Contracts;
using Repositories.Entities;
using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Repositories.Repos
{
    public class SchedulerRepo : ISchedulerRepo
    {
        readonly IConfiguration __config;
        readonly IToolsEmailRepository __emailRepo;
        public SchedulerRepo(IConfiguration config, IToolsEmailRepository emailRepo)
        {
            __config = config;
            __emailRepo = emailRepo;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }

        // promo/autoclosing
        public async Task<IList<AutoCloseDto>> GetAutoClosing()
        {
            using IDbConnection conn = Connection;
            var __param = new DynamicParameters();
            var result = await conn.QueryAsync<AutoCloseDto>("ip_promo_autoclose_list", commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.AsList();
        }

        // config/getreminderlist
        public async Task<IList<ReminderList>> GetReminderList()
        {
            using IDbConnection conn = Connection;
            var result = await conn.QueryAsync<ReminderList>("ip_reminder_list", commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.AsList();
        }

        public async Task<IList<ReminderPendingApproval>> GetReminderPendingApproval()
        {
            List<ReminderPendingApproval> __reminder = new();
            using IDbConnection conn = Connection;
            using (var __obj = await conn.QueryMultipleAsync("[dbo].[ip_reminder_pending_approval_aging]", commandType: CommandType.StoredProcedure, commandTimeout: 180))
            {
                ReminderPendingApproval __result = new()
                {
                    Aging = new List<ReminderPending1>()
                };
                foreach (ReminderPending1 r in __obj.Read<ReminderPending1>())
                    __result.Aging.Add(new ReminderPending1()
                    {
                        num = r.num,
                        category = r.category,
                        UserID = r.UserID,
                        PIC = r.PIC,
                        qty15 = r.qty15,
                        val15 = r.val15,
                        qty610 = r.qty610,
                        val610 = r.val610,
                        qty10 = r.qty10,
                        val10 = r.val10,
                        qtytot = r.qtytot,
                        valtot = r.valtot
                    });
                __result.PendingPromo = new List<ReminderPendingPromo>();
                foreach (ReminderPendingPromo r in __obj.Read<ReminderPendingPromo>())
                    __result.PendingPromo.Add(new ReminderPendingPromo()
                    {
                        promo_id = r.promo_id,
                        last_status = r.last_status,
                        promo_initiator = r.promo_initiator,
                        initiator_name = r.initiator_name,
                        creation_date = r.creation_date,
                        Channel = r.Channel,
                        sub_account = r.sub_account,
                        promo_start = r.promo_start,
                        promo_end = r.promo_end,
                        activity_name = r.activity_name,
                        mechanism_1 = r.mechanism_1,
                        investment = r.investment,
                        aging = r.aging
                    });
                __result.EmailPending = new List<ReminderPendingEmail>();
                foreach (ReminderPendingEmail r in __obj.Read<ReminderPendingEmail>())
                    __result.EmailPending.Add(new ReminderPendingEmail()
                    {
                        email_to = r.email_to,
                        email_cc = r.email_cc
                    });

                __reminder.Add(__result);
            }
            return __reminder.AsList();
        }

        private string generateLinkAttachment(string baseUrl, int promoId, List<string> attachment)
        {
            string path = baseUrl + " /assets/media/promo//<row1 (ini dari db)>/<rawurlencode(file name)>";
            List<string> __res = new List<string>();
            foreach (string link in attachment)
            {
                string[] arr = link.Split(new char[] { '/' });
                __res.Add($"<a href='{baseUrl}/assets/media/promo/{promoId}/{arr[0].Trim()}/{Uri.EscapeDataString(arr[1].Trim())}'>{arr[1].Trim()}</a>");
            }
            return String.Join("<br>", __res);
        }

        private string splitMechanisme(string mecha)
        {
            string[] arr = mecha.Split(new char[] { ';' });
            return String.Join("<br>", arr);
        }
        private string GenerateEmailReminderForApprover(List<PromoSendBackDataReminder> lsPromoForApprover, string baseUrl)
        {
            var sb = new StringBuilder();
            //approver value
            //0 = waiting approval->initiator
            //1 = waiting approval->approver
            //2 = sendback->initiator

            string htmlTemplatePath = "template/email-reminder-approval-approver.html";
            string htmlContent = System.IO.File.ReadAllText(htmlTemplatePath);

            // Loop Data
            int no = 1;
            foreach (var promo in lsPromoForApprover)
            {
                approverParamKey key = new approverParamKey();
                key.promoId = promo.promoId.ToString();
                key.refId = promo.promoRefId.ToString();
                key.nameApprover = lsPromoForApprover.FirstOrDefault().profileName;
                key.profileId = lsPromoForApprover.FirstOrDefault().profileBy;
                key.sy = promo.startPromo.ToString("yyyy");

                string encriptedParam = Encrypt(JsonConvert.SerializeObject(key));
                string urlApprove = baseUrl + "/promo/approval-recon/email/approve?p=" + encriptedParam;
                string urlSendback = baseUrl + "/promo/approval-recon/email/send-back?p=" + encriptedParam;

                sb.AppendLine("<tr>");
                sb.AppendLine($"<td style=\"white-space: nowrap;text-align: center\">{no}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap; text-align: center;\"><div style=\"display: flex; align-items: center; justify-content: center;\"><a href=\"{urlApprove}\" class=\"button\" style=\"margin-right: 3px;\">Approve</a>&nbsp<a href=\"{urlSendback}\" class=\"button\">Send Back</a></div></td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.promoRefId}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.distributor}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.initiatorName}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.subAccount}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.groupBrand}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap; text-align: left;\">{splitMechanisme(promo.mechanism)}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.startPromo.ToString("dd-MM-yyyy")}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.endPromo.ToString("dd-MM-yyyy")}</td>");
                sb.AppendLine($"<td style=\"text-align: right\">{promo.cost.ToString("N2")}</td>");
                sb.AppendLine($"<td style=\"text-align: right\">{promo.cr.ToString("N2")}%</td>");
                sb.AppendLine($"<td style=\"text-align: right\">{promo.roi.ToString("N2")}%</td>");

                // Attachment
                sb.AppendLine($"<td <td style=\"white-space: nowrap;\">{generateLinkAttachment(baseUrl, promo.promoId, promo.lsFileAttachment)}</td>");
                sb.AppendLine("</tr>");
                no++;
            }

            htmlContent = htmlContent.Replace("{{HTMLRowData}}", sb.ToString());
            htmlContent = htmlContent.Replace("{{profileName}}", lsPromoForApprover.FirstOrDefault().profileName);
            return htmlContent;
        }

        private string ToUrlSafeBase64(string base64)
        {
            return base64.Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }
        private string Encrypt(string plaintext, string key = "0123456789abcdef0123456789abcdef",
            string iv = "abcdef0123456789")
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

                return ToUrlSafeBase64(Convert.ToBase64String(encryptedBytes));
            }
        }
        private string GenerateEmailReminderForInitiator(List<PromoSendBackDataReminder> lsPromoForInitiator,
            List<PromoSendBackDataReminder> lsPromoSendbackForInitiator, string baseUrl)
        {
            var sb = new StringBuilder();
            //approver value
            //0 = waiting approval->initiator
            //1 = waiting approval->approver
            //2 = sendback->initiator

            string htmlTemplatePath = "template/email-reminder-approval-initiator.html";
            string htmlContent = System.IO.File.ReadAllText(htmlTemplatePath);

            // Loop Data
            int no = 1;
            foreach (var promo in lsPromoForInitiator)
            {
                sb.AppendLine("<tr>");

                sb.AppendLine($"<td style=\"text-align: center\">{no}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.promoRefId}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.distributor}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.initiatorName}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.subAccount}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.groupBrand}</td>");
                sb.AppendLine($"<td style=\"white-space: nowrap; text-align: left;\">{splitMechanisme(promo.mechanism)}</td>");
                sb.AppendLine($"<td>{promo.startPromo.ToString("dd-MM-yyyy")}</td>");
                sb.AppendLine($"<td>{promo.endPromo.ToString("dd-MM-yyyy")}</td>");
                sb.AppendLine($"<td style=\"text-align: right\">{promo.cost.ToString("N2")}</td>");
                sb.AppendLine($"<td style=\"text-align: right\">{promo.cr.ToString("N2")}%</td>");
                sb.AppendLine($"<td style=\"text-align: right\">{promo.roi.ToString("N2")}%</td>");

                // Attachment
                sb.AppendLine($"<td>{generateLinkAttachment(baseUrl, promo.promoId, promo.lsFileAttachment)}</td>");

                sb.AppendLine("</tr>");
                no++;
            }

            htmlContent = htmlContent.Replace("{{HTMLRowData}}", sb.ToString());
            sb = new StringBuilder();
            // Loop Data
            no = 1;
            if (lsPromoSendbackForInitiator.Count() > 0)
            {
                foreach (var promo in lsPromoSendbackForInitiator)
                {
                    sb.AppendLine("<tr>");

                    sb.AppendLine($"<td style=\"text-align: center\">{no}</td>");
                    sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.promoRefId}</td>");
                    sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.distributor}</td>");
                    sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.initiatorName}</td>");
                    sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.subAccount}</td>");
                    sb.AppendLine($"<td style=\"white-space: nowrap;\">{promo.groupBrand}</td>");
                    sb.AppendLine($"<td style=\"white-space: nowrap; text-align: left;\">{splitMechanisme(promo.mechanism)}</td>");
                    sb.AppendLine($"<td>{promo.startPromo.ToString("dd-MM-yyyy")}</td>");
                    sb.AppendLine($"<td>{promo.endPromo.ToString("dd-MM-yyyy")}</td>");
                    sb.AppendLine($"<td style=\"text-align: right\">{promo.cost.ToString("N2")}</td>");
                    sb.AppendLine($"<td style=\"text-align: right\">{promo.cr.ToString("N2")}%</td>");
                    sb.AppendLine($"<td style=\"text-align: right\">{promo.roi.ToString("N2")}%</td>");

                    sb.AppendLine($"<td>{promo.SendBackReason}</td>");

                    sb.AppendLine("</tr>");
                    no++;
                }
            } else
            {
                sb.AppendLine("<tr>");

                sb.AppendLine($"<td style=\"text-align: center\" colspan=13>No Promo Available</td>");
                sb.AppendLine("</tr>");

            }


            htmlContent = htmlContent.Replace("{{HTMLRowDataSendBack}}", sb.ToString());
            htmlContent = htmlContent.Replace("{{profileName}}", lsPromoForInitiator.FirstOrDefault().profileName);
            return htmlContent;
        }
        public async Task<bool> SendEmailApprovalReminder()
        {
            bool __res = false;
            string GetSettingPromoDeployment = @"
                select setvalue from appsetting 
                where setname='webUrl' 
                order by Id";
            try
            {
                string webUrl = String.Empty;
                //                SchedulerRepo scheduler = new SchedulerRepo(_configuration, __emailRepo);
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var _res2 = await conn.QueryAsync<string>(GetSettingPromoDeployment);
                    string[] settings = _res2.ToArray();
                    webUrl = settings[0];
                }
                var lsPromo = await GetReminderPendingApprovalList();
                lsPromo = lsPromo.Where(x => x.period == 2025).ToList();
                if (lsPromo != null)
                {
                    // get apprrover lsEmail for C1
                    var lsPromoC1 = lsPromo.Where(x => x.promoCycle == 1 && x.approver == 1).ToList();
                    var lsEmail = lsPromoC1.Select(x => x.profileEmail).Distinct().ToList();
                    foreach (var selEmail in lsEmail)
                    {
                        List<string> lsRecepient = new List<string>();
                        lsRecepient.Add(selEmail);
                        //approver value
                        //0 = waiting approval->initiator
                        //1 = waiting approval->approver
                        //2 = sendback->initiator

                        //filter promoc1 by email
                        var lsPromoForApprover = lsPromoC1.ToList().Where(x => x.approver == 1 && x.profileEmail == selEmail).ToList();

                        if (lsPromoForApprover.Count() > 0)
                        {
                            // generate email
                            string email_body = GenerateEmailReminderForApprover(lsPromoForApprover, webUrl);
                            EmailBody body = new EmailBody();
                            body.body = email_body;
                            body.subject = "[APPROVAL REMINDER] Promo Requires Approval";
                            body.email = lsRecepient.ToArray();
                            BGLogger.WriteLog($"SEND [APPROVAL REMINDER] for Approver Cycle 1 to {selEmail}");
                            await __emailRepo.SendEmail(body);
                        }
                        __res = true;
                    }

                    // get initiator lsEmail for C1
                    lsPromoC1 = lsPromo.Where(x => x.promoCycle == 1 && x.approver != 1).ToList();
                    lsEmail = lsPromoC1.Select(x => x.profileEmail).Distinct().ToList();

                    foreach (var selEmail in lsEmail)
                    {
                        List<string> lsRecepient = new List<string>();
                        lsRecepient.Add(selEmail);
                        //lsRecepient.Add("andrie.isharyono@xvautomation.com");
                        //approver value
                        //0 = waiting approval->initiator
                        //1 = waiting approval->approver
                        //2 = sendback->initiator

                        //filter promoc1 by email
                        var lsPromoForInitiator = lsPromoC1.ToList().Where(x => x.approver == 0 && x.profileEmail == selEmail).ToList();
                        var lsPromoSendbackForInitiator = lsPromoC1.ToList().Where(x => x.approver == 2 && x.profileEmail == selEmail).ToList();

                        if (lsPromoForInitiator.Count > 0 || lsPromoSendbackForInitiator.Count > 0)
                        {
                            string email_body = GenerateEmailReminderForInitiator(lsPromoForInitiator, lsPromoSendbackForInitiator, webUrl);
                            EmailBody body = new EmailBody();
                            body.body = email_body;
                            body.subject = "[APPROVAL REMINDER] Promo Requires Approval";
                            body.email = lsRecepient.ToArray();

                            BGLogger.WriteLog($"SEND [APPROVAL REMINDER] for Initiator Cycle 1 to {selEmail}");

                            await __emailRepo.SendEmail(body);
                        }
                        __res = true;
                    }


                    // get apprrover lsEmail for C2
                    var lsPromoC2 = lsPromo.Where(x => x.promoCycle == 2 && x.approver == 1).ToList();
                    lsEmail = lsPromoC2.Select(x => x.profileEmail).Distinct().ToList();
                    foreach (var selEmail in lsEmail)
                    {
                        List<string> lsRecepient = new List<string>();
                        lsRecepient.Add(selEmail);
                        //lsRecepient.Add("andrie.isharyono@xvautomation.com");
                        //approver value
                        //0 = waiting approval->initiator
                        //1 = waiting approval->approver
                        //2 = sendback->initiator

                        //filter promoc1 by email
                        var lsPromoForApprover = lsPromoC2.ToList().Where(x => x.approver == 1 && x.profileEmail == selEmail).ToList();

                        // generate email
                        string email_body = GenerateEmailReminderForApprover(lsPromoForApprover, webUrl);
                        EmailBody body = new EmailBody();
                        body.body = email_body;
                        body.subject = "[APPROVAL REMINDER] Promo Reconciliation Requires Approval";
                        body.email = lsRecepient.ToArray();
                        BGLogger.WriteLog($"SEND [APPROVAL REMINDER] for Approver Cycle 2 to {selEmail}");
                        await __emailRepo.SendEmail(body);
                        __res = true;
                    }

                    // get initiator lsEmail for C2
                    lsPromoC2 = lsPromo.Where(x => x.promoCycle == 2 && x.approver != 1).ToList();
                    lsEmail = lsPromoC2.Select(x => x.profileEmail).Distinct().ToList();
                    foreach (var selEmail in lsEmail)
                    {
                        List<string> lsRecepient = new List<string>();
                        lsRecepient.Add(selEmail);
                        //lsRecepient.Add("andrie.isharyono@xvautomation.com");
                        //approver value
                        //0 = waiting approval->initiator
                        //1 = waiting approval->approver
                        //2 = sendback->initiator

                        //filter promoc1 by email
                        var lsPromoForInitiator = lsPromoC2.ToList().Where(x => x.approver == 0 && x.profileEmail == selEmail).ToList();
                        var lsPromoSendbackForInitiator = lsPromoC2.ToList().Where(x => x.approver == 2 && x.profileEmail == selEmail).ToList();

                        // generate email
                        string email_body = GenerateEmailReminderForInitiator(lsPromoForInitiator, lsPromoSendbackForInitiator, webUrl);
                        EmailBody body = new EmailBody();
                        body.body = email_body;
                        body.subject = "[APPROVAL REMINDER] Promo Reconciliation Requires Approval";
                        body.email = lsRecepient.ToArray();
                        BGLogger.WriteLog($"SEND [APPROVAL REMINDER] for Approver Cycle 2 to {selEmail}");
                        await __emailRepo.SendEmail(body);
                        __res = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return __res;
        }
   

        public async Task<List<PromoSendBackDataReminder>> GetReminderPendingApprovalList()
        {
            using IDbConnection conn = Connection;
            try
            {
                BGLogger.WriteLog("Start Sending Pending Approval Email Reminder");
                var __obj = await conn.QueryAsync<PromoSendBackDataReminder>("[dbo].[ip_reminder_promo_approval]",
                    commandType: CommandType.StoredProcedure, commandTimeout: 180);
                //get email receipient
                //var lsEmail = __obj.ToList().Select(x=>x.profileEmail).ToList();
                //foreach (var s in lsEmail)
                //{
                //    //approver value
                //    //0 = waiting approval->initiator
                //    //1 = waiting approval->approver
                //    //2 = sendback->initiator

                //    var lsPromoForApprover = __obj.ToList().Where(x=>x.approver==1).ToList();
                //    var lsPromoForInitiator = __obj.ToList().Where(x=>x.approver==0).ToList();
                //    var lsPromoSendbackForInitiator = __obj.ToList().Where(x=>x.approver==2).ToList();


                //}
                return __obj.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<BlitzNotif>> BlitzTranferNotif()
        {
            List<BlitzNotif> __blitzNotif = new();
            using IDbConnection conn = Connection;
            try
            {
                var __query = new DynamicParameters();
                using (var __re = await conn.QueryMultipleAsync("[dbo].[ip_blitz_transfer_notif_c]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180))
                {
                    BlitzNotif __result = new()
                    {
                        itemtype = new List<ItemType>()
                    };
                    foreach (ItemType r in __re.Read<ItemType>())
                        __result.itemtype.Add(new ItemType()
                        {
                            type = r.type,
                            code = r.code,
                            desc = r.desc
                        });
                    __result.blitzemail = new List<BlitzEmail>();
                    foreach (BlitzEmail r in __re.Read<BlitzEmail>())
                        __result.blitzemail.Add(new BlitzEmail()
                        {
                            email_to = r.email_to,
                            email_cc = r.email_cc,
                            email_subject = r.email_subject
                        });

                    __blitzNotif.Add(__result);
                }
                return __blitzNotif.AsList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task PromoAutoClose()
        {
            using IDbConnection conn = Connection;
            await conn.ExecuteAsync("ip_promo_closure_auto", commandType: CommandType.StoredProcedure, commandTimeout: 180);
        }

        public async Task<SchedulerStandardResult> CancelPromo(int promoId, string userId, string statusCode, string approverEmail)
        {
            using IDbConnection conn = Connection;
            var __param = new DynamicParameters();
            __param.Add("@promoid", promoId);
            __param.Add("@userid", userId);
            __param.Add("@statuscode", statusCode);
            __param.Add("@ApproverEmail", approverEmail);

            var result = await conn.QueryAsync<SchedulerStandardResult>("ip_promo_cancel_approval", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.FirstOrDefault()!;
        }

        public async Task<SchedulerStandardResult> CancelPromoPlanning(int promoPlanId, string userId, string notes)
        {
            using IDbConnection conn = Connection;
            var __param = new DynamicParameters();
            __param.Add("@promoplanid", promoPlanId);
            __param.Add("@reason", notes);
            __param.Add("@userid", userId);

            var result = await conn.QueryAsync<SchedulerStandardResult>("ip_promo_planning_cancel", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.FirstOrDefault()!;
        }

        public async Task<PromoApprovalReminderRegularSend> GetPromoApprovalReminderRegularSend()
        {
            PromoApprovalReminderRegularSend res = new();
            try
            {
                using IDbConnection conn = Connection;

                var result = await conn.QueryMultipleAsync("ip_tools_promo_approval_reminder_regular_send", commandType: CommandType.StoredProcedure, commandTimeout:180);
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

        public async Task<PromoForScheduler> GetPromoSchedulerById(int id)
        {
            try
            {
                PromoForScheduler __res = new();
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", id);
                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        var __promoHeader = __obj.Read<PromoDisplayData>().FirstOrDefault();
                        var __regions = __obj.Read<PromoRegionRes>().ToList();
                        var __channels = __obj.Read<PromoChannelRes>().ToList();
                        var __subChannels = __obj.Read<PromoSubChannelRes>().ToList();
                        var __accounts = __obj.Read<PromoAccountRes>().ToList();
                        var __subAccounts = __obj.Read<PromoSubAccountRes>().ToList();
                        var __brands = __obj.Read<PromoBrandRes>().ToList();
                        var __products = __obj.Read<PromoProductRes>().ToList();
                        var __activities = __obj.Read<PromoActivityRes>().ToList();
                        var __subActivities = __obj.Read<PromoSubActivityRes>().ToList();
                        var __attachments = __obj.Read<PromoAttachment>().ToList();
                        var __listApprovalStatus = __obj.Read<ApprovalRes>().ToList();
                        var __mechanisms = __obj.Read<MechanismData>().ToList();
                        var __investment = __obj.Read<PromoReconInvestmentData>().ToList();
                        var __groupBrand = __obj.Read<object>().ToList();

                        __res.PromoHeader = __promoHeader!;
                        __res.Regions = __regions;
                        __res.Channels = __channels;
                        __res.SubChannels = __subChannels;
                        __res.Accounts = __accounts;
                        __res.SubAccounts = __subAccounts;
                        __res.Brands = __brands;
                        __res.Skus = __products;
                        __res.Activities = __activities;
                        __res.SubActivities = __subActivities;
                        __res.Attachments = __attachments;
                        __res.ListApprovalStatus = __listApprovalStatus;
                        __res.Mechanisms = __mechanisms;
                        __res.Investments = __investment;
                        __res.GroupBrand = __groupBrand;
                    }
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoReconScheduler> GetPromoReconSchedulerById(int Id)
        {
            try
            {
                PromoReconScheduler __res = new();
                List<PromoReconScheduler> __promoRecon = new();
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", Id);

                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display_recon]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        var __promoHeader = __obj.Read<SchedulerPromoRecon>().FirstOrDefault();
                        var __regions = __obj.Read<PromoRegionRes>().ToList();
                        var __channels = __obj.Read<PromoChannelRes>().ToList();
                        var __subChannels = __obj.Read<PromoSubChannelRes>().ToList();
                        var __accounts = __obj.Read<PromoAccountRes>().ToList();
                        var __subAccounts = __obj.Read<PromoSubAccountRes>().ToList();
                        var __brands = __obj.Read<PromoBrandRes>().ToList();
                        var __products = __obj.Read<PromoProductRes>().ToList();
                        var __activities = __obj.Read<PromoActivityRes>().ToList();
                        var __subActivities = __obj.Read<PromoSubActivityRes>().ToList();
                        var __attachments = __obj.Read<PromoAttachment>().ToList();
                        var __listApprovalStatus = __obj.Read<ApprovalRes>().ToList();
                        var __sKPValidation = __obj.Read<SKPValidation>().FirstOrDefault();
                        var __investment = __obj.Read<PromoReconInvestmentData>().ToList();
                        var __mechanisms = __obj.Read<MechanismData>().ToList();

                        __res.PromoHeader = __promoHeader;
                        __res.Regions = __regions;
                        __res.Channels = __channels;
                        __res.SubChannels = __subChannels;
                        __res.Accounts = __accounts;
                        __res.SubAccounts = __subAccounts;
                        __res.Brands = __brands;
                        __res.Skus = __products;
                        __res.Activity = __activities;
                        __res.SubActivity = __subActivities;
                        __res.Attachments = __attachments;
                        __res.ListApprovalStatus = __listApprovalStatus;
                        __res.SKPValidations = __sKPValidation;
                        __res.Investments = __investment;
                        __res.Mechanisms = __mechanisms;
                    }
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}
