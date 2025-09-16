using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Configuration;

namespace Repositories.Repos
{
    public class PromoItemRepo : IPromoItemRepo
    {
        readonly IConfiguration __config;
        public PromoItemRepo(IConfiguration config)
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

        public async Task<GetPromoItem> GetConfigPromoItems(string categoryShortDesc)
        {
            GetPromoItem __res = new();
            using IDbConnection conn = Connection;
            var __param = new DynamicParameters();
            __param.Add("@category", categoryShortDesc);

            var __obj = await conn.QueryMultipleAsync("dbo.ip_config_promo_items_get", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

            __res.PromoConfig = __obj.Read<object>().FirstOrDefault();
            __res.EnableConfig = __obj.Read<object>().FirstOrDefault();


            return __res;
        }

        public async Task<IList<object>> GetConfigPromoItemsHistory(int year, string? categoryShortDesc)
        {
            using IDbConnection conn = Connection;
            var __param = new DynamicParameters();
            __param.Add("@year", year);
            __param.Add("@category", categoryShortDesc);
            var result = await conn.QueryAsync<object>("dbo.ip_config_promo_items_gethis", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList();
        }

        public async Task UpdateConfigPromoItem(
            int categoryId,
            string? userId,
            string? userEmail,
            PromoItem configPromoItem
            )
        {
            using IDbConnection conn = Connection;
            DataTable __dT = _castToDataTable(new PromoItem(), null!);
            __dT.Rows.Add
            (
                configPromoItem.budgetYear,
                configPromoItem.promoPlanning,
                configPromoItem.budgetSource,
                configPromoItem.entity,
                configPromoItem.distributor,
                configPromoItem.subCategory,
                configPromoItem.activity,
                configPromoItem.subActivity,
                configPromoItem.subActivityType,
                configPromoItem.startPromo,
                configPromoItem.endPromo,
                configPromoItem.activityName,
                configPromoItem.initiatorNotes,
                configPromoItem.incrSales,
                configPromoItem.investment,
                configPromoItem.channel,
                configPromoItem.subChannel,
                configPromoItem.account,
                configPromoItem.subAccount,
                configPromoItem.region,
                configPromoItem.groupBrand,
                configPromoItem.brand,
                configPromoItem.SKU,
                configPromoItem.mechanism,
                configPromoItem.Attachment,
                configPromoItem.ROI,
                configPromoItem.CR
            );
            var __param = new DynamicParameters();
            __param.Add("@configPromoItems", __dT.AsTableValuedParameter());
            __param.Add("@id", categoryId);
            __param.Add("@userid", userId);
            __param.Add("@useremail", userEmail);

            var result = await conn.QueryAsync<UpdatePromoItem>("dbo.ip_config_promo_items_save", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
        }
    }
}