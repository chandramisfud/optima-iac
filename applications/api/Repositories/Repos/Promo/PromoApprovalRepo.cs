using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities;
using Repositories.Entities.Models;
using Repositories.Entities.Models.PromoApproval;
using System.Data;
using System.Data.SqlClient;

namespace Repositories.Repos
{
    public class PromoApprovalRepository : IPromoApprovalRepository
    {
        readonly IConfiguration __config;
        public PromoApprovalRepository(IConfiguration config)
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
        public async Task<IList<PromoApprovalView>> GetPromoApprovalLP(string year, int category,
            int entity, int distributor, int BudgetParent, int channel, string userid)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@periode", year);
            __param.Add("@category", category);
            __param.Add("@entity", entity);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);

            var result = await conn.QueryAsync<PromoApprovalView>("ip_promo_approval_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList();
        }

        public async Task<IEnumerable<BaseDropDownList>> GetAllEntity()
        {
            IEnumerable<BaseDropDownList> principals;

            using (IDbConnection conn = Connection)
            {

                var sql = "Select * from tbmst_principal where ISNULL(IsDeleted, 0) = 0";

                principals = await conn.QueryAsync<BaseDropDownList>(sql);
            }
            return principals;
        }

        public async Task<IList<BaseDropDownList>> GetDistributorList(int budgetid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __query = new DynamicParameters();

                __query.Add("@budgetid", budgetid);
                __query.Add("@attribute", "distributor");
                __query.Add("@parent", __parent.AsTableValuedParameter());


                conn.Open();
                var child = await conn.QueryAsync<BaseDropDownList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoReviseV3Dto> GetPromoByPrimaryId(int id)
        {
            List<PromoReviseV3Dto> __budget = new();
            using (IDbConnection conn = Connection)
            {
                var __query = new DynamicParameters();
                __query.Add("@Id", id);
                __query.Add("@LongDesc", string.Empty);
                using var __re = await conn.QueryMultipleAsync("[dbo].[ip_promo_v3_select]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                PromoReviseV3Dto __result = new()
                {
                    PromoHeader = __re.Read<PromoRevise>().FirstOrDefault()!,
                    Regions = new List<PromoRegionRes>()
                };
                foreach (PromoRegionRes r in __re.Read<PromoRegionRes>())
                    __result.Regions.Add(new PromoRegionRes()
                    {
                        flag = r.flag,
                        id = r.id,
                        longDesc = r.longDesc
                    });
                __result.Channels = new List<PromoChannelRes>();
                foreach (PromoChannelRes c in __re.Read<PromoChannelRes>())
                    __result.Channels.Add(new PromoChannelRes()
                    {
                        flag = c.flag,
                        id = c.id,
                        longDesc = c.longDesc,
                    });
                __result.SubChannels = new List<PromoSubChannelRes>();
                foreach (PromoSubChannelRes sc in __re.Read<PromoSubChannelRes>())
                    __result.SubChannels.Add(new PromoSubChannelRes()
                    {
                        flag = sc.flag,
                        id = sc.id,
                        longDesc = sc.longDesc,
                    });
                __result.Accounts = new List<PromoAccountRes>();
                foreach (PromoAccountRes a in __re.Read<PromoAccountRes>())
                    __result.Accounts.Add(new PromoAccountRes()
                    {
                        flag = a.flag,
                        id = a.id,
                        longDesc = a.longDesc,
                    });

                __result.SubAccounts = new List<PromoSubAccountRes>();
                foreach (PromoSubAccountRes sa in __re.Read<PromoSubAccountRes>())
                    __result.SubAccounts.Add(new PromoSubAccountRes()
                    {
                        flag = sa.flag,
                        id = sa.id,
                        longDesc = sa.longDesc,
                    });

                __result.Brands = new List<PromoBrandRes>();
                foreach (PromoBrandRes sa in __re.Read<PromoBrandRes>())
                    __result.Brands.Add(new PromoBrandRes()
                    {
                        flag = sa.flag,
                        id = sa.id,
                        longDesc = sa.longDesc,
                    });

                __result.Skus = new List<PromoProductRes>();
                foreach (PromoProductRes sa in __re.Read<PromoProductRes>())
                    __result.Skus.Add(new PromoProductRes()
                    {
                        flag = sa.flag,
                        id = sa.id,
                        longDesc = sa.longDesc,
                    });


                __result.Activity = new List<PromoActivityRes>();
                foreach (PromoActivityRes sa in __re.Read<PromoActivityRes>())
                    __result.Activity.Add(new PromoActivityRes()
                    {

                        id = sa.id,
                        longDesc = sa.longDesc,
                        ActivityLongDesc = sa.ActivityLongDesc
                    });

                __result.SubActivity = new List<PromoSubActivityRes>();
                foreach (PromoSubActivityRes sa in __re.Read<PromoSubActivityRes>())
                    __result.SubActivity.Add(new PromoSubActivityRes()
                    {

                        id = sa.id,
                        longDesc = sa.longDesc,
                        SubActivityLongDesc = sa.SubActivityLongDesc
                    });

                __result.Attachments = new List<PromoAttachment>();
                foreach (PromoAttachment sa in __re.Read<PromoAttachment>())
                    __result.Attachments.Add(new PromoAttachment()
                    {
                        PromoId = sa.PromoId,
                        FileName = sa.FileName,
                        DocLink = sa.DocLink

                    });

                __result.ListApprovalStatus = new List<ApprovalRes>();
                foreach (ApprovalRes sa in __re.Read<ApprovalRes>())
                    __result.ListApprovalStatus.Add(new ApprovalRes()
                    {
                        StatusCode = sa.StatusCode,
                        StatusDesc = sa.StatusDesc
                    });

                __result.Mechanism = new List<MechanismSelect>();
                foreach (MechanismSelect sa in __re.Read<MechanismSelect>())
                    __result.Mechanism.Add(new MechanismSelect()
                    {
                        MechanismId = sa.MechanismId,
                        Mechanism = sa.Mechanism,
                        Notes = sa.Notes,
                        ProductId = sa.ProductId,
                        Product = sa.Product,
                        BrandId = sa.BrandId,
                        Brand = sa.Brand
                    });
                __result.Investment = __re.Read<object>().ToList();
                // group brand result E2#38
                __result.GroupBrand = __re.Read<object>().ToList();
                __budget.Add(__result);
            }
            return __budget.FirstOrDefault()!;
        }

        public async Task<ErrorMessageDto> ApprovalPromoWithSKP(PromoSKP promoSKP)
        {
            DataTable __ba = Tools.CastToDataTable(new SKPValidationDto(), null!);
            __ba.Rows.Add
            (
                promoSKP.PromoSKPHeader!.PromoId
                , promoSKP.PromoSKPHeader.SKPDraftAvail
                , promoSKP.PromoSKPHeader.SKPDraftAvailOn
                , promoSKP.PromoSKPHeader.SKPDraftAvailBy
                , promoSKP.PromoSKPHeader.SKPDraftAvailBfrAct60
                , promoSKP.PromoSKPHeader.SKPDraftAvailBfrAct60On
                , promoSKP.PromoSKPHeader.SKPDraftAvailBfrAct60By
                , promoSKP.PromoSKPHeader.PeriodMatch
                , promoSKP.PromoSKPHeader.PeriodMatchOn
                , promoSKP.PromoSKPHeader.PeriodMatchBy
                , promoSKP.PromoSKPHeader.InvestmentMatch
                , promoSKP.PromoSKPHeader.InvestmentMatchOn
                , promoSKP.PromoSKPHeader.InvestmentMatchBy
                , promoSKP.PromoSKPHeader.MechanismMatch
                , promoSKP.PromoSKPHeader.MechanismMatchOn
                , promoSKP.PromoSKPHeader.MechanismMatchBy
                , promoSKP.PromoSKPHeader.SKPSign7
                , promoSKP.PromoSKPHeader.SKPSign7On
                , promoSKP.PromoSKPHeader.SKPSign7By
                , promoSKP.PromoSKPHeader.EntityDraft
                , promoSKP.PromoSKPHeader.EntityDraftOn
                , promoSKP.PromoSKPHeader.EntityDraftBy
                , promoSKP.PromoSKPHeader.BrandDraft
                , promoSKP.PromoSKPHeader.BrandDraftOn
                , promoSKP.PromoSKPHeader.BrandDraftBy
                , promoSKP.PromoSKPHeader.PeriodDraft
                , promoSKP.PromoSKPHeader.PeriodDraftOn
                , promoSKP.PromoSKPHeader.PeriodDraftBy
                , promoSKP.PromoSKPHeader.ActivityDescDraft
                , promoSKP.PromoSKPHeader.ActivityDescDraftOn
                , promoSKP.PromoSKPHeader.ActivityDescDraftBy
                , promoSKP.PromoSKPHeader.MechanismDraft
                , promoSKP.PromoSKPHeader.MechanismDraftOn
                , promoSKP.PromoSKPHeader.MechanismDraftBy
                , promoSKP.PromoSKPHeader.InvestmentDraft
                , promoSKP.PromoSKPHeader.InvestmentDraftOn
                , promoSKP.PromoSKPHeader.InvestmentDraftBy
                , promoSKP.PromoSKPHeader.Entity
                , promoSKP.PromoSKPHeader.EntityOn
                , promoSKP.PromoSKPHeader.EntityBy
                , promoSKP.PromoSKPHeader.Brand
                , promoSKP.PromoSKPHeader.BrandOn
                , promoSKP.PromoSKPHeader.BrandBy
                , promoSKP.PromoSKPHeader.ActivityDesc
                , promoSKP.PromoSKPHeader.ActivityDescOn
                , promoSKP.PromoSKPHeader.ActivityDescBy

            );
            using IDbConnection conn = Connection;
            var __query = new DynamicParameters();
            __query.Add("@PromoSKP", __ba.AsTableValuedParameter());
            __query.Add("@promoid", promoSKP.promoid);
            __query.Add("@statuscode", promoSKP.statuscode);
            __query.Add("@notes", promoSKP.notes);
            __query.Add("@approvaldate", promoSKP.approvaldate);
            __query.Add("@useremail", promoSKP.useremail);

            conn.Open();
            var p = await conn.QueryAsync<ErrorMessageDto>("[dbo].[ip_approval_promo_withskp]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return p.FirstOrDefault()!;
        }


        //Promo Approval Recon
        public async Task<IList<PromoApprovalView>> GetPromoApprovalReconLP(string year, int category, int entity, int distributor, int BudgetParent, int channel, string userid)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@periode", year);
            __param.Add("@category", category);
            __param.Add("@entity", entity);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);

            var result = await conn.QueryAsync<PromoApprovalView>("ip_promo_approval_list_recon", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList();
        }

        public async Task<ErrorMessageDto> ApprovalPromoRecon(int promoid, string statuscode, string notes, string useremail)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            var __query = new DynamicParameters();
            __query.Add("@promoid", promoid);
            __query.Add("@statuscode", statuscode);
            __query.Add("@notes", notes);
            __query.Add("@approvaldate", TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone));
            __query.Add("@useremail", useremail);
            conn.Open();
            var p = await conn.QueryAsync<ErrorMessageDto>("[dbo].[ip_approval_promo_recon]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return p.FirstOrDefault()!;
        }

        public async Task<PromoReconV3> GetPromoReconV3(int Id, string LongDesc = "")
        {
            List<PromoReconV3> __budget = new();
            using (IDbConnection conn = Connection)
            {
                var __query = new DynamicParameters();
                __query.Add("@Id", Id);
                __query.Add("@LongDesc", LongDesc);
                using var __re = await conn.QueryMultipleAsync("[dbo].[ip_promo_v3_select_recon]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                PromoReconV3 __result = new()
                {
                    PromoHeader = __re.Read<PromoReconHeader>().FirstOrDefault()!,

                    Regions = new List<PromoRegionRes>()
                };
                foreach (PromoRegionRes r in __re.Read<PromoRegionRes>())
                    __result.Regions.Add(new PromoRegionRes()
                    {
                        flag = r.flag,
                        id = r.id,
                        longDesc = r.longDesc
                    });
                __result.Channels = new List<PromoChannelRes>();
                foreach (PromoChannelRes c in __re.Read<PromoChannelRes>())
                    __result.Channels.Add(new PromoChannelRes()
                    {
                        flag = c.flag,
                        id = c.id,
                        longDesc = c.longDesc,
                    });
                __result.SubChannels = new List<PromoSubChannelRes>();
                foreach (PromoSubChannelRes sc in __re.Read<PromoSubChannelRes>())
                    __result.SubChannels.Add(new PromoSubChannelRes()
                    {
                        flag = sc.flag,
                        id = sc.id,
                        longDesc = sc.longDesc,
                    });
                __result.Accounts = new List<PromoAccountRes>();
                foreach (PromoAccountRes a in __re.Read<PromoAccountRes>())
                    __result.Accounts.Add(new PromoAccountRes()
                    {
                        flag = a.flag,
                        id = a.id,
                        longDesc = a.longDesc,
                    });

                __result.SubAccounts = new List<PromoSubAccountRes>();
                foreach (PromoSubAccountRes sa in __re.Read<PromoSubAccountRes>())
                    __result.SubAccounts.Add(new PromoSubAccountRes()
                    {
                        flag = sa.flag,
                        id = sa.id,
                        longDesc = sa.longDesc,
                    });

                __result.Brands = new List<PromoBrandRes>();
                foreach (PromoBrandRes sa in __re.Read<PromoBrandRes>())
                    __result.Brands.Add(new PromoBrandRes()
                    {
                        flag = sa.flag,
                        id = sa.id,
                        longDesc = sa.longDesc,
                    });

                __result.Skus = new List<PromoProductRes>();
                foreach (PromoProductRes sa in __re.Read<PromoProductRes>())
                    __result.Skus.Add(new PromoProductRes()
                    {
                        flag = sa.flag,
                        id = sa.id,
                        longDesc = sa.longDesc,
                    });
                __result.Activity = new List<PromoActivityRes>();
                foreach (PromoActivityRes sa in __re.Read<PromoActivityRes>())
                    __result.Activity.Add(new PromoActivityRes()
                    {

                        id = sa.id,
                        longDesc = sa.longDesc,
                    });
                __result.SubActivity = new List<PromoSubActivityRes>();
                foreach (PromoSubActivityRes sa in __re.Read<PromoSubActivityRes>())
                    __result.SubActivity.Add(new PromoSubActivityRes()
                    {
                        id = sa.id,
                        longDesc = sa.longDesc,
                    });
                __result.Attachments = new List<PromoAttachment>();
                foreach (PromoAttachment sa in __re.Read<PromoAttachment>())
                    __result.Attachments.Add(new PromoAttachment()
                    {
                        PromoId = sa.PromoId,
                        FileName = sa.FileName,
                        DocLink = sa.DocLink
                    });

                __result.ListApprovalStatus = new List<ApprovalRes>();
                foreach (ApprovalRes sa in __re.Read<ApprovalRes>())
                    __result.ListApprovalStatus.Add(new ApprovalRes()
                    {
                        StatusCode = sa.StatusCode,
                        StatusDesc = sa.StatusDesc
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
                __result.Investments = new List<PromoReconInvestment>();
                foreach (PromoReconInvestment sa in __re.Read<PromoReconInvestment>())
                    __result.Investments.Add(new PromoReconInvestment()
                    {
                        Investment = sa.Investment,
                        NormalSales = sa.NormalSales,
                        IncrSales = sa.IncrSales,
                        TotSales = sa.TotSales,
                        Roi = sa.Roi,
                        CostRatio = sa.CostRatio
                    });

                __result.Mechanism = new List<MechanismSelect>();
                foreach (MechanismSelect sa in __re.Read<MechanismSelect>())
                    __result.Mechanism.Add(new MechanismSelect()
                    {
                        MechanismId = sa.MechanismId,
                        Mechanism = sa.Mechanism,
                        Notes = sa.Notes,
                        ProductId = sa.ProductId,
                        Product = sa.Product,
                        BrandId = sa.BrandId,
                        Brand = sa.Brand
                    });
                //Add:  AND Oct 11 2023 E2#38
                __result.GroupBrand = __re.Read<object>().ToList();
                __budget.Add(__result);
            }
            return __budget.FirstOrDefault()!;
        }

        public async Task<IList<object>> GetCategoryListforPromoApproval()
        {
            using IDbConnection conn = Connection;
            var __query = @"SELECT 
                                Id, 
                                ShortDesc [categoryShortDesc], 
                                LongDesc [categoryLongDesc]
                            FROM tbmst_category WHERE ISNULL(IsDeleted, 0) = 0";
            var __res = await conn.QueryAsync<object>(__query);
            return __res.ToList();
        }

        public async Task<IList<object>> GetCategoryListforReconApproval()
        {
            using IDbConnection conn = Connection;
            var __query = @"SELECT 
                                Id, 
                                ShortDesc [categoryShortDesc], 
                                LongDesc [categoryLongDesc]
                            FROM tbmst_category WHERE ISNULL(IsDeleted, 0) = 0";
            var __res = await conn.QueryAsync<object>(__query);
            return __res.ToList();
        }

        public async Task<DNGetByIdforPromoApproval> GetDNDetailbyIdforPromoApproval(int id)
        {
            try
            {
                DNGetByIdforPromoApproval __res = null!;
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __obj = await conn.QueryMultipleAsync("ip_debetnote_select", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    {
                        __res = __obj.Read<DNGetByIdforPromoApproval>().FirstOrDefault()!;
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

        public async Task<IList<object>> GetDNPaidforPromoApprovalRecon(int id)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@id", id);

            var result = await conn.QueryAsync<object>("ip_promo_recon_DNPaid", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList()!;
        }

        public async Task<IList<object>> GetDNClaimforPromoApprovalRecon(int id)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@id", id);

            var result = await conn.QueryAsync<object>("ip_promo_recon_DNClaim", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList()!;
        }

        public async Task<IEnumerable<DNMaterial>> GetTaxLevelforPromoApprovalRecon()
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
    }
}
