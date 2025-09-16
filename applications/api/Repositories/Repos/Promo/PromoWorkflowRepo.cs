using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Repositories.Entities.Models.DN;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities;

namespace Repositories.Repos.Promo
{
    public class PromoWorkflowRepository : IPromoWorkflowRepository
    {
        readonly IConfiguration __config;
        public PromoWorkflowRepository(IConfiguration config)
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

        public async Task<PromoWorkflowResult> GetPromoWorkflow(string refid)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@RefId", refid);
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
            var groupBrand = result.Read<object>();

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
                GroupBrand = groupBrand.ToList(),
            };

            return __res;
        }

        public async Task<PromoReconByIdDto> GetPromoWorkflowById(int id)
        {
            PromoReconByIdDto __result = new();
            using (IDbConnection conn = Connection)
            {
                var __query = new DynamicParameters();
                __query.Add("@Id", id);

                using var __re = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display_recon]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                __result.PromoHeader = __re.Read<PromoReconHeader>().FirstOrDefault()!;

                __result.Regions = new List<PromoAttibuteById>();
                foreach (PromoAttibuteById r in __re.Read<PromoAttibuteById>())
                    __result.Regions.Add(new PromoAttibuteById()
                    {
                        flag = r.flag,
                        id = r.id,
                        longDesc = r.longDesc,
                    });
                __result.Channels = new List<PromoAttibuteById>();
                foreach (PromoAttibuteById c in __re.Read<PromoAttibuteById>())
                    __result.Channels.Add(new PromoAttibuteById()
                    {
                        flag = c.flag,
                        id = c.id,
                        longDesc = c.longDesc,
                    });
                __result.SubChannels = new List<PromoAttibuteById>();
                foreach (PromoAttibuteById sc in __re.Read<PromoAttibuteById>())
                    __result.SubChannels.Add(new PromoAttibuteById()
                    {
                        flag = sc.flag,
                        id = sc.id,
                        longDesc = sc.longDesc,
                    });
                __result.Accounts = new List<PromoAttibuteById>();
                foreach (PromoAttibuteById a in __re.Read<PromoAttibuteById>())
                    __result.Accounts.Add(new PromoAttibuteById()
                    {
                        flag = a.flag,
                        id = a.id,
                        longDesc = a.longDesc,
                    });
                __result.SubAccounts = new List<PromoAttibuteById>();
                foreach (PromoAttibuteById sa in __re.Read<PromoAttibuteById>())
                    __result.SubAccounts.Add(new PromoAttibuteById()
                    {
                        flag = sa.flag,
                        id = sa.id,
                        longDesc = sa.longDesc,
                    });

                __result.Brands = new List<PromoAttibuteById>();
                foreach (PromoAttibuteById br in __re.Read<PromoAttibuteById>())
                    __result.Brands.Add(new PromoAttibuteById()
                    {
                        flag = br.flag,
                        id = br.id,
                        longDesc = br.longDesc,
                    });

                __result.Skus = new List<PromoAttibuteById>();
                foreach (PromoAttibuteById pr in __re.Read<PromoAttibuteById>())
                    __result.Skus.Add(new PromoAttibuteById()
                    {
                        flag = pr.flag,
                        id = pr.id,
                        longDesc = pr.longDesc,
                    });

                __result.Activity = new List<PromoAttibuteById>();
                foreach (PromoAttibuteById ac in __re.Read<PromoAttibuteById>())
                    __result.Activity.Add(new PromoAttibuteById()
                    {
                        flag = ac.flag,
                        id = ac.id,
                        longDesc = ac.longDesc,
                    });

                __result.SubActivity = new List<PromoAttibuteById>();
                foreach (PromoAttibuteById sac in __re.Read<PromoAttibuteById>())
                    __result.SubActivity.Add(new PromoAttibuteById()
                    {
                        flag = sac.flag,
                        id = sac.id,
                        longDesc = sac.longDesc,
                    });

                __result.attachments = new List<PromoAttachmentById>();
                foreach (PromoAttachmentById at in __re.Read<PromoAttachmentById>())
                    __result.attachments.Add(new PromoAttachmentById()
                    {
                        fileName = at.fileName,
                        docLink = at.docLink,
                    });

                __result.listApprovalStatus = new List<ListApprovalStatusById>();
                foreach (ListApprovalStatusById las in __re.Read<ListApprovalStatusById>())
                    __result.listApprovalStatus.Add(new ListApprovalStatusById()
                    {
                        statusCode = las.statusCode,
                        statusDesc = las.statusDesc,
                    });
                __result.SKPValidations = new List<SKPValidation>();

                foreach (SKPValidation si in __re.Read<SKPValidation>())
                    __result.SKPValidations.Add(new SKPValidation()
                    {
                        PromoId = si.PromoId,
                        SKPDraftAvail = si.SKPDraftAvail,
                        SKPDraftAvailOn = si.SKPDraftAvailOn,
                        SKPDraftAvailBy = si.SKPDraftAvailBy,
                        SKPDraftAvailBfrAct60 = si.SKPDraftAvailBfrAct60,
                        SKPDraftAvailBfrAct60On = si.SKPDraftAvailBfrAct60On,
                        SKPDraftAvailBfrAct60By = si.SKPDraftAvailBfrAct60By,
                        PeriodMatch = si.PeriodMatch,
                        PeriodMatchOn = si.PeriodMatchOn,
                        PeriodMatchBy = si.PeriodMatchBy,
                        InvestmentMatch = si.InvestmentMatch,
                        InvestmentMatchOn = si.InvestmentMatchOn,
                        InvestmentMatchBy = si.InvestmentMatchBy,
                        MechanismMatch = si.MechanismMatch,
                        MechanismMatchOn = si.MechanismMatchOn,
                        MechanismMatchBy = si.MechanismMatchBy,
                        SKPSign7 = si.SKPSign7,
                        SKPSign7On = si.SKPSign7On,
                        SKPSign7By = si.SKPSign7By,
                        EntityDraft = si.EntityDraft,
                        EntityDraftOn = si.EntityDraftOn,
                        EntityDraftBy = si.EntityDraftBy,
                        BrandDraft = si.BrandDraft,
                        BrandDraftOn = si.BrandDraftOn,
                        BrandDraftBy = si.BrandDraftBy,
                        PeriodDraft = si.PeriodDraft,
                        PeriodDraftOn = si.PeriodDraftOn,
                        PeriodDraftBy = si.PeriodDraftBy,
                        ActivityDescDraft = si.ActivityDescDraft,
                        ActivityDescDraftOn = si.ActivityDescDraftOn,
                        ActivityDescDraftBy = si.ActivityDescDraftBy,
                        MechanismDraft = si.MechanismDraft,
                        MechanismDraftOn = si.MechanismDraftOn,
                        MechanismDraftBy = si.MechanismDraftBy,
                        InvestmentDraft = si.InvestmentDraft,
                        InvestmentDraftOn = si.InvestmentDraftOn,
                        InvestmentDraftBy = si.InvestmentDraftBy,
                        Entity = si.Entity,
                        EntityOn = si.EntityOn,
                        EntityBy = si.EntityBy,
                        Brand = si.Brand,
                        BrandOn = si.BrandOn,
                        BrandBy = si.BrandBy,
                        ActivityDesc = si.ActivityDesc,
                        ActivityDescOn = si.ActivityDescOn,
                        ActivityDescBy = si.ActivityDescBy,
                    });

                __result.Investments = new List<Entities.Models.PromoApproval.PromoReconInvestment>();

                foreach (Entities.Models.PromoApproval.PromoReconInvestment sa in __re.Read<Entities.Models.PromoApproval.PromoReconInvestment>())
                    __result.Investments.Add(new Entities.Models.PromoApproval.PromoReconInvestment()
                    {
                        Investment = sa.Investment,
                        NormalSales = sa.NormalSales,
                        IncrSales = sa.IncrSales,
                        TotSales = sa.TotSales,
                        Roi = sa.Roi,
                        CostRatio = sa.CostRatio
                    });
                __result.Mechanisms = new List<MechanismById>();
                foreach (MechanismById mc in __re.Read<MechanismById>())
                    __result.Mechanisms.Add(new MechanismById()
                    {
                        mechanismId = mc.mechanismId,
                        mechanism = mc.mechanism,
                        notes = mc.notes,
                        productId = mc.productId,
                        product = mc.product,
                        brandId = mc.brandId,
                        brand = mc.brand,
                    });

                __result.GroupBrand = __re.Read<object>().ToList();

            }
            return __result;
        }
        //Added: AND 2022Nov10
        public async Task<List<PromoWorkflowDN>> GetPromoWorkflowDN(string refid)
        {
            using IDbConnection conn = Connection;
            var __param = new DynamicParameters();
            __param.Add("@RefId", refid);
            var result = await conn.QueryAsync<PromoWorkflowDN>("ip_promoid_workflow_dn", __param,
                commandType: CommandType.StoredProcedure, commandTimeout: 180);

            return result.ToList();
        }
        public async Task<IList<PromoWorkFlowChanges>> GetPromoWorkFlowChanges(string refid)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@RefId", refid);
            var result = await conn.QueryAsync<PromoWorkFlowChanges>("ip_promoid_workflow_change", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList();
        }
        public async Task<IList<PromoWorkflowHistory>> GetPromoWorkFlowHistory(string refid)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@RefId", refid);
            var result = await conn.QueryAsync<PromoWorkflowHistory>("ip_promoid_workflow_history", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList();
        }

        public async Task<IList<object>> GetPromoWorkflowTimeline(string refId)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@RefId", refId);
            var result = await conn.QueryAsync<object>("ip_promoid_workflow_timeline", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList();
        }
    }
}
