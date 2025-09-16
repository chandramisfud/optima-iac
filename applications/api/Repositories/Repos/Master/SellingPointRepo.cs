using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class SellingPointRepository : ISellingPointRepository
    {
        readonly IConfiguration __config;
        public SellingPointRepository(IConfiguration config)
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
        public async Task<SellingPointCreateReturn> CreateSellingPoint(SellingPointCreate body)
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
                                DECLARE @return2 varchar(255);
                                -- Cek Jika data sudah ada

                                IF NOT EXISTS
                                (
                                    SELECT Id FROM tbmst_sellingpoint
                                    WHERE 
                                    isnull(IsDelete, 0) = 0 
                                    AND 
                                    (RefId = @RefId
                                    OR LongDesc = @LongDesc)
                                )
                                BEGIN
                                INSERT INTO [dbo].[tbmst_sellingpoint]
                                (
                                    [AreaCode]
                                    ,RefId
                                    ,RegionId
                                    ,ProfitCenter
                                    ,[LongDesc]
                                    ,[ShortDesc]
                                    ,[IsActive]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,[CreatedEmail]
                                )
                                VALUES
                                (
                                    @AreaCode
                                    ,@RefId
                                    ,@RegionId
                                    ,@ProfitCenter
                                    ,@LongDesc
                                    ,@ShortDesc
                                    ,1
                                    ,@CreateOn
                                    ,@CreateBy
                                    ,@CreatedEmail
                                )
                                SET @identity = (SELECT SCOPE_IDENTITY())
                                SELECT Id, AreaCode, RegionId, ProfitCenter, LongDesc, ShortDesc, CreateOn, CreateBy, RefId, CreatedEmail FROM tbmst_sellingpoint WHERE Id=@identity
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @return = (SELECT TOP 1 RefId FROM tbmst_sellingpoint WHERE LongDesc = @LongDesc OR RefId=@RefId)
                                SET @return2 = (SELECT TOP 1 LongDesc FROM tbmst_sellingpoint WHERE RefId=@RefId OR LongDesc=@LongDesc)
                                SET @message = 'SellingPoint with RefId = ' + @return + ' and Description = ' + @return2 + ' is already exist'

                                RAISERROR (@message, -- Message text.
                                16, -- Severity.
                                1 -- State.
                                );
                                END";

                var __res = await conn.QueryAsync<SellingPointCreateReturn>(__query, new
                {
                    RefId = body.RefId,
                    AreaCode = body.AreaCode,
                    RegionId = body.RegionId,
                    ProfitCenter = body.ProfitCenter,
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

        public async Task<SellingPointDeleteReturn> DeleteSellingPoint(SellingPointDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    UPDATE [dbo].[tbmst_sellingpoint]
                                    SET
                                    IsActive = 0
                                    ,IsDelete=1
                                    ,DeleteBy=@DeleteBy
                                    ,DeleteOn=@DeleteOn
                                    ,DeleteEmail=@DeleteEmail
                                    WHERE
                                    Id=@Id
                                    SELECT Id, DeleteBy, IsDelete, DeleteOn, DeleteEmail, RefId FROM tbmst_sellingpoint WHERE Id=@Id 
                                    ";
                var __res = await conn.QueryAsync<SellingPointDeleteReturn>(__query, new
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

        public async Task<IList<ProfitCenterforSellingPoint>> GetProfitCenterforSellingPoint()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT ProfitCenter,ProfitCenterDesc FROM tbmst_profit_center";
                var __res = await conn.QueryAsync<ProfitCenterforSellingPoint>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<RegionforSellingPoint>> GetRegionforSellingPoint()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_region WHERE ISNULL(IsDelete,0)=0 ";
                var __res = await conn.QueryAsync<RegionforSellingPoint>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<SellingPointModel> GetSellingPointById(SellingPointById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                SELECT * FROM tbmst_sellingpoint
                                WHERE Id = @Id AND ISNULL(IsDelete, 0) = 0";
                var __res = await conn.QueryAsync<SellingPointModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP> GetSellingPointLandingPage(string keyword,
        string sortColumn,
         string sortDirection = "ASC", int dataDisplayed = 10, int pageNum = 0)
        {
            BaseLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = @" CONCAT_WS(' ', RegionId, RegionRefId, RegionLongDesc,
                    RegionShortDesc,
                    ProfitCenter,
                    ProfitCenterDesc,
                    SellingPointId,
                    SellingPointRefId,
                    SellingPointLongDesc,
                    SellingPointShortDesc,
                    AreaCode
                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_sellingpoint_lp
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_sellingpoint_lp
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
                res.Data = __res.Read<SellingPointSelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<SellingPointUpdateReturn> UpdateSellingPoint(SellingPointUpdate body)
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
                            SELECT id FROM tbmst_sellingpoint
                            WHERE
                            RefId = @RefId
                            AND LongDesc = @LongDesc
                            AND isnull(IsDelete, 0) = 0
                            and id<>@id

                            )
                            BEGIN
                            UPDATE [dbo].[tbmst_sellingpoint]
                            SET
                            RefId = @RefId
                            ,[AreaCode] = @AreaCode
                            ,[RegionId] = @RegionId
                            ,[ProfitCenter] = @ProfitCenter
                            ,[LongDesc] = @LongDesc
                            ,[ShortDesc] = @ShortDesc
                            ,[ModifiedOn] = @ModifiedOn
                            ,[ModifiedBy] = @ModifiedBy
                            ,[ModifiedEmail] = @ModifiedEmail
                            WHERE
                            Id=@Id
                            SELECT Id, AreaCode, RegionId,ProfitCenter, LongDesc, ShortDesc, ModifiedOn, ModifiedBy, ModifiedEmail, RefId FROM tbmst_sellingpoint WHERE Id=@Id
                            END
                            ELSE
                            BEGIN
                            -- message error jika data sudah ada
                            SET @messageout='SellingPoint already exist'
                            RAISERROR (@messageout, -- Message text.
                            16, -- Severity.
                            1 -- State.
                            );

                            END
                            ";
                var __res = await conn.QueryAsync<SellingPointUpdateReturn>(__query, new
                {
                    Id = body.Id,
                    RefId = body.RefId,
                    AreaCode = body.AreaCode,
                    RegionId = body.RegionId,
                    ProfitCenter = body.ProfitCenter,
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