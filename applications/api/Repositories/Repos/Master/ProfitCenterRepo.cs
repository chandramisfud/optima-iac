using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class ProfitCenterRepository : IProfitCenterRepository
    {
        readonly IConfiguration __config;
        public ProfitCenterRepository(IConfiguration config)
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
        public async Task<ProfitCenterCreateReturn> CreateProfitCenter(ProfitCenterCreate body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                DECLARE @message varchar(255);
                                DECLARE @return varchar(255);
                                -- Cek Jika data sudah ada
                                IF NOT EXISTS
                                (
                                SELECT ProfitCenter FROM [tbmst_profit_center] 
                                WHERE ProfitCenterDesc = @ProfitCenterDesc
                                )
                                BEGIN
                                INSERT INTO [dbo].[tbmst_profit_center]
                                ([ProfitCenter]
                                ,[ProfitCenterDesc]
                                ,[CreateOn]
                                ,[CreateBy]
                                ,[CreatedEmail]
                                ) 
                                VALUES
                                (
                                @ProfitCenter
                                ,@ProfitCenterDesc
                                ,@CreateOn
                                ,@CreateBy
                                ,@CreatedEmail
                                )
                                SELECT ProfitCenter, ProfitCenterDesc, CreateOn, CreateBy, CreatedEmail FROM [tbmst_profit_center] WHERE ProfitCenter=@ProfitCenter
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @return = (SELECT ProfitCenter FROM [tbmst_profit_center] WHERE ProfitCenterDesc=@ProfitCenterDesc)
                                SET @message= 'ProfitCenter = ' + @return + ' is already exist'
                                RAISERROR (@message, -- Message text.
                                16, -- Severity.
                                1 -- State.
                                );
                                END";
                var __res = await conn.QueryAsync<ProfitCenterCreateReturn>(__query, new
                {
                    ProfitCenter = body.ProfitCenter,
                    ProfitCenterDesc = body.ProfitCenterDesc,
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


        public async Task<ProfitCenterModel> GetProfitCenterById(ProfitCenterById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    SELECT * FROM [tbmst_profit_center] 
                                    WHERE ProfitCenter = @ProfitCenter";
                var __res = await conn.QueryAsync<ProfitCenterModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP> GetProfitCenterLandingPage(string keyword, string sortColumn, string sortDirection, int dataDisplayed, int pageNum)
        {
            BaseLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = " CONCAT_WS(' ', ProfitCenter, ProfitCenterDesc) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM tbmst_profit_center 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM tbmst_profit_center
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
                res.Data = __res.Read<ProfitCenterSelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<ProfitCenterUpdateReturn> UpdateProfitCenter(ProfitCenterUpdate body)
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
                    SELECT ProfitCenter FROM [tbmst_profit_center] 
                    WHERE ProfitCenterDesc = @ProfitCenterDesc
                    AND ProfitCenter<>@ProfitCenter
                    )
                    BEGIN
                    UPDATE [dbo].[tbmst_profit_center]
                    SET
                    [ProfitCenterDesc] = @ProfitCenterDesc
                    ,[ModifiedOn] = @ModifiedOn
                    ,[ModifiedBy] = @ModifiedBy
                    ,[ModifiedEmail] = @ModifiedEmail
                    WHERE 
                    ProfitCenter=@ProfitCenter
                    SELECT ProfitCenter, ProfitCenterDesc, ModifiedOn, ModifiedBy, ModifiedEmail FROM [tbmst_profit_center] WHERE ProfitCenter=@ProfitCenter
                    END
                    ELSE
                    BEGIN
                    -- message error jika data sudah ada
                    SET @messageout='ProfitCenter already exist'
                    RAISERROR (@messageout, -- Message text.
                    16, -- Severity.
                    1 -- State.
                    );
                    END
                    ";
                var __res = await conn.QueryAsync<ProfitCenterUpdateReturn>(__query, new
                {
                    ProfitCenter = body.ProfitCenter,
                    ProfitCenterDesc = body.ProfitCenterDesc,
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