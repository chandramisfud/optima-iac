using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Contracts;
using Dapper;
using Repositories.Entities.Report;
//using Repositories.Entities.Models.DN;
using Repositories.Entities.Models.PromoApproval;
using Repositories.Entities.Models;
using Repositories.Entities.Models.Promo;
using Repositories.Entities;

namespace Repositories.Repos
{
    public class PromoSKPValidationRepository : IPromoSKPValidationRepository
    {
        readonly IConfiguration __config;
        public PromoSKPValidationRepository(IConfiguration config)
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

        public async Task<IList<SKPValidationView>> GetPromoListSKPFlagging(string periode, int entity,
            int distributor, int BudgetParent, int channel, bool cancelstatus,
            DateTime start_from, DateTime start_to, int status, string userid)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@periode", periode);
            __param.Add("@datenow", TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone));
            __param.Add("@entity", entity);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);
            __param.Add("@cancelstatus", cancelstatus);
            __param.Add("@start_from", start_from);
            __param.Add("@start_to", start_to);
            __param.Add("@status", status);


            var result = await conn.QueryAsync<SKPValidationView>("ip_promo_list_skpflagging", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList();
        }

        public async Task<IList<SKPValidationEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<SKPValidationEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SKPValidationDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
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
                var child = await conn.QueryAsync<SKPValidationDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SKPValidationChannelList>> GetChannelList(string userid, int[] arrayParent)
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
                var child = await conn.QueryAsync<SKPValidationChannelList>("ip_getattribute_bymapping", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoSelectSKP> GetPromoWithSKP(int Id, string LongDesc)
        {
            PromoSelectSKP __result = new();
            using (IDbConnection conn = Connection)
            {
                var __query = new DynamicParameters();
                __query.Add("@Id", Id);
                __query.Add("@LongDesc", LongDesc);
                using var __re = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_withskp]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                __result.PromoHeader = __re.Read<PromoRevise>().FirstOrDefault();
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
                        DistributorDraft = si.DistributorDraft,
                        DistributorDraftOn = si.DistributorDraftOn,
                        DistributorDraftBy = si.DistributorDraftBy,
                        Distributor = si.Distributor,
                        DistributorOn = si.DistributorOn,
                        DistributorBy = si.DistributorBy,
                        ChannelDraft = si.ChannelDraft,
                        ChannelDraftOn = si.ChannelDraftOn,
                        ChannelDraftBy = si.ChannelDraftBy,
                        Channel = si.Channel,
                        ChannelOn = si.ChannelOn,
                        ChannelBy = si.ChannelBy,
                        StoreNameDraft = si.StoreNameDraft,
                        StoreNameDraftOn = si.StoreNameDraftOn,
                        StoreNameDraftBy = si.StoreNameDraftBy,
                        StoreName = si.StoreName,
                        StoreNameOn = si.StoreNameOn,
                        StoreNameBy = si.StoreNameBy,
                        skpstatus = si.skpstatus,
                        skp_notes = si.skp_notes
                    });

                __result.Mechanisms = new List<MechanismSelect>();
                foreach (MechanismSelect sa in __re.Read<MechanismSelect>())
                    __result.Mechanisms.Add(new MechanismSelect()
                    {
                        MechanismId = sa.MechanismId,
                        Mechanism = sa.Mechanism,
                        Notes = sa.Notes,
                        ProductId = sa.ProductId,
                        Product = sa.Product,
                        BrandId = sa.BrandId,
                        Brand = sa.Brand
                    });
                // read investment
                List<object> invests = __re.Read<object>().ToList();
                // read groupBreand
                __result.GroupBrand = __re.Read<object>().ToList();
            }
            return __result;
        }

        public async Task<ErrorMessageDto> UpdateApprovalPromoWithSKPFlagging(PromoSKP promoSKP)
        {
            DataTable __ba = Helper._castToDataTable(new SKPValidationDto(), null!);
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
                , promoSKP.PromoSKPHeader.DistributorDraft
                , promoSKP.PromoSKPHeader.DistributorDraftOn
                , promoSKP.PromoSKPHeader.DistributorDraftBy
                , promoSKP.PromoSKPHeader.Distributor
                , promoSKP.PromoSKPHeader.DistributorOn
                , promoSKP.PromoSKPHeader.DistributorBy
                , promoSKP.PromoSKPHeader.ChannelDraft
                , promoSKP.PromoSKPHeader.ChannelDraftOn
                , promoSKP.PromoSKPHeader.ChannelDraftBy
                , promoSKP.PromoSKPHeader.Channel
                , promoSKP.PromoSKPHeader.ChannelOn
                , promoSKP.PromoSKPHeader.ChannelBy
                , promoSKP.PromoSKPHeader.StoreNameDraft
                , promoSKP.PromoSKPHeader.StoreNameDraftOn
                , promoSKP.PromoSKPHeader.StoreNameDraftBy
                , promoSKP.PromoSKPHeader.StoreName
                , promoSKP.PromoSKPHeader.StoreNameOn
                , promoSKP.PromoSKPHeader.StoreNameBy
                , promoSKP.PromoSKPHeader.skpstatus
                , promoSKP.PromoSKPHeader.skp_notes

            );
            using IDbConnection conn = Connection;
            var __query = new DynamicParameters();
            __query.Add("@PromoSKP", __ba.AsTableValuedParameter());
            __query.Add("@promoid", promoSKP.promoid);
            __query.Add("@statuscode", promoSKP.statuscode);
            __query.Add("@notes", promoSKP.notes);
            __query.Add("@approvaldate", promoSKP.approvaldate);
            conn.Open();
            var p = await conn.QueryAsync<ErrorMessageDto>("[dbo].[ip_promo_skpflagging]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return p.FirstOrDefault()!;
        }

        public async Task<SKPValidationLandingPage> GetSKPValidationDownload(string period, int entityId, int distributorId, int budgetParentId,
            int channelId, string profileId, int cancelStatus, string startFrom, string startTo, int submissionParam, int status,
            string keyword, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10)
        {
            SKPValidationLandingPage res = new();
            using (IDbConnection conn = Connection)
                try
                {
                    var startData = pageNum * dataDisplayed;
                    keyword = (keyword) ?? "";

                    var __param = new DynamicParameters();
                    __param.Add("@periode", period);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@userid", profileId);
                    __param.Add("@cancelstatus", cancelStatus);
                    __param.Add("@start_from", startFrom);
                    __param.Add("@start_to", startTo);
                    __param.Add("@submission_param", submissionParam);
                    __param.Add("@status", status);
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@filter", "");
                    __param.Add("@txtSearch", keyword);

                    var __object = await conn.QueryMultipleAsync("ip_skp_validate_report_p", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    res.Data = __object.Read<SKPValidationData>().ToList();

                    var count = __object.ReadSingle<SKPValidationRecordCount>();

                    res.totalCount = count.recordsTotal;
                    res.filteredCount = count.recordsFiltered;

                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
            return res;
        }
    }
}
