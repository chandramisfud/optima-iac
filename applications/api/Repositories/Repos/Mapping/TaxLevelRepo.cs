using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class TaxLevelRepo : ITaxLevelRepo
    {
        readonly IConfiguration __config;
        public TaxLevelRepo(IConfiguration config)
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

        public async Task<TaxLevelCreateReturn> CreateTaxLevel(TaxLevelCreate body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"DECLARE @message varchar(255);
                                    DECLARE @identity INT;
                                    DECLARE @return1 varchar(255);
                                    -- Cek Jika data sudah ada
                                    IF NOT EXISTS
                                    (
                                    SELECT MaterialNumber FROM tbset_mapping_material
                                    WHERE MaterialNumber = @MaterialNumber
                                    AND isnull(IsDelete, 0) = 0
                                    )
                                    BEGIN
                                    INSERT INTO [dbo].[tbset_mapping_material]
                                    (
                                        MaterialNumber,
                                        Description,
                                        WHT_Type,
                                        WHT_Code,
                                        Purpose,
                                        Entity,
                                        EntityId,
                                        PPNPct,
                                        PPHPct,
                                        CreateOn,
                                        CreateBy,
                                        CreatedEmail
                                    ) 
                                    VALUES
                                    (
                                        @MaterialNumber,
                                        @Description,
                                        @WHT_Type,
                                        @WHT_Code,
                                        @Purpose,
                                        @Entity,
                                        @EntityId,
                                        @PPNPct,
                                        @PPHPct,
                                        @CreateOn,
                                        @CreateBy,
                                        @CreatedEmail
                                    )
                                    SET @identity = (SELECT SCOPE_IDENTITY())
                                    SELECT 
                                        Id, 
                                        MaterialNumber,
                                        Description,
                                        WHT_Type,
                                        WHT_Code,
                                        Entity,
                                        EntityId,
                                        PPNPct,
                                        PPHPct,
                                        CreateOn,
                                        CreateBy,
                                        CreatedEmail
                                    FROM tbset_mapping_material WHERE Id=@identity
                                    END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @return1 = (SELECT MaterialNumber FROM tbset_mapping_material WHERE MaterialNumber=@MaterialNumber)
                                    SET @message= 'Mapping TaxLevel = ' + @return1 + ' is already exist'
                                    RAISERROR (@message, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );
                                    END";

                var __res = await conn.QueryAsync<TaxLevelCreateReturn>(__query, new
                {
                    MaterialNumber = body.MaterialNumber,
                    Description = body.Description,
                    WHT_Type = body.WHT_Type,
                    WHT_Code = body.WHT_Code,
                    Purpose = body.Purpose,
                    Entity = body.Entity,
                    EntityId = body.EntityId,
                    PPNPct = body.PPNPct,
                    PPHPct = body.PPHPct,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    CreateBy = body.CreateBy,
                    CreatedEmail = body.CreatedEmail
                });
                return __res.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<TaxLevelDeleteReturn> DeleteTaxLevel(TaxLevelDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"INSERT INTO tbhis_mapping_material
                                (
                                    [ParentId]
                                    ,[MaterialNumber]
                                    ,[Description]
                                    ,[WHT_Type]
                                    ,[WHT_Code]
                                    ,[Purpose]
                                    ,[Entity]
                                    ,[EntityId]
                                    ,[PPNPct]
                                    ,[PPHPct]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,[ModifiedOn]
                                    ,[ModifiedBy]
                                    ,[IsDelete]
                                    ,[DeleteOn]
                                    ,[DeleteBy]
                                    ,[CreatedEmail]
                                    ,[ModifiedEmail]
                                    ,[DeleteEmail]
                                )
                                SELECT
                                    [Id]
                                    ,[MaterialNumber]
                                    ,[Description]
                                    ,[WHT_Type]
                                    ,[WHT_Code]
                                    ,[Purpose]
                                    ,[Entity]
                                    ,[EntityId]
                                    ,[PPNPct]
                                    ,[PPHPct]      
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,[ModifiedOn]
                                    ,[ModifiedBy]
                                    ,1             [IsDelete]
                                    ,@DeleteOn     [DeleteOn]
                                    ,@UserId       [DeleteBy]
                                    ,[CreatedEmail]
                                    ,[ModifiedEmail]
                                    ,@UserLogin    [DeleteEmail]
                                    FROM tbset_mapping_material
                                    WHERE Id = @Id

                                    DELETE tbset_mapping_material
                                    WHERE Id  = @Id
                                    SELECT Id, MaterialNumber, DeleteBy, IsDelete, DeleteOn, DeleteEmail, parentId 
                                    FROM tbhis_mapping_material WHERE parentId = @Id
                                    ";
                var __res = await conn.QueryAsync<TaxLevelDeleteReturn>(__query, new
                {
                    Id = body.Id,
                    UserId = body.UserId,
                    UserLogin = body.UserLogin,
                    DeleteOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)
                });
                return __res.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<EntityforTaxLevel>> GetEntityforTaxLevel()
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"SELECT Id, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<EntityforTaxLevel>(__query);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<TaxLevelList>> GetTaxLevelDownload()
        {
            try
            {
                using IDbConnection conn = Connection;
                var result = await conn.QueryAsync<TaxLevelList>("ip_mst_map_material_list_dl", commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP> GetTaxLevelLandingPage(string keyword, string sortColumn, string sortDirection, int dataDisplayed, int pageNum)
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
                        MaterialNumber,
                        MaterialNumber,
                        Description,
                        WHT_Code,
                        Purpose,
                        Entity,
                        PPNPct,
                        PPHPct
                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_map_material_lp
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_map_material_lp
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
                res.Data = __res.Read<TaxLevelList>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
    }
}