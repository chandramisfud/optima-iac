using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNValidationbyFinanceRepo : IDNValidationbyFinanceRepo
    {
        readonly IConfiguration __config;
        public DNValidationbyFinanceRepo(IConfiguration config)
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

        public async Task DeletePromoAttachmentForValidationbyFinance(int PromoId, string DocLink)
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

        public async Task<DNValidationbyFinance> DNChangeStatusValidatebyFinanceMultiApproval(
            string status,
            string userid,
            List<DNId> dnid
        )
        {
            try
            {
                DataTable __dT = new();

                __dT.Columns.Add("keyid", typeof(int));
                foreach (DNId v in dnid)
                    __dT.Rows.Add(v.dnid);

                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@status", status);
                __param.Add("@userid", userid);
                __param.Add("@id", __dT.AsTableValuedParameter());

                var result = await conn.QueryAsync<DNValidationbyFinance>("ip_dn_changestatus", __param, commandTimeout: 180, commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNFilter>> DNFilterValidateByFinance(string userid, string status, int entity, string TaxLevel, DataTable dn)
        {
            try
            {
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var result = await conn.QueryAsync<DNFilter>("ip_debetnote_filter_validate_by_finance",
                new
                {
                    userid = userid,
                    status = status,
                    entity = entity,
                    TaxLevel = TaxLevel,
                    dn = dn.AsTableValuedParameter("TemplateDebetNote")

                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);

                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DNValidationbyFinance> DNValidationbyFinance(DNValidationbyFinanceParam param)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@dnid", param.DNId);
                __param.Add("@status", param.StatusCode);
                __param.Add("@notes", param.Notes);
                __param.Add("@userid", param.userid);
                __param.Add("@taxlevel", param.TaxLevel);

                var result = await conn.QueryAsync<DNValidationbyFinance>("ip_dn_validation", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DNValidationbyFinance> DNValidationParalelCompleteness(int DNId, string status, string notes,
            string userid, string taxlevel, int entityId, int promoId, bool isDNPromo,
            string wHTType,
            string statusPPH,
            double pphPct,
            double pphAmt,
            DNDocCompletenessforValidationbyFinance DNDocCompletenessHeader)
        {
            try
            {
                DataTable __dT = _castToDataTable(new DNDocCompletenessforValidationbyFinance(), null!);
                __dT.Rows.Add
                (
                    DNDocCompletenessHeader.DNId
                    , DNDocCompletenessHeader.Original_Invoice_from_retailers
                    , DNDocCompletenessHeader.Tax_Invoice
                    , DNDocCompletenessHeader.Promotion_Agreement_Letter
                    , DNDocCompletenessHeader.Trading_Term
                    , DNDocCompletenessHeader.Sales_Data
                    , DNDocCompletenessHeader.Copy_of_Mailer
                    , DNDocCompletenessHeader.Copy_of_Photo_Doc
                    , DNDocCompletenessHeader.List_of_Transfer

                );
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@dnid", DNId);
                __param.Add("@status", status);
                __param.Add("@notes", notes);
                __param.Add("@userid", userid);
                __param.Add("@taxlevel", taxlevel);
                __param.Add("@EntityId", entityId);
                __param.Add("@PromoIdNew", promoId);
                __param.Add("@IsDNPromo", isDNPromo);
                __param.Add("@wHTType", wHTType);
                __param.Add("@statusPPH", statusPPH);
                __param.Add("@pphPct", pphPct);
                __param.Add("@pphAmt", pphAmt);
                __param.Add("@DN_Doc_Completeness", __dT.AsTableValuedParameter());

                var result = await conn.QueryAsync<DNValidationbyFinance>("ip_dn_validation_paralel_doc_completeness", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<DNValidationbyFinance> DNFinValidationDocCompleteness(int DNId, string userid,
            DNDocCompletenessforValidationbyFinance DNDocCompletenessHeader)
        {
            try
            {
                DataTable __dT = _castToDataTable(new DNDocCompletenessforValidationbyFinance(), null!);
                __dT.Rows.Add
                (
                    DNDocCompletenessHeader.DNId
                    , DNDocCompletenessHeader.Original_Invoice_from_retailers
                    , DNDocCompletenessHeader.Tax_Invoice
                    , DNDocCompletenessHeader.Promotion_Agreement_Letter
                    , DNDocCompletenessHeader.Trading_Term
                    , DNDocCompletenessHeader.Sales_Data
                    , DNDocCompletenessHeader.Copy_of_Mailer
                    , DNDocCompletenessHeader.Copy_of_Photo_Doc
                    , DNDocCompletenessHeader.List_of_Transfer

                );
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@dnid", DNId);
                __param.Add("@userid", @userid);
                __param.Add("@DN_Doc_Completeness", __dT.AsTableValuedParameter());

                var result = await conn.QueryAsync<DNValidationbyFinance>("ip_dn_fin_validation_doc_completeness", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNValidationbyFinanceDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
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
                var child = await conn.QueryAsync<DNValidationbyFinanceDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return child.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DNValidationbyFinancebyId> GetDNbyIdforValidationbyFinance(int id)
        {
            try
            {
                DNValidationbyFinancebyId? __res = null;
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __obj = await conn.QueryMultipleAsync("ip_debetnote_select", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    {
                        __res = __obj.Read<DNValidationbyFinancebyId>().FirstOrDefault();
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

        public async Task<IList<DNValidationbyFinanceWithTickable>> GetDNValidationbyFinance(string status, string userid, int entityid, int distributorid, string TaxLevel)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@status", status);
                __param.Add("@userid", userid);
                __param.Add("@entityid", entityid);
                __param.Add("@distributorid", distributorid);
                __param.Add("@TaxLevel", TaxLevel);
                var result = await conn.QueryAsync<DNValidationbyFinanceWithTickable>("ip_debetnote_list_for_validatebyfinance_paralel", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNValidationbyFinanceEntityList>> GetEntityList()
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<DNValidationbyFinanceEntityList>(__query);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromobyIdforValidationbyFinanceRC> GetPromobyIdforDNValidationbyFinanceRC(int id)
        {
            try
            {
                PromobyIdforValidationbyFinanceRC __res = new();
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", id);
                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
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
                        var __mechanism = __obj.Read<MechanismData>().ToList();
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
                        __res.Mechanism = __mechanism;
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
        public async Task<PromobyIdforValidationbyFinanceDC> GetPromobyIdforDNValidationbyFinanceDC(int id)
        {
            try
            {
                PromobyIdforValidationbyFinanceDC __res = new();
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", id);
                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
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
        public async Task<RCorDCValue> SelectPromoRCorDC(int id)
        {
            using IDbConnection conn = Connection;
            var __queryRC = @"SELECT CAST( CASE WHEN CategoryId = 3 THEN 1 ELSE 0 END AS bit) as RCorDC FROM tbtrx_promo WHERE id = @id";
            var res = await conn.QueryAsync<RCorDCValue>(__queryRC, new { id = id });
            return res.FirstOrDefault()!;
        }
    }
}