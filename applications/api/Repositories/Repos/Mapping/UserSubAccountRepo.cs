using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class UserSubAccountRepo : IUserSubAccountRepo
    {
        readonly IConfiguration __config;
        public UserSubAccountRepo(IConfiguration config)
        {
            __config = config;
        }
        //   public IDbConnection Connection => throw new NotImplementedException();
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }
        public async Task<IList<ChannelforUserSubAccount>> GetChannelforUserSubAccount()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_channel WHERE ISNULL(IsDelete,0)=0 ";
                var __res = await conn.QueryAsync<ChannelforUserSubAccount>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<SubChannelforUserSubAccount>> GetSubChannelforUserSubAccount(int ChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subchannel ts 
                                    inner join tbmst_channel tc on ts.ChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @ChannelId";
                var __res = await conn.QueryAsync<SubChannelforUserSubAccount>(__query, new { ChannelId = ChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<AccountforUserSubAccount>> GetAccountforUserSubAccount(int SubChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_account ts 
                                    inner join tbmst_subchannel tc on ts.SubChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @SubChannelId";
                var __res = await conn.QueryAsync<AccountforUserSubAccount>(__query, new { SubChannelId = SubChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<SubAccountforUserSubAccount>> GetSubAccountforUserSubAccount(int AccountId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subaccount ts 
                                    inner join tbmst_account tc on ts.AccountId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @AccountId";
                var __res = await conn.QueryAsync<SubAccountforUserSubAccount>(__query, new { AccountId = AccountId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<UserSubAccountCreateReturn> CreateUserSubAccount(UserSubAccountCreate body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"DECLARE @message varchar(255);
                                    DECLARE @identity INT;
                                    DECLARE @return1 varchar(255);
                                    DECLARE @return2 varchar(255);
                                    -- Cek Jika data sudah ada
                                    IF NOT EXISTS
                                    (
                                    SELECT Id FROM tbset_user_subaccount 
                                    WHERE UserId = @UserId
                                    AND SubAccountId = @SubAccountId
                                    AND isnull(IsDeleted, 0) = 0
                                    )
                                    BEGIN
                                    INSERT INTO [dbo].[tbset_user_subaccount]
                                    (
                                    UserId,
                                    SubAccountId,
                                    CreateOn,
                                    CreateBy,
                                    CreatedEmail
                                    ) 
                                    VALUES
                                    (
                                    @UserId
                                    ,@SubAccountId
                                    ,@CreateOn
                                    ,@CreateBy
                                    ,@CreatedEmail
                                    )
                                    SET @identity = (SELECT SCOPE_IDENTITY())
                                    SELECT 
                                    Id, 
                                    UserId,
                                    SubAccountId,
                                    CreateOn,
                                    CreateBy,
                                    CreatedEmail
                                    FROM tbset_user_subaccount WHERE Id=@identity
                                    END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @return1 = (SELECT b.LongDesc FROM tbset_user_subaccount a INNER JOIN tbmst_subaccount b on a.SubAccountId=b.Id WHERE a.SubAccountId=@SubAccountId AND a.UserId=@UserId )
                                    SET @return2 = (SELECT UserId FROM tbset_user_subaccount WHERE SubAccountId=@SubAccountId AND UserId=@UserId )
                                    SET @message= 'Mapping User to SubAccount with UserId = ' + @return2 + ' and Sub Account = ' + @return1 + ' is already exist'
                                    RAISERROR (@message, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );
                                    END";

                var __res = await conn.QueryAsync<UserSubAccountCreateReturn>(__query, new
                {
                    UserId = body.UserId,
                    SubAccountId = body.SubAccountId,
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
        public async Task<UserSubAccountDeleteReturn> DeleteUserSubAccount(UserSubAccountDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"INSERT INTO tbhis_user_subaccount
                                    ( 
                                    Id
                                    ,UserId
                                    ,SubAccountId
                                    ,IsActive
                                    ,CreateOn
                                    ,CreateBy
                                    ,IsDeleted
                                    ,DeletedOn
                                    ,DeletedBy
                                    ,CreatedEmail
                                    ,DeleteEmail
                                    )
                                    SELECT
                                    Id
                                    ,UserId
                                    ,SubAccountId
                                    ,IsActive
                                    ,CreateOn
                                    ,CreateBy
                                    ,1 IsDeleted
                                    ,@DeletedOn
                                    ,@DeletedBy DeletedBy
                                    ,CreatedEmail
                                    ,@DeleteEmail DeleteEmail
                                    FROM
                                        tbset_user_subaccount
                                    WHERE Id = @Id

                                    DELETE tbset_user_subaccount
                                    WHERE Id = @Id
                                    
                                    SELECT Id, DeleteBy, IsDeleted, DeletedOn, DeleteEmail 
                                    FROM tbhis_user_subaccount WHERE Id = @Id
                                    ";

                var __res = await conn.QueryAsync<UserSubAccountDeleteReturn>(__query, new
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

        public async Task<IList<UserSubAccountModel>> GetUserSubAccountDownload()
        {
            try
            {
                using IDbConnection conn = Connection;
                var result = await conn.QueryAsync<UserSubAccountModel>("ip_mst_user_subaccount_list_dl", commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }


        }

        public async Task<BaseLP> GetUserSubAccountLandingPage(string keyword,
        string sortColumn,
        string sortDirection = "ASC",
        int dataDisplayed = 10,
         int pageNum = 0)
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
                                    UserId,
                                    channelid,
                                    channel,
                                    subchannelid,
                                    subchannel,
                                    accountid,
                                    account,
                                    SubAccountId,
                                    SubAccount
                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_map_user_subaccount
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_map_user_subaccount
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
                res.Data = __res.Read<UserSubAccountLandingPage>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<IList<UserIdUserSubAccount>> GetUserIdUserSubAccount()
        {

            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id FROM tbset_user WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<UserIdUserSubAccount>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}