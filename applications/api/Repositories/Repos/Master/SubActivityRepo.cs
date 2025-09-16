using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{

    public class SubActivityRepository : ISubActivityRepository
    {
        readonly IConfiguration __config;
        public SubActivityRepository(IConfiguration config)
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

        public async Task<IList<ActivityforSubActivity>> ActivityforSubActivity(int SubCategoryId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_activity ts 
                                    inner join tbmst_subcategory tc on ts.SubCategoryId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @SubCategoryId";
                var __res = await conn.QueryAsync<ActivityforSubActivity>(__query, new { SubCategoryId = SubCategoryId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ActivityTypeforSubActivity>> ActivityTypeforSubActivity()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_subactivity_type WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<ActivityTypeforSubActivity>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<CategoryforSubActivity>> CategoryforSubActivity()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<CategoryforSubActivity>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }


        public async Task<SubActivityCreateReturn> CreateSubActivity(SubActivityCreate body)
        {
             DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"DECLARE @message varchar(255)
                                    DECLARE @identity INT;
                                    DECLARE @return varchar(255);
                                    -- Cek Jika data sudah ada
                                    IF NOT EXISTS(
                                    SELECT id FROM tbmst_subactivity 
                                    WHERE CategoryId = @CategoryId
                                    AND SubCategoryId = @SubCategoryId
                                    AND ActivityId = @ActivityId
                                    AND LongDesc = @LongDesc
                                    AND isnull(IsDeleted, 0) = 0
                                    )
                                    BEGIN
                                    -- insert data jika data belum ada
                                    INSERT INTO tbmst_subactivity(
                                    CategoryId
                                    , SubCategoryId 
                                    , ActivityId 
                                    , SubActivityTypeId 
                                    , ShortDesc 
                                    , LongDesc 
                                    , CreateOn 
                                    , CreateBy 
                                    , CreatedEmail 
                                    )
                                    VALUES(
                                    @CategoryId
                                    , @SubCategoryId 
                                    , @ActivityId 
                                    , @SubActivityTypeId 
                                    , @ShortDesc 
                                    , @LongDesc 
                                    , @CreateOn 
                                    , @CreateBy 
                                    , @CreatedEmail 
                                    )

                                    SET @identity = (SELECT SCOPE_IDENTITY())
                                    SELECT Id, CategoryId, SubCategoryId, ActivityId, SubActivityTypeId, LongDesc, ShortDesc, CreateOn, CreateBy, RefId, CreatedEmail FROM tbmst_subactivity WHERE Id=@identity END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @return = (SELECT RefId FROM tbmst_subactivity WHERE LongDesc=@LongDesc)
                                    SET @message= 'SubActivity with RefId = ' + @return + ' is already exist'
                                    RAISERROR (@message, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );

                                    END";
                var __res = await conn.QueryAsync<SubActivityCreateReturn>(__query, new
                {
                    CategoryId = body.CategoryId,
                    SubCategoryId = body.SubCategoryId,
                    ActivityId = body.ActivityId,
                    SubActivityTypeId = body.SubActivityTypeId,
                    LongDesc = body.LongDesc,
                    ShortDesc = body.ShortDesc,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    CreateBy = body.CreateBy,
                    CreatedEmail = body.CreatedEmail
                }
                );
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<SubActivityDeleteReturn> DeleteSubActivity(SubActivityDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    UPDATE [dbo].[tbmst_subactivity]
                                    SET
                                    IsActive = 0
                                    ,IsDeleted=1
                                    ,DeleteBy=@DeleteBy
                                    ,DeleteOn=@DeleteOn
                                    ,DeleteEmail=@DeleteEmail
                                    WHERE
                                    Id=@Id
                                    SELECT Id, DeleteBy, IsDeleted, DeleteOn, DeleteEmail, RefId FROM tbmst_subactivity WHERE Id=@Id 
                                    ";
                var __res = await conn.QueryAsync<SubActivityDeleteReturn>(__query, new
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

        public async Task<SubActivityModel> GetSubActivityById(SubActivityById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    SELECT 
                                    a.id as CategoryId, 
                                    b.id as SubCategoryId, 
                                    c.id as ActivityId, 
                                    e.id as SubActivityTypeId,
                                    d.id as SubActivityId,
                                    d.Id, 
                                    d.RefId,
                                    d.LongDesc, 
                                    d.ShortDesc 
                                    FROM tbmst_subactivity d
                                    INNER JOIN tbmst_activity c ON d.ActivityId = c.Id
                                    INNER JOIN tbmst_subcategory b ON c.SubCategoryId = b.Id
                                    INNER JOIN tbmst_category a ON b.CategoryId = a.Id
                                    INNER JOIN tbmst_subactivity_type e ON d.SubActivityTypeId = e.id
                                    WHERE d.Id = @Id
                                    AND ISNULL(a.IsDeleted, 0) = 0 
                                    AND ISNULL(b.IsDeleted, 0) = 0
                                    AND ISNULL(c.IsDeleted, 0) = 0 
                                    AND ISNULL(d.IsDeleted, 0) = 0 
                                    AND ISNULL(e.IsDeleted, 0) = 0;";
                var __res = await conn.QueryAsync<SubActivityModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP> GetSubActivityLandingPage(string keyword, string sortColumn,
         string sortDirection = "ASC",
          int dataDisplayed = 10, int pageNum = 0)
        {
            BaseLP? res = null;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = @" CONCAT_WS(' ', 
                                    CategoryId,
                                    CategoryRefId,
                                    CategoryLongDesc,
                                    CategoryShortDesc,
                                    SubCategoryId,
                                    SubCategoryRefId,
                                    SubCategoryLongDesc,
                                    SubCategoryShortDesc,
                                    ActivityId,
                                    ActivityRefId,
                                    ActivityLongDesc,
                                    ActivityShortDesc,
                                    SubActivityTypeId,
                                    SubActivityTypeRefId,
                                    SubActivityTypeLongDesc,
                                    SubActivityTypeShortDesc,
                                    SubActivityId,
                                    SubActivityRefId,
                                    SubActivityLongDesc,
                                    SubActivityShortDesc
                                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_subactivity_lp
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_subactivity_lp
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
                res.Data = __res.Read<SubActivitySelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<IList<SubCategoryforSubActivity>> SubCategoryforSubActivity(int CategoryId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subcategory ts 
                                    inner join tbmst_category tc on ts.CategoryId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @CategoryId";
                var __res = await conn.QueryAsync<SubCategoryforSubActivity>(__query, new { CategoryId = CategoryId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<SubActivityUpdateReturn> UpdateSubActivity(SubActivityUpdate body)
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
                                SELECT id FROM tbmst_subactivity 
                                WHERE CategoryId = @CategoryId
                                AND SubCategoryId = @SubCategoryId
                                AND ActivityId = @ActivityId
                                AND LongDesc = @LongDesc
                                AND isnull(IsDeleted, 0) = 0
                                and id<>@id


                                )
                                BEGIN
                                UPDATE [dbo].[tbmst_subactivity]
                                SET
                                [CategoryId] = @CategoryId
                                ,[SubCategoryId] = @SubCategoryId
                                ,[ActivityId] = @ActivityId
                                ,[SubActivityTypeId] = @SubActivityTypeId
                                ,[LongDesc] = @LongDesc
                                ,[ShortDesc] = @ShortDesc
                                ,[ModifiedOn] = @ModifiedOn
                                ,[ModifiedBy] = @ModifiedBy
                                ,[ModifiedEmail] = @ModifiedEmail
                                WHERE 
                                Id=@Id
                                SELECT Id, CategoryId, SubCategoryId, ActivityId, SubActivityTypeId, LongDesc, ShortDesc, ModifiedOn, ModifiedBy, ModifiedEmail, RefId FROM tbmst_subactivity WHERE Id=@Id
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @messageout='SubActivity already exist'
                                RAISERROR (@messageout, -- Message text.
                                16, -- Severity.
                                1 -- State.
                                );

                                END
                                ";
                var __res = await conn.QueryAsync<SubActivityUpdateReturn>(__query, new
                {
                    id = body.Id,
                    CategoryId = body.CategoryId,
                    SubCategoryId = body.SubCategoryId,
                    ActivityId = body.ActivityId,
                    SubActivityTypeId = body.SubActivityTypeId,
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