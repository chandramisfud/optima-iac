using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNCreateInvoiceRepo : IDNCreateInvoiceRepo
    {
        readonly IConfiguration __config;
        public DNCreateInvoiceRepo(IConfiguration config)
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
        //debetnote/invoice/store
        public async Task<InvoiceDto> CreateInvoice(
            int DistributorId,
            int EntityId,
            decimal DPPAmount,
            decimal PPNpct,
            decimal InvoiceAmount,
            string Desc,
            string UserId,
            IList<DNIdReadytoInvoiceArray> DNId,
            string TaxLevel,
            string dnPeriod,
            int categoryId
            )
        {
            try
            {
                DataTable __dn = new();
                __dn.Columns.Add("keyid", typeof(int));
                foreach (DNIdReadytoInvoiceArray v in DNId)
                    __dn.Rows.Add(v.DNId);
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@distributorid", DistributorId);
                __param.Add("@entityid", EntityId);
                __param.Add("@dpp", DPPAmount);
                __param.Add("@ppn", PPNpct);
                __param.Add("@invoiceamt", InvoiceAmount);
                __param.Add("@desc", Desc);
                __param.Add("@userid", UserId);
                __param.Add("@TaxLevel", TaxLevel);
                __param.Add("@id", __dn.AsTableValuedParameter());
                __param.Add("@dnPeriod", dnPeriod);
                __param.Add("@categoryId", categoryId);
                var result = await conn.QueryAsync<InvoiceDto>("ip_dn_create_invoice", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //promoattachment/delete
        public async Task DeletePromoAttachmentDNCreateInvoice(int PromoId, string DocLink)
        {
            try
            {
                using IDbConnection conn = Connection;
                await conn.ExecuteAsync(@"DELETE FROM tbtrx_promo_doclink where PromoId=@PromoId and DocLink=@DocLink"
                , new
                {
                    PromoId,
                    DocLink

                });
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // debetnote/ready_to_invoice [POST]
        public async Task<DNCreateInvoice> DNChangeStatusReadytoInvoice(DNChangeStatusReadytoInvoice param)
        {
            try
            {
                DataTable __dT = new();

                __dT.Columns.Add("keyid", typeof(int));
                foreach (DNIdReadytoInvoiceArray v in param.DNId!)
                    __dT.Rows.Add(v.DNId);
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@status", param.status);
                __param.Add("@userid", param.UserId);
                __param.Add("@id", __dT.AsTableValuedParameter());
                var result = await conn.QueryAsync<DNCreateInvoice>("ip_dn_changestatus", __param, commandTimeout: 180, commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}
        public async Task<IList<DNFilter>> DNFilterforCreatedInvoice(
            string userid,
            string status,
            int entity,
            string TaxLevel,
            DataTable dn,
            int invoiceId,
            string dnPeriod,
            int categoryId
            )
        {
            using IDbConnection conn = Connection;
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            var result = await conn.QueryAsync<DNFilter>("ip_invoice_filter",
            new
            {
                userid,
                status,
                entity,
                TaxLevel,
                invoiceId,
                dnPeriod,
                categoryId,
                dn = dn.AsTableValuedParameter("TemplateDebetNote")
            }
                , commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList();
        }

        //debetnote/reject
        public async Task<DNRejectCreateInvoiceGlobalResponse> DNRejectCreateInvoice(int dnid, string reason, string userid)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@dnid", dnid);
                __param.Add("@reason", reason);
                __param.Add("@userid", userid);

                var result = await conn.QueryAsync<DNRejectCreateInvoiceGlobalResponse>("ip_debetnote_reject", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

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

        public Task<UserProfileDataByIdforDNCreateInvoice> GetById(string id)
        {
            try
            {
                using IDbConnection conn = Connection;
                var userDictionary = new Dictionary<string, UserProfileDataByIdforDNCreateInvoice>();
                string sql = @"SELECT A.[id]
                            ,A.[username]
                            ,A.[email]
                            ,A.[password]
                            ,A.[usergroupid]
                            ,A.[userlevel]
                            ,A.[isLogin]
                            ,A.[lastLogin]
                            ,A.[department]
                            ,A.[jobtitle]
                            ,A.[contactinfo]
                            ,B.[DistributorId]
                            ,C.[ShortDesc] DistributorShortDesc
                            ,C.[LongDesc] DistributorLongDesc
                            ,C.[CompanyName] 
                            ,A.[registered]
                            ,A.[code]
                            ,A.[password_change]
                            ,A.[token]
                            ,A.[token_date]
                            ,A.[userinput]
                            ,A.[dateinput]
                            ,A.[useredit]
                            ,A.[dateedit]
                            ,A.[isdeleted]
                            ,A.[deletedby]
                            ,A.[deletedon]
                FROM tbset_user AS A LEFT JOIN 
                tbset_user_distributor AS B ON A.id = B.UserId LEFT JOIN
                tbmst_distributor AS C ON B.DistributorId = C.Id
                WHERE A.id =@id";
                var list = conn.Query<UserProfileDataByIdforDNCreateInvoice, ListDistributorDNCreateInvoice, UserProfileDataByIdforDNCreateInvoice>(
                sql, (user, distributor) =>
                {

                    if (!userDictionary.TryGetValue(user.id!, out UserProfileDataByIdforDNCreateInvoice? userEntry))
                    {
                        userEntry = user;
                        userEntry.distributorlist = new List<ListDistributorDNCreateInvoice>();
                        userDictionary.Add(userEntry.id!, userEntry);
                    }

                    userEntry.distributorlist!.Add(distributor);
                    return userEntry;
                },
                    new { Id = id },
                    splitOn: "DistributorId")
                    .Distinct()
                    .FirstOrDefault();

                return Task.FromResult(list)!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<object>> GetCategoryDropdownList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT id, shortDesc, longDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<object>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DistributorforCreateInvoice>> GetDistributorforCreateInvoice(int entityId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT DistributorId [Id], td.ShortDesc, td.LongDesc, td.CompanyName  FROM tbset_principal_distributor a LEFT JOIN 
                                    tbmst_distributor td 
                                    on DistributorId = td.Id  
                                    WHERE ISNULL(a.IsDeleted,0)=0  
                                    AND ISNULL(td.IsDeleted,0)=0 AND a.PrincipalId = @entityId ";
                var __res = await conn.QueryAsync<DistributorforCreateInvoice>(__query, new { entityId = entityId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        //debetnote/invoicetaxlevel
        public async Task<IList<DNDetailforInvoice>> GetDNByStatusforInvoiceTaxLevel(string status, string userid, int entityid, int distributorid, string TaxLevel, string dnPeriod, int categoryId)
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
                __param.Add("@dnPeriod", dnPeriod);
                __param.Add("@categoryId", categoryId);
                var result = await conn.QueryAsync<DNDetailforInvoice>("ip_debetnote_list_forinvoice", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //debetnote/getbyId/
        public async Task<DNGetbyIdforCreateInvoice> GetDNGetbyIdforCreateInvoice(int id)
        {

            try
            {
                DNGetbyIdforCreateInvoice? __res = new();
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __obj = await conn.QueryMultipleAsync("ip_debetnote_select", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    {
                        __res = __obj.Read<DNGetbyIdforCreateInvoice>().FirstOrDefault()!;
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
        //debetnote/ready_to_invoice
        public async Task<IList<DNDetailforInvoice>> GetDNStatusReadytoInvoice(string status, string userid, int entityid, int distributorid)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@status", status);
                __param.Add("@userid", userid);
                __param.Add("@entityid", entityid);
                __param.Add("@distributorid", distributorid);

                var result = await conn.QueryAsync<DNDetailforInvoice>("ip_debetnote_list_bystatus", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //Select entity
        public async Task<IList<DNCreateInvoiceEntityList>> GetEntityList()
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<DNCreateInvoiceEntityList>(__query);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //debetnote/getInvoiceById
        public async Task<DNCreateInvoice> GetInvoiceById(int id)
        {
            try
            {
                List<DNCreateInvoice> __invoice = new();
                DNCreateInvoice __result = new();

                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@id", id);
                using (var __re = await conn.QueryMultipleAsync("ip_dn_invoice_detail", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180))
                {
                    List<DNDetailforInvoice> __detail = new();

                    __result = __re.Read<DNCreateInvoice>().FirstOrDefault()!;

                    if (__result != null)
                    {
                        __detail = new List<DNDetailforInvoice>();
                        __detail = __re.Read<DNDetailforInvoice>().ToList();
                        __result.StandartDetailDN = __detail;
                    }
                }
                return __result!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //debetnote/invoicelist
        public async Task<object> GetInvoiceList(string createdate, int entity, int distributor, string profileid,
            string sortColumn, string sortDirection="ASC", int length = 10, int start = 0, string? txtSearch = null
            )
        {
            try
            {
                using (IDbConnection conn = Connection)
                {

                    var __param = new DynamicParameters();
                    __param.Add("@createdate", createdate);
                    __param.Add("@entity", entity);
                    __param.Add("@distributor", distributor);
                    __param.Add("@profileid", profileid);
                    __param.Add("@length", length);
                    __param.Add("@start", start);
                    __param.Add("@txtSearch", txtSearch);
                    __param.Add("@SortColumn", sortColumn);
                    __param.Add("@SortDirection", sortDirection);

                    var __res = await conn.QueryMultipleAsync("ip_getinvoice_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                    List<object> __data = __res.Read<object>().ToList();

                    var res = __res.ReadSingle<BaseLP2>();
                    res.Data = __data.Cast<object>().ToList();
                    return res;
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //debetnote/printinvoice
        public async Task<InvoicePrintDto> GetPrintInvoicebyId(int id)
        {
            try
            {
                List<InvoicePrintDto> __invoice = new();
                InvoicePrintDto __result = new();

                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@id", id);
                using (var __re = await conn.QueryMultipleAsync("ip_getinvoice_print", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180))
                {
                    List<InvoiceDetailDNDto> __detail = new();

                    __result = __re.Read<InvoicePrintDto>().FirstOrDefault()!;

                    if (__result != null)
                    {
                        __detail = new List<InvoiceDetailDNDto>();
                        __detail = __re.Read<InvoiceDetailDNDto>().ToList();
                        __result.DetailDN = __detail;
                    }
                }
                return __result!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //select taxlevel
        public async Task<IList<SelectTaxLevel>> GetTaxLevelList()
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"SELECT * FROM [dbo].[tbset_mapping_material] ";
                var __res = await conn.QueryAsync<SelectTaxLevel>(__query);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //debetnote/invoice/update
        public async Task<InvoiceDto> UpdateInvoice(
            int InvoiceId,
            int DistributorId,
            int EntityId,
            decimal DPPAmount,
            decimal PPNpct,
            decimal InvoiceAmount,
            string Desc,
            string UserId,
            IList<DNIdReadytoInvoiceArray> DNId,
            string TaxLevel,
            string dnPeriod,
            int categoryId
            )
        {
            try
            {
                DataTable __dn = new();

                __dn.Columns.Add("keyid", typeof(int));
                foreach (DNIdReadytoInvoiceArray v in DNId)
                    __dn.Rows.Add(v.DNId);
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@invoiceid", InvoiceId);
                __param.Add("@distributorid", DistributorId);
                __param.Add("@entityid", EntityId);
                __param.Add("@dpp", DPPAmount);
                __param.Add("@ppn", PPNpct);
                __param.Add("@invoiceamt", InvoiceAmount);
                __param.Add("@desc", Desc);
                __param.Add("@userid", UserId);
                __param.Add("@TaxLevel", TaxLevel);
                __param.Add("@dnPeriod", dnPeriod);
                __param.Add("@categoryId", categoryId);
                __param.Add("@id", __dn.AsTableValuedParameter());

                var result = await conn.QueryAsync<InvoiceDto>("ip_dn_update_invoice", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}