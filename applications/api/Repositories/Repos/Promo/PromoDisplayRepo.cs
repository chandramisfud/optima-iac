using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;
using Repositories.Entities.Models.Promo;
using Repositories.Entities.Models.PromoApproval;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos.Promo
{
    public class PromoDisplayRepository : IPromoDisplayRepository
    {
        readonly IConfiguration __config;
        public PromoDisplayRepository(IConfiguration config)
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
        public async Task<PromoDisplayLP> GetPromoDisplayLP(
            string year,
            int entity,
            int distributor,
            int BudgetParent,
            int channel,
            string userid,
            bool cancelstatus,
            int start = 0,
            int length = 10,
            string filter = "ASC",
            string txtsearch = ""
            )
        {
            PromoDisplayLP res = new();
            using IDbConnection conn = Connection;

            var pageNum = start * length;
            txtsearch = (txtsearch) ?? "";

            var __param = new DynamicParameters();
            __param.Add("@periode", year);
            __param.Add("@entity", entity);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);
            __param.Add("@cancelstatus", cancelstatus);
            __param.Add("@start", pageNum);
            __param.Add("@length", length);
            __param.Add("@filter", filter);
            __param.Add("@txtsearch", txtsearch);

            var __object = await conn.QueryMultipleAsync("ip_promo_v3_list_display_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            var data = __object.Read<PromoDisplayDataforLP>().ToList();
            var count = __object.ReadSingle<RecordTotal>();
            res.recordsTotal = count.recordsTotal;
            res.recordsFiltered = count.recordsFiltered;

            res.Data = data;


            return res;
        }

        public async Task<PromoReconByIdDto> GetPromoDisplayById(int id, string profile="")
        {
            PromoReconByIdDto __result = new();
            using (IDbConnection conn = Connection)
            {
                var __query = new DynamicParameters();
                __query.Add("@Id", id);
                __query.Add("@profile", profile);

                using var __re = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display_recon]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
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
                //Add:  AND Oct 11 2023 E2#38
                __result.GroupBrand = __re.Read<object>().ToList();
            }
            return __result;
        }

        public async Task<IList<PromoView>> GetPromoListByStatus(string year, int entity, int distributor,
            int BudgetParent, int channel, string userid, bool cancelstatus)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@periode", year);
            __param.Add("@entity", entity);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);
            __param.Add("@cancelstatus", cancelstatus);

            var result = await conn.QueryAsync<PromoView>("ip_promo_list_bystatus", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return result.ToList();
        }

        public async Task<PromoDisplayList> GetPromoDisplaybyId(int id)
        {
            try
            {
                PromoDisplayList __res = new();
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
    }
}
