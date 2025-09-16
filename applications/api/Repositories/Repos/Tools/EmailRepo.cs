using System.Data;
using System.Data.SqlClient;
using Dapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using System.Net.Mail;

namespace Repositories.Repos
{
    public class ToolsEmailRepository : IToolsEmailRepository
    {
        readonly IConfiguration __config;
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }
        private readonly Entities.Dtos.EmailSettings _emailSettings;
        public ToolsEmailRepository(IConfiguration config)
        {
            __config = config;
            _emailSettings = new EmailSettings
            {
                MailServer = __config.GetSection("EmailSettings:MailServer").Value,
                Sender = __config.GetSection("EmailSettings:Sender").Value,
                Password = __config.GetSection("EmailSettings:Password").Value,
                MailPort = Convert.ToInt16(__config.GetSection("EmailSettings:MailPort").Value),
                SenderName = __config.GetSection("EmailSettings:SenderName").Value
            };

        }

        public async Task<ResendEmailApproval> resendEmailApproval()
        {
            ResendEmailApproval __res = new ResendEmailApproval();
            using (IDbConnection conn = Connection)
            {
                var res = await conn.QueryMultipleAsync("xva_resend_email_approval", commandType: CommandType.StoredProcedure, commandTimeout: 180);
                if (res != null)
                {
                    __res.dataCycle1 = res.Read<object>().ToList();
                    __res.dataCycle2 = res.Read<object>().ToList();
                }
                return __res;
            }

        }

        public async Task<IList<EmailResult>> GetEmailConfig(EmailBodyReq body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@id", body.id);
                __param.Add("@param", body.param);
                var result = await conn.QueryAsync<EmailResult>("ip_set_config_email", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();

            }
            catch (Exception __ex)
            {
                throw new InvalidOperationException(__ex.Message);
            }
        }


        public Task SendEmail(Entities.Dtos.EmailBody emailBodyDto)
        {
            try
            {

                if (_emailSettings == null)
                {
                    throw new InvalidOperationException("Email settings not configured");
                }

                if (string.IsNullOrEmpty(_emailSettings.MailServer) ||
                    string.IsNullOrEmpty(_emailSettings.Sender) ||
                    string.IsNullOrEmpty(_emailSettings.Password))
                {
                    throw new InvalidOperationException("Email settings are incomplete");
                }

                if (emailBodyDto?.email == null || !emailBodyDto.email.Any())
                {
                    throw new ArgumentException("Email recipients are required");
                }

                // cek user name from tbset_user
                using IDbConnection conn = Connection;
                string qry = "SELECT username FROM tbset_user" +
                    " WHERE email=@email" +
                    " AND ISNULL(isdeleted,0)=0";

                InternetAddressList listEmail = new();
                foreach (var e in emailBodyDto.email)
                {
                    if (string.IsNullOrEmpty(e)) continue;

                    var userResult = conn.Query(qry, new { email = e }).FirstOrDefault();
                    string userName = userResult?.username ?? e;
                    listEmail.Add(new MailboxAddress(userName, e.Trim()));
                }

                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(_emailSettings.Sender, _emailSettings.Sender));
                mail.To.AddRange(listEmail);
                mail.Subject = emailBodyDto.subject ?? "No Subject";

                if (emailBodyDto.cc != null && emailBodyDto.cc.Any())
                {
                    InternetAddressList listCC = new();
                    foreach (var cc in emailBodyDto.cc)
                    {
                        if (string.IsNullOrEmpty(cc)) continue;

                        var userResult = conn.Query(qry, new { email = cc }).FirstOrDefault();
                        string userName = userResult?.username ?? cc;
                        listCC.Add(new MailboxAddress(userName, cc.Trim()));
                    }
                    if (listCC.Any())
                    {
                        mail.Cc.AddRange(listCC);
                    }
                }

                // Handle BCC
                if (emailBodyDto.bcc != null && emailBodyDto.bcc.Any())
                {
                    InternetAddressList listBCC = new();
                    foreach (var bcc in emailBodyDto.bcc)
                    {
                        if (string.IsNullOrEmpty(bcc)) continue;

                        var userResult = conn.Query(qry, new { email = bcc }).FirstOrDefault();
                        string userName = userResult?.username ?? bcc;
                        listBCC.Add(new MailboxAddress(userName, bcc.Trim()));
                    }
                    if (listBCC.Any())
                    {
                        mail.Bcc.AddRange(listBCC);
                    }
                }

                var builder = new BodyBuilder();

                // Handle attachments
                if (emailBodyDto.attachment != null && emailBodyDto.attachment.Any())
                {
                    foreach (var file in emailBodyDto.attachment)
                    {
                        if (file != null && file.Length > 0 && !string.IsNullOrEmpty(file.FileName))
                        {
                            using var ms = new MemoryStream();
                            file.CopyTo(ms);
                            byte[] fileBytes = ms.ToArray();

                            var contentType = !string.IsNullOrEmpty(file.ContentType)
                                ? MimeKit.ContentType.Parse(file.ContentType)
                                : new MimeKit.ContentType("application", "octet-stream");

                            builder.Attachments.Add(file.FileName, fileBytes, contentType);
                        }
                    }
                }

                builder.HtmlBody = emailBodyDto.body ?? "";
                mail.Body = builder.ToMessageBody();

                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect(_emailSettings.MailServer,
                               _emailSettings.MailPort,
                               SecureSocketOptions.StartTls);
                client.Authenticate(_emailSettings.Sender, _emailSettings.Password);
                client.Send(mail);
                client.Disconnect(true);
            }
            catch (SmtpException smtpEx)
            {
                throw new SmtpException($"SMTP Error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Email sending failed: {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }
}
