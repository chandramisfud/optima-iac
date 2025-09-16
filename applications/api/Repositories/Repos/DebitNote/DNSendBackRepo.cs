using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class DNSendBackRepo : IDNSendBackRepo
    {
        readonly IConfiguration __config;
        public DNSendBackRepo(IConfiguration config)
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

        // debetnote/cancel
        public async Task<DNGlobalResponse> CancelDN(DNCancelBody body)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@dnid", body.dnid);
                __param.Add("@reason", body.reason);
                __param.Add("@userid", body.userid);


                var result = await conn.QueryAsync<DNGlobalResponse>("ip_debetnote_cancel", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // dnattachment/store/sendback
        public async Task<DNGlobalResponse> CreateDNAttachmentSendback(DNAttachmentBody body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@id", body.DNId);
                __param.Add("@doclink", body.DocLink);
                __param.Add("@filename", body.FileName);
                __param.Add("@createby", body.CreateBy);

                var result = await conn.QueryAsync<DNGlobalResponse>("ip_dn_attachment_store_sendback", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // dnattachment/delete
        public async Task DeleteDNAttachment(DNAttachmentBody body)
        {
            try
            {
                using IDbConnection conn = Connection;
                await conn.ExecuteAsync(@"DELETE FROM tbtrx_dn_doclink where DNId=@id and Filename=@filename"
                , new
                {
                    id = body.DNId,
                    filename = body.FileName
                });
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // debetnote/print/
        public async Task<DNPrint> DNPrint(int id)
        {
            try
            {
                List<DNPrint> __dn = new();
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@id", id);

                using (var __re = await conn.QueryMultipleAsync("ip_debetnote_print", __param, commandType: CommandType.StoredProcedure, commandTimeout:180))
                {
                    DNPrint __result = new();
                    __result = __re.Read<DNPrint>().FirstOrDefault()!;
                    __dn.Add(__result);
                }
                return __dn.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // master/getAttributeByUser
        public async Task<DNDistributorEntity> GetAttributeByUser(string userid)
        {
            try
            {
                List<DNDistributorEntity> __mapp = new();
                using IDbConnection conn = Connection;

                IList<EntityDropDownDto> __entity = new List<EntityDropDownDto>();
                IList<MasterGlobalData> __distributor = new List<MasterGlobalData>();
                IList<MasterGlobalData> __channel = new List<MasterGlobalData>();
                IList<MasterGlobalData> __subchannel = new List<MasterGlobalData>();
                IList<MasterGlobalData> __account = new List<MasterGlobalData>();
                IList<MasterGlobalData> __subaccount = new List<MasterGlobalData>();

                DNDistributorEntity __result = new();

                var __param = new DynamicParameters();
                __param.Add("@userid", userid);

                using (var __re = await conn.QueryMultipleAsync("ip_get_account_byuser", __param, commandType: CommandType.StoredProcedure, commandTimeout:180))
                {
                    __entity = __re.Read<EntityDropDownDto>().ToList();
                    __distributor = __re.Read<MasterGlobalData>().ToList();
                    __channel = __re.Read<MasterGlobalData>().ToList();
                    __subchannel = __re.Read<MasterGlobalData>().ToList();
                    __account = __re.Read<MasterGlobalData>().ToList();
                    __subaccount = __re.Read<MasterGlobalData>().ToList();

                    __result.Entity = __entity;
                    __result.Distributor = __distributor;
                    __result.Channel = __channel;
                    __result.SubChannel = __subchannel;
                    __result.Account = __account;
                    __result.SubAccount = __subaccount;
                }
                return __result;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // debetnote/list/sendbackdist/
        public async Task<object> GetDNSendbackDistributor(string periode, int accountId, string userid, bool isdnmanual,
             string sortColumn, string sortDirection, int length = 10, int start = 0, string? txtSearch = null)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@periode", periode);
                __param.Add("@entity", 0);
                __param.Add("@distributor", 0);
                __param.Add("@channel", 0);
                __param.Add("@account", accountId);
                __param.Add("@userid", userid);
                __param.Add("@isdnmanual", isdnmanual);
                __param.Add("@length", length);
                __param.Add("@start", start);
                __param.Add("@txtSearch", txtSearch);
                __param.Add("@SortColumn", sortColumn);
                __param.Add("@SortDirection", sortDirection);

                var __res = await conn.QueryMultipleAsync("ip_debetnote_list_sendback_distributor", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                List<object> __data = __res.Read<object>().ToList();

                var res = __res.ReadSingle<BaseLP2>();
                res.Data = __data.Cast<object>().ToList();
                return res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // debetnote/getbyId/
        public async Task<DNGetById> GetDNbyIdforDNSendBack(int id)
        {
            try
            {
                DNGetById __res = null!;
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __obj = await conn.QueryMultipleAsync("ip_debetnote_select", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        __res = __obj.Read<DNGetById>().FirstOrDefault()!;
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

        // debetnote/update/sendback
        public async Task<DNCreationReturn> UpdateDNSendback(DNCreationParam body)
        {
            try
            {
                DataTable __sellpo = new();
                DataTable __attachment = _castToDataTable(new DNAttachment(), null!);

                __sellpo.Columns.Add("sellpoint", typeof(string));
                foreach (DNSellpoint v in body.sellpoint!)
                    __sellpo.Rows.Add(v.sellpoint);

                foreach (DNAttachment v in body.dnattachment!)
                    __attachment.Rows.Add(v.FileName, v.DocLink);

                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@Id", body.Id);
                __param.Add("@Periode", body.Periode);
                __param.Add("@EntityId", body.EntityId);
                __param.Add("@DistributorId", body.DistributorId);
                __param.Add("@ActivityDesc", body.ActivityDesc);
                __param.Add("@AccountId", body.AccountId);
                __param.Add("@PromoId", body.PromoId);
                __param.Add("@IntDocNo", body.IntDocNo);
                __param.Add("@MemDocNo", body.MemDocNo);
                __param.Add("@DPP", body.DPP);
                __param.Add("@DNAmount", body.DNAmount);
                __param.Add("@FeeDesc", body.FeeDesc);
                __param.Add("@FeePct", body.FeePct);
                __param.Add("@FeeAmount", body.FeeAmount);
                __param.Add("@PPNPct", body.PPNPct);
                __param.Add("@DeductionDate", body.DeductionDate);
                __param.Add("@userid", body.UserId);
                __param.Add("TaxLevel", body.TaxLevel);
                __param.Add("@sellingpoint", __sellpo.AsTableValuedParameter());
                __param.Add("@attachment", __attachment.AsTableValuedParameter());
                __param.Add("@pphPct", body.pphPct);
                __param.Add("@pphAmt", body.pphAmt);
                __param.Add("@statusPPH", body.statusPPH);
                __param.Add("@FPDate", body.FPDate);
                __param.Add("@FPNumber", body.FPNumber);


                var result = await conn.QueryAsync<DNCreationReturn>("ip_debetnote_update_sendback", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        private DataTable _castToDataTable<T>(T __type, List<T> __contents)
        {
            DataTable datas = new();
            try
            {
                PropertyInfo[] __columns = typeof(T).GetProperties();
                foreach (PropertyInfo v in __columns)
                {
                    if (v.GetCustomAttribute(typeof(System.ComponentModel.DisplayNameAttribute)) is not System.ComponentModel.DisplayNameAttribute __dispName)
                        datas.Columns.Add(v.Name, v.PropertyType);
                    else
                        datas.Columns.Add(__dispName.DisplayName, v.PropertyType);
                }
                if (__contents != null)
                {
                    foreach (var r in __contents)
                    {
                        DataRow __row = datas.NewRow();
                        foreach (PropertyInfo v in __columns)
                        {
                            if (v.GetCustomAttribute(typeof(System.ComponentModel.DisplayNameAttribute)) is not System.ComponentModel.DisplayNameAttribute __dispName)
                                __row[v.Name] = v.GetValue(r);
                            else
                                __row[__dispName.DisplayName] = v.GetValue(r);
                        }
                        datas.Rows.Add(__row);
                    }
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return datas;
        }
    }
}