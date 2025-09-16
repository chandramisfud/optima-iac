using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNWorkflowRepo : IDNWorkflowRepo
    {
        readonly IConfiguration __config;
        public DNWorkflowRepo(IConfiguration config)
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
        // debetnote/print/
        public async Task<DNPrint> DNPrintforDNWorkflow(int id)
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
        // DNGetById
        public async Task<DNGetById> GetDNbyIdforDNWorkflow(int id)
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
        // debetnoteid/workflow
        public async Task<DNIdWorkflow> GetDNWorkflow(string RefId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = new DynamicParameters();
                __query.Add("@refid", RefId);
                var result = await conn.QueryMultipleAsync("ip_dnid_workflow", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                var statusres = result.Read<StatusResult>();
                var debetnoteres = result.Read<DebetNoteResult>();
                var sellingpointres = result.Read<SellingPointResult>();
                var fileattachres = result.Read<FileAttachResult>();
                var doccompletenessres = result.Read<DocumentCompletenessResult>();

                DNIdWorkflow __res = new()
                {
                    statusresult = statusres.ToList(),
                    debetnoteresult = debetnoteres.ToList(),
                    sellingpointresult = sellingpointres.ToList(),
                    fileattactresult = fileattachres.ToList(),
                    doccompletenessresult = doccompletenessres.ToList()
                };

                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // debetnoteid/workflow_change
        public async Task<IList<DNIdWorkflowChange>> GetDNWorkflowChange(string RefId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@refid", RefId);

                var result = await conn.QueryAsync<DNIdWorkflowChange>("ip_dnid_workflow_change", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // debetnoteid/workflow_history
        public async Task<IList<DNIdWorkflowHistory>> GetDNWorkflowHistory(string RefId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@refid", RefId);

                var result = await conn.QueryAsync<DNIdWorkflowHistory>("ip_dnid_workflow_history", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // promo/workflow
        public async Task<PromoWorkflowResult> GetPromoWorkflowforDNWorkflow(string RefId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@RefId", RefId);
                var result = await conn.QueryMultipleAsync("ip_promoid_workflow", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                var promoWorkflowDtos = result.Read<PromoWorkflowDto>();
                var regionDtos = result.Read<RegionDtos>();
                var channelDtos = result.Read<ChannelDtos>();
                var subChannelDtos = result.Read<SubChannelDtos>();
                var accountDtos = result.Read<AccountDtos>();
                var subAccountDtos = result.Read<SubAccountDtos>();
                var brandDtos = result.Read<BrandDtos>();
                var skuDtos = result.Read<SkuDtos>();
                var activitiesDtos = result.Read<ActivitiesDtos>();
                var subActivitiesDtos = result.Read<SubActivitiesDtos>();
                var fileAttachDtos = result.Read<FileAttachDtos>();
                var masterStatusApprovalDtos = result.Read<MasterStatusApprovalDtos>();
                var investmentDtos = result.Read<InvestmentDtos>();
                var statusPromoDto = result.Read<PromoStatusDtos>();
                var mechanismDtos = result.Read<MechanismSelect>();

                PromoWorkflowResult __res = new()
                {
                    Promo = promoWorkflowDtos.ToList(),
                    Region = regionDtos.ToList(),
                    Channel = channelDtos.ToList(),
                    SubChannel = subChannelDtos.ToList(),
                    Account = accountDtos.ToList(),
                    SubAccount = subAccountDtos.ToList(),
                    Brand = brandDtos.ToList(),
                    Sku = skuDtos.ToList(),
                    Activity = activitiesDtos.ToList(),
                    SubActivity = subActivitiesDtos.ToList(),
                    FileAttach = fileAttachDtos.ToList(),
                    StatusApproval = masterStatusApprovalDtos.ToList(),
                    Investment = investmentDtos.ToList(),
                    StatusPromo = statusPromoDto.ToList(),
                    Mechanism = mechanismDtos.ToList(),

                };
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}