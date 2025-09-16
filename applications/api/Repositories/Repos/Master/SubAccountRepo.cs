using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class SubAccountRepository : ISubAccountRepository
    {
        readonly IConfiguration __config;
        public SubAccountRepository(IConfiguration config)
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

        public async Task<SubAccountCreateReturn> CreateSubAccount(SubAccountCreate body)
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
                                        SELECT Id FROM tbmst_subaccount 
                                        WHERE AccountId = @AccountId
                                        AND LongDesc = @LongDesc
                                        AND isnull(IsDelete, 0) = 0
                                        )
                                        BEGIN
                                        INSERT INTO [dbo].[tbmst_subaccount]
                                        (
                                        [AccountId]
                                        ,[LongDesc]
                                        ,[ShortDesc]
                                        ,[IsActive]
                                        ,[CreateOn]
                                        ,[CreateBy]
                                        ,[CreatedEmail]
                                        ) 
                                        VALUES
                                        (
                                        @AccountId
                                        ,@LongDesc
                                        ,@ShortDesc
                                        ,1
                                        ,@CreateOn
                                        ,@CreateBy
                                        ,@CreatedEmail
                                        )
                                        SET @identity = (SELECT SCOPE_IDENTITY())
                                        SELECT Id, AccountId, LongDesc, ShortDesc, CreateOn, CreateBy, RefId, CreatedEmail FROM tbmst_subaccount WHERE Id=@identity
                                        END
                                        ELSE
                                        BEGIN
                                        -- message error jika data sudah ada
                                        SET @return = (SELECT RefId FROM tbmst_subaccount WHERE LongDesc=@LongDesc)
                                        SET @message= 'SubAccount with RefId = ' + @return + ' is already exist'
                                        RAISERROR (@message, -- Message text.
                                        16, -- Severity.
                                        1 -- State.
                                        );
                                        END";
                var __res = await conn.QueryAsync<SubAccountCreateReturn>(__query, new
                {
                    AccountId = body.AccountId,
                    LongDesc = body.LongDesc,
                    ShortDesc = body.ShortDesc,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    CreateBy = body.CreateBy,
                    CreatedEmail = body.CreatedEmail
                });
                return __res.FirstOrDefault()!;
            }
            catch
            (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<SubAccountDeleteReturn> DeleteSubAccount(SubAccountDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                UPDATE [dbo].[tbmst_subaccount]
                                SET
                                IsActive = 0
                                ,IsDelete=1
                                ,DeleteBy=@DeleteBy
                                ,DeleteOn=@DeleteOn
                                ,DeleteEmail=@DeleteEmail
                                WHERE
                                Id=@Id
                                SELECT Id, DeleteBy, IsDelete, DeleteOn, DeleteEmail, RefId FROM tbmst_subaccount WHERE Id=@Id 
                                ";
                var __res = await conn.QueryAsync<SubAccountDeleteReturn>(__query,
                new
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

        public async Task<IList<SubAccountAccount>> GetAccountforSubAccount(int SubChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_account ts 
                                    inner join tbmst_subchannel tc on ts.SubChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @SubChannelId";
                var __res = await conn.QueryAsync<SubAccountAccount>(__query, new { SubChannelId = SubChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<AccountSubChannel>> GetSubChannelforSubAccount(int ChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                            from tbmst_subchannel ts 
                            inner join tbmst_channel tc on ts.ChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                            where isnull(ts.IsDelete, 0) = 0 and  tc.id = @ChannelId";
                var __res = await conn.QueryAsync<AccountSubChannel>(__query, new { ChannelId = ChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<AccountChannel>> GetChannelforSubAccount()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_channel WHERE ISNULL(IsDelete,0)=0 ";
                var __res = await conn.QueryAsync<AccountChannel>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<SubAccountModel> GetSubAccountById(SubAccountById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT 
                                    tc.id as ChannelId, 
                                    tsc.id as SubChannelId,
                                    ta.id as AccountId,
                                    tsa.Id, 
                                    tsa.RefId, 
                                    tsa.LongDesc,  
                                    tsa.ShortDesc
                                    FROM tbmst_subaccount tsa
                                    INNER JOIN tbmst_account ta ON tsa.AccountId = ta.id
                                    INNER JOIN tbmst_subchannel tsc ON ta.SubChannelId = tsc.Id 
                                    INNER JOIN tbmst_channel tc ON tsc.ChannelId = tc.Id 
                                    WHERE tsa.Id = @Id 
                                    AND ISNULL(tsa.IsDelete, 0) = 0 
                                    AND ISNULL(ta.IsDelete, 0) = 0 
                                    AND ISNULL(tsc.IsDelete, 0) = 0 
                                    AND ISNULL(tc.IsDelete, 0) = 0;";
                var __res = await conn.QueryAsync<SubAccountModel>
                (__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<SubAccountUpdateReturn> UpdateSubAccount(SubAccountUpdate body)
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
                                SELECT id FROM tbmst_subaccount 
                                WHERE AccountId = @AccountId
                                AND LongDesc = @LongDesc
                                AND isnull(IsDelete, 0) = 0
                                and id<>@id
                                )
                                BEGIN
                                UPDATE [dbo].[tbmst_subaccount]
                                SET
                                [AccountId] = @AccountId
                                ,[LongDesc] = @LongDesc
                                ,[ShortDesc] = @ShortDesc
                                ,[ModifiedOn] = @ModifiedOn
                                ,[ModifiedBy] = @ModifiedBy
                                ,[ModifiedEmail] = @ModifiedEmail
                                WHERE 
                                Id=@Id
                                SELECT Id, AccountId, LongDesc, ShortDesc, ModifiedOn, ModifiedBy, ModifiedEmail, RefId FROM tbmst_subaccount WHERE Id=@Id
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @messageout='SubAccount already exist'
                                RAISERROR (@messageout, -- Message text.
                                16, -- Severity.
                                1 -- State.
                                );
                                END
                                ";
                var __res = await conn.QueryAsync<SubAccountUpdateReturn>(__query, new
                {
                    id = body.Id,
                    AccountId = body.AccountId,
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

        public async Task<BaseLP> GetSubAccountLandingPage(string keyword, string sortColumn,
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
                                    ChannelId,
                                    ChannelRefId,
                                    ChannelLongDesc,
                                    ChannelShortDesc,
                                    SubChannelId,
                                    SubChannelRefId,
                                    SubChannelLongDesc,
                                    SubChannelShortDesc,
                                    AccountId,
                                    AccountRefId,
                                    AccountLongDesc,
                                    AccountShortDesc,
                                    SubAccountId,
                                    SubAccountRefId,
                                    SubAccountLongDesc,
                                    SubAccountShortDesc
                                    ) 
                                    LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_subaccount_lp
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_subaccount_lp
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
                res.Data = __res.Read<SubAccountSelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
    }

}