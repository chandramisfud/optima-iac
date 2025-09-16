using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class RegionRepository : IRegionRepository
    {
        readonly IConfiguration __config;
        public RegionRepository(IConfiguration config)
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
        public async Task<RegionCreateReturn> CreateRegion(RegionCreate body)
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
                            SELECT Id FROM tbmst_region 
                            WHERE LongDesc = @LongDesc
                            AND isnull(IsDelete, 0) = 0
                            )
                            BEGIN
                            INSERT INTO [dbo].[tbmst_Region]
                            ([LongDesc]
                            ,[ShortDesc]
                            ,[IsActive]
                            ,[CreateOn]
                            ,[CreateBy]
                            ,[CreatedEmail]
                            ) 
                            VALUES
                            (
                            @LongDesc
                            ,@ShortDesc
                            ,1
                            ,@CreateOn
                            ,@CreateBy
                            ,@CreatedEmail
                            )
                            SET @identity = (SELECT SCOPE_IDENTITY())
                            SELECT Id, LongDesc, ShortDesc, CreateOn, CreateBy, RefId, CreatedEmail FROM tbmst_region WHERE Id=@identity
                            END
                            ELSE
                            BEGIN
                            -- message error jika data sudah ada
                            SET @return = (SELECT RefId FROM tbmst_region WHERE LongDesc=@LongDesc)
                            SET @message= 'Region with RefId = ' + @return + ' is already exist'
                            RAISERROR (@message, -- Message text.
                            16, -- Severity.
                            1 -- State.
                            );
                            END";
                var __res = await conn.QueryAsync<RegionCreateReturn>(__query, new
                {
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

        public async Task<RegionDeleteReturn> DeleteRegion(RegionDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    UPDATE [dbo].[tbmst_Region]
                                    SET
                                    IsActive = 0
                                    ,IsDelete=1
                                    ,DeleteBy=@DeleteBy
                                    ,DeleteOn=@DeleteOn
                                    ,DeleteEmail=@DeleteEmail
                                    WHERE
                                    Id=@Id
                                    SELECT Id, DeleteBy, IsDelete, DeleteOn, DeleteEmail, RefId FROM tbmst_Region WHERE Id=@Id 
                                    ";
                var __res = await conn.QueryAsync<RegionDeleteReturn>(__query, new
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

        public async Task<RegionModel> GetRegionById(RegionById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    SELECT * FROM tbmst_region 
                                    WHERE Id = @Id AND ISNULL(IsDelete, 0) = 0";
                var __res = await conn.QueryAsync<RegionModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }


        public async Task<BaseLP> GetRegionLandingPage(string keyword, string sortColumn, string sortDirection, int dataDisplayed, int pageNum)
        {
            BaseLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = " CONCAT_WS(' ', RefId, LongDesc, ShortDesc) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM tbmst_region  WHERE isnull(IsDelete, 0)=0 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM tbmst_region
                        WHERE {0} AND isnull(IsDelete, 0)=0
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
                res.Data = __res.Read<RegionSelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<RegionUpdateReturn> UpdateRegion(RegionUpdate body)
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
                                SELECT id FROM tbmst_Region 
                                WHERE LongDesc = @LongDesc
                                AND isnull(IsDelete, 0) = 0
                                and id<>@id

                                )
                                BEGIN
                                UPDATE [dbo].[tbmst_Region]
                                SET
                                [LongDesc] = @LongDesc
                                ,[ShortDesc] = @ShortDesc
                                ,[ModifiedOn] = @ModifiedOn
                                ,[ModifiedBy] = @ModifiedBy
                                ,[ModifiedEmail] = @ModifiedEmail
                                WHERE 
                                Id=@Id
                                SELECT Id, LongDesc, ShortDesc, ModifiedOn, ModifiedBy, ModifiedEmail, RefId FROM tbmst_Region WHERE Id=@Id
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @messageout='Region already exist'
                                RAISERROR (@messageout, -- Message text.
                                16, -- Severity.
                                1 -- State.
                                );

                                END
                                ";
                var __res = await conn.QueryAsync<RegionUpdateReturn>(__query, new
                {
                    Id = body.Id,
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