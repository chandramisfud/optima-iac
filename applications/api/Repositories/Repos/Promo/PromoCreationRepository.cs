using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Repositories.Entities;
using Repositories.Contracts;
using Repositories.Entities.Models;
using System.Reflection;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Microsoft.AspNetCore.Http.HttpResults;
using Repositories.Entities.Dtos;
using Entities;

namespace Repositories.Repos
{
    public partial class PromoCreationRepository : IPromoCreationRepository
    {
        readonly IConfiguration __config;
        public PromoCreationRepository(IConfiguration config)
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

        public async Task<BaseLP2> GetPromoCreationLandingPage(string year, int entity, int distributor,
            int categoryId, string profileId,
            string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            using IDbConnection conn = Connection;
            BaseLP2 res;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@periode", year);
                __param.Add("@entity", entity);
                __param.Add("@distributor", distributor);
                __param.Add("@category", categoryId);
                __param.Add("@budgetparent", 0);
                __param.Add("@channel", 0);
                __param.Add("@userid", profileId);
                __param.Add("@cancelstatus", 0);
                __param.Add("@txtSearch", keyword);
                __param.Add("@sort", sortDirection);
                __param.Add("@order", sortColumn);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);

                var __res = await conn.QueryMultipleAsync("ip_promo_v4_list_creation_p", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
//                var __data = __res.Read<PromoCreationLPDto>().Cast<object>().ToList();
                List<PromoCreationLPDto> __data = __res.Read<PromoCreationLPDto>().ToList();

                res = __res.ReadSingle<BaseLP2>();
                res.Data = __data.Cast<object>().ToList(); 
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<object> GetPromoCreationDownload(string year, int entity = 0, int distributor = 0,
            int categoryId = 0, string profileId = "")
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@periode", year);
                __param.Add("@category", categoryId);
                __param.Add("@entity", entity);
                __param.Add("@distributor", distributor);
                __param.Add("@userid ", profileId);

                conn.Open();
                var p = await conn.QueryAsync<object>("[dbo].[ip_promo_creation_download]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<PromoSourcePlanningDto>> GetPromoCreationSourcePlanning(string year, int entity, int distributor, string profileId)
        {
            List<PromoSourcePlanningDto> __promoSourcePlanning = new();
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@periode", year);
                __param.Add("@entity", entity);
                __param.Add("@distributor", distributor);
                __param.Add("@userid", profileId);

                using (var __res = await conn.QueryMultipleAsync("[dbo].[ip_promo_planning_v3_get_sourceforpromo]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180))
                {
                    __promoSourcePlanning = __res.Read<PromoSourcePlanningDto>().ToList();

                }
                return __promoSourcePlanning;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<PromoSourceBudgetDto>> GetPromoCreationSourceBudget(
            string year, int entity, int distributor, int subCategory, int activity, int subActivity,
            int[] arrayRegion, int[] arrayChannel, int[] arraySubChannel, int[] arrayAccount, int[] arraySubAccount, int[] arrayBrand, int[] arraySKU, string profileId
            )
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

        public async Task<IList<BaseDropDownList>> GetPromoCreationChannel(string profileId)
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

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bymapping]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoCreationSubChannel(int[] arrayParent, string profileId)
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

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bymapping]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoCreationAccount(int[] arrayParent, string profileId)
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

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bymapping]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoCreationSubAccount(int[] arrayParent, string profileId)
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

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bymapping]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoCreationChannelByPromoId(int promoId, string profileId)
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

        public async Task<IList<BaseDropDownList>> GetPromoCreationSubChannelByPromoId(int promoId, int[] arrayParent, string profileId)
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

        public async Task<IList<BaseDropDownList>> GetPromoCreationAccountByPromoId(int promoId, int[] arrayParent, string profileId)
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

        public async Task<IList<BaseDropDownList>> GetPromoCreationSubAccountByPromoId(int promoId, int[] arrayParent, string profileId)
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

        public async Task<PromoCreationSKPDraftDto> GetPromoCreationSKPDraft(int id)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@Id", id);

                conn.Open();
                var p = await conn.QueryAsync<PromoCreationSKPDraftDto>("[dbo].[ip_skp_draft]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
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

        public async Task<PromoResponseDto> PromoCreationCreate(PromoCreationDto promo)
        {

            DataTable __ba = _castToDataTable(new PromoTypeDto(), null!);
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

            DataTable __child = _castToDataTable(new PromoChildsTypeDto(), null!);
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

            DataTable __attachment = _castToDataTable(new PromoAttachmentStore(), null!);
            foreach (PromoAttachmentStore v in promo.promoAttachment!)
                __attachment.Rows.Add(v.FileName, v.DocLink);

            DataTable __mec = _castToDataTable(new MechanismType(), null!);
            foreach (MechanismType v in promo.Mechanisms!)
                __mec.Rows.Add(v.id, v.mechanism, v.notes, v.productId, v.product, v.brandId, v.brand);

            using IDbConnection conn = Connection;
            var __query = new DynamicParameters();
            __query.Add("@IsNew", true);
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

        public async Task<PromoResponseDto> PromoCreationUpdate(PromoCreationDto promo)
        {
            try
            {
                DataTable __ba = _castToDataTable(new PromoTypeDto(), null!);
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

                DataTable __child = _castToDataTable(new PromoChildsTypeDto(), null!);
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

                DataTable __attachment = _castToDataTable(new PromoAttachmentStore(), null!);
                foreach (PromoAttachmentStore v in promo.promoAttachment!)
                    __attachment.Rows.Add(v.FileName, v.DocLink);

                DataTable __mec = _castToDataTable(new MechanismType(), null!);
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
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoCreationByIdDto> GetPromoCreationById(int id)
        {
            List<PromoCreationByIdDto> __promo = new();
            using (IDbConnection conn = Connection)
            {
                var __query = new DynamicParameters();
                __query.Add("@Id", id);
                __query.Add("@LongDesc", "");

                using var __re = await conn.QueryMultipleAsync("[dbo].[ip_promo_v3_select]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                PromoCreationByIdDto __result = new()
                {
                    PromoHeader = __re.Read<PromoHeader>().FirstOrDefault()!,
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

                __result.Investment = __re.Read<object>().ToList();
                // group brand result E2#38
                __result.GroupBrand = __re.Read<object>().ToList();

                __promo.Add(__result);
            }
            return __promo.FirstOrDefault()!;
        }

        public async Task<PromoExistDto> GetPromoExist(string period, string activityDesc, int[] arrayChannel, int[] arrayAccount, string startPromo, string endPromo)
        {
            using IDbConnection conn = Connection;
            try
            {

                DataTable __channel = new("ArrayIntType");
                __channel.Columns.Add("keyid");
                foreach (int v in arrayChannel)
                    __channel.Rows.Add(v);


                DataTable __account = new("ArrayIntType");
                __account.Columns.Add("keyid");
                foreach (int v in arrayAccount)
                    __account.Rows.Add(v);

                var __param = new DynamicParameters();
                __param.Add("@periode", period);
                __param.Add("@ActivityDesc", activityDesc);
                __param.Add("@Channel", __channel.AsTableValuedParameter());
                __param.Add("@Account", __account.AsTableValuedParameter());
                __param.Add("@startPromo", startPromo);
                __param.Add("@endPromo", endPromo);

                conn.Open();
                var p = await conn.QueryAsync<PromoExistDto>("[dbo].[ip_promo_exist]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetPromoExistDC(string period, int distributor, int subActivity, int subActivityType,
            string startPromo, string endPromo)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@periode", period);
                __param.Add("@distributorId", distributor);
                __param.Add("@subActivityId", subActivity);
                __param.Add("@subActivityTypeId", subActivityType);
                __param.Add("@startPromo", startPromo);
                __param.Add("@endPromo", endPromo);

                conn.Open();
                var p = await conn.QueryAsync<object>("[dbo].[ip_promo_exist_dc]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<bool> PromoCreationAttachment(int promoId, string docLink, string fileName, string createBy)
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

        public async Task<bool> PromoDeleteAttachment(int promoId, string docLink)
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

        public async Task<LatePromoDto> GetLatePromoDays()
        {
            using IDbConnection conn = Connection;
            try
            {
                conn.Open();
                var p = await conn.QueryAsync<LatePromoDto>("[dbo].[ip_conf_latepromocreation]", commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<PromoResponseDto> PromoCancelRequest(int promoId, string profileId, string notes, string reqEmail)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@promoid", promoId);
                __param.Add("@userid", profileId);
                __param.Add("@notes", notes);
                __param.Add("@requestEmail", reqEmail);

                conn.Open();
                var p = await conn.QueryAsync<PromoResponseDto>("[dbo].[ip_promo_cancel_request]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<CancelReasonDto>> GetCancelReason()
        {
            using IDbConnection conn = Connection;
            try
            {
                conn.Open();
                var p = await conn.QueryAsync<CancelReasonDto>("[dbo].[ip_mst_cancelreason_list]", commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<IList<object>> GetGroupBrandByEntity(int entityId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __res = await conn.QueryAsync<object>(@"
                        SELECT id, longDesc
                        FROM tbmst_brand_group 
                        where PrincipalId = " + entityId);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<object>> GetBrandByGroupId(int grpBrandId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __res = await conn.QueryAsync<object>(@"
                        SELECT brnd.id , brnd.longDesc 
                        FROM tbmst_brand brnd
                        LEFT JOIN tbmst_brand_group bg on bg.id = brnd.BrandGroupId
                        where bg.id=@pgrpBrandId
                    ", new { pgrpBrandId = grpBrandId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<object>> GetSubCategoryId(int CategoryId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __res = await conn.QueryAsync<object>(
                    @"select ts.id, ts.shortdesc, ts.longDesc
                    from tbmst_subCategory ts 
                    inner join tbmst_Category tc on ts.CategoryId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @CategoryId
                    ", new { CategoryId = CategoryId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<object>> GetSubChannel(int[] ChannelId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __res = await conn.QueryAsync<object>(
                    @"select ts.id, ts.longDesc 
                    from tbmst_subchannel ts 
                    inner join tbmst_channel tc on ts.ChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                    where isnull(ts.IsDelete, 0) = 0 and  tc.id IN @ChannelId
                    ", new { ChannelId = ChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<object>> GetAccount(int[] SubChannelId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __res = await conn.QueryAsync<object>(
                    @"select ts.id, ts.longDesc 
                    from tbmst_account ts 
                    inner join tbmst_subchannel tc on ts.SubChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                    where isnull(ts.IsDelete, 0) = 0 and tc.id IN @SubChannelId
                    ", new { SubChannelId = SubChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<object>> GetSubAccount(int[] AccountId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __res = await conn.QueryAsync<object>(
                    @"select ts.id, ts.longDesc 
                    from tbmst_subaccount ts 
                    inner join tbmst_account tc on ts.AccountId = tc.Id and isnull(tc.IsDelete, 0) = 0
                    where isnull(ts.IsDelete, 0) = 0 and  tc.id IN @AccountId
                    ", new { AccountId = AccountId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<object>> GetActivityandSubActivityId(int subCategoryId)
        {
            using IDbConnection conn = Connection;
            var __query = @"SELECT 
                                a.Id [subActivityId], 
                                a.ShortDesc [subActivitySortDesc], 
                                a.LongDesc [subActivityLongDesc], 
                                b.Id [activityId], 
                                b.ShortDesc [activityShortDesc], 
                                b.LongDesc [activityLongDesc]
                            FROM tbmst_subactivity a INNER JOIN tbmst_activity b on b.Id = a.ActivityId
                            WHERE ISNULL(a.IsDeleted, 0) = 0 AND ISNULL(b.IsDeleted, 0) = 0 AND b.SubCategoryId = @subCategoryId";
            var __res = await conn.QueryAsync<object>(__query, new { subCategoryId = subCategoryId });
            return __res.ToList();
        }

        public async Task<IList<object>> GetCategoryList()
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

        public async Task<object> GetPromoMechanismValidate(int promoId, int entityId, int subCategoryId, int activityId, int subActivityId, int skuId, int channelId, string startFrom, string startTo)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@entityid", entityId);
                __param.Add("@subcategoryid", subCategoryId);
                __param.Add("@activityid", activityId);
                __param.Add("@subactivityid", subActivityId);
                __param.Add("@productid", skuId);
                __param.Add("@channelid", channelId);
                __param.Add("@startdate", startFrom);
                __param.Add("@enddate", startTo);
                __param.Add("@promoid", promoId);

                conn.Open();
                var p =  conn.QueryMultiple("[dbo].[ip_promo_mechanism_validate]", __param, 
                    commandType: CommandType.StoredProcedure, commandTimeout: 180);
                var _msg = p.Read();
                var _mechanism = p.Read();
                object res = new
                {
                    validation = _msg,
                    mechanism = _mechanism
                };
                return res;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}
