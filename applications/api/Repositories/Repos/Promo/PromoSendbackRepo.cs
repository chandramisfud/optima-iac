using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Entities.Models.PromoSendback;
using Repositories.Contracts.Promo;
using Repositories.Entities;
using Repositories.Entities.Models.PromoApproval;
using Repositories.Entities.Models;
using Repositories.Entities.Configuration;

namespace Repositories.Repos
{
    public class PromoSendbackRepository : IPromoSendbackRepository
    {
        readonly IConfiguration __config;
        public PromoSendbackRepository(IConfiguration config)
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

        public async Task<IList<InitiatorView>> GetPromoSendbackLP(string year, int entity, int distributor,
            int BudgetParent, int channel, string userid, int categoryId = 0)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@periode", year);
            __param.Add("@entity", entity);
            __param.Add("@category", categoryId);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);

            var result = await conn.QueryAsync<InitiatorView>("ip_promo_list_sendback", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
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

        public async Task<PromoReviseV3Dto> GetPromoSendbackById(int id)
        {
            PromoReviseV3Dto __result = new();
            using (IDbConnection conn = Connection)
            {
                var __query = new DynamicParameters();
                __query.Add("@Id", id);
                __query.Add("@LongDesc", string.Empty);
                using var __re = await conn.QueryMultipleAsync("[dbo].[ip_promo_v3_select]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                __result.PromoHeader = __re.Read<PromoRevise>().FirstOrDefault()!;
                __result.Regions = new List<PromoRegionRes>();
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
                //Add:  AND Oct 11 2023 E2#38
                __result.Investment = __re.Read<object>().ToList();
                __result.GroupBrand = __re.Read<object>().ToList();
            }
            return __result;
        }
        public async Task<PromoResponseDto> PromoSendback(PromoCreationDto promo)
        {
            try
            {
                DataTable __ba = Helper._castToDataTable(new PromoTypeDto(), null!);
                __ba.Rows.Add
                (
                    promo.PromoHeader!.promoId
                    , promo.PromoHeader.promoPlanId
                    , promo.PromoHeader.allocationId
                    , promo.PromoHeader.allocationRefId
                    , promo.PromoHeader.principalShortDesc
                    , promo.PromoHeader.categoryShortDesc
                    , promo.PromoHeader.budgetMasterId
                    , promo.PromoHeader.categoryId
                    , promo.PromoHeader.subCategoryId
                    , promo.PromoHeader.activityId
                    , promo.PromoHeader.subActivityId
                    , promo.PromoHeader.activityDesc
                    , promo.PromoHeader.startPromo
                    , promo.PromoHeader.endPromo
                    , promo.PromoHeader.mechanisme1
                    , promo.PromoHeader.mechanisme2
                    , promo.PromoHeader.mechanisme3
                    , promo.PromoHeader.mechanisme4
                    , promo.PromoHeader.investment
                    , promo.PromoHeader.normalSales
                    , promo.PromoHeader.incrSales
                    , promo.PromoHeader.roi
                    , promo.PromoHeader.costRatio
                    , promo.PromoHeader.statusApproval
                    , promo.PromoHeader.notes
                    , promo.PromoHeader.tsCoding
                    , promo.PromoHeader.createOn
                    , promo.PromoHeader.createBy
                    , promo.PromoHeader.initiator_notes
                    , promo.PromoHeader.createdEmail
                    , promo.PromoHeader.modifReason

                );

                DataTable __child = Helper._castToDataTable(new PromoChildsTypeDto(), null!);
                DataTable __reg = __child.Clone();
                foreach (Region v in promo.Regions!)
                    __reg.Rows.Add(null, v.id, 1);

                DataTable __chan = __child.Clone();
                foreach (Channel v in promo.Channels!)
                    __chan.Rows.Add(null, v.id, 1);

                DataTable __subchan = __child.Clone();
                foreach (SubChannel v in promo.SubChannels!)
                    __subchan.Rows.Add(v.id, v.id, 1);

                DataTable __acc = __child.Clone();
                foreach (Account v in promo.Accounts!)
                    __acc.Rows.Add(v.id, v.id, 1);

                DataTable __subacc = __child.Clone();
                foreach (SubAccount v in promo.SubAccounts!)
                    __subacc.Rows.Add(v.id, v.id, 1);

                DataTable __brand = __child.Clone();
                foreach (Brand v in promo.Brands!)
                    __brand.Rows.Add(null, v.id, 1);

                DataTable __sku = __child.Clone();
                foreach (Product v in promo.Skus!)
                    __sku.Rows.Add(v.id, v.id, 1);

                DataTable __attachment = Helper._castToDataTable(new PromoAttachmentStore(), null!);
                foreach (PromoAttachmentStore v in promo.promoAttachment!)
                    __attachment.Rows.Add(v.FileName, v.DocLink);

                DataTable __mec = Helper._castToDataTable(new MechanismType(), null!);
                foreach (MechanismType v in promo.Mechanisms!)
                    __mec.Rows.Add(v.id, v.mechanism, v.notes, v.productId, v.product, v.brandId, v.brand);

                using IDbConnection conn = Connection;
                var __query = new DynamicParameters();
                __query.Add("@IsNew", false);
                __query.Add("@Promo", __ba.AsTableValuedParameter());
                __query.Add("@Region", __reg.AsTableValuedParameter());
                __query.Add("@Channel", __chan.AsTableValuedParameter());
                __query.Add("@Subchannel", __subchan.AsTableValuedParameter());
                __query.Add("@Account", __acc.AsTableValuedParameter());
                __query.Add("@Subaccount", __subacc.AsTableValuedParameter());
                __query.Add("@Brand", __brand.AsTableValuedParameter());
                __query.Add("@Sku", __sku.AsTableValuedParameter());
                __query.Add("@attachment", __attachment.AsTableValuedParameter());
                __query.Add("@Mechanism", __mec.AsTableValuedParameter());

                conn.Open();
                var p = await conn.QueryAsync<PromoResponseDto>("[dbo].[ip_promo_v4_insert]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        //RECON

        /// <summary>
        ///  rec
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="LongDesc"></param>
        /// <returns></returns>
        public async Task<PromoReconV3> GetPromoSendbackReconById(int Id, string LongDesc)
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
                __result.PromoConfigItem = __re.Read<object>().ToList();

                __budget.Add(__result);
            }
            return __budget.FirstOrDefault()!;
        }

        public async Task<IList<InitiatorView>> GetPromoSendbackReconLP(string year, int entity, int distributor,
            int BudgetParent, int channel, string userid, int categoryId = 0)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@periode", year);
            __param.Add("@entity", entity);
            __param.Add("@category", categoryId);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);

            var result = await conn.QueryAsync<InitiatorView>("ip_promo_list_sendback_recon", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList();
        }


        public async Task<List<object>> GetAttributeByPromo(string userid, int[] arrayParent, string attribute, int promoId)
        {
            using IDbConnection conn = Connection;
            DataTable __parent = new("ArrayIntType");
            __parent.Columns.Add("keyid");
            foreach (int v in arrayParent)
                __parent.Rows.Add(v);

            var __query = new DynamicParameters();

            __query.Add("@userid", userid);
            __query.Add("@attribute", attribute);
            __query.Add("@parent", __parent.AsTableValuedParameter());
            __query.Add("@promoid", promoId);

            var res = await conn.QueryAsync<object>("ip_getattribute_bypromoid", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);

            return res.ToList();
        }
        public async Task<List<object>> GetAttributeByParent(int budgetid, int[] arrayParent, string attribute, string status)
        {
            using IDbConnection conn = Connection;
            DataTable __parent = new("ArrayIntType");
            __parent.Columns.Add("keyid");
            foreach (int v in arrayParent)
                __parent.Rows.Add(v);

            var __query = new DynamicParameters();

            __query.Add("@budgetid", budgetid);
            __query.Add("@attribute", attribute);
            __query.Add("@parent", __parent.AsTableValuedParameter());
            __query.Add("@status", status);

            var res = await conn.QueryAsync<object>("ip_getattribute_byparent_new", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);

            return res.ToList();
        }

        public async Task<ErrorMessageDto> PromoSendbackRecon(PromoReconCreationDto promo)
        {
            List<PromoReconV4TypeDto> lsPromoHeader = new()
            {
                promo.PromoHeader!
            };

            DataTable __ba = Helper._castToDataTableV2(lsPromoHeader);
            DataTable __child = Helper._castToDataTable(new PromoChildsTypeDto(), null!);
            DataTable __reg = __child.Clone();
            foreach (Region v in promo.Regions!)
                __reg.Rows.Add(null, v.id, 1);

            DataTable __chan = __child.Clone();
            foreach (Channel v in promo.Channels!)
                __chan.Rows.Add(null, v.id, 1);

            DataTable __subchan = __child.Clone();
            foreach (SubChannel v in promo.SubChannels!)
                __subchan.Rows.Add(v.id, v.id, 1);

            DataTable __acc = __child.Clone();
            foreach (Account v in promo.Accounts!)
                __acc.Rows.Add(v.id, v.id, 1);

            DataTable __subacc = __child.Clone();
            foreach (SubAccount v in promo.SubAccounts!)
                __subacc.Rows.Add(v.id, v.id, 1);

            DataTable __brand = __child.Clone();
            foreach (Brand v in promo.Brands!)
                __brand.Rows.Add(null, v.id, 1);

            DataTable __sku = __child.Clone();
            foreach (Product v in promo.Skus!)
                __sku.Rows.Add(v.id, v.id, 1);

            DataTable __mec = Helper._castToDataTable(new MechanismType(), null!);
            foreach (MechanismType v in promo.Mechanisms!)
                __mec.Rows.Add(v.id, v.mechanism, v.notes, v.productId, v.product, v.brandId, v.brand);

            using IDbConnection conn = Connection;
            var __query = new DynamicParameters();
            __query.Add("@IsNew", false);
            __query.Add("@Promo", __ba.AsTableValuedParameter());
            __query.Add("@Region", __reg.AsTableValuedParameter());
            __query.Add("@Channel", __chan.AsTableValuedParameter());
            __query.Add("@Subchannel", __subchan.AsTableValuedParameter());
            __query.Add("@Account", __acc.AsTableValuedParameter());
            __query.Add("@Subaccount", __subacc.AsTableValuedParameter());
            __query.Add("@Brand", __brand.AsTableValuedParameter());
            __query.Add("@Sku", __sku.AsTableValuedParameter());
            __query.Add("@Mechanism", __mec.AsTableValuedParameter());
            __query.Add("@Reconciled", promo.Reconciled);
            __query.Add("@ReconciledUpd", promo.ReconciledUpd);

            conn.Open();
            var p = await conn.QueryAsync<ErrorMessageDto>("[dbo].[ip_promo_v4_insert_recon]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return p.FirstOrDefault()!;
        }
        public async Task<IList<object>> GetCategoryListforPromoSendBack()
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
        public async Task<IList<object>> GetCategoryListforPromoReconSendBack()
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

        public async Task<IList<PromoSourceBudgetDto>> GetPromoSendbackSourceBudget(string year, int entity, int distributor, int subCategory, int activity, int subActivity, int[] arrayRegion, int[] arrayChannel, int[] arraySubChannel, int[] arrayAccount, int[] arraySubAccount, int[] arrayBrand, int[] arraySKU, string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __region = new("ArrayIntType");
                __region.Columns.Add("keyid");
                foreach (int v in arrayRegion)
                    __region.Rows.Add(v);

                DataTable __channel = new("ArrayIntType");
                __channel.Columns.Add("keyid");
                foreach (int v in arrayChannel)
                    __channel.Rows.Add(v);

                DataTable __subchannel = new("ArrayIntType");
                __subchannel.Columns.Add("keyid");
                foreach (int v in arraySubChannel)
                    __subchannel.Rows.Add(v);

                DataTable __account = new("ArrayIntType");
                __account.Columns.Add("keyid");
                foreach (int v in arrayAccount)
                    __account.Rows.Add(v);

                DataTable __subaccount = new("ArrayIntType");
                __subaccount.Columns.Add("keyid");
                foreach (int v in arraySubAccount)
                    __subaccount.Rows.Add(v);

                DataTable __brand = new("ArrayIntType");
                __brand.Columns.Add("keyid");
                foreach (int v in arrayBrand)
                    __brand.Rows.Add(v);

                DataTable __sku = new("ArrayIntType");
                __sku.Columns.Add("keyid");
                foreach (int v in arraySKU)
                    __sku.Rows.Add(v);

                var __param = new DynamicParameters();
                __param.Add("@periode", year);
                __param.Add("@entity", entity);
                __param.Add("@distributor", distributor);
                __param.Add("@userid", profileId);
                __param.Add("@region", __region.AsTableValuedParameter());
                __param.Add("@channel", __channel.AsTableValuedParameter());
                __param.Add("@subchannel", __subchannel.AsTableValuedParameter());
                __param.Add("@account", __account.AsTableValuedParameter());
                __param.Add("@subaccount", __subaccount.AsTableValuedParameter());
                __param.Add("@brand", __brand.AsTableValuedParameter());
                __param.Add("@product", __sku.AsTableValuedParameter());
                __param.Add("@subcategory", subCategory);
                __param.Add("@activity", activity);
                __param.Add("@subactivity", subActivity);

                conn.Open();
                var p = await conn.QueryAsync<PromoSourceBudgetDto>("[dbo].[ip_budgetallocation_list_for_planning]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}
