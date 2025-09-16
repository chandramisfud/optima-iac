using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class ProductRepository : IProductRepository
    {
        readonly IConfiguration __config;
        public ProductRepository(IConfiguration config)
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
        public async Task<ProductCreateReturn> CreateProduct(ProductCreate body)
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
                                        SELECT Id FROM tbmst_product 
                                        WHERE PrincipalId = @PrincipalId
                                        AND BrandId = @BrandId
                                        AND LongDesc = @LongDesc
                                        AND isnull(IsDeleted, 0) = 0
                                        )
                                        BEGIN
                                        INSERT INTO [dbo].[tbmst_product]
                                        (
                                        [PrincipalId]
                                        ,[BrandId]
                                        ,[LongDesc]
                                        ,[ShortDesc]
                                        ,[IsActive]
                                        ,[CreateOn]
                                        ,[CreateBy]
                                        ,[CreatedEmail]
                                        ,[SeqNo]
                                        ) 
                                        VALUES
                                        (
                                        @PrincipalId
                                        ,@BrandId
                                        ,@LongDesc
                                        ,@ShortDesc
                                        ,1
                                        ,@CreateOn
                                        ,@CreateBy
                                        ,@CreatedEmail
                                        ,@SeqNo
                                        )
                                        SET @identity = (SELECT SCOPE_IDENTITY())
                                        SELECT Id, PrincipalId, BrandId, LongDesc, ShortDesc, CreateOn, CreateBy, RefId, CreatedEmail, SeqNo FROM tbmst_product WHERE Id=@identity
                                        END
                                        ELSE
                                        BEGIN
                                        -- message error jika data sudah ada
                                        SET @return = (SELECT RefId FROM tbmst_product WHERE LongDesc=@LongDesc)
                                        SET @message= 'Product with RefId = ' + @return + ' is already exist'
                                        RAISERROR (@message, -- Message text.
                                        16, -- Severity.
                                        1 -- State.
                                        );
                                        END";
                var __res = await conn.QueryAsync<ProductCreateReturn>(__query, new
                {
                    PrincipalId = body.PrincipalId,
                    BrandId = body.BrandId,
                    LongDesc = body.LongDesc,
                    ShortDesc = body.ShortDesc,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    CreateBy = body.CreateBy,
                    CreatedEmail = body.CreatedEmail,
                    SeqNo = body.SeqNo
                });
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<ProductDeleteReturn> DeleteProduct(ProductDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"UPDATE [dbo].[tbmst_product]
                                    SET
                                    IsActive = 0
                                    ,IsDeleted=1
                                    ,DeletedBy=@DeletedBy
                                    ,DeletedOn=@DeletedOn
                                    ,DeleteEmail=@DeleteEmail
                                    WHERE
                                    Id=@Id
                                    SELECT Id, DeletedBy, IsDeleted, DeletedOn, DeleteEmail, RefId FROM tbmst_product WHERE Id=@Id 
                                    ";
                var __res = await conn.QueryAsync<ProductDeleteReturn>(__query, new
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

        public async Task<IList<BrandforProduct>> GetBrandforProduct(int PrincipalId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_brand ts 
                                    inner join tbmst_principal tc on ts.PrincipalId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @PrincipalId";
                var __res = await conn.QueryAsync<BrandforProduct>(__query, new { PrincipalId = PrincipalId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<EntityforProduct>> GetEntityforProduct()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<EntityforProduct>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<ProductModel> GetProductById(ProductById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT 
                                    tp.id as PrincipalId,
                                    tb.id as BrandId,
                                    tsku.id,
                                    tsku.RefId,
                                    tsku.LongDesc,
                                    tsku.ShortDesc
                                    FROM tbmst_product tsku
                                    INNER JOIN tbmst_brand tb ON tsku.BrandId = tb.id
                                    INNER JOIN tbmst_principal tp ON tb.PrincipalId = tp.Id
                                    WHERE tsku.Id = @Id
                                    AND ISNULL(tsku.IsDeleted,0) = 0
                                    AND ISNULL(tb.IsDeleted,0) = 0
                                    AND ISNULL(tp.IsDeleted,0) = 0;";
                var __res = await conn.QueryAsync<ProductModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP> GetProductLandingPage(string keyword,
         string sortColumn, string sortDirection = "ASC", int dataDisplayed = 10, int pageNum = 0)
        {
            BaseLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = @" CONCAT_WS(' ', 
                                    EntityId,
                                    EntityRefId,
                                    EntityLongDesc,
                                    EntityShortDesc,
                                    BrandId,
                                    BrandRefId,
                                    BrandLongDesc,
                                    BrandShortDesc,
                                    ProductId,
                                    ProductRefId,
                                    ProductLongDesc,
                                    ProductShortDesc
                                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_product_lp
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_product_lp
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
                res.Data = __res.Read<ProductSelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<ProductUpdateReturn> UpdateProduct(ProductUpdate body)
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
                                        SELECT id FROM tbmst_product 
                                        WHERE BrandId = @BrandId
                                        AND LongDesc = @LongDesc
                                        AND isnull(IsDeleted, 0) = 0
                                        and id<>@id
                                        )
                                        BEGIN
                                        UPDATE [dbo].[tbmst_product]
                                        SET
                                        [PrincipalId] = @PrincipalId
                                        ,[BrandId] = @BrandId
                                        ,[LongDesc] = @LongDesc
                                        ,[ShortDesc] = @ShortDesc
                                        ,[ModifiedOn] = @ModifiedOn
                                        ,[ModifiedBy] = @ModifiedBy
                                        ,[ModifiedEmail] = @ModifiedEmail
                                        WHERE 
                                        Id=@Id
                                        SELECT Id, PrincipalId, BrandId, LongDesc, ShortDesc, ModifiedOn, ModifiedBy, ModifiedEmail, RefId, SeqNo FROM tbmst_product WHERE Id=@Id
                                        END
                                        ELSE
                                        BEGIN
                                        -- message error jika data sudah ada
                                        SET @messageout='Product already exist'
                                        RAISERROR (@messageout, -- Message text.
                                        16, -- Severity.
                                        1 -- State.
                                        );
                                        END
                                        ";
                var __res = await conn.QueryAsync<ProductUpdateReturn>(__query, new
                {
                    Id = body.Id,
                    PrincipalId = body.PrincipalId,
                    BrandId = body.BrandId,
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
    }
}