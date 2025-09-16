using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class ROIandCRRepo : IROIandCRRepo
    {
        readonly IConfiguration __config;
        public ROIandCRRepo(IConfiguration config)
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

        public async Task<IList<ConfigRoi>> GetConfigRoiList(int CategoryId, int SubCategoryId, int ActivityId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@CategoryId", CategoryId);
                __param.Add("@SubCategoryId", SubCategoryId);
                __param.Add("@ActivityId", ActivityId);

                var result = await conn.QueryAsync<ConfigRoi>("dbo.ip_conf_roi_cr_list", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task CreateConfigRoi(ConfigRoiStore body)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __ba = _castToDataTable(new SetRoiCrType(), null!);
                foreach (SetRoiCrType v in body.Config!)
                    __ba.Rows.Add(v.Id, v.MinimumROI, v.MaksimumROI, v.MinimumCostRatio, v.MaksimumCostRatio);

                var __param = new DynamicParameters();
                __param.Add("@config", __ba.AsTableValuedParameter());
                __param.Add("@UserId", body.UserId);
                __param.Add("@UserEmail", body.CreatedEmail);

                var result = await conn.QueryAsync<ConfigRoiStore>("dbo.ip_conf_roi_cr_ins", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task DeleteConfigRoi(ConfigRoiDelete body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@Id", body.id);
                __param.Add("@UserId", body.UserId);

                var result = await conn.QueryAsync<ConfigRoi>("dbo.ip_conf_roi_cr_del", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ListCategory>> GetCategoryList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var sql = @"SELECT Id, ShortDesc, RefId, LongDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<ListCategory>(sql);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ListSubCategory>> GetSubCategoryList(int CategoryId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var sql = @"SELECT 
                            ts.Id,
                            ts.RefId,
                            ts.ShortDesc,
                            ts.LongDesc
                            FROM tbmst_subcategory ts 
                            INNER JOIN 
                            tbmst_category tc ON ts.CategoryId = tc.Id
                            AND ISNULL(tc.IsDeleted,0)= 0  
                            WHERE tc.Id = @CategoryId
                            AND ISNULL(ts.IsDeleted,0) = 0";
                var __res = await conn.QueryAsync<ListSubCategory>(sql, new { CategoryId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ListActivity>> GetActivityList(int SubCategoryId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var sql = @"select ta.Id, ta.RefId, ta.ShortDesc, ta.LongDesc
                            from tbmst_activity ta 
                            inner join tbmst_subcategory ts on ta.SubCategoryId = ts.id and isnull(ts.IsDeleted, 0) = 0
                            where isnull(ta.IsDeleted, 0) = 0 and  ts.id = @subcategoryid";
                var __res = await conn.QueryAsync<ListActivity>(sql, new { SubCategoryId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}