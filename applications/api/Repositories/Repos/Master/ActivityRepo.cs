using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class ActivityRepository : IActivityRepository
    {
        readonly IConfiguration __config;
        public ActivityRepository(IConfiguration config)
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
        public async Task<ActivityCreateReturn> CreateActivity(ActivityCreate body)
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
                                    SELECT Id FROM tbmst_activity 
                                    WHERE SubCategoryId = @SubCategoryId
                                    AND LongDesc = @LongDesc
                                    AND isnull(IsDeleted, 0) = 0
                                    )
                                    BEGIN
                                    INSERT INTO [dbo].[tbmst_Activity]
                                    (
                                    [CategoryId]
                                    ,[SubCategoryId]
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
                                    ,@SubCategoryId
                                    ,@LongDesc
                                    ,@ShortDesc
                                    ,1
                                    ,@CreateOn
                                    ,@CreateBy
                                    ,@CreatedEmail
                                    )
                                    SET @identity = (SELECT SCOPE_IDENTITY())
                                    SELECT Id, CategoryId, SubCategoryId, LongDesc, ShortDesc, CreateOn, CreateBy, RefId, CreatedEmail FROM tbmst_Activity WHERE Id=@identity
                                    END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @return = (SELECT RefId FROM tbmst_Activity WHERE LongDesc=@LongDesc)
                                    SET @message= 'Activity with RefId = ' + @return + ' is already exist'
                                    RAISERROR (@message, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );
                                    END";
                var __res = await conn.QueryAsync<ActivityCreateReturn>(__query, new
                {
                    CategoryId = body.CategoryId,
                    SubCategoryId = body.SubCategoryId,
                    LongDesc = body.LongDesc,
                    ShortDesc = body.ShortDesc,
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

        public async Task<ActivityDeleteReturn> DeleteActivity(ActivityDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    UPDATE [dbo].[tbmst_Activity]
                                    SET
                                    IsActive = 0
                                    ,IsDeleted=1
                                    ,DeleteBy=@DeleteBy
                                    ,DeleteOn=@DeleteOn
                                    ,DeleteEmail=@DeleteEmail
                                    WHERE
                                    Id=@Id
                                    SELECT Id, DeleteBy, IsDeleted, DeleteOn, DeleteEmail, RefId FROM tbmst_Activity WHERE Id=@Id 
                                    ";
                var __res = await conn.QueryAsync<ActivityDeleteReturn>(__query, new
                {
                    Id = body.Id,
                    DeleteOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    DeleteBy = body.DeleteBy,
                    DeleteEmail = body.DeleteEmail
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<ActivityModel> GetActivityById(ActivityById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT 
                                    tcat.id as CategoryId,
                                    tscat.id as SubCategoryId,
                                    tact.id, 
                                    tact.RefId, 
                                    tact.LongDesc, 
                                    tact.ShortDesc
                                    FROM tbmst_activity tact
                                    INNER JOIN tbmst_subcategory tscat ON tact.SubCategoryId = tscat.Id
                                    INNER JOIN tbmst_category tcat ON tscat.CategoryId = tcat.Id 
                                    WHERE tact.Id = @Id 
                                    AND ISNULL(tscat.IsDeleted, 0) = 0 
                                    AND ISNULL(tcat.IsDeleted, 0) = 0 
                                    AND ISNULL(tact.IsDeleted, 0) = 0;";
                var __res = await conn.QueryAsync<ActivityModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<BaseLP> GetActivityLandingPage(
            string keyword, string sortColumn,
             string sortDirection = "ASC", int dataDisplayed = 10, int pageNum = 0)
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
                    CategoryRefId,
                    SubCategoryId,
                    SubCategoryLongDesc,
                    SubCategoryShortDesc,
                    SubCategoryRefid,
                    ActivityId,
                    ActivityLongDesc,
                    ActivityShortDesc,
                    ActivityRefId
                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_activity_lp
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_activity_lp
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
                res.Data = __res.Read<ActivitySelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<IList<ActivityCategory>> GetCategoryforActivity()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<ActivityCategory>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ActivitySubCategory>> GetSubCategoryforActivity(int CategoryId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.ShortDesc, ts.LongDesc 
                                    from tbmst_subCategory ts 
                                    inner join tbmst_Category tc on ts.CategoryId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @CategoryId";
                var __res = await conn.QueryAsync<ActivitySubCategory>(__query, new { CategoryId = CategoryId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<ActivityUpdateReturn> UpdateActivity(ActivityUpdate body)
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
                                SELECT id FROM tbmst_Activity 
                                WHERE SubCategoryId = @SubCategoryId
                                AND LongDesc = @LongDesc
                                AND isnull(IsDeleted, 0) = 0
                                and id<>@id

                                )
                                BEGIN
                                UPDATE [dbo].[tbmst_Activity]
                                SET
                                [SubCategoryId] = @SubCategoryId
                                ,[LongDesc] = @LongDesc
                                ,[ShortDesc] = @ShortDesc
                                ,[ModifiedOn] = @ModifiedOn
                                ,[ModifiedBy] = @ModifiedBy
                                ,[ModifiedEmail] = @ModifiedEmail			

                                WHERE 
                                Id=@Id
                                SELECT Id, SubCategoryId, LongDesc, ShortDesc, ModifiedOn, ModifiedBy, ModifiedEmail, RefId FROM tbmst_Activity WHERE Id=@Id
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @messageout='Activity already exist'
                                RAISERROR (@messageout, -- Message text.
                                16, -- Severity.
                                1 -- State.
                                );

                                END
                                ";
                var __res = await conn.QueryAsync<ActivityUpdateReturn>(__query, new
                {
                    Id = body.Id,
                    SubCategoryId = body.SubCategoryId,
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

