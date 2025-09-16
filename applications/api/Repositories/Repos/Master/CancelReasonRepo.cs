using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class CancelReasonRepository : ICancelReasonRepository
    {
        readonly IConfiguration __config;
        public CancelReasonRepository(IConfiguration config)
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
        public async Task<CancelReasonCreateReturn> CreateCancelReason(CancelReasonCreate body)
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
                                SELECT Id FROM [tbmst_cancelreason] 
                                WHERE LongDesc = @LongDesc
                                AND isnull(IsDeleted, 0) = 0
                                )
                                BEGIN
                                INSERT INTO [dbo].[tbmst_cancelreason]
                                (
                                [LongDesc]
                                ,[CreateOn]
                                ,[CreateBy]
                                ,[CreatedEmail]
                                ) 
                                VALUES
                                (
                                @LongDesc
                                ,@CreateOn
                                ,@CreateBy
                                ,@CreatedEmail
                                )
                                SET @identity = (SELECT SCOPE_IDENTITY())
                                SELECT Id, LongDesc, CreateOn, CreateBy, CreatedEmail FROM [tbmst_cancelreason] WHERE Id=@identity
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @return = (SELECT Id FROM [tbmst_cancelreason] WHERE LongDesc=@LongDesc)
                                SET @message= 'CancelReason With Id = ' + @return + ' is already exist'
                                RAISERROR (@message, -- Message text.
                                16, -- Severity.
                                1 -- State.
                                );
                                END";
                var __res = await conn.QueryAsync<CancelReasonCreateReturn>(__query, new
                {
                    LongDesc = body.LongDesc,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    CreateBy = body.CreateBy,
                    CreatedEmail = body.CreatedEmail,
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<CancelReasonDeleteReturn> DeleteCancelReason(CancelReasonDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                UPDATE [dbo].[tbmst_cancelreason]
                                SET
                                IsDeleted=1
                                ,DeletedBy=@DeletedBy
                                ,DeletedOn=@DeletedOn
                                ,DeleteEmail=@DeleteEmail
                                WHERE
                                Id=@Id
                                SELECT Id, DeletedBy, IsDeleted, DeletedOn, DeleteEmail FROM [tbmst_cancelreason] WHERE Id=@Id 
                                ";
                var __res = await conn.QueryAsync<CancelReasonDeleteReturn>(__query, new
                {
                    Id = body.Id,
                    DeletedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    DeletedBy = body.DeletedBy,
                    DeleteEmail = body.DeleteEmail,
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<CancelReasonModel> GetCancelReasonById(CancelReasonById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                SELECT * FROM [tbmst_cancelreason] 
                                WHERE Id = @Id AND ISNULL(IsDeleted, 0) = 0";
                var __res = await conn.QueryAsync<CancelReasonModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP> GetCancelReasonLandingPage(string keyword,
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
                    userFilter = " CONCAT_WS(' ', Id, LongDesc) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM tbmst_cancelreason WHERE isnull(IsDeleted, 0)=0 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM tbmst_cancelreason
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
                res.Data = __res.Read<CancelReasonSelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<CancelReasonUpdateReturn> UpdateCancelReason(CancelReasonUpdate body)
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
                            SELECT id FROM [tbmst_cancelreason] 
                            WHERE LongDesc = @LongDesc
                            AND isnull(IsDeleted, 0) = 0
                            and id<>@id

                            )
                            BEGIN
                            UPDATE [dbo].[tbmst_cancelreason]
                            SET
                            [LongDesc] = @LongDesc
                            ,[ModifiedOn] = @ModifiedOn
                            ,[ModifiedBy] = @ModifiedBy
                            ,[ModifiedEmail] = @ModifiedEmail
                            WHERE 
                            Id=@Id
                            SELECT Id, LongDesc, ModifiedOn, ModifiedBy, ModifiedEmail FROM [tbmst_cancelreason] WHERE Id=@Id
                            END
                            ELSE
                            BEGIN
                            -- message error jika data sudah ada
                            SET @messageout='CancelReason already exist'
                            RAISERROR (@messageout, -- Message text.
                            16, -- Severity.
                            1 -- State.
                            );

                            END
                            ";
                var __res = await conn.QueryAsync<CancelReasonUpdateReturn>(__query, new
                {
                    Id = body.Id,
                    LongDesc = body.LongDesc,
                    ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    ModifiedBy = body.ModifiedBy,
                    ModifiedEmail = body.ModifiedEmail,
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
