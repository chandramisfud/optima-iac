using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class SKUBlitzRepo : ISKUBlitzRepo
    {
        readonly IConfiguration __config;
        public SKUBlitzRepo(IConfiguration config)
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
        public async Task<SKUBlitzCreateReturn> CreateSKUBlitz(SKUBlitzCreate body)
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
                                    SELECT Id FROM tbset_map_product_sap 
                                    WHERE ProductId = @SKUId
                                    AND SAPCode = @SAPCode
                                    AND isnull(IsDelete, 0) = 0
                                    )
                                    BEGIN
                                    INSERT INTO [dbo].[tbset_map_product_sap]
                                    (
                                    ProductId
                                    ,SAPCode
                                    ,CreateOn
                                    ,CreateBy
                                    ,CreatedEmail
                                    ) 
                                    VALUES
                                    (
                                    @SKUId
                                    ,@SAPCode
                                    ,@CreateOn
                                    ,@CreateBy
                                    ,@CreatedEmail
                                    )
                                    SET @identity = (SELECT SCOPE_IDENTITY())
                                    SELECT 
                                    Id, 
                                    ProductId,
                                    SAPCode,
                                    CreateOn,
                                    CreateBy,
                                    CreatedEmail
                                    FROM tbset_map_product_sap WHERE Id=@identity
                                    END
                                    ELSE
                                    BEGIN
                                    -- message error jika data sudah ada
                                    SET @return1 = (SELECT b.LongDesc FROM tbset_map_product_sap a INNER JOIN tbmst_product b on a.ProductId=b.Id WHERE a.ProductId=@SKUId AND a.SAPCode=@SAPCode )
                                    SET @return2 = (SELECT SAPCode FROM tbset_map_product_sap WHERE SAPCode=@SAPCode AND ProductId=@SKUId )
                                    SET @message= 'Mapping SKU to Blitz with SKU = ' + @return1 + ' and Blitz SKU Code = ' + @return2 + ' is already exist'
                                    RAISERROR (@message, -- Message text.
                                    16, -- Severity.
                                    1 -- State.
                                    );
                                    END";

                var __res = await conn.QueryAsync<SKUBlitzCreateReturn>(__query, new
                {
                    SKUId = body.SKUId,
                    SAPCode = body.SAPCode,
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
        public async Task<SKUBlitzDeleteReturn> DeleteSKUBlitz(SKUBlitzDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"INSERT INTO tbhis_map_product_sap
                                    ( 
                                   [ParentId]
                                    ,[ProductId]
                                    ,[SAPCode]
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
                                    id
                                    ,[ProductId]
                                    ,[SAPCode]
                                    ,[CreateOn]
                                    ,[CreateBy]
                                    ,[ModifiedOn]
                                    ,[ModifiedBy]
                                    ,1                 [IsDelete]
                                    ,@DeleteOn         [DeleteOn]
                                    ,@DeleteBy           [DeleteBy]
                                    ,[CreatedEmail]
                                    ,[ModifiedEmail]
                                    ,@DeleteEmail        [DeleteEmail]
                                    FROM
                                        tbset_map_product_sap
                                    WHERE Id = @Id

                                    DELETE tbset_map_product_sap
                                    WHERE Id = @Id
                                    
                                    SELECT Id, DeleteBy, IsDelete, DeleteOn, DeleteEmail, parentId 
                                    FROM tbhis_map_product_sap WHERE parentId = @Id
                                    ";

                var __res = await conn.QueryAsync<SKUBlitzDeleteReturn>(__query, new
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

        public async Task<IList<SKUBlitzModel>> GetSKUBlitzDownload()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT        
		                            Id, 
		                            EntityId, 
		                            Entity, 
		                            BrandId, 
		                            Brand, 
		                            SKUId, 
		                            SKU, 
		                            IsActive, 
		                            CreateOn, 
		                            CreateBy, 
		                            CreatedEmail,
		                            isDelete, 
		                            DeleteOn, 
		                            DeleteBy,
		                            DeleteEmail,
		                            SAPCode
	                            FROM
		                            (
			                            SELECT        
				                            t.Id, 
				                            p.PrincipalId   AS EntityId, 
				                            pr.LongDesc     AS Entity, 
				                            p.BrandId, 
				                            b.LongDesc      AS Brand, 
				                            t.ProductId     AS SKUId, 
				                            p.LongDesc      AS SKU, 
				                            p.IsActive, 
				                            t.CreateOn, 
				                            t.CreateBy, 
				                            t.CreatedEmail,
				                            t.isDelete, 
				                            t.DeleteOn, 
				                            t.DeleteBy,
				                            t.DeleteEmail,
				                            t.SAPCode
			                            FROM            
				                            tbset_map_product_sap t 
				                            INNER JOIN tbmst_product   p  ON t.ProductId   = p.Id 
				                            INNER JOIN tbmst_principal pr ON p.PrincipalId = pr.Id 
				                            INNER JOIN tbmst_brand     b  ON p.BrandId     = b.Id

			                            UNION ALL

			                            SELECT        
				                            t.ParentId      AS id, 
				                            p.PrincipalId   AS EntityId, 
				                            pr.LongDesc     AS Entity, 
				                            p.BrandId, 
				                            b.LongDesc      AS Brand, 
				                            t.ProductId     AS SKUId, 
				                            p.LongDesc      AS SKU, 
				                            p.IsActive, 
				                            t.CreateOn, 
				                            t.CreateBy, 
				                            t.CreatedEmail,
				                            t.isDelete, 
				                            t.DeleteOn, 
				                            t.DeleteBy,
				                            t.DeleteEmail,
				                            t.SAPCode
			                            FROM            
				                            tbhis_map_product_sap t 
				                            INNER JOIN tbmst_product   p  ON t.ProductId   = p.Id 
				                            INNER JOIN tbmst_principal pr ON p.PrincipalId = pr.Id 
				                            INNER JOIN tbmst_brand     b  ON p.BrandId     = b.Id
		                            ) dl";
                var result = await conn.QueryAsync<SKUBlitzModel>(__query);
                return result.AsList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<BaseLP> GetSKUBlitzLandingPage(string keyword,
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
                                    EntityId,
                                    Entity,
                                    BrandId,
                                    Brand,
                                    SKUId,
                                    SKU,
                                    IsActive,
                                    CreateOn,
                                    CreateBy,
                                    CreatedEmail,
                                    SAPCode
                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_map_sku_blitz
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_map_sku_blitz
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
                res.Data = __res.Read<SKUBlitzLandingPage>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<IList<EntityforSKUBlitz>> GetEntityforSKUBlitz()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<EntityforSKUBlitz>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BrandforSKUBlitz>> GetBrandforSKUBlitz(int PrincipalId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_brand ts 
                                    inner join tbmst_principal tc on ts.PrincipalId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @PrincipalId";
                var __res = await conn.QueryAsync<BrandforSKUBlitz>(__query, new { PrincipalId = PrincipalId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SKUforSKUBlitz>> GetSKUforSKUBlitz(int BrandId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_product ts 
                                    inner join tbmst_brand tc on ts.BrandId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @BrandId";
                var __res = await conn.QueryAsync<SKUforSKUBlitz>(__query, new { BrandId = BrandId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}