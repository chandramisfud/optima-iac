using Dapper;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Repositories.Contracts;
using Repositories.Entities;
using Repositories.Entities.BudgetAllocation;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

namespace Repositories.Repos
{
    public  partial class PromoCreationV2Repository : IPromoCreationV2Repository
    {
        readonly IConfiguration __config;
        public PromoCreationV2Repository(IConfiguration config)
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

        public async Task<object> GetPromoCreationAttributeList(string profileId)
        {
            PromoCreationV2AttributeList __res = new();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@profileid", profileId);

                    using (var __resp = await conn.QueryMultipleAsync("ip_promo_creation_add_list", __param,
                        commandType: CommandType.StoredProcedure, commandTimeout: 180))
                    {
                        __res.distibutor = __resp.Read<object>().ToList();
                        __res.entity = __resp.Read<object>().ToList();
                        __res.grpBrand = __resp.Read<object>().ToList();
                        __res.sku = __resp.Read<object>().ToList();
                        __res.category = __resp.Read<object>().ToList();
                        __res.subCategory = __resp.Read<object>().ToList();
                        __res.activity = __resp.Read<object>().ToList();
                        __res.subActivity = __resp.Read<object>().ToList();
                        __res.channel = __resp.Read<object>().ToList();
                        __res.subChannel = __resp.Read<object>().ToList();
                        __res.account = __resp.Read<object>().ToList();
                        __res.subAccount = __resp.Read<object>().ToList();
                        __res.region = __resp.Read<object>().ToList();
                        __res.configCalculator = __resp.Read<object>().ToList();

                    }
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __res;
        }

        public async Task<object> GetPromoCreationMechanismWithStatus(int entityId, int subCategoryId, 
            int activityId, int subActivityId, int skuId, int channelId, int brandId, string startFrom, string startTo)
        {

            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@entityid", entityId);
                    __param.Add("@subcategoryid", subCategoryId);
                    __param.Add("@activityid", activityId);
                    __param.Add("@subactivityid", subActivityId);
                    __param.Add("@productid", skuId);
                    __param.Add("@channelid", channelId);
                    __param.Add("@brandid", brandId);
                    __param.Add("@startdate", startFrom);
                    __param.Add("@enddate", startTo);

                    var p = await conn.QueryMultipleAsync("[dbo].[ip_mst_mechanism_get]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    return new
                    {
                        mechanism = p.Read<MechanisSourceDto>().ToList(),
                        mechanismeAvailable = p.Read<bool>().First()

                    };

                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoCreationSSValue(int period, int channelid, int subchannelid, int accountid, 
            int subaccountid, int groupbrandid, DateTime promostart, DateTime promoend)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@period", period);
                    __param.Add("@channelid", channelid);
                    __param.Add("@subchannelid", subchannelid);
                    __param.Add("@accountid", accountid);
                    __param.Add("@subaccountid", subaccountid);
                    __param.Add("@groupbrandid ", groupbrandid);
                    __param.Add("@promostart ", promostart);
                    __param.Add("@promoend ", promoend);

                  
                    return await conn.QueryAsync<object>("[dbo].[ip_ss_ssvalue_promoid_get]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoCreationPSValue(int period, int distributorId, int groupBrandId,
            DateTime promoStart, DateTime promoEnd)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@period", period);
                    __param.Add("@distributorId", distributorId);
                    __param.Add("@groupbrandid", groupBrandId);
                    __param.Add("@promostart", promoStart);
                    __param.Add("@promoend", promoEnd);

                  
                    return await conn.QueryAsync<object>("ip_ps_value_promoid_get", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetPromoCreationBaseline(int promoid, int period, DateTime date, int pType, int distributor, 
         int[] region, int channel, int subChannel, int account, int subaccount, 
         int[] product, int subCategory, int subActivity, int grpBrand, DateTime promostart, DateTime promoend)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@p_promoid", promoid);
                    __param.Add("@p_period", period);
                    __param.Add("@p_date", date);
                    __param.Add("@p_type", pType);
                    __param.Add("@p_distributor", distributor);

                    DataTable dtregion = new("AttributeType");
                    dtregion.Columns.Add("id");
                    if (region != null)
                    {
                        foreach (var item in region)
                        {
                            dtregion.Rows.Add(item);
                        }
                    }
                    else
                    {
                        dtregion.Rows.Add(0);
                    }
                    __param.Add("@p_region", dtregion.AsTableValuedParameter());
                    __param.Add("@p_channel", channel);
                    __param.Add("@p_subchannel", subChannel);
                    __param.Add("@p_account", account);
                    __param.Add("@p_subaccount", subaccount);

                    DataTable dtproduct = new("AttributeType");
                    dtproduct.Columns.Add("id");
                    if (product != null)
                    {
                        foreach (var item in product)
                        {
                            dtproduct.Rows.Add(item);
                        }
                    }
                    else
                    {
                        dtproduct.Rows.Add(0);
                    }
                    __param.Add("@p_product", dtproduct.AsTableValuedParameter());
                    __param.Add("@p_subCategory", subCategory);
                    __param.Add("@p_subActivity", subActivity);
                    __param.Add("@p_groupbrand", grpBrand);
                    __param.Add("@p_startpromo", promostart);
                    __param.Add("@p_endpromo", promoend);


                    return await conn.QueryAsync<object>("[dbo].[ip_promo_baseline_get]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoCreationCR(int period, int subactivityid, int subaccountid, int distributorid,
           int groupbrandid)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@period", period);
                    __param.Add("@subactivityid", subactivityid);
                    __param.Add("@subaccountid", subaccountid);
                    __param.Add("@distributorid", distributorid);                
                    __param.Add("@groupbrandid ", groupbrandid);
              
                    return await conn.QueryAsync<object>("[dbo].[ip_promo_cr_get]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> SetPromoCreationInsert(DataTable promo, DataTable region, DataTable sku, 
            DataTable attachment, DataTable mechanism )
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@IsNew", true);
                    __param.Add("@promo", promo.AsTableValuedParameter());
                    __param.Add("@region", region.AsTableValuedParameter());
                    __param.Add("@sku", sku.AsTableValuedParameter());
                    __param.Add("@attachment", attachment.AsTableValuedParameter());
                    __param.Add("@Mechanism", mechanism.AsTableValuedParameter());

                    var _qryRes = await conn.QueryMultipleAsync("[dbo].[ip_promo_insert]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    promoCreationResult _res = _qryRes.Read<promoCreationResult>().First();
                    if (_res.isSendEmail)
                    {
                        _res.dataEmail = _qryRes.Read<object>().First();
                    }
                    return _res;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> SetPromoCreationInsertDC(DataTable promo, DataTable region, DataTable sku,
          DataTable attachment, DataTable mechanism)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@IsNew", true);
                    __param.Add("@promo", promo.AsTableValuedParameter());
                    __param.Add("@region", region.AsTableValuedParameter());
                    __param.Add("@sku", sku.AsTableValuedParameter());
                    __param.Add("@attachment", attachment.AsTableValuedParameter());
                    __param.Add("@Mechanism", mechanism.AsTableValuedParameter());

                    var _qryRes = await conn.QueryMultipleAsync("[dbo].[ip_promo_insert_DC]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    promoCreationResult _res = _qryRes.Read<promoCreationResult>().First();
                    if (_res.isSendEmail)
                    {
                        _res.dataEmail = _qryRes.Read<object>().First();
                    }
                    return _res;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> SetPromoCreationUpdate(DataTable promo, DataTable region, DataTable sku,
           DataTable attachment, DataTable mechanism)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    //__param.Add("@IsNew", false);
                    __param.Add("@promo", promo.AsTableValuedParameter());
                    __param.Add("@region", region.AsTableValuedParameter());
                    __param.Add("@sku", sku.AsTableValuedParameter());
                    __param.Add("@attachment", attachment.AsTableValuedParameter());
                    __param.Add("@mechanism", mechanism.AsTableValuedParameter());

                    var _qryRes = await conn.QueryMultipleAsync("[dbo].[ip_promo_update]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    promoCreationResult _res = _qryRes.Read<promoCreationResult>().First();
                    if (_res.isSendEmail)
                    {
                        _res.dataEmail = _qryRes.Read<object>().First();
                    }
                    return _res;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> SetPromoCreationUpdateDC(DataTable promo, DataTable region, DataTable sku,
         DataTable attachment, DataTable mechanism)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    //__param.Add("@IsNew", false);
                    __param.Add("@promo", promo.AsTableValuedParameter());
                    __param.Add("@region", region.AsTableValuedParameter());
                    __param.Add("@sku", sku.AsTableValuedParameter());
                    __param.Add("@attachment", attachment.AsTableValuedParameter());
                    __param.Add("@mechanism", mechanism.AsTableValuedParameter());

                    var _qryRes = await conn.QueryMultipleAsync("[dbo].[ip_promo_update_DC]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    promoCreationResult _res = _qryRes.Read<promoCreationResult>().First();
                    if (_res.isSendEmail)
                    {
                        _res.dataEmail = _qryRes.Read<object>().First();
                    }
                    return _res;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetPromoCreationById(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var result = new
                    {
                        promo = __resp.Read<object>().First(),
                        region = __resp.Read<object>().ToList(),                    
                        sku = __resp.Read<object>().ToList(),                      
                        attachment = __resp.Read<object>().ToList(),
                        promoStatus = __resp.Read<object>().ToList(),
                        mechanism = __resp.Read<object>().ToList()
                    };

                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoCreationDCById(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data_DC]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var result = new
                    {
                        promo = __resp.Read<object>().First(),
                        region = __resp.Read<object>().ToList(),
                        sku = __resp.Read<object>().ToList(),
                        attachment = __resp.Read<object>().ToList(),
                        promoStatus = __resp.Read<object>().ToList(),
                        mechanism = __resp.Read<object>().ToList()
                    };

                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public Task SendEmail(Entities.Dtos.EmailBody emailBodyDto)
        {
            try
            {
               var _emailSettings = new EmailSettings
                {
                    MailServer = __config.GetSection("EmailSettings:MailServer").Value,
                    Sender = __config.GetSection("EmailSettings:Sender").Value,
                    Password = __config.GetSection("EmailSettings:Password").Value,
                    MailPort = Convert.ToInt16(__config.GetSection("EmailSettings:MailPort").Value),
                    SenderName = __config.GetSection("EmailSettings:SenderName").Value
                };

                // cek user name from tbset_user
                using IDbConnection conn = Connection;
                string qry = "SELECT username FROM tbset_user" +
                    " WHERE email=@email" +
                    " AND ISNULL(isdeleted,0)=0";

                InternetAddressList listEmail = new();
                foreach (var e in emailBodyDto.email!)
                {
                    var __res = conn.Query(qry, new { email = e });
                    string userName = __res.FirstOrDefault()!.username;
                    listEmail.Add(new MailboxAddress(userName, e.Trim()));
                }
                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(_emailSettings.Sender, _emailSettings.Sender));
                mail.To.AddRange(listEmail);
                mail.Subject = emailBodyDto.subject;
                if (emailBodyDto.cc != null)
                {
                    InternetAddressList listCC = new();
                    foreach (var cc in emailBodyDto.cc)
                    {
                        if (!String.IsNullOrEmpty(cc))
                        {
                            var __res = conn.Query(qry, new { email = cc });
                            string userName = __res.FirstOrDefault()!.UserName;
                            listCC.Add(new MailboxAddress(userName, cc.Trim()));
                        }
                    }
                    mail.Cc.AddRange(listCC);
                }
                if (emailBodyDto.bcc != null)
                {
                    InternetAddressList listBCC = new();
                    foreach (var bcc in emailBodyDto.bcc)
                    {
                        if (!String.IsNullOrEmpty(bcc))
                        {
                            var __res = conn.Query(qry, new { email = bcc });
                            string userName = __res.FirstOrDefault()!.UserName;

                            listBCC.Add(new MailboxAddress(userName, bcc.Trim()));
                        }
                    }
                    mail.Bcc.AddRange(listBCC);
                }
                var builder = new BodyBuilder();
                if (emailBodyDto.attachment != null)
                {
                    byte[] fileBytes;
                    foreach (var file in emailBodyDto.attachment)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, MimeKit.ContentType.Parse(file.ContentType));
                        }
                    }
                }

                builder.HtmlBody = emailBodyDto.body;
                mail.Body = builder.ToMessageBody();
                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect(_emailSettings.MailServer,
                               _emailSettings.MailPort,
                               SecureSocketOptions.StartTls);
                client.Authenticate(_emailSettings.Sender, _emailSettings.Password);
                client.Send(mail);
                client.Disconnect(true);
            }
            catch (Exception __ex)
            {
                throw new InvalidOperationException(__ex.Message);
            }

            return Task.CompletedTask;
        }
      

        public async Task<object> GetPromoDisplayById(int id, string profile="")
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);
                    __param.Add("@profile", profile);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data_display]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var result = new
                    {
                        promo = __resp.Read<object>().First(),
                        region = __resp.Read<object>().ToList(),
                        sku = __resp.Read<object>().ToList(),
                        attachment = __resp.Read<object>().ToList(),
                        promoStatus = __resp.Read<object>().ToList(),
                        mechanism = __resp.Read<object>().ToList(),
                        workflow = __resp.Read<object>().ToList(),
                        dn = __resp.ReadSingle<object>(),
                        calculatorRecon = __resp.Read<object>().ToList()
                };

                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<PromoDisplayList> GetPromoDisplayByIdForSendEmail(int id)
        {
            try
            {
                PromoDisplayList __res = new();
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", id);
                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    {
                        __res.PromoHeader = __obj.Read<PromoDisplayData>().FirstOrDefault();
                        __res.Regions = __obj.Read<PromoRegionRes>().ToList();
                        __res.Channels = __obj.Read<PromoChannelRes>().ToList();
                        __res.SubChannels = __obj.Read<PromoSubChannelRes>().ToList();
                        __res.Accounts = __obj.Read<PromoAccountRes>().ToList();
                        __res.SubAccounts = __obj.Read<PromoSubAccountRes>().ToList();
                        __res.Brands = __obj.Read<PromoBrandRes>().ToList();
                        __res.Skus = __obj.Read<PromoProductRes>().ToList();
                        __res.Activities = __obj.Read<PromoActivityRes>().ToList();
                        __res.SubActivities = __obj.Read<PromoSubActivityRes>().ToList();
                        __res.Attachments = __obj.Read<PromoAttachment>().ToList();
                        __res.ListApprovalStatus = __obj.Read<ApprovalRes>().ToList();
                        __res.Mechanisms = __obj.Read<MechanismData>().ToList();
                        __res.Investments = __obj.Read<PromoReconInvestmentData>().ToList();
                        __res.GroupBrand = __obj.Read<object>().ToList();                  
                      
                    }
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoDisplayEmailById(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data_display_recon]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var result = new
                    {
                        promo = __resp.Read<object>().First(),
                        region = __resp.Read<object>().ToList(),
                        sku = __resp.Read<object>().ToList(),
                        attachment = __resp.Read<object>().ToList(),
                        promoStatus = __resp.Read<object>().ToList(),
                        mechanism = __resp.Read<object>().ToList(),
                        prevMechanism = __resp.Read<object>().ToList(),
                        workflow = __resp.Read<object>().ToList()
                    };

                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetPromoReconDisplayById(int id, string profile="")
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);
                    __param.Add("@profile", profile);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data_display_recon]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var _promo = __resp.ReadSingleOrDefault<object>();
                    var _region = __resp.Read<object>().ToList();
                    var _sku = __resp.Read<object>().ToList();
                    var _attachment = __resp.Read<object>().ToList();
                    var _promoStatus = __resp.Read<object>().ToList();
                    var _mechanism = __resp.Read<object>().ToList();
                    var _prevMechanism = __resp.Read<object>().ToList();
                    var _workflow = __resp.Read<object>().ToList();

                    // this part not used on dispay
                    var resultNotUsed = new
                    {
                        promo = __resp.ReadSingleOrDefault<object>(),
                        region = __resp.Read<object>().ToList(),
                        sku = __resp.Read<object>().ToList(),
                        attachment = __resp.Read<object>().ToList(),
                        promoStatus = __resp.Read<object>().ToList(),
                        mechanism = __resp.Read<object>().ToList(),
                        workflow = __resp.Read<object>().ToList(),
                    };

                    var _calculatorRecon = __resp.Read<object>().ToList();

                    // DN Part
                    var _DNTotal = __resp.ReadSingleOrDefault<dynamic>();
                    var _DNClaim = __resp.Read<object>().ToList();
                    var _DNPaid = __resp.Read<object>().ToList();

                    var result = new
                    {
                        promo = _promo,
                        region = _region,
                        sku = _sku,
                        attachment = _attachment,
                        promoStatus = _promoStatus,
                        mechanism = _mechanism,
                        prevMechanism = _prevMechanism,
                        workflow = _workflow,
                        calculatorRecon = _calculatorRecon,
                        dnClaim = new
                        {
                            total = _DNTotal.totalClaim,
                            listDN = _DNClaim
                        },
                        dnPaid = new
                        {
                            total = _DNTotal.totalPaid,
                            listDN = _DNPaid
                        }
                    };
                    
                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoDisplayDCById(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data_display_DC]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var result = new
                    {
                        promo = __resp.ReadSingleOrDefault<object>(),
                        region = __resp.Read<object>().ToList(),
                        sku = __resp.Read<object>().ToList(),
                        attachment = __resp.Read<object>().ToList(),
                        promoStatus = __resp.Read<object>().ToList(),
                        mechanism = __resp.Read<object>().ToList(),
                    };

                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoReconDisplayDCById(int id, string profile="")
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);
                    __param.Add("@profile", profile);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data_display_recon_DC]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var _promo = __resp.ReadSingleOrDefault<object>();
                    var _region = __resp.Read<object>().ToList();
                    var _sku = __resp.Read<object>().ToList();
                    var _attachment = __resp.Read<object>().ToList();
                    var _promoStatus = __resp.Read<object>().ToList();
                    var _mechanism = __resp.Read<object>().ToList();


                    // this part not used on dispay
                    var resultNotUsed = new
                    {
                        mechanism = __resp.Read<object>().ToList(),
                        workflow1 = __resp.Read<object>().ToList(),
                        promo = __resp.ReadSingleOrDefault<object>(),
                        region = __resp.Read<object>().ToList(),
                        sku = __resp.Read<object>().ToList(),
                        attachment = __resp.Read<object>().ToList(),
                        promoStatus = __resp.Read<object>().ToList(),
                        mechanism1 = __resp.Read<object>().ToList(),
                        workflow = __resp.Read<object>().ToList()
                    };

                    var _DNTotal = __resp.ReadSingleOrDefault<dynamic>();
                    var _DNClaim = __resp.Read<object>().ToList();
                    var _DNPaid = __resp.Read<object>().ToList();

                    var result = new
                    {
                        promo = _promo,
                        region = _region,
                        sku = _sku,
                        attachment = _attachment,
                        promoStatus = _promoStatus,
                        mechanism = _mechanism,
                        dnClaim = new
                        {
                            total = _DNTotal.totalClaim,
                            listDN = _DNClaim
                        },
                        dnPaid = new
                        {
                            total = _DNTotal.totalPaid,
                            listDN = _DNPaid
                        }
                    };
                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoBudget(int period, int categoryId, int subCategoryId, int channelId,
            int subChannelId, int accountId, int subAccountId, int distributorId, int groupBrandId, int subActivityTypeId,
            int activityId, int subActivityId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@period", period);
                    __param.Add("@categoryId", categoryId);
                    __param.Add("@subCategoryId", subCategoryId);
                    __param.Add("@channelId", channelId);
                    __param.Add("@subChannelId", subChannelId);
                    __param.Add("@accountId", accountId);
                    __param.Add("@subAccountId", subAccountId);
                    __param.Add("@distributorId", distributorId);
                    __param.Add("@groupBrandId", groupBrandId);
                    __param.Add("@subActivityTypeId", subActivityTypeId);
                    __param.Add("@activityId", activityId);
                    __param.Add("@subActivityId", subActivityId);

                    return await conn.QueryAsync<object>("[dbo].[ip_promo_budget_get]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoDisplayWorkflow(string refid)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@refid", refid);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data_display_workflow]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var result = new
                    {
                        promo = __resp.Read<object>().First(),
                        region = __resp.Read<object>().ToList(),
                        sku = __resp.Read<object>().ToList(),
                        attachment = __resp.Read<object>().ToList(),
                        promoStatus = __resp.Read<object>().ToList(),
                        mechanism = __resp.Read<object>().ToList(),
                        prevMechanism = __resp.Read<object>().ToList(),
                        dn = __resp.ReadSingleOrDefault<object>()
                    };

                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoDisplayWorkflowpdf(int id, string profile)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);
                    __param.Add("@profile", profile);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data_display_recon]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                    var _promo = __resp.ReadSingleOrDefault<object>();
                    var _region = __resp.Read<object>().ToList();
                    var _sku = __resp.Read<object>().ToList();
                    var _attachment = __resp.Read<object>().ToList();
                    var _promoStatus = __resp.Read<object>().ToList();
                    var _mechanism = __resp.Read<object>().ToList();
                    var _prevMechanism = __resp.Read<object>().ToList();
                    var _workflow = __resp.Read<object>().ToList();


                    var _promoCreation = new
                    {
                        promo = __resp.ReadSingleOrDefault<object>(),
                        region = __resp.Read<object>().ToList(),
                        sku = __resp.Read<object>().ToList(),
                        attachment = __resp.Read<object>().ToList(),
                        promoStatus = __resp.Read<object>().ToList(),
                        mechanism = __resp.Read<object>().ToList(),
                        workflow = __resp.Read<object>().ToList()
                    };

                    var _calculatorRecon = __resp.Read<object>().ToList();

                    var _promoRecon = new
                    {
                        promo = _promo,
                        region = _region,
                        sku = _sku,
                        attachment = _attachment,
                        promoStatus = _promoStatus,
                        mechanism = _mechanism,
                        prevMechanism = _prevMechanism,
                        workflow = _workflow,
                        calculatorRecon = _calculatorRecon
                    };

                    var dataPromoCreation = _promoCreation.promo;
                    var result = new Object();
                    if (dataPromoCreation == null)
                    {
                        result = new
                        {
                            promoCreation = _promoRecon,
                            promoRecon = dataPromoCreation
                        };
                    } else
                    {
                        result = new
                        {
                            promoCreation = _promoCreation,
                            promoRecon = _promoRecon
                        };
                    }

                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}