using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{

    public class GroupBrandRepo : IGroupBrandRepo
    {
        readonly IConfiguration __config;
        public GroupBrandRepo(IConfiguration config)
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

        public async Task<GroupBrandCreateReturn> CreateGroupBrand(GroupBrandCreate body)
        {
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
                                    SELECT Id FROM tbset_brand_group 
                                    WHERE BrandId = @BrandId
                                    AND isnull(IsDeleted, 0) = 0
                                )
                            BEGIN
                            INSERT INTO [dbo].[tbset_brand_group]
                                (
                                    [BrandId]
                                    ,[LongDesc]
                                    ,[ShortDesc]
                                    ,[SAPCode]
                                    ,[IsActive]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,[CreatedEmail]
                                ) 
                            VALUES
                                (
                                    @BrandId
                                    ,@LongDesc
                                    ,@ShortDesc
                                    ,@SAPCode
                                    ,1
                                    ,GETDATE()
                                    ,@CreateBy
                                    ,@CreatedEmail
                                )
                            SET @identity = (SELECT SCOPE_IDENTITY())
                                SELECT Id, BrandId, LongDesc, ShortDesc, SAPCode, CreateOn, CreateBy, CreatedEmail FROM tbset_brand_group WHERE Id=@identity
                            END
                                ELSE
                            BEGIN
                            -- message error jika data sudah ada
                                SET @return = (SELECT BrandId FROM tbset_brand_group WHERE LongDesc=@LongDesc)
                                SET @message= 'Group Brand with BrandId = ' + @return + ' is already exist'
                            RAISERROR 
                            (
                                @message, -- Message text.
                                16, -- Severity.
                                1 -- State.
                            );
                            END";
                var __res = await conn.QueryAsync<GroupBrandCreateReturn>(__query, body);
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<GroupBrandDeleteReturn> DeleteGroupBrand(GroupBrandDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                UPDATE [dbo].[tbset_brand_group]
                                SET
                                IsActive = 0
                                ,IsDeleted=1
                                ,DeletedBy=@DeletedBy
                                ,DeletedOn=@DeletedOn
                                ,DeleteEmail=@DeleteEmail
                                WHERE
                                Id=@Id
                                SELECT Id, DeletedBy, IsDeleted, DeletedOn, DeleteEmail FROM tbset_brand_group WHERE Id=@Id 
                                    ";
                var __res = await conn.QueryAsync<GroupBrandDeleteReturn>(__query,                 new
                {
                    Id = body.Id,
                    DeletedBy = body.DeletedBy,
                    DeletedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    DeleteEmail = body.DeleteEmail
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<GroupBrandModel> GetGroupBrandById(int Id)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                SELECT * FROM tbset_brand_group 
                                WHERE Id = @Id AND ISNULL(IsDeleted, 0) = 0";
                var __res = await conn.QueryAsync<GroupBrandModel>(__query, new { Id = Id });
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP> GetGroupBrandLandingPage(string keyword, string sortColumn, string sortDirection, int dataDisplayed, int pageNum)
        {
            BaseLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = @" CONCAT_WS(' ', 
                    Id, 
                    BrandId, 
                    LongDesc,
                    ShortDesc,
                    SAPCode
                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data
                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM tbset_brand_group WHERE isnull(IsDeleted, 0)=0 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM tbset_brand_group
                        WHERE {0} AND isnull(IsDeleted, 0)=0 
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
                res.Data = __res.Read<GroupBrandSelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<GroupBrandUpdateReturn> UpdateGroupBrand(GroupBrandUpdate body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                DECLARE @messageout varchar(255)
                                -- Cek Jika data sudah ada
                                IF NOT EXISTS(
                                SELECT Id FROM tbset_brand_group 
                                    WHERE BrandId = @BrandId
                                    AND isnull(IsDeleted, 0) = 0
                                    and Id<>@Id
                                )
                                BEGIN
                                UPDATE [dbo].[tbset_brand_group]
                                SET
                                    [BrandId] = @BrandId
                                    ,[LongDesc] = @LongDesc
                                    ,[ShortDesc] = @ShortDesc
                                    ,[SAPCode] = @SAPCode
                                    ,[ModifiedOn] = GETDATE()
                                    ,[ModifiedBy] = @ModifiedBy
                                    ,[ModifiedEmail] = @ModifiedEmail
                                WHERE 
                                    Id=@Id
                                
                                SELECT Id, BrandId, LongDesc, ShortDesc, ModifiedOn, ModifiedBy, ModifiedEmail, SAPCode FROM tbset_brand_group WHERE Id=@Id
                                END
                                    ELSE
                                    BEGIN
                                -- message error jika data sudah ada
                                    SET @messageout='Group Brand already exist'
                                    RAISERROR 
                                    (
                                        @messageout, -- Message text.
                                        16, -- Severity.
                                        1 -- State.
                                    );
                                END";
                var __res = await conn.QueryAsync<GroupBrandUpdateReturn>(__query, body);
                return __res.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}