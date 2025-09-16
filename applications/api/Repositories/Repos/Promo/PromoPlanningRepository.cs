using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Repositories.Entities;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Model;
using System.Security.Principal;
using System.Reflection;
using Entities;
using Repositories.Entities.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using static Dapper.SqlMapper;

namespace Repositories.Repos
{
    public partial class PromoPlanningRepository : IPromoPlanningRepository
    {
        readonly IConfiguration __config;
        public PromoPlanningRepository(IConfiguration config)
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
        public async Task<BaseLP2> GetPromoPlanningLandingPage(string year, int entity, int distributor, string createFrom, string createTo, string startFrom, string startTo, string profileId,
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
                __param.Add("@create_from", createFrom);
                __param.Add("@create_to", createTo);
                __param.Add("@start_from", startFrom);
                __param.Add("@start_to", startTo);
                __param.Add("@userid", profileId);

                __param.Add("@txtSearch", keyword);
                __param.Add("@sort", sortDirection);
                __param.Add("@order", sortColumn);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                __param.Add("@filter", "");

                var __res = await conn.QueryMultipleAsync("ip_promo_planning_v3_list_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                var __data = __res.Read<PromoPlanningLPDto>().Cast<object>().ToList();
                res = __res.ReadSingle<BaseLP2>();
                res.Data = __data;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
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
                if (arrayParent != null)
                {
                    foreach (int v in arrayParent)
                        __parent.Rows.Add(v);
                }
                else
                {
                    __parent.Rows.Add(0);
                }
                var __query = new DynamicParameters();

                __query.Add("@budgetid", budgetid);
                __query.Add("@attribute", "distributor");
                __query.Add("@parent", __parent.AsTableValuedParameter());


                conn.Open();
                var child = await conn.QueryAsync<BaseDropDownList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
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
        public async Task<PromoResponseDto> PromoPlanningCreate(PromoPlanningDto body)
        {

            DataTable __ba = _castToDataTable(new PromoPlanningTypeDto(), null!);
            __ba.Rows.Add
            (
                body.PromoPlanningHeader!.promoPlanId
                , body.PromoPlanningHeader.periode
                , body.PromoPlanningHeader.distributorId
                , body.PromoPlanningHeader.entityId
                , body.PromoPlanningHeader.principalShortDesc
                , body.PromoPlanningHeader.categoryShortDesc
                , body.PromoPlanningHeader.categoryId
                , body.PromoPlanningHeader.subCategoryId
                , body.PromoPlanningHeader.activityId
                , body.PromoPlanningHeader.subActivityId
                , body.PromoPlanningHeader.activityDesc
                , body.PromoPlanningHeader.startPromo
                , body.PromoPlanningHeader.endPromo
                , body.PromoPlanningHeader.mechanisme1
                , body.PromoPlanningHeader.mechanisme2
                , body.PromoPlanningHeader.mechanisme3
                , body.PromoPlanningHeader.mechanisme4
                , body.PromoPlanningHeader.investment
                , body.PromoPlanningHeader.normalSales
                , body.PromoPlanningHeader.incrSales
                , body.PromoPlanningHeader.roi
                , body.PromoPlanningHeader.costRatio
                , body.PromoPlanningHeader.notes
                , body.PromoPlanningHeader.createOn
                , body.PromoPlanningHeader.createBy
                , body.PromoPlanningHeader.initiator_notes
                , body.PromoPlanningHeader.createdEmail
                , body.PromoPlanningHeader.modifReason

            );

            DataTable __child = _castToDataTable(new PromoChildsTypeDto(), null!);
            DataTable __reg = __child.Clone();
            foreach (Region v in body.Regions!)
                __reg.Rows.Add(null, v.id, 1);

            DataTable __chan = __child.Clone();
            foreach (Channel v in body.Channels!)
                __chan.Rows.Add(null, v.id, 1);

            DataTable __subchan = __child.Clone();
            foreach (SubChannel v in body.SubChannels!)
                __subchan.Rows.Add(v.id, v.id, 1);

            DataTable __acc = __child.Clone();
            foreach (Account v in body.Accounts!)
                __acc.Rows.Add(v.id, v.id, 1);

            DataTable __subacc = __child.Clone();
            foreach (SubAccount v in body.SubAccounts!)
                __subacc.Rows.Add(v.id, v.id, 1);

            DataTable __brand = __child.Clone();
            foreach (Brand v in body.Brands!)
                __brand.Rows.Add(null, v.id, 1);

            DataTable __sku = __child.Clone();
            foreach (Product v in body.Skus!)
                __sku.Rows.Add(v.id, v.id, 1);

            DataTable __mec = _castToDataTable(new MechanismType(), null!);
            foreach (MechanismType v in body.Mechanisms!)
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
            __query.Add("@Mechanism", __mec.AsTableValuedParameter());

            conn.Open();
            var p = await conn.QueryAsync<PromoResponseDto>("[dbo].[ip_promo_planning_v4_insert]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return p.FirstOrDefault()!;
        }

        public async Task<PromoResponseDto> PromoPlanningUpdate(PromoPlanningDto body)
        {

            DataTable __ba = _castToDataTable(new PromoPlanningTypeDto(), null!);
            __ba.Rows.Add
            (
                body.PromoPlanningHeader!.promoPlanId
                , body.PromoPlanningHeader.periode
                , body.PromoPlanningHeader.distributorId
                , body.PromoPlanningHeader.entityId
                , body.PromoPlanningHeader.principalShortDesc
                , body.PromoPlanningHeader.categoryShortDesc
                , body.PromoPlanningHeader.categoryId
                , body.PromoPlanningHeader.subCategoryId
                , body.PromoPlanningHeader.activityId
                , body.PromoPlanningHeader.subActivityId
                , body.PromoPlanningHeader.activityDesc
                , body.PromoPlanningHeader.startPromo
                , body.PromoPlanningHeader.endPromo
                , body.PromoPlanningHeader.mechanisme1
                , body.PromoPlanningHeader.mechanisme2
                , body.PromoPlanningHeader.mechanisme3
                , body.PromoPlanningHeader.mechanisme4
                , body.PromoPlanningHeader.investment
                , body.PromoPlanningHeader.normalSales
                , body.PromoPlanningHeader.incrSales
                , body.PromoPlanningHeader.roi
                , body.PromoPlanningHeader.costRatio
                , body.PromoPlanningHeader.notes
                , body.PromoPlanningHeader.createOn
                , body.PromoPlanningHeader.createBy
                , body.PromoPlanningHeader.initiator_notes
                , body.PromoPlanningHeader.createdEmail
                , body.PromoPlanningHeader.modifReason

            );

            DataTable __child = _castToDataTable(new PromoChildsTypeDto(), null!);
            DataTable __reg = __child.Clone();
            foreach (Region v in body.Regions!)
                __reg.Rows.Add(null, v.id, 1);

            DataTable __chan = __child.Clone();
            foreach (Channel v in body.Channels!)
                __chan.Rows.Add(null, v.id, 1);

            DataTable __subchan = __child.Clone();
            foreach (SubChannel v in body.SubChannels!)
                __subchan.Rows.Add(null, v.id, 1);

            DataTable __acc = __child.Clone();
            foreach (Account v in body.Accounts!)
                __acc.Rows.Add(null, v.id, 1);

            DataTable __subacc = __child.Clone();
            foreach (SubAccount v in body.SubAccounts!)
                __subacc.Rows.Add(null, v.id, 1);

            DataTable __brand = __child.Clone();
            foreach (Brand v in body.Brands!)
                __brand.Rows.Add(null, v.id, 1);

            DataTable __sku = __child.Clone();
            foreach (Product v in body.Skus!)
                __sku.Rows.Add(null, v.id, 1);

            DataTable __mec = _castToDataTable(new MechanismType(), null!);
            foreach (MechanismType v in body.Mechanisms!)
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

            conn.Open();
            var p = await conn.QueryAsync<PromoResponseDto>("[dbo].[ip_promo_planning_v4_insert]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return p.FirstOrDefault()!;
        }

        public async Task<IList<PromoPlanningDownloadDto>> GetPromoPlanningDownload(string year, int entity = 0, int distributor = 0, string createFrom = "", string createTo = "", string startFrom = "", string startTo = "", string profileId = "")
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@periode", year);
                __param.Add("@entity", entity);
                __param.Add("@distributor", distributor);
                __param.Add("@create_from", createFrom);
                __param.Add("@create_to", createTo);
                __param.Add("@start_from", startFrom);
                __param.Add("@start_to", startTo);
                __param.Add("@userid", profileId);

                conn.Open();
                var p = await conn.QueryAsync<PromoPlanningDownloadDto>("[dbo].[ip_promo_planning_v3_list_download]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetAttributeByParent(int budgetid, string attribute, int[] arrayParent, string isDeleted)
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
                __query.Add("@attribute", attribute);
                __query.Add("@parent", __parent.AsTableValuedParameter());
                __query.Add("@status", isDeleted);


                conn.Open();
                var child = await conn.QueryAsync<BaseDropDownList>("ip_getattribute_byparent_new", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IEnumerable<SubCategoryDropDownList>> GetSubCategory()
        {
            IEnumerable<SubCategoryDropDownList> subCategorys;

            using (IDbConnection conn = Connection)
            {

                var sql = "SELECT A.[Id]" +
                    ",A.[CategoryId]" +
                    ",A.[LongDesc]" +
                    ",A.[ShortDesc]" +
                    ",A.[CreateBy]" +
                    ",A.[RefId]" +
                    ",B.[LongDesc] CategoryLongDesc" +
                    ",B.[ShortDesc] CategoryShortDesc" +
                    " FROM tbmst_subcategory AS A " +
                    "LEFT JOIN tbmst_category AS B ON A.CategoryId = B.Id WHERE ISNULL(A.IsDeleted, 0) = 0";

                subCategorys = await conn.QueryAsync<SubCategoryDropDownList>(sql);
            }
            return subCategorys;
        }

        public async Task<PromoPlanningByIdDto> GetPromoPlanningById(int id)
        {
            List<PromoPlanningByIdDto> __promoplanning = new();
            using (IDbConnection conn = Connection)
            {
                var __query = new DynamicParameters();
                __query.Add("@Id", id);

                using var __re = await conn.QueryMultipleAsync("[dbo].[ip_promo_planning_v3_select]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                PromoPlanningByIdDto __result = new()
                {
                    PromoPlanningHeader = __re.Read<PromoPlanningHeader>().FirstOrDefault()!,

                    Regions = new List<PromoPlanningAttibuteById>()
                };
                foreach (PromoPlanningAttibuteById r in __re.Read<PromoPlanningAttibuteById>())
                    __result.Regions.Add(new PromoPlanningAttibuteById()
                    {
                        flag = r.flag,
                        id = r.id,
                        longDesc = r.longDesc,
                    });
                __result.Channels = new List<PromoPlanningAttibuteById>();
                foreach (PromoPlanningAttibuteById c in __re.Read<PromoPlanningAttibuteById>())
                    __result.Channels.Add(new PromoPlanningAttibuteById()
                    {
                        flag = c.flag,
                        id = c.id,
                        longDesc = c.longDesc,
                    });
                __result.SubChannels = new List<PromoPlanningAttibuteById>();
                foreach (PromoPlanningAttibuteById sc in __re.Read<PromoPlanningAttibuteById>())
                    __result.SubChannels.Add(new PromoPlanningAttibuteById()
                    {
                        flag = sc.flag,
                        id = sc.id,
                        longDesc = sc.longDesc,
                    });
                __result.Accounts = new List<PromoPlanningAttibuteById>();
                foreach (PromoPlanningAttibuteById a in __re.Read<PromoPlanningAttibuteById>())
                    __result.Accounts.Add(new PromoPlanningAttibuteById()
                    {
                        flag = a.flag,
                        id = a.id,
                        longDesc = a.longDesc,
                    });
                __result.SubAccounts = new List<PromoPlanningAttibuteById>();
                foreach (PromoPlanningAttibuteById sa in __re.Read<PromoPlanningAttibuteById>())
                    __result.SubAccounts.Add(new PromoPlanningAttibuteById()
                    {
                        flag = sa.flag,
                        id = sa.id,
                        longDesc = sa.longDesc,
                    });

                __result.Brands = new List<PromoPlanningAttibuteById>();
                foreach (PromoPlanningAttibuteById br in __re.Read<PromoPlanningAttibuteById>())
                    __result.Brands.Add(new PromoPlanningAttibuteById()
                    {
                        flag = br.flag,
                        id = br.id,
                        longDesc = br.longDesc,
                    });

                __result.Skus = new List<PromoPlanningAttibuteById>();
                foreach (PromoPlanningAttibuteById pr in __re.Read<PromoPlanningAttibuteById>())
                    __result.Skus.Add(new PromoPlanningAttibuteById()
                    {
                        flag = pr.flag,
                        id = pr.id,
                        longDesc = pr.longDesc,
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

                __promoplanning.Add(__result);
            }
            return __promoplanning.FirstOrDefault()!;
        }

        public async Task<IList<MechanisSourceDto>> GetPromoMechanism(int entityId, int subCategoryId, int activityId, int subActivityId, int skuId, int channelId, string startFrom, string startTo)
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

                conn.Open();
                var p = await conn.QueryAsync<MechanisSourceDto>("[dbo].[ip_mst_mechanism_get]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoAttributeRegion()
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@ID", "");
                __param.Add("@NAME", "");

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_select_region]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoPlanningChannel(string profileId)
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
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bymapping]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<BaseDropDownList>> GetPromoPlanningChannelByPlanningId(int promoPlanningId, string profileId)
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
                __param.Add("@promoplanningid", promoPlanningId);

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bypromoplanningid]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoPlanningSubChannel(int[] arrayParent, string profileId)
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
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bymapping]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<BaseDropDownList>> GetPromoPlanningSubChannelByPlanningId(int promoPlanningId, int[] arrayParent, string profileId)
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
                __param.Add("@promoplanningid", promoPlanningId);

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bypromoplanningid]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoPlanningAccount(int[] arrayParent, string profileId)
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
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bymapping]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<BaseDropDownList>> GetPromoPlanningAccountByPlanningId(int promoPlanningId, int[] arrayParent, string profileId)
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
                __param.Add("@promoplanningid", promoPlanningId);

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bypromoplanningid]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetPromoPlanningSubAccount(int[] arrayParent, string profileId)
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
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bymapping]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<BaseDropDownList>> GetPromoPlanningSubAccountByPlanningId(int promoPlanningId, int[] arrayParent, string profileId)
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
                __param.Add("@promoplanningid", promoPlanningId);

                conn.Open();
                var p = await conn.QueryAsync<BaseDropDownList>("[dbo].[ip_getattribute_bypromoplanningid]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoBaselineDto> GetBaselineSales(
            int promoId, int period, string dateCreation, int typePromo, int subCategoryId, int subActivityId, int distributorId, string startPromo, string endPromo,
            int[] arrayRegion, int[] arrayChannel, int[] arraySubChannel, int[] arrayAccount, int[] arraySubAccount, int[] arrayBrand, int[] arraySKU
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
                __param.Add("@p_promoid", promoId);
                __param.Add("@p_period", period);
                __param.Add("@p_date", dateCreation);
                __param.Add("@p_type", typePromo);
                __param.Add("@p_distributor", distributorId);
                __param.Add("@p_region", __region.AsTableValuedParameter());
                __param.Add("@p_channel", __channel.AsTableValuedParameter());
                __param.Add("@p_subchannel", __subchannel.AsTableValuedParameter());
                __param.Add("@p_account", __account.AsTableValuedParameter());
                __param.Add("@p_subaccount", __subaccount.AsTableValuedParameter());
                __param.Add("@p_brand", __brand.AsTableValuedParameter());
                __param.Add("@p_product", __sku.AsTableValuedParameter());
                __param.Add("@p_subcategory", subCategoryId);
                __param.Add("@p_startpromo", startPromo);
                __param.Add("@p_endpromo", endPromo);
                __param.Add("@p_subactivity", subActivityId);

                conn.Open();
                var p = await conn.QueryAsync<PromoBaselineDto>("[dbo].[ip_promo_get_baseline]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoConfigROICRDto> GetPromoConfigROICR(int subActivityId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@id", subActivityId);

                conn.Open();
                var p = await conn.QueryAsync<PromoConfigROICRDto>("[dbo].[ip_get_config_cr_roi]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoPlanningExistDto> GetPromoPlanningExist(string period, string activityDesc, int[] arrayChannel, int[] arrayAccount, string startPromo, string endPromo)
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
                var p = await conn.QueryAsync<PromoPlanningExistDto>("[dbo].[ip_promo_planning_exist]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<PromoInvestmentTypeDto>> GetPromoPlanningInvestmentType(int subActivityId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = new DynamicParameters();

                __query.Add("@subactivityid", subActivityId);

                conn.Open();
                var child = await conn.QueryAsync<PromoInvestmentTypeDto>("ip_get_investment_type", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }


        public async Task<PromoResponseDto> PromoPlanningCancel(PromoPlanningCancelDto promoPlanningCancel)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = new DynamicParameters();

                __query.Add("@promoplanid", promoPlanningCancel.promoPlanningId);
                __query.Add("@reason", promoPlanningCancel.reason);
                __query.Add("@userid", promoPlanningCancel.profileId);

                conn.Open();
                var p = await conn.QueryAsync<PromoResponseDto>("ip_promo_planning_cancel", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return p.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<PromoPlanningView>> GetPromoPlanningByConditions(string periode, int entityId, int distributorId, string create_from, string create_to, string start_from, string start_to, string userId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@periode", periode);
                __param.Add("@entity", entityId);
                __param.Add("@distributor", distributorId);
                __param.Add("@create_from", create_from);
                __param.Add("@create_to", create_to);
                __param.Add("@start_from", start_from);
                __param.Add("@start_to", start_to);
                __param.Add("@userid", userId);

                var result = await conn.QueryAsync<PromoPlanningView>("ip_promo_planning_list", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // api/promoplan/approval
        public async Task<PlanningApprovalResult> PromoPlannningApproval(DataTable importPromoPlan, string userId)
        {
            try
            {
                List<PlanningApprovalResult> __plan = new();
                using (IDbConnection conn = Connection)
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    var __query = new DynamicParameters();
                    __query.Add("@ApprovalPlanType", importPromoPlan.AsTableValuedParameter());
                    __query.Add("@userid", userId);

                    using var __re = await conn.QueryMultipleAsync("[dbo].[ip_promo_plan_approval]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    PlanningApprovalResult __result = new()
                    {
                        planningheader = __re.Read<planningheader>().FirstOrDefault()!,

                        planning = new List<approvaldetailresult>()
                    };

                    foreach (approvaldetailresult r in __re.Read<approvaldetailresult>())
                        __result.planning.Add(new approvaldetailresult()
                        {
                            planningid = r.planningid,
                            planningrefid = r.planningrefid,
                            tscode = r.tscode,
                            investment = r.investment
                        });
                    __plan.Add(__result);
                }
                return __plan.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP2> GetPromoTobeCreated(string profileId, int pageNumber, int pageSize, string keyword)
        {
            using IDbConnection conn = Connection;
            BaseLP2? res;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@userid", profileId);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                __param.Add("@filter", "");
                __param.Add("@txtSearch", keyword);

                var __res = await conn.QueryMultipleAsync("ip_promo_tobecreated", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                var __data = __res.Read<PromoPlanningLPDto>().Cast<object>().ToList();
                res = __res.ReadSingle<BaseLP2>();
                res.Data = __data;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;

        }
    }
}
