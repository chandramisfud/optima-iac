using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class InvestmentTypeRepository : IInvestmentTypeRepository
    {
        readonly IConfiguration __config;
        public InvestmentTypeRepository(IConfiguration config)
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

        public async Task<InvestmentTypeCreateReturn> CreateInvestmentType(InvestmentTypeCreate body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                DECLARE @message varchar(255);
                                DECLARE @identity INT;
                                DECLARE @return varchar(255);
                                -- Cek Jika data sudah ada
                                IF NOT EXISTS
                                (
                                SELECT Id FROM tbmst_investment_type
                                WHERE LongDesc = @LongDesc
                                AND isnull(IsDeleted, 0) = 0
                                )
                                BEGIN
                                INSERT INTO [dbo].[tbmst_investment_type]
                                (
                                [RefId]
                                ,[LongDesc]
                                ,[CreateOn]
                                ,[CreateBy]
                                ,[CreatedEmail]
                                ) 
                                VALUES
                                (
                                    @RefId
                                ,@LongDesc
                                ,@CreateOn
                                ,@CreateBy
                                ,@CreatedEmail
                                )
                                SET @identity = (SELECT SCOPE_IDENTITY())
                                SELECT Id, LongDesc, CreateOn, CreateBy, RefId, CreatedEmail FROM tbmst_investment_type WHERE Id=@identity
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @return = (SELECT RefId FROM tbmst_investment_type WHERE LongDesc=@LongDesc)
                                SET @message= 'InvestmentType with RefId = ' + @return + ' is already exist'
                                RAISERROR (@message, -- Message text.
                                16, -- Severity.
                                1 -- State.
                                );
                                END";
                var __res = await conn.QueryAsync<InvestmentTypeCreateReturn>(__query, new
                {
                    RefId = body.RefId,
                    LongDesc = body.LongDesc,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    CreateBy = body.CreateBy,
                    CreatedEmail = body.CreatedEmail
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<InvestmentTypeDeleteReturn> DeleteInvestmentType(InvestmentTypeDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                UPDATE [dbo].[tbmst_investment_type]
                                SET
                                IsDeleted=1
                                ,DeleteBy=@DeleteBy
                                ,DeleteOn=@DeleteOn
                                ,DeleteEmail=@DeleteEmail
                                WHERE
                                Id=@Id
                                SELECT Id, DeleteBy, IsDeleted, DeleteOn, DeleteEmail, RefId FROM tbmst_investment_type WHERE Id=@Id 
                                ";
                var __res = await conn.QueryAsync<InvestmentTypeDeleteReturn>(__query, new
                {
                    Id = body.Id,
                    DeleteBy = body.DeleteBy,
                    DeleteOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    DeleteEmail = body.DeleteEmail
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<InvestmentTypeModel> GetInvestmentTypeById(InvestmentTypeById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    SELECT * FROM tbmst_investment_type
                                    WHERE Id = @Id AND ISNULL(IsDeleted, 0) = 0";
                var __res = await conn.QueryAsync<InvestmentTypeModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<BaseLP> GetInvestmentTypeLandingPage(string keyword,
        string sortColumn,
         string sortDirection = "ASC",
          int dataDisplayed = 10,
           int pageNum = 0
           )
        {
            BaseLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = " CONCAT_WS(' ', RefId, LongDesc) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM tbmst_investment_type 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM tbmst_investment_type
                        WHERE {0}  
                        ORDER BY {1} {2}                     
                        ";

                // if set -1 dont add paging format
                if (dataDisplayed >= 0)
                {
                    __query += String.Format(paging, pageNum, dataDisplayed);
                }

                __query = string.Format(__query, userFilter, sortColumn, sortDirection);
                using IDbConnection conn = Connection;
                var __res = await conn.QueryMultipleAsync(__query);
                res = __res.ReadSingle<BaseLP>();
                res.Data = __res.Read<InvestmentTypeModel>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<IList<InvestmentResultMap>> InvestmentTypeMap(int activityid, int subactivityid, int categoryid, int subcategoryid)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@activityid", activityid);
                __param.Add("@subactivityid", subactivityid);
                __param.Add("@categoryid", categoryid);
                __param.Add("@subcategoryid", subcategoryid);

                var result = await conn.QueryAsync<InvestmentResultMap>("ip_mst_investment_type_map_lp", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<InvestmentTypeUpdateReturn> UpdateInvestmentType(InvestmentTypeUpdate body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                DECLARE @messageout varchar(255)
                                -- Cek Jika data sudah ada
                                IF NOT EXISTS(
                                SELECT id FROM tbmst_investment_type 
                                WHERE LongDesc = @LongDesc
                                AND isnull(IsDeleted, 0) = 0
                                and id<>@id
                                )
                                BEGIN
                                UPDATE [dbo].[tbmst_investment_type]
                                SET
                                RefId = @RefId
                                ,[LongDesc] = @LongDesc
                                ,[ModifiedOn] = @ModifiedOn
                                ,[ModifiedBy] = @ModifiedBy
                                ,[ModifiedEmail] = @ModifiedEmail
                                WHERE 
                                Id=@Id
                                SELECT Id, LongDesc, ModifiedOn, ModifiedBy, ModifiedEmail,RefId FROM tbmst_investment_type WHERE Id=@Id
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @messageout='InvestmentType already exist'
                                RAISERROR (@messageout, -- Message text.
                                16, -- Severity.
                                1 -- State.
                                );

                                END
                                ";
                var __res = await conn.QueryAsync<InvestmentTypeUpdateReturn>(__query, new
                {
                    id = body.Id,
                    RefId = body.RefId,
                    LongDesc = body.LongDesc,
                    ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    ModifiedBy = body.ModifiedBy,
                    ModifiedEmail = body.ModifiedEmail
                });
                return __res.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task CreateInvestmentTypeMapping(InvestmentTypeMappingCreate body)
        {
            try
            {
                using IDbConnection conn = Connection;
                DataTable __ba = _castToDataTable(new InvestmentDataType(), null!);
                foreach (InvestmentDataType v in body.investment!)
                    __ba.Rows.Add(v.id, v.SubActivityId, v.InvestmentTypeId);

                var __param = new DynamicParameters();
                __param.Add("@isnew", 1);
                __param.Add("@InvestmentType", __ba.AsTableValuedParameter());
                __param.Add("@userid", body.userid);

                await conn.ExecuteAsync("dbo.ip_mst_investment_type_map_ins", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task DeleteInvestmentTypeMapping(InvestmentTypeMappingDelete body)
        {
            try
            {
                using IDbConnection conn = Connection;
                DataTable __ba = _castToDataTable(new InvestmentDataType(), null!);
                foreach (InvestmentDataType v in body.investment!)
                    __ba.Rows.Add(v.id, v.SubActivityId, v.InvestmentTypeId);

                var __param = new DynamicParameters();
                __param.Add("@isnew", 3);
                __param.Add("@InvestmentType", __ba.AsTableValuedParameter());
                __param.Add("@userid", body.userid);

                await conn.ExecuteAsync("dbo.ip_mst_investment_type_map_ins", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<CategoryInvestmentMap>> GetCategoryforInvestmentMap()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<CategoryInvestmentMap>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SubCategoryInvestmentMap>> GetSubCategoryInvestmentMap(int CategoryId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subcategory ts 
                                    inner join tbmst_category tc on ts.CategoryId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @CategoryId";
                var __res = await conn.QueryAsync<SubCategoryInvestmentMap>(__query, new { CategoryId = CategoryId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ActivityInvestmentMap>> GetActivityInvestmentMap(int SubCategoryId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.Id, ts.LongDesc 
                                    from tbmst_activity ts 
                                    inner join tbmst_subcategory tc on ts.SubCategoryId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @SubCategoryId";
                var __res = await conn.QueryAsync<ActivityInvestmentMap>(__query, new { SubCategoryId = SubCategoryId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SubActivityInvestmentMap>> GetSubActivityInvestmentMap(int ActivityId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subactivity ts 
                                    inner join tbmst_activity tc on ts.ActivityId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @ActivityId";
                var __res = await conn.QueryAsync<SubActivityInvestmentMap>(__query, new { ActivityId = ActivityId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<InvestmentTypeforInvestmentMap>> InvestmentTypeInvestmentMap()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, RefId, LongDesc FROM tbmst_investment_type WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<InvestmentTypeforInvestmentMap>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<InvestmentTypeActivateReturn> ActivateInvestmentType(InvestmentTypeActivate body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                UPDATE [dbo].[tbmst_investment_type]
                                SET
                                IsDeleted=0
                                ,ModifiedBy=@ModifiedBy
                                ,ModifiedOn=@ModifiedOn
                                ,ModifiedEmail=@ModifiedEmail
                                WHERE
                                Id=@Id
                                SELECT Id, ModifiedBy, ModifiedEmail, LongDesc, RefId FROM tbmst_investment_type WHERE Id=@Id 
                                ";
                var __res = await conn.QueryAsync<InvestmentTypeActivateReturn>(__query, new
                {
                    Id = body.Id,
                    ModifiedBy = body.ModifiedBy,
                    ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    ModifiedEmail = body.ModifiedEmail
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}

