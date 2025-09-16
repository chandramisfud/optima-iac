using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class PICDNManualRepo : IPICDNManualRepo
    {
        readonly IConfiguration __config;
        public PICDNManualRepo(IConfiguration config)
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
        public async Task<IList<ChannelforPICDNManual>> GetChannelforPICDNManual()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_channel WHERE ISNULL(IsDelete,0)=0 ";
                var __res = await conn.QueryAsync<ChannelforPICDNManual>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<SubChannelforPICDNManual>> GetSubChannelforPICDNManual(int ChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subchannel ts 
                                    inner join tbmst_channel tc on ts.ChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @ChannelId";
                var __res = await conn.QueryAsync<SubChannelforPICDNManual>(__query, new { ChannelId = ChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<AccountforPICDNManual>> GetAccountforPICDNManual(int SubChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_account ts 
                                    inner join tbmst_subchannel tc on ts.SubChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @SubChannelId";
                var __res = await conn.QueryAsync<AccountforPICDNManual>(__query, new { SubChannelId = SubChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<SubAccountforPICDNManual>> GetSubAccountforPICDNManual(int AccountId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subaccount ts 
                                    inner join tbmst_account tc on ts.AccountId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @AccountId";
                var __res = await conn.QueryAsync<SubAccountforPICDNManual>(__query, new { AccountId = AccountId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<UserIdPICDNManual>> GetUserIdPICDNManual()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id FROM tbset_user WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<UserIdPICDNManual>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PICDNManualCreateReturn> CreatePICDNManual(PICDNManualCreate body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"DECLARE @message varchar(255);
                                    DECLARE @identity INT;
                                    DECLARE @return varchar(255);
                                    -- Cek Jika data sudah ada
                                    IF NOT EXISTS
                                    (
                                    SELECT Id FROM tbset_matrix_dnmanual_assignment_new 
                                    WHERE SubAccountId = @SubAccountId
                                    AND isnull(IsDelete, 0) = 0
                                    )
                                    BEGIN
                                    INSERT INTO [dbo].[tbset_matrix_dnmanual_assignment_new]
                                    (
                                    ChannelId
                                    ,SubChannelId
                                    ,AccountId
                                    ,SubAccountId
                                    ,Pic1
                                    ,Pic2
                                    ,IsActive
                                    ,CreatedOn
                                    ,CreatedBy
                                    ,CreatedEmail
                                    ) 
                                    VALUES
                                    (
                                    @ChannelId
                                    ,@SubChannelId
                                    ,@AccountId
                                    ,@SubAccountId
                                    ,@Pic1
                                    ,@Pic2
                                    ,1
                                    ,@CreatedOn
                                    ,@CreatedBy
                                    ,@CreatedEmail
                                    )
                                    SET @identity = (SELECT SCOPE_IDENTITY())
                                    SELECT 
                                    Id
                                    ,ChannelId
                                    ,SubChannelId
                                    ,AccountId
                                    ,SubAccountId
                                    ,Pic1
                                    ,Pic2
                                    ,CreatedOn
                                    ,CreatedBy
                                    ,CreatedEmail
                                    FROM tbset_matrix_dnmanual_assignment_new WHERE Id=@identity
                                    END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @return = (SELECT b.LongDesc FROM tbset_matrix_dnmanual_assignment_new a INNER JOIN tbmst_subaccount b on a.SubAccountId=b.Id WHERE a.SubAccountId=@SubAccountId )
                                    SET @message= 'Mapping PIC to DN Manual Sub Account = ' + @return + ' is already exist'
                                    RAISERROR (@message, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );
                                    END";

                var __res = await conn.QueryAsync<PICDNManualCreateReturn>(__query, new
                {
                    ChannelId = body.ChannelId,
                    SubChannelId = body.SubChannelId,
                    AccountId = body.AccountId,
                    SubAccountId = body.SubAccountId,
                    Pic1 = body.Pic1,
                    Pic2 = body.Pic2,
                    CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    CreatedBy = body.CreatedBy,
                    CreatedEmail = body.CreatedEmail
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<PICDNManualDeleteReturn> DeletePICDNManual(PICDNManualDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"INSERT INTO tbhis_matrix_dnmanual_assignment_new
                                    ( 
                                    [ParentId]
                                    ,[ChannelId]
                                    ,[SubChannelId]
                                    ,[AccountId]
                                    ,[SubAccountId]
                                    ,[Pic1]
                                    ,[Pic2]
                                    ,[IsActive]
                                    ,[CreatedOn]
                                    ,[CreatedBy]
                                    ,[ModifiedOn]
                                    ,[ModifiedBy]
                                    ,[IsDeleted]
                                    ,[DeletedOn]
                                    ,[DeletedBy]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,[IsDelete]
                                    ,[DeleteOn]
                                    ,[DeleteBy]
                                    ,[CreatedEmail]
                                    ,[ModifiedEmail]
                                    ,[DeleteEmail]
                                    )
                                    SELECT
                                    id
                                    ,[ChannelId]
                                    ,[SubChannelId]
                                    ,[AccountId]
                                    ,[SubAccountId]
                                    ,[Pic1]
                                    ,[Pic2]
                                    ,[IsActive]
                                    ,[CreatedOn]
                                    ,[CreatedBy]
                                    ,[ModifiedOn]
                                    ,[ModifiedBy]
                                    ,1                    [IsDeleted]
                                    ,@DeletedOn
                                    ,@DeleteBy              [DeletedBy]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,1                    [IsDelete]
                                    ,@DeleteOn            [DeleteOn]
                                    ,@DeletedBy              [DeleteBy]
                                    ,[CreatedEmail]
                                    ,[ModifiedEmail]
                                    ,@DeleteEmail           [DeleteEmail]
                                    FROM
                                        tbset_matrix_dnmanual_assignment_new
                                    WHERE Id = @Id

                                    DELETE tbset_matrix_dnmanual_assignment_new
                                    WHERE Id = @Id
                                    
                                    SELECT Id, DeleteBy, IsDelete, DeleteOn,  DeletedBy, IsDeleted, DeletedOn, DeleteEmail, parentId 
                                    FROM tbhis_matrix_dnmanual_assignment_new WHERE ParentId = @Id
                                    ";

                var __res = await conn.QueryAsync<PICDNManualDeleteReturn>(__query, new
                {
                    Id = body.Id,
                    DeletedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    DeleteOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    DeletedBy = body.DeletedBy,
                    DeleteBy = body.DeleteBy,
                    DeleteEmail = body.DeleteEmail

                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<PICDNMAnualModel>> GetPICDNManualDownload()
        {
            using IDbConnection conn = Connection;
            var result = await conn.QueryAsync<PICDNMAnualModel>("ip_mst_map_matrix_dnmanual_assignment_new_list_dl", commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.AsList();
        }

        public async Task<BaseLP> GetPICDNManualLandingPage(string keyword,
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
                                    ChannelId,
                                    Channel,
                                    SubChannelId,
                                    SubChannel,
                                    AccountId,
                                    Account,
                                    SubAccountId,
                                    SubAccount,
                                    Pic1,
                                    Pic2,
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
                        FROM vw_matrix_dnmanual_assignment
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_matrix_dnmanual_assignment
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
                res.Data = __res.Read<PICDNManualLandingPage>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
    }
}