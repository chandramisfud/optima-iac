using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Report;

namespace Repositories.Repos
{
    public class DNCreationRepo : IDNCreationRepo
    {
        readonly IConfiguration __config;
        public DNCreationRepo(IConfiguration config)
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
        // debetnote_p/list/
        public async Task<DNLandingPage> GetDNListLandingPage(
             string period,
            int entityId,
            int distributorId,
            int channelId,
            int accountId,
            string profileId,
            bool isdnmanual,
            string search,
            string sortColumn,
            int pageNum = 0,
            int dataDisplayed = 10,
            string sortDirection = "ASC"
        )
        {
            try
            {
                DNLandingPage res = new();
                using IDbConnection conn = Connection;
                var strData = pageNum * dataDisplayed;
                search = (search) ?? "";
                var __param = new DynamicParameters();
                __param.Add("@periode", period);
                __param.Add("@entity", entityId);
                __param.Add("@distributor", distributorId);
                __param.Add("@channel", channelId);
                __param.Add("@account", accountId);
                __param.Add("@userid", profileId);
                __param.Add("@isdnmanual", isdnmanual);
                __param.Add("@start", strData);
                __param.Add("@length", dataDisplayed);
                __param.Add("@filter", "");
                __param.Add("@txtsearch", search);

                var __object = await conn.QueryMultipleAsync("ip_debetnote_list_p", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                {
                    var data = __object.Read<DNList>().ToList();
                    var count = __object.ReadSingle<DNRecord>();

                    res.totalCount = count.recordsTotal;
                    res.filteredCount = count.recordsFiltered;

                    res.Data = data;

                }
                return res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // debetnote/getbyId
        public async Task<DNGetById> GetDebetNoteById(int id)
        {
            try
            {
                DNGetById __res = null!;
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __obj = await conn.QueryMultipleAsync("ip_debetnote_select", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
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

                var result = await conn.QueryAsync<DNGlobalResponse>("ip_debetnote_cancel", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
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

                using (var __re = await conn.QueryMultipleAsync("ip_get_account_byuser", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180))
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
        // promo/getPromoForDn
        public async Task<IList<DNApprovedPromoList>> GetApprovedPromoforDNCreation(string periode, int entity, int channel, int account, string userid)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@periode", periode);
                __param.Add("@entity", entity);
                __param.Add("@channel", channel);
                __param.Add("@account", account);
                __param.Add("@userid", userid);

                var result = await conn.QueryAsync<DNApprovedPromoList>("ip_promo_approved_list_for_dn", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // master/getDistEntityByUser/
        public async Task<DNDistributorEntity> GetDistributorEntityByUserId(string userid)
        {
            try
            {
                DNDistributorEntity __debetnote = new();
                using IDbConnection conn = Connection;

                IList<EntityDropDownDto> __entity = new List<EntityDropDownDto>();
                IList<MasterGlobalData> __distributor = new List<MasterGlobalData>();
                DNDistributorEntity __result = new();

                var __param = new DynamicParameters();
                __param.Add("@userid", userid);

                using (var __re = await conn.QueryMultipleAsync("ip_getdistentity_byuser", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180))
                {
                    __entity = __re.Read<EntityDropDownDto>().ToList();
                    __distributor = __re.Read<MasterGlobalData>().ToList();
                    __result.Entity = __entity;
                    __result.Distributor = __distributor;

                }
                return __result;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // sellingpoint/getByUser
        public async Task<IEnumerable<DNSellingPoint>> GetSellingPointByUser(string userid)
        {
            try
            {
                IEnumerable<DNSellingPoint> __res;
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@userid", userid);
                    __res = await conn.QueryAsync<DNSellingPoint>("[dbo].[ip_select_sellpoint_byuser]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //  mapmaterial/all
        public async Task<IEnumerable<DNMaterial>> GetTaxLevel()
        {
            try
            {
                IEnumerable<DNMaterial> __res;
                using (IDbConnection conn = Connection)
                {
                    var __query = @"SELECT * FROM [dbo].[tbset_mapping_material] ";
                    __res = await conn.QueryAsync<DNMaterial>(__query);
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // dncreation/taxlevel?entityid
        public async Task<List<DNCreationTaxLevel>> DNCreationTaxLevel(string entityid, string whtType="")
        {
            try
            {
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var __param = new DynamicParameters();
                __param.Add("@entityid", entityid);
                __param.Add("@whtType", whtType);
                var __res = await conn.QueryAsync<DNCreationTaxLevel>("[ip_get_tax_level]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // debetnote/store
        public async Task<DNStoreResultSearch> GetDebetnoteStoreValidate(DNCreationParam body)
        {
            DNStoreResultSearch result = new()
            {
                errorcode = 2,
                messageout = "Debetnote not exist"
            };

            try
            {
                using IDbConnection conn = Connection;
                var param = new
                {
                    ActivityDesc = body.ActivityDesc,
                    activityLen = body.ActivityDesc!.Length,
                    IntDocNo = body.IntDocNo,
                    docLen = body.IntDocNo!.Length
                };
                string qry = String.Format(@"  SELECT 
                                DN.id
                                , DN.refid
                                , DistributorId
                                , ActivityDesc
                                , DPP
                                , IntDocNo 
                                , pcp.LongDesc entity
                                , dis.LongDesc distributor
                                FROM tbtrx_debetnote as DN
                                LEFT JOIN tbmst_principal as pcp on pcp.id = PrincipalId
                                LEFT JOIN tbmst_distributor as dis on dis.id = DistributorId
                                WHERE DistributorId = {0}
                                AND PrincipalId = {1}
                                --AND DPP = @DPP #882
                                AND isnull(IsCancel, 0) = 0
                                AND (
                                        (ActivityDesc = @ActivityDesc AND DATALENGTH(ActivityDesc) = @activityLen)                      
                                    )
                                
                                ", body.DistributorId, body.EntityId);
                var __res = await conn.QueryAsync<DNStoreResultSearch>(qry, param);
                if (__res.Any()) //data exist
                {
                    result = __res.FirstOrDefault()!;
                    result.errorcode = 1;
                    result.messageout = "Debetnote with similar data already exist";
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return result;
        }
        // debetnote/store
        public async Task<DNCreationReturn> CreateDN(DNCreationParam body)
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
                __param.Add("@IsDNPromo", body.IsDNPromo);
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
                __param.Add("@TaxLevel", body.TaxLevel);
                __param.Add("@sellingpoint", __sellpo.AsTableValuedParameter());
                __param.Add("@attachment", __attachment.AsTableValuedParameter());
                __param.Add("@pphPct", body.pphPct);
                __param.Add("@pphAmt", body.pphAmt);
                __param.Add("@statusPPH", body.statusPPH);
                __param.Add("@FPNumber", body.FPNumber);
                __param.Add("@FPDate", body.FPDate);
                __param.Add("@statusPPN", body.statusPPN);
                __param.Add("@WHTType", body.WHTType);

                var result = await conn.QueryAsync<DNCreationReturn>("ip_debetnote_create", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // debetnote/update
        public async Task<DNCreationReturn> UpdateDN(DNCreationParam body)
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
                __param.Add("@IsDNPromo", body.IsDNPromo);
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
                __param.Add("@FPNumber", body.FPNumber);
                __param.Add("@FPDate", body.FPDate);
                __param.Add("@statusPPN", body.statusPPN);
                __param.Add("@WHTType", body.WHTType);

                var result = await conn.QueryAsync<DNCreationReturn>("ip_debetnote_update", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // dnattachment/store
        public async Task<DNGlobalResponse> CreateDNAttachment(DNAttachmentBody body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@id", body.DNId);
                __param.Add("@doclink", body.DocLink);
                __param.Add("@filename", body.FileName);
                __param.Add("@createby", body.CreateBy);

                var result = await conn.QueryAsync<DNGlobalResponse>("ip_dn_attachment_store", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
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

                using (var __re = await conn.QueryMultipleAsync("ip_debetnote_print", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180))
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
        // select Sub Account
        public async Task<IList<DNCreationSubAccountList>> GetSubAccountList(string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@userid", profileId);

                conn.Open();
                var __query = @"select c.Id, c.LongDesc  from tbset_map_distributor_account a
                        inner join tbset_user_distributor b on a.DistributorId=b.DistributorId and IIF(@userid='0','',b.Userid) =IIF(@userid='0','',@userid)
                        inner join tbmst_subaccount c on a.SubAccountId=c.Id 
                        group by b.UserId,c.Id, c.LongDesc
                        order by c.LongDesc";
                var child = await conn.QueryAsync<DNCreationSubAccountList>(__query, __param);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // Select Entity 
        public async Task<IList<DNCreationEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<DNCreationEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // Select Channel
        public async Task<IList<DNCreationChannelList>> GetChannelList(string userid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __query = new DynamicParameters();

                __query.Add("@userid", userid);
                __query.Add("@attribute", "channel");
                __query.Add("@parent", __parent.AsTableValuedParameter());


                conn.Open();
                var child = await conn.QueryAsync<DNCreationChannelList>("ip_getattribute_bymapping", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<DNStoreResultSearch> GetDebetnoteUpdateValidate(DNCreationParam body)
        {

            DNStoreResultSearch result = new()
            {
                errorcode = 2,
                messageout = "Debetnote not exist"
            };

            using (IDbConnection conn = Connection)
            {
                var param = new
                {
                    ActivityDesc = body.ActivityDesc,
                    activityLen = body.ActivityDesc!.Length,
                    IntDocNo = body.IntDocNo,
                    docLen = body.IntDocNo!.Length
                };
                string qry = String.Format(@"  SELECT 
                                DN.id
                                , DN.refid
                                , DistributorId
                                , ActivityDesc
                                , DPP
                                , IntDocNo 
                                , pcp.LongDesc entity
                                , dis.LongDesc distributor
                                FROM tbtrx_debetnote as DN
                                LEFT JOIN tbmst_principal as pcp on pcp.id = PrincipalId
                                LEFT JOIN tbmst_distributor as dis on dis.id = DistributorId
                                WHERE DistributorId = {0}
                                AND PrincipalId = {1}
                                --AND DPP = @DPP #882
                                AND isnull(IsCancel, 0) = 0
                                AND (
                                         (ActivityDesc = @ActivityDesc AND DATALENGTH(ActivityDesc) = @activityLen)
                                    )
                                AND DN.id <> {2}
                                ", body.DistributorId, body.EntityId, body.Id);
                var __res = await conn.QueryAsync<DNStoreResultSearch>(qry, param);
                if (__res.Any()) //data exist
                {
                    result = __res.FirstOrDefault()!;
                    result.errorcode = 1;
                    result.messageout = "Debetnote with similar data already exist";
                }
            }
            return result;
        }
        public async Task<DNCreationGetWHTType> GetDNCreationGetWHTType(int promoId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = new DynamicParameters();
                __query.Add("@promoId", promoId);
                var __res = await conn.QueryAsync<DNCreationGetWHTType>("[dbo].[ip_get_wht_distributor_mapping]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return __res.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}