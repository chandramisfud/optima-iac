using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class MasterEntityRepository : IEntityRepository
    {
        readonly IConfiguration __config;
        public MasterEntityRepository(IConfiguration config)
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
        public async Task<EntityCreateReturn> CreateEntity(EntityCreate body)
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
                                SELECT Id FROM tbmst_principal			
                                WHERE LongDesc = @LongDesc			
                                AND isnull(IsDeleted, 0) = 0			
                                )			
                                BEGIN			
                                INSERT INTO [dbo].[tbmst_principal]			
                                ([LongDesc]			
                                ,[ShortDesc]			
                                ,[DescForInvoice]			
                                ,[EntityUp]			
                                ,[EntityAddress]			
                                ,[CompanyName]			
                                ,[EntityNPWP]			
                                ,[ShortDesc2]			
                                ,[IsActive]			
                                ,[CreateOn]			
                                ,[CreateBy]			
                                ,[CreatedEmail]			
                                ) 			
                                VALUES			
                                (			
                                @LongDesc			
                                ,@ShortDesc			
                                ,@DescForInvoice		
                                ,@EntityUp		
                                ,@EntityAddress		
                                ,@CompanyName		
                                ,@EntityNPWP		
                                ,@ShortDesc2		
                                ,1			
                                ,@CreateOn
                                ,@CreateBy			
                                ,@CreatedEmail			
                                )			
                                SET @identity = (SELECT SCOPE_IDENTITY())			
                                SELECT Id, LongDesc, ShortDesc, CreateOn, CreateBy, RefId, CreatedEmail, DescForInvoice, EntityUp, EntityAddress, CompanyName, EntityNPWP, ShortDesc2 FROM tbmst_principal WHERE Id = @identity	
                                        
                                END
                                ELSE
                                        
                                BEGIN
                                -- message error jika data sudah ada
                                        
                                SET @return = (SELECT RefId FROM tbmst_principal WHERE LongDesc = @LongDesc)			
                                SET @message = 'Entity with RefId = ' + @return + ' is already exist'
                                        
                                RAISERROR(@message, --Message text.

                                16, --Severity.

                                1-- State.

                                );
                                END";

                var __res = await conn.QueryAsync<EntityCreateReturn>(__query, new
                {
                    LongDesc = body.LongDesc,
                    ShortDesc = body.ShortDesc,
                    DescForInvoice = body.DescForInvoice,
                    EntityUp = body.EntityUp,
                    EntityAddress = body.EntityAddress,
                    CompanyName = body.CompanyName,
                    EntityNPWP = body.EntityNPWP,
                    ShortDesc2 = body.ShortDesc2,
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

        public async Task<EntityDeleteReturn> DeleteEntity(EntityDelete body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"			
                                UPDATE [dbo].[tbmst_principal]			
                                SET			
                                IsActive = 0			
                                ,IsDeleted=1			
                                ,DeletedBy=@DeletedBy			
                                ,DeletedOn=@DeletedOn			
                                ,DeleteEmail=@DeleteEmail			
                                WHERE			
                                Id=@Id			
                                SELECT Id, DeletedBy, IsDeleted, DeletedOn, DeleteEmail, RefId FROM tbmst_principal WHERE Id=@Id 			
                                ";
                var __res = await conn.QueryAsync<EntityDeleteReturn>(__query, new
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

        public async Task<EntityModel> GetEntityById(EntityById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"			
                                    SELECT * FROM tbmst_principal 			
                                    WHERE Id = @Id AND ISNULL(IsDeleted, 0) = 0";
                var __res = await conn.QueryAsync<EntityModel>(__query, body);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP> GetEntityLandingPage(
            string keyword
            , string sortColumn
            , string sortDirection = "ASC"
            , int dataDisplayed = 10
            , int pageNum = 0
            )
        {
            BaseLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = " CONCAT_WS(' ', RefId, LongDesc, ShortDesc) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM tbmst_principal WHERE isnull(IsDeleted, 0)=0 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM tbmst_principal
                        WHERE {0} AND isnull(IsDeleted, 0)=0
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
                res.Data = __res.Read<EntityModel>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<EntityUpdateReturn> UpdateEntity(EntityUpdate body)
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
                                SELECT id FROM tbmst_principal 			
                                WHERE LongDesc = @LongDesc			
                                AND isnull(IsDeleted, 0) = 0			
                                and id<>@id			
                                            
                                )			
                                BEGIN			
                                UPDATE [dbo].[tbmst_principal]			
                                SET			
                                [LongDesc] = @LongDesc			
                                ,[ShortDesc] = @ShortDesc			
                                ,[DescForInvoice]	=@DescForInvoice
                                ,[EntityUp]	=@EntityUp
                                ,[EntityAddress] = @EntityAddress
                                ,[CompanyName]	=@CompanyName
                                ,[EntityNPWP]	=@EntityNPWP
                                ,[ShortDesc2]	=@ShortDesc2
                                ,[ModifiedOn] = @ModifiedOn			
                                ,[ModifiedBy] = @ModifiedBy
                                ,[ModifiedEmail] = @ModifiedEmail			
    
                                WHERE 			
                                Id=@Id			
                                SELECT Id, LongDesc, ShortDesc, ModifiedOn, ModifiedBy, ModifiedEmail, RefId, DescForInvoice, EntityUp, EntityAddress, CompanyName, EntityNPWP, ShortDesc2 FROM tbmst_principal WHERE Id = @Id	
                                        
                                END
                                ELSE
                                        
                                BEGIN
                                -- message error jika data sudah ada
                                        
                                SET @messageout = 'Entity already exist'
                                        
                                RAISERROR(@messageout, --Message text.

                                16, --Severity.

                                1-- State.

                                );

                                END";

                var __res = await conn.QueryAsync<EntityUpdateReturn>(__query, new
                {
                    Id = body.Id,
                    LongDesc = body.LongDesc,
                    ShortDesc = body.ShortDesc,
                    DescForInvoice = body.DescForInvoice,
                    EntityUp = body.EntityUp,
                    EntityAddress = body.EntityAddress,
                    CompanyName = body.CompanyName,
                    EntityNPWP = body.EntityNPWP,
                    ShortDesc2 = body.ShortDesc2,
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



