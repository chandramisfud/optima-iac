using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{

    public class SubCategoryRepository : ISubCategoryRepository
    {
        readonly IConfiguration __config;
        public SubCategoryRepository(IConfiguration config)
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
        public async Task<SubCategoryCreateReturn> CreateSubCategory(SubCategoryCreate body)
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
                                    SELECT Id FROM tbmst_subcategory 
                                    WHERE CategoryId = @CategoryId
                                    AND LongDesc = @LongDesc
                                    AND isnull(IsDeleted, 0) = 0
                                    )
                                    BEGIN
                                    INSERT INTO [dbo].[tbmst_subcategory]
                                    ([CategoryId]
                                    ,[LongDesc]
                                    ,[ShortDesc]
                                    ,[IsActive]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,[CreatedEmail]
                                    ) 
                                    VALUES
                                    (
                                    @CategoryId
                                    ,@LongDesc
                                    ,@ShortDesc
                                    ,1
                                    ,@CreateOn
                                    ,@CreateBy
                                    ,@CreatedEmail
                                    )
                                    SET @identity = (SELECT SCOPE_IDENTITY())
                                    SELECT Id, CategoryId, LongDesc, ShortDesc, CreateOn, CreateBy, RefId, CreatedEmail FROM tbmst_subcategory WHERE Id=@identity
                                    END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @return = (SELECT RefId FROM tbmst_subcategory WHERE LongDesc=@LongDesc)
                                    SET @message= 'SubCategory with RefId = ' + @return + ' is already exist'
                                    RAISERROR (@message, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );
                                    END";
                var __res = await conn.QueryAsync<SubCategoryCreateReturn>(__query, new
                {
                    categoryId = body.CategoryId,
                    shortDesc = body.ShortDesc,
                    longDesc = body.LongDesc,
                    createOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    createBy = body.CreateBy,
                    createdEmail = body.CreatedEmail

                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<SubCategoryDeleteReturn> DeleteSubCategory(SubCategoryDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                UPDATE [dbo].[tbmst_subcategory]
                                SET
                                IsActive = 0
                                ,IsDeleted=1
                                ,DeleteBy=@DeleteBy
                                ,DeleteOn=@DeleteOn
                                ,DeleteEmail=@DeleteEmail
                                WHERE
                                Id=@Id
                                SELECT Id, DeleteBy, IsDeleted, DeleteOn, DeleteEmail, RefId FROM tbmst_subcategory WHERE Id=@Id 
                                ";
                var __res = await conn.QueryAsync<SubCategoryDeleteReturn>(__query, new
                {
                    DeleteBy = body.DeleteBy,
                    DeleteOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    DeleteEmail = body.DeleteEmail,
                    Id = body.Id
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SubCategorytoCategory>> GetCategoryforSubCategory()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<SubCategorytoCategory>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP> GetSubCategoryLandingPage
        (
            string keyword
            , string sortColumn
            , string sortDirection = "ASC"
            , int dataDisplayed = 10
            , int pageNum = 0
            )
        {
            BaseLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = @" CONCAT_WS(' ', 
                                    CategoryId,
                                    CategoryLongDesc,
                                    CategoryShortDesc,
                                    CategoryRefid,
                                    SubCategoryId,
                                    SubCategoryLongDesc,
                                    SubCategoryShortDesc,
                                    SubCategoryRefid
                                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_subcategory_lp
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_subcategory_lp
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
                res.Data = __res.Read<SubCategorySelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<SubCategoryModel> GetSubCategoryById(SubCategoryById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    SELECT * FROM tbmst_subcategory 
                                    WHERE Id = @Id AND ISNULL(IsDeleted, 0) = 0";
                var __res = await conn.QueryAsync<SubCategoryModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<SubCategoryUpdateReturn> UpdateSubCategory(SubCategoryUpdate body)
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
                                    SELECT id FROM tbmst_subcategory 
                                    WHERE CategoryId = @CategoryId
                                    AND LongDesc = @LongDesc
                                    AND isnull(IsDeleted, 0) = 0
                                    and id<>@id

                                    )
                                    BEGIN
                                    UPDATE [dbo].[tbmst_subcategory]
                                    SET
                                    [CategoryId] = @CategoryId
                                    ,[LongDesc] = @LongDesc
                                    ,[ShortDesc] = @ShortDesc
                                    ,[ModifiedOn] = @ModifiedOn
                                    ,[ModifiedBy] = @ModifiedBy
                                    ,[ModifiedEmail] = @ModifiedEmail
                                    WHERE 
                                    Id=@Id
                                    SELECT Id, CategoryId, LongDesc, ShortDesc, ModifiedOn, ModifiedBy, ModifiedEmail, RefId FROM tbmst_subcategory WHERE Id=@Id
                                    END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @messageout='SubCategory already exist'
                                    RAISERROR (@messageout, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );

                                    END
                                    ";
                var __res = await conn.QueryAsync<SubCategoryUpdateReturn>(__query, new
                {
                    id = body.Id,
                    CategoryId = body.CategoryId,
                    LongDesc = body.LongDesc,
                    ShortDesc = body.ShortDesc,
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
    }
}