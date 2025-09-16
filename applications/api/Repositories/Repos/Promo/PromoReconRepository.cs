using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Repositories.Entities;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Configuration;


namespace Repositories.Repos
{
    public partial class PromoReconRepository : IPromoReconRepository
    {
        readonly IConfiguration __config;
        public PromoReconRepository(IConfiguration config)
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

        public async Task<BaseLP2> GetPromoReconLandingPage(string year, int entity, int distributor, string userid,
            int categoryId, int BudgetParent, int channel, DateTime start_from, DateTime start_to,
            string keyword, int pageNumber, int pageSize)
        {
            using IDbConnection conn = Connection;
            BaseLP2 res;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@periode", year);
                __param.Add("@entity", entity);
                __param.Add("@category", categoryId);
                __param.Add("@distributor", distributor);
                __param.Add("@budgetparent", BudgetParent);
                __param.Add("@channel", channel);
                __param.Add("@userid", userid);
                __param.Add("@cancelstatus", 0);
                __param.Add("@create_from", new DateTime(1999, 12, 1));
                __param.Add("@create_to", start_to.AddMonths(12));
                __param.Add("@start_from", start_from);
                __param.Add("@start_to", start_to);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                __param.Add("@filter", "");
                __param.Add("@txtsearch", keyword);

                var __res = await conn.QueryMultipleAsync("ip_promo_list_reconciliation_p", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                var __data = __res.Read<Entities.Models.PromoReconciliationPage>().Cast<object>().ToList();
                res = __res.ReadSingle<BaseLP2>();
                res.Data = __data;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<IList<Entities.Models.PromoReconciliationPage>> GetPromoReconDownload(string year,
            DateTime start_from, DateTime start_to, string profileId = "",
            int categoryId = 0, int entity = 0, int channel = 0, int distributor = 0)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@periode", year);
                __param.Add("@entity", entity);
                __param.Add("@category", categoryId);
                __param.Add("@distributor", distributor);
                __param.Add("@budgetparent", 0);
                __param.Add("@channel", 0);
                __param.Add("@userid", profileId);
                __param.Add("@cancelstatus", 0);
                __param.Add("@create_from", new DateTime(1999, 12, 1));
                __param.Add("@create_to", start_to.AddMonths(12));
                __param.Add("@start_from", start_from);
                __param.Add("@start_to", start_to);
                __param.Add("@txtSearch", null);
                __param.Add("@filter", "");
                __param.Add("@start", 0);
                __param.Add("@length", -1);

                conn.Open();
                var p = await conn.QueryAsync<Entities.Models.PromoReconciliationPage>("[dbo].[ip_promo_list_reconciliation_p]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoReconChannelByPromoId(int promoId, string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                __parent.Rows.Add(0);

                var __param = new DynamicParameters();
                __param.Add("@userid", profileId);
                __param.Add("@attribute", "channel");
                __param.Add("@parent", __parent.AsTableValuedParameter());
                __param.Add("@promoid", promoId);

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bypromoid]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoReconSubChannelByPromoId(int promoId, int[] arrayParent, string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __param = new DynamicParameters();
                __param.Add("@userid", profileId);
                __param.Add("@attribute", "subchannel");
                __param.Add("@parent", __parent.AsTableValuedParameter());
                __param.Add("@promoid", promoId);

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bypromoid]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoReconAccountByPromoId(int promoId, int[] arrayParent, string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __param = new DynamicParameters();
                __param.Add("@userid", profileId);
                __param.Add("@attribute", "account");
                __param.Add("@parent", __parent.AsTableValuedParameter());
                __param.Add("@promoid", promoId);

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bypromoid]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoReconSubAccountByPromoId(int promoId, int[] arrayParent, string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __param = new DynamicParameters();
                __param.Add("@userid", profileId);
                __param.Add("@attribute", "subaccount");
                __param.Add("@parent", __parent.AsTableValuedParameter());
                __param.Add("@promoid", promoId);

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bypromoid]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoResponseDto> PromoReconUpdate(PromoReconCreationDto promo)
        {
            DataTable __ba = Helper._castToDataTable(new PromoReconV4TypeDto(), null!);
            __ba.Rows.Add
            (
                promo.PromoHeader!.PromoId
                , promo.PromoHeader.PromoPlanId
                , promo.PromoHeader.AllocationId
                , promo.PromoHeader.AllocationRefId
                , promo.PromoHeader.PrincipalShortDesc
                , promo.PromoHeader.CategoryShortDesc
                , promo.PromoHeader.BudgetMasterId
                , promo.PromoHeader.CategoryId
                , promo.PromoHeader.SubCategoryId
                , promo.PromoHeader.ActivityId
                , promo.PromoHeader.SubActivityId
                , promo.PromoHeader.ActivityDesc
                , promo.PromoHeader.StartPromo
                , promo.PromoHeader.EndPromo
                , promo.PromoHeader.Mechanisme1
                , promo.PromoHeader.Mechanisme2
                , promo.PromoHeader.Mechanisme3
                , promo.PromoHeader.Mechanisme4
                , promo.PromoHeader.Investment
                , promo.PromoHeader.NormalSales
                , promo.PromoHeader.IncrSales
                , promo.PromoHeader.Roi
                , promo.PromoHeader.CostRatio
                , promo.PromoHeader.StatusApproval
                , promo.PromoHeader.Notes
                , promo.PromoHeader.TsCoding
                , promo.PromoHeader.CreateOn
                , promo.PromoHeader.CreateBy
                , promo.PromoHeader.initiator_notes
                , promo.PromoHeader.actual_sales
                , promo.PromoHeader.CreatedEmail
                , promo.PromoHeader.ModifReason

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
            var p = await conn.QueryAsync<PromoResponseDto>("[dbo].[ip_promo_v4_insert_recon]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return p.FirstOrDefault()!;

        }

        public async Task<Entities.Models.PromoReconByIdDto> GetPromoReconById(int id)
        {
            List<PromoReconByIdDto> __promo = new();
            using (IDbConnection conn = Connection)
            {
                var __query = new DynamicParameters();
                __query.Add("@Id", id);
                __query.Add("@LongDesc", "");

                using var __re = await conn.QueryMultipleAsync("[dbo].[ip_promo_v3_select_recon]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                PromoReconByIdDto __result = new()
                {
                    PromoHeader = __re.Read<PromoReconHeader>().FirstOrDefault()!,

                    Regions = new List<PromoAttibuteById>()
                };
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
                __result.PromoConfigItem = __re.Read<object>().ToList();
                __promo.Add(__result);
            }
            return __promo.FirstOrDefault()!;

        }

        public async Task<bool> PromoReconAttachment(int promoId, string docLink, string fileName, string createBy)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            bool res = false;
            using (IDbConnection conn = Connection)
            {
                var __res = await conn.ExecuteAsync(@"
                DELETE FROM [dbo].[tbtrx_promo_doclink] WHERE PromoId=@promoid AND DocLink=@doclink;
                INSERT INTO tbtrx_promo_doclink(PromoId,DocLink,FileName,CreateOn,Createby)
                VALUES(@promoid,@doclink,@filename,@createon,@createby)"
                , new
                {
                    promoid = promoId,
                    doclink = docLink,
                    filename = fileName,
                    createon = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    createby = createBy
                });
                res = __res > 0;
            }
            return res;
        }

        public async Task<bool> PromoReconDeleteAttachment(int promoId, string docLink)
        {
            bool res = false;
            using (IDbConnection conn = Connection)
            {
                var __res = await conn.ExecuteAsync(@"
                    DELETE FROM tbtrx_promo_doclink 
                    WHERE PromoId=@id and doclink=@docLink"
                , new
                {
                    id = promoId,
                    doclink = docLink

                });
                res = __res > 0;
            }
            return res;
        }

        public async Task<IList<object>> GetCategoryListforPromoRecon()
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

        public async Task<IList<object>> GetPromoReconSubActivitybyActivityId(int activityId, string isDeleted)
        {
            using IDbConnection conn = Connection;
            var __query = @"SELECT
                            m.id, m.RefId [refId], m.LongDesc [subActivity], tc.Id [categoryId], tc.LongDesc category, ts2.Id [subCategoryId], ts2.LongDesc subCategory,ta.Id [activityId], ta.LongDesc activity, 
                            tst.Id [subActivityTypeId], tst.LongDesc subActivityType, ISNULL(tmpps.AllowEdit,0) [allowEdit], ISNULL(m.IsDeleted,0) [isDeleted]
                            FROM tbmst_subactivity m
                            left join tbset_map_promorecon_period_subactivity tmpps on m.Id = tmpps.SubActivityId
                            INNER JOIN tbmst_subactivity_type tst ON m.SubActivityTypeId = tst.Id  
                            INNER JOIN tbmst_activity ta ON m.ActivityId = ta.id
                            INNER JOIN tbmst_subcategory ts2 ON m.SubCategoryId = ts2.Id
                            INNER JOIN tbmst_category tc ON m.CategoryId = tc.Id
                            WHERE m.ActivityId = @activityId AND IIF(@isDeleted = 'all', '0', m.IsDeleted)=IIF(@isDeleted = 'all', '0', 0)";
            var __res = await conn.QueryAsync<object>(__query, new { activityId = activityId, isDeleted = isDeleted });
            return __res.ToList();
        }

        public async Task<List<object>> GetPromoSubactivity(string activityId)
        {
            using IDbConnection conn = Connection;
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            var __query = @"SELECT * FROM vw_map_promorecon_subactivity_lp WHERE ActivityId  = @activityId";
            var __res = await conn.QueryAsync<object>(__query, new { activityId = activityId });
            return __res.ToList();
        }
    }
}
