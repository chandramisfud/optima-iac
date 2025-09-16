using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNInvoiceNotificationByDanoneRepo : IDNInvoiceNotificationByDanoneRepo
    {
        readonly IConfiguration __config;
        public DNInvoiceNotificationByDanoneRepo(IConfiguration config)
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

        public async Task DeletePromoAttachmentDNInvoiceNotification(int PromoId, string DocLink)
        {
            try
            {
                using IDbConnection conn = Connection;
                await conn.ExecuteAsync(@"DELETE FROM tbtrx_promo_doclink where PromoId=@PromoId and DocLink=@DocLink"
                , new
                {
                    PromoId = PromoId,
                    DocLink = DocLink

                });
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DNChangeStatusInvoiceNotif> DNChangeStatusReadytoInvoice(
            string status,
            string userid,
            List<DNId> dnid
        )
        {
            try
            {
                DataTable __dn = new();

                __dn.Columns.Add("keyid", typeof(int));
                foreach (DNId v in dnid)
                    __dn.Rows.Add(v.dnid);

                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@status", status);
                __param.Add("@userid", userid);
                __param.Add("@id", __dn.AsTableValuedParameter());

                var result = await conn.QueryAsync<DNChangeStatusInvoiceNotif>("ip_dn_changestatus", __param, commandTimeout: 180, commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DNRejectInvoiceNotificationGlobalResponse> DNRejectInvoiceNotification(int dnid, string reason, string userid)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@dnid", dnid);
                __param.Add("@reason", reason);
                __param.Add("@userid", userid);

                var result = await conn.QueryAsync<DNRejectInvoiceNotificationGlobalResponse>("ip_debetnote_reject_paralel", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNInvoiceNotificationbyDanoneDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
        {
            try
            {
                using IDbConnection conn = Connection;
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __query = new DynamicParameters();

                __query.Add("@budgetid", budgetid);
                __query.Add("@attribute", "distributor");
                __query.Add("@parent", __parent.AsTableValuedParameter());


                conn.Open();
                var child = await conn.QueryAsync<DNInvoiceNotificationbyDanoneDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DNGetbyIdforInvoiceNotification> GetDNbyIdInvoiceNotification(int id)
        {
            try
            {
                DNGetbyIdforInvoiceNotification __res = null!;
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __obj = await conn.QueryMultipleAsync("ip_debetnote_select", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        __res = __obj.Read<DNGetbyIdforInvoiceNotification>().FirstOrDefault()!;
                        if (__res == default)
                        {
                            return __res = null!;
                        }
                        var __sellPoint = __obj.Read<DNSellpoint>().ToList();
                        var __dnAttachment = __obj.Read<DNAttachment>().ToList();
                        var __dnDocCompletness = __obj.Read<DNDocCompleteness>().ToList();

                        __res.sellpoint = __sellPoint;
                        __res.dnattachment = __dnAttachment;
                        __res.DNDocCompletenessHeader = __dnDocCompletness;
                    }
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNDto>> GetDNValidatebyDanone(string status, string userid, int entityid, int distributorid)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@status", status);
                __param.Add("@userid", userid);
                __param.Add("@entityid", entityid);
                __param.Add("@distributorid", distributorid);

                var result = await conn.QueryAsync<DNDto>("ip_debetnote_list_bystatus", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNInvoiceNotificationbyDanoneEntityList>> GetEntityList()
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<DNInvoiceNotificationbyDanoneEntityList>(__query);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}