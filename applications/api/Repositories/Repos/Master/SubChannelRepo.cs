using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{

    public class SubChannelRepository : ISubChannelRepository
    {
        readonly IConfiguration __config;
        public SubChannelRepository(IConfiguration config)
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
        public async Task<SubChannelCreateReturn> CreateSubChannel(SubChannelCreate body)
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
                                    SELECT Id FROM tbmst_subchannel 
                                    WHERE ChannelId = @ChannelId
                                    AND LongDesc = @LongDesc
                                    AND isnull(IsDelete, 0) = 0
                                    )
                                    BEGIN
                                    INSERT INTO [dbo].[tbmst_subchannel]
                                    (
                                    [ChannelId]
                                    ,[LongDesc]
                                    ,[ShortDesc]
                                    ,[IsActive]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,[CreatedEmail]
                                    ) 
                                    VALUES
                                    (
                                    @ChannelId
                                    ,@LongDesc
                                    ,@ShortDesc
                                    ,1
                                    ,@CreateOn
                                    ,@CreateBy
                                    ,@CreatedEmail
                                    )
                                    SET @identity = (SELECT SCOPE_IDENTITY())
                                    SELECT Id, ChannelId, LongDesc, ShortDesc, CreateOn, CreateBy, RefId, CreatedEmail FROM tbmst_subchannel WHERE Id=@identity
                                    END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @return = (SELECT RefId FROM tbmst_subchannel WHERE LongDesc=@LongDesc)
                                    SET @message= 'SubChannel with RefId = ' + @return + ' is already exist'
                                    RAISERROR (@message, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );
                                    END";
                var __res = await conn.QueryAsync<SubChannelCreateReturn>(__query, new
                {
                    ChannelId = body.ChannelId,
                    ShortDesc = body.ShortDesc,
                    LongDesc = body.LongDesc,
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

        public async Task<SubChannelDeleteReturn> DeleteSubChannel(SubChannelDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                UPDATE [dbo].[tbmst_subchannel]
                                SET
                                IsActive = 0
                                ,IsDelete= 1
                                ,DeleteBy=@DeleteBy
                                ,DeleteOn=@DeleteOn
                                ,DeleteEmail=@DeleteEmail
                                WHERE
                                Id=@Id
                                SELECT Id, DeleteBy, IsDelete, DeleteOn, DeleteEmail, RefId FROM tbmst_subchannel WHERE Id=@Id 
                                ";
                var __res = await conn.QueryAsync<SubChannelDeleteReturn>(__query, new
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

        public async Task<IList<SubChanneltoChannel>> GetChannelLongDescforSubChannel()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_channel WHERE ISNULL(IsDelete,0)=0 ";
                var __res = await conn.QueryAsync<SubChanneltoChannel>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<SubChannelModel> GetSubChannelById(SubChannelById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    SELECT * FROM tbmst_subchannel 
                                    WHERE Id = @Id AND ISNULL(IsDelete, 0) = 0";
                var __res = await conn.QueryAsync<SubChannelModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP> GetSubChannelLandingPage(string keyword, string sortColumn, string sortDirection, int dataDisplayed, int pageNum)
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
                                    ChannelLongDesc,
                                    ChannelShortDesc,
                                    ChannelRefid,
                                    SubChannelId,
                                    SubChannelLongDesc,
                                    SubChannelShortDesc,
                                    SubChannelRefid
                                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_subchannel_lp
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_subchannel_lp
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
                res.Data = __res.Read<SubChannelSelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<SubChannelUpdateReturn> UpdateSubChannel(SubChannelUpdate body)
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
                                    SELECT id FROM tbmst_subchannel 
                                    WHERE ChannelId = @ChannelId
                                    AND LongDesc = @LongDesc
                                    AND isnull(IsDelete, 0) = 0
                                    and id<>@id

                                    )
                                    BEGIN
                                    UPDATE [dbo].[tbmst_subchannel]
                                    SET
                                    [ChannelId] = @ChannelId
                                    ,[LongDesc] = @LongDesc
                                    ,[ShortDesc] = @ShortDesc
                                    ,[ModifiedOn] = @modifiedOn
                                    ,[ModifiedBy] = @ModifiedBy
                                    ,[ModifiedEmail] = @ModifiedEmail
                                    WHERE 
                                    Id=@Id
                                    SELECT Id, ChannelId, LongDesc, ShortDesc, ModifiedOn, ModifiedBy, ModifiedEmail, RefId FROM tbmst_subchannel WHERE Id=@Id
                                    END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @messageout='SubChannel already exist'
                                    RAISERROR (@messageout, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );

                                    END
                                    ";
                var __res = await conn.QueryAsync<SubChannelUpdateReturn>(__query, new
                {
                    id = body.Id,
                    ChannelId = body.ChannelId,
                    longDesc = body.LongDesc,
                    shortDesc = body.ShortDesc,
                    modifiedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    modifiedBy = body.ModifiedBy,
                    modifiedEmail = body.ModifiedEmail
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