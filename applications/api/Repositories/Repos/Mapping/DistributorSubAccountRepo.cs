using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class DistributorSubAccountRepo : IDistributorSubAccountRepo
    {
        readonly IConfiguration __config;
        public DistributorSubAccountRepo(IConfiguration config)
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
        public async Task<IList<DistributorforDistributorSubAccount>> GetDistributorforDistributorSubAccount()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_distributor WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<DistributorforDistributorSubAccount>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<ChannelforDistributorSubAccount>> GetChannelforDistributorSubAccount()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_channel WHERE ISNULL(IsDelete,0)=0 ";
                var __res = await conn.QueryAsync<ChannelforDistributorSubAccount>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<SubChannelforDistributorSubAccount>> GetSubChannelforDistributorSubAccount(int ChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subchannel ts 
                                    inner join tbmst_channel tc on ts.ChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @ChannelId";
                var __res = await conn.QueryAsync<SubChannelforDistributorSubAccount>(__query, new { ChannelId = ChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<AccountforDistributorSubAccount>> GetAccountforDistributorSubAccount(int SubChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_account ts 
                                    inner join tbmst_subchannel tc on ts.SubChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @SubChannelId";
                var __res = await conn.QueryAsync<AccountforDistributorSubAccount>(__query, new { SubChannelId = SubChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<SubAccountforDistributorSubAccount>> GetSubAccountforDistributorSubAccount(int AccountId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subaccount ts 
                                    inner join tbmst_account tc on ts.AccountId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @AccountId";
                var __res = await conn.QueryAsync<SubAccountforDistributorSubAccount>(__query, new { AccountId = AccountId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DistributorSubAccountCreateReturn> CreateDistributorSubAccount(DistributorSubAccountCreate body)
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
                                    SELECT Id FROM tbset_map_distributor_account 
                                    WHERE DistributorId = @DistributorId
                                    AND SubAccountId = @SubAccountId
                                    AND isnull(IsDelete, 0) = 0
                                    )
                                    BEGIN
                                    INSERT INTO [dbo].[tbset_map_distributor_account]
                                    (
                                    DistributorId,
                                    ChannelId,
                                    SubChannelId,
                                    AccountId,
                                    SubAccountId,
                                    CreateOn,
                                    CreateBy,
                                    CreatedEmail
                                    ) 
                                    VALUES
                                    (
                                    @DistributorId
                                    ,@ChannelId
                                    ,@SubChannelId
                                    ,@AccountId
                                    ,@SubAccountId
                                    ,@CreateOn
                                    ,@CreateBy
                                    ,@CreatedEmail
                                    )
                                    SET @identity = (SELECT SCOPE_IDENTITY())
                                    SELECT 
                                    Id, 
                                    DistributorId,
                                    ChannelId,
                                    SubChannelId,
                                    AccountId,
                                    SubAccountId,
                                    CreateOn,
                                    CreateBy,
                                    CreatedEmail
                                    FROM tbset_map_distributor_account WHERE Id=@identity
                                    END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @return1 = (SELECT b.LongDesc FROM tbset_map_distributor_account a INNER JOIN tbmst_distributor b on a.DistributorId=b.Id WHERE a.DistributorId=@DistributorId AND a.SubAccountId=@SubAccountId )
                                    SET @return2 = (SELECT b.LongDesc FROM tbset_map_distributor_account a INNER JOIN tbmst_subaccount b on a.SubAccountId=b.Id WHERE a.DistributorId=@DistributorId AND a.SubAccountId=@SubAccountId )
                                    SET @message= 'Mapping Distributor to SubAccount with Distributor = ' + @return1 + ' and Sub Account = ' + @return2 + ' is already exist'
                                    RAISERROR (@message, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );
                                    END";

                var __res = await conn.QueryAsync<DistributorSubAccountCreateReturn>(__query,
                new
                {
                    DistributorId = body.DistributorId,
                    ChannelId = body.ChannelId,
                    SubChannelId = body.SubChannelId,
                    AccountId = body.AccountId,
                    SubAccountId = body.SubAccountId,
                    CreateBy = body.CreateBy,
                    CreatedEmail = body.CreatedEmail,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<DistributorSubAccountDeleteReturn> DeleteDistributorSubAccount(DistributorSubAccountDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"INSERT INTO tbhis_map_distributor_account
                                    ( 
                                    [DistributorId]
                                    ,[ChannelId]
                                    ,[SubChannelId]
                                    ,[AccountId]
                                    ,[SubAccountId]
                                    ,[IsDelete]
                                    ,[DeleteOn]
                                    ,[DeleteBy]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,[parentId]
                                    ,[CreatedEmail]
                                    ,[DeleteEmail]
                                    )
                                    SELECT
                                    [DistributorId]
                                    ,[ChannelId]
                                    ,[SubChannelId]
                                    ,[AccountId]
                                    ,[SubAccountId]
                                    ,1                     [IsDelete]
                                    ,@DeleteOn            [DeleteOn]
                                    ,@DeleteBy             [DeleteBy]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,id
                                    ,[CreatedEmail]
                                    ,@DeleteEmail            [DeleteEmail]
                                    FROM
                                        tbset_map_distributor_account
                                    WHERE Id = @Id

                                    DELETE tbset_map_distributor_account
                                    WHERE Id = @Id
                                    
                                    SELECT Id, DeleteBy, IsDelete, DeleteOn, DeleteEmail, parentId 
                                    FROM tbhis_map_distributor_account WHERE parentId = @Id
                                    ";

                var __res = await conn.QueryAsync<DistributorSubAccountDeleteReturn>(__query, new
                {
                    Id = body.Id,
                    DeleteBy = body.DeleteBy,
                    DeleteEmail = body.DeleteEmail,
                    DeleteOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DistributorSubAccountDownload>> GetDistributorSubAccountDownload()
        {
            using IDbConnection conn = Connection;
            var result = await conn.QueryAsync<DistributorSubAccountDownload>("ip_mst_map_distributor_account_list_dl", commandType: CommandType.StoredProcedure, commandTimeout:180);
            return result.AsList();

        }

        public async Task<BaseLP> GetDistributorSubAccountLandingPage(string keyword,
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
                    id,
                    DistributorId,
                    Distributor,
                    ChannelId,
                    Channel,
                    SubChannelId,
                    SubChannel,
                    AccountId,
                    Account,
                    SubAccountId,
                    SubAccount,
                    CreateOn,
                    CreateBy,
                    CreatedEmail
                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_map_distributor_subaccount
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_map_distributor_subaccount
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
                res.Data = __res.Read<DistributorSubAccountList>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
    }
}