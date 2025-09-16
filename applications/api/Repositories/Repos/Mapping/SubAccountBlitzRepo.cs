using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class SubAccountBlitzRepo : ISubAccountBlitzRepo
    {
        readonly IConfiguration __config;
        public SubAccountBlitzRepo(IConfiguration config)
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
        public async Task<IList<ChannelforSubAccountBlitz>> GetChannelforSubAccountBlitz()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_channel WHERE ISNULL(IsDelete,0)=0 ";
                var __res = await conn.QueryAsync<ChannelforSubAccountBlitz>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<SubChannelforSubAccountBlitz>> GetSubChannelforSubAccountBlitz(int ChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subchannel ts 
                                    inner join tbmst_channel tc on ts.ChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @ChannelId";
                var __res = await conn.QueryAsync<SubChannelforSubAccountBlitz>(__query, new { ChannelId = ChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<AccountforSubAccountBlitz>> GetAccountforSubAccountBlitz(int SubChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_account ts 
                                    inner join tbmst_subchannel tc on ts.SubChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @SubChannelId";
                var __res = await conn.QueryAsync<AccountforSubAccountBlitz>(__query, new { SubChannelId = SubChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<SubAccountforSubAccountBlitz>> GetSubAccountforSubAccountBlitz(int AccountId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subaccount ts 
                                    inner join tbmst_account tc on ts.AccountId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @AccountId";
                var __res = await conn.QueryAsync<SubAccountforSubAccountBlitz>(__query, new { AccountId = AccountId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<SubAccountBlitzCreateReturn> CreateSubAccountBlitz(SubAccountBlitzCreate body)
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
                                    SELECT Id FROM tbset_map_subaccount_sap 
                                    WHERE SubAccountId = @SubAccountId
                                    AND SAPCode = @SAPCode
                                    AND isnull(IsDelete, 0) = 0
                                    )
                                    BEGIN
                                    INSERT INTO [dbo].[tbset_map_subaccount_sap]
                                    (
                                    ChannelId,
                                    SubChannelId,
                                    AccountId,
                                    SubAccountId,
                                    SAPCode,
                                    CreateOn,
                                    CreateBy,
                                    CreatedEmail
                                    ) 
                                    VALUES
                                    (
                                    @ChannelId
                                    ,@SubChannelId
                                    ,@AccountId
                                    ,@SubAccountId
                                    ,@SAPCode
                                    ,@CreateOn
                                    ,@CreateBy
                                    ,@CreatedEmail
                                    )
                                    SET @identity = (SELECT SCOPE_IDENTITY())
                                    SELECT 
                                    Id, 
                                    ChannelId,
                                    SubChannelId,
                                    AccountId,
                                    SubAccountId,
                                    SAPCode,
                                    CreateOn,
                                    CreateBy,
                                    CreatedEmail
                                    FROM tbset_map_subaccount_sap WHERE Id=@identity
                                    END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @return1 = (SELECT b.LongDesc FROM tbset_map_subaccount_sap a INNER JOIN tbmst_subaccount b on a.SubAccountId=b.Id WHERE a.SubAccountId=@SubAccountId AND a.SAPCode=@SAPCode )
                                    SET @return2 = (SELECT SAPCode FROM tbset_map_subaccount_sap WHERE SubAccountId=@SubAccountId AND SAPCode=@SAPCode )
                                    SET @message= 'Mapping SubAccount to Blitz with Sub Account = ' + @return1 + ' and Blitz Sub Account Code = ' + @return2 + ' is already exist'
                                    RAISERROR (@message, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );
                                    END";

                var __res = await conn.QueryAsync<SubAccountBlitzCreateReturn>(__query, new
                {
                    ChannelId = body.ChannelId,
                    SubChannelId = body.SubChannelId,
                    AccountId = body.AccountId,
                    SubAccountId = body.SubAccountId,
                    SAPCode = body.SAPCode,
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
        public async Task<SubAccountBlitzDeleteReturn> DeleteSubAccountBlitz(SubAccountBlitzDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"INSERT INTO tbhis_map_subaccount_sap
                                    ( 
                                    Id
                                    ,[SubAccountId]
                                    ,SAPCode
                                    ,[IsDelete]
                                    ,[DeleteOn]
                                    ,[DeleteBy]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,[CreatedEmail]
                                    ,[DeleteEmail]
                                    )
                                    SELECT
                                    [Id]
                                    ,[SubAccountId]
                                    ,SAPCode
                                    ,1                     [IsDelete]
                                    ,@CreateOn            [DeleteOn]
                                    ,@DeleteBy             [DeleteBy]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,[CreatedEmail]
                                    ,@DeleteEmail            [DeleteEmail]
                                    FROM
                                        tbset_map_subaccount_sap
                                    WHERE Id = @Id

                                    DELETE tbset_map_subaccount_sap
                                    WHERE Id = @Id
                                    
                                    SELECT Id, DeleteBy, IsDelete, DeleteOn, DeleteEmail 
                                    FROM tbhis_map_subaccount_sap WHERE Id = @Id
                                    ";

                var __res = await conn.QueryAsync<SubAccountBlitzDeleteReturn>(__query, new
                {
                    Id = body.Id,
                    DeleteBy = body.DeleteBy,
                    DeleteEmail = body.DeleteEmail,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SubAccountBlitzModel>> GetSubAccountBlitzDownload()
        {
            try
            {
                using IDbConnection conn = Connection;
                var result = await conn.QueryAsync<SubAccountBlitzModel>("ip_mst_map_subaccount_sap_list_dl");
                return result.AsList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<BaseLP> GetSubAccountBlitzLandingPage(string keyword,
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
                            AccountId,
                            Account,
                            subAccountid,
                            subAccount,
                            ChannelId,
                            Channel,
                            SubChannelId,
                            SubChannel,
                            sapCode
                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_map_subaccount_blitz
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_map_subaccount_blitz
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
                res.Data = __res.Read<SubAccountBlitzLandingPage>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
    }
}