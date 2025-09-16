using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class MechanismRepository : IMechanismRepository
    {
        readonly IConfiguration __config;
        public MechanismRepository(IConfiguration config)
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
        public async Task<ReturnInsertMechanisme> CreateMechanisme(InsertMechanismeBody body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"
                                DECLARE @message varchar(255);
                                DECLARE @identity INT;
                                DECLARE @return varchar(255);
                                -- Cek Jika data sudah ada
                                IF NOT EXISTS
                                (
                                SELECT Id FROM tbmst_mechanism 
                                WHERE 
                                Entity = @Entity
                                AND SubCategory = @SubCategory
                                AND Activity = @Activity
                                AND SubActivity = @SubActivity
                                AND ProductId = @ProductId
                                AND Mechanism = @Mechanism
                                AND ChannelId = @ChannelId
                                AND StartDate = @StartDate
                                AND EndDate = @EndDate
                                AND isnull(IsDelete, 0) = 0
                                )
                                BEGIN
                                INSERT INTO [dbo].[tbmst_mechanism]
                                (
                                	EntityId
                                    ,Entity
                                    ,SubCategoryId
                                    ,SubCategory
                                    ,ActivityId
                                    ,Activity
                                    ,SubActivityId
                                    ,SubActivity
                                    ,ProductId
                                    ,Product
                                    ,Requirement
                                    ,Discount
                                    ,Mechanism
                                    ,ChannelId
                                    ,Channel
                                    ,StartDate
                                    ,EndDate
                                    ,CreateOn
                                    ,CreateBy
                                    ,CreatedEmail
                                ) 
                                VALUES
                                (
                                    @EntityId
                                    ,@Entity
                                    ,@SubCategoryId
                                    ,@SubCategory
                                    ,@ActivityId
                                    ,@Activity
                                    ,@SubActivityId
                                    ,@SubActivity
                                    ,@ProductId
                                    ,@Product
                                    ,@Requirement
                                    ,@Discount
                                    ,@Mechanism
                                    ,@ChannelId
                                    ,@Channel
                                    ,@StartDate
                                    ,@EndDate
                                    ,@CreateOn
                                    ,@CreateBy
                                    ,@CreatedEmail
                                )
                                SET @identity = (SELECT SCOPE_IDENTITY())
                                SELECT 
                                Id
                                ,EntityId
                                ,Entity
                                ,SubCategoryId
                                ,SubCategory
                                ,ActivityId
                                ,Activity
                                ,SubActivityId
                                ,SubActivity
                                ,ProductId
                                ,Product
                                ,Requirement
                                ,Discount
                                ,Mechanism
                                ,ChannelId
                                ,Channel
                                ,StartDate
                                ,EndDate
                                ,CreateOn
                                ,CreateBy
                                ,CreatedEmail
                                FROM tbmst_mechanism WHERE Id=@identity
                                END
                                ELSE
                                BEGIN
                                -- message error jika data sudah ada
                                SET @return = (SELECT Mechanism FROM tbmst_mechanism 
                                WHERE 
                                Entity = @Entity
                                AND SubCategory = @SubCategory
                                AND Activity = @Activity
                                AND SubActivity = @SubActivity
                                AND ProductId = @ProductId
                                AND Mechanism = @Mechanism
                                AND ChannelId = @ChannelId
                                AND StartDate = @StartDate
                                AND EndDate = @EndDate                                
                                )
                                SET @message= 'Mechanism with Mechanism = ' + @return + ' is already exist'
                                RAISERROR (@message, -- Message text.
                                16, -- Severity.
                                1 -- State.
                                );
                                END";
                var __res = await conn.QueryAsync<ReturnInsertMechanisme>(__query, new
                {
                    EntityId = body.EntityId,
                    Entity = body.Entity,
                    SubCategoryId = body.SubCategoryId,
                    SubCategory = body.SubCategory,
                    ActivityId = body.ActivityId,
                    Activity = body.Activity,
                    SubActivityId = body.SubActivityId,
                    SubActivity = body.SubActivity,
                    ProductId = body.ProductId,
                    Product = body.Product,
                    Requirement = body.Requirement,
                    Discount = body.Discount,
                    Mechanism = body.Mechanism,
                    ChannelId = body.ChannelId,
                    Channel = body.Channel,
                    StartDate = body.StartDate,
                    EndDate = body.EndDate,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    CreateBy = body.CreateBy,
                    CreatedEmail = body.CreatedEmail,
                });
                return __res.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task DeleteMechanisme(DeleteMechanismeBody body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"			
                                    INSERT INTO tbhis_mechanism(
                                        ParentId,
                                        Entity,
                                        SubCategory,
                                        Activity,
                                        SubActivity,
                                        ProductId,
                                        Product,
                                        Mechanism,
                                        ChannelId,
                                        Channel,
                                        StartDate,
                                        EndDate,
                                        CreateOn,
                                        CreateBy,
                                        CreatedEmail,
                                        ModifiedOn,
                                        ModifiedBy,
                                        ModifiedEmail,
                                        DeleteOn,
                                        DeleteBy,
                                        DeleteEmail 
                                    ) 
                                    SELECT
                                        Id,
                                        Entity,
                                        SubCategory,
                                        Activity,
                                        SubActivity,
                                        ProductId,
                                        Product,
                                        Mechanism,
                                        ChannelId,
                                        Channel,
                                        StartDate,
                                        EndDate,
                                        CreateOn,
                                        CreateBy,
                                        CreatedEmail,
                                        ModifiedOn,
                                        ModifiedBy,
                                        ModifiedEmail,
                                        @DeleteOn,
                                        @DeleteBy,
                                        @DeleteEmail 	
                                    FROM tbmst_mechanism WHERE Id = @Id
	                                DELETE FROM tbmst_mechanism WHERE Id = @Id
                                    ";
                await conn.ExecuteAsync(__query, new
                {
                    Id = body.Id,
                    DeleteOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    DeleteBy = body.DeleteBy,
                    DeleteEmail = body.DeleteEmail
                });
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<GetAttributeByParentRes>> GetMechanismAttributeByParent(GetAttributeByParentBodyReq body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@attribute", body.attribute);
                __param.Add("@longdesc", body.longdesc);
                var result = await conn.QueryAsync<GetAttributeByParentRes>("ip_mst_mechanism_getattribute_byparent", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<MechanismModel>> GetMechanismeListById(GetMechanismById body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@id", body.id);

                var result = await conn.QueryAsync<MechanismModel>("ip_mst_mechanism_getbyid", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<MechanismeListByParamRes>> GetMechanismeListByParam(GetMechanismByParam body)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@entityid", body.entityid);
                __param.Add("@subcategoryid", body.subcategoryid);
                __param.Add("@activityid", body.activityid);
                __param.Add("@subactivityid", body.subactivityid);
                __param.Add("@productid", body.productid);
                __param.Add("@channelid", body.channelid);
                __param.Add("@startdate", body.startdate);
                __param.Add("@enddate", body.enddate);

                var result = await conn.QueryAsync<MechanismeListByParamRes>("ip_mst_mechanism_get", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<IList<MechanismModel>> GetMechanismeLists()
        {
            using IDbConnection conn = Connection;
            try
            {
                var result = await conn.QueryAsync<MechanismModel>("ip_mst_mechanism_list", commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<IList<ResponseImportDto>> ImportMechanism(DataTable mechanism, string userid, string useremail)
        {
            using IDbConnection conn = Connection;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var result = await conn.QueryAsync<ResponseImportDto>("ip_mst_mechanism_upload",
                new
                {
                    mechanism = mechanism.AsTableValuedParameter("MstMechanismType"),
                    userid = userid,
                    useremail = useremail
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);

                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<ImportRecordTotal> ImportMechanismWithStatInfo(DataTable mechanism, string userid, string useremail)
        {
            using IDbConnection conn = Connection;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var __result = await conn.QueryMultipleAsync("ip_mst_mechanism_upload",
                new
                {
                    mechanism = mechanism.AsTableValuedParameter("MstMechanismType"),
                    userid = userid,
                    useremail = useremail
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);

                var resp = __result.Read();
                return __result.Read<ImportRecordTotal>().First();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<ReturnUpdateMechanisme> UpdateMechanisme(UpdateMechanismeBody body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"DECLARE @message varchar(255);
                                DECLARE @identity INT;
                                DECLARE @return varchar(255);
                                -- Cek Jika data sudah ada
                                IF NOT EXISTS
                                (
                                    SELECT Id FROM tbmst_mechanism 
                                    WHERE 
                                    Entity = @Entity
                                    AND SubCategory = @SubCategory
                                    AND Activity = @Activity
                                    AND SubActivity = @SubActivity
                                    AND ProductId = @ProductId
                                    AND Mechanism = @Mechanism
                                    AND ChannelId = @ChannelId
                                    AND StartDate = @StartDate
                                    AND EndDate = @EndDate
                                    AND Id<>@Id
                                )
                                BEGIN
                                UPDATE 
                                    tbmst_mechanism SET
                                    EntityId=@EntityId,
                                    Entity=@Entity,
                                    SubCategoryId=@SubCategoryId,
                                    SubCategory=@SubCategory,
                                    ActivityId=@ActivityId,
                                    Activity=@Activity,
                                    SubActivityId=@SubActivityId,
                                    SubActivity=@SubActivity,
                                    ProductId=@ProductId,
                                    Product=@Product,
                                    Requirement=@Requirement,
                                    Discount=@Discount,
                                    Mechanism=@Mechanism,
                                    ChannelId=@ChannelId,
                                    Channel=@Channel,
                                    StartDate=@StartDate,
                                    EndDate=@EndDate,
                                    ModifiedOn=@ModifiedOn,
                                    ModifiedBy=@ModifiedBy,
                                    ModifiedEmail=@ModifiedEmail
                                    WHERE Id = @Id
                                SELECT
                                    Id
                                    ,EntityId
                                    ,Entity
                                    ,SubCategoryId
                                    ,SubCategory
                                    ,ActivityId
                                    ,Activity
                                    ,SubActivityId
                                    ,SubActivity
                                    ,ProductId
                                    ,Product
                                    ,Requirement
                                    ,Discount
                                    ,Mechanism
                                    ,ChannelId
                                    ,Channel
                                    ,StartDate
                                    ,EndDate
                                    ,ModifiedOn
                                    ,ModifiedBy
                                    ,ModifiedEmail
                                    FROM tbmst_mechanism 
                                    WHERE Id = @Id
                                END
                                    ELSE
                                    BEGIN
                                    SET @return = (SELECT Mechanism FROM tbmst_mechanism 
                                                WHERE 
                                                Entity = @Entity
                                                AND SubCategory = @SubCategory
                                                AND Activity = @Activity
                                                AND SubActivity = @SubActivity
                                                AND ProductId = @ProductId
                                                AND Mechanism = @Mechanism
                                                AND ChannelId = @ChannelId
                                                AND StartDate = @StartDate
                                                AND EndDate = @EndDate                                
                                                )
                                        SET @message='Mechanism with Mechanism = ' + @return + ' already exist'
                                        RAISERROR (@message, -- Message text.
                                        16, -- Severity.
                                        1 -- State.
                                        );
                                END

                            /* Create History Insert */
                            INSERT INTO tbhis_mechanism(
                                    ParentId
                                    ,EntityId
                                    ,Entity
                                    ,SubCategoryId
                                    ,SubCategory
                                    ,ActivityId
                                    ,Activity
                                    ,SubActivityId
                                    ,SubActivity
                                    ,ProductId
                                    ,Product
                                    ,Requirement
                                    ,Discount
                                    ,Mechanism
                                    ,ChannelId
                                    ,Channel
                                    ,StartDate
                                    ,EndDate
                                    ,ModifiedOn
                                    ,ModifiedBy
                                    ,ModifiedEmail			
                            ) VALUES (
                                @Id
                                ,@EntityId
                                ,@Entity
                                ,@SubCategoryId
                                ,@SubCategory
                                ,@ActivityId
                                ,@Activity
                                ,@SubActivityId
                                ,@SubActivity
                                ,@ProductId
                                ,@Product
                                ,@Requirement
                                ,@Discount
                                ,@Mechanism
                                ,@ChannelId
                                ,@Channel
                                ,@StartDate
                                ,@EndDate
                                ,@ModifiedOn
                                ,@ModifiedBy
                                ,@ModifiedEmail	
                            )";
                var result = await conn.QueryAsync<ReturnUpdateMechanisme>(__query, new
                {
                    Id = body.Id,
                    EntityId = body.EntityId,
                    Entity = body.Entity,
                    SubCategoryId = body.SubCategoryId,
                    SubCategory = body.SubCategory,
                    ActivityId = body.ActivityId,
                    Activity = body.Activity,
                    SubActivityId = body.SubActivityId,
                    SubActivity = body.SubActivity,
                    ProductId = body.ProductId,
                    Product = body.Product,
                    Requirement = body.Requirement,
                    Discount = body.Discount,
                    Mechanism = body.Mechanism,
                    ChannelId = body.ChannelId,
                    Channel = body.Channel,
                    StartDate = body.StartDate,
                    EndDate = body.EndDate,
                    ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    ModifiedBy = body.ModifiedBy,
                    ModifiedEmail = body.ModifiedEmail
                });
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<IList<EntityForMechanism>> GetEntityForMechanisms()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<EntityForMechanism>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ChannelforMechanism>> GetChannelForMechanisms()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_channel WHERE ISNULL(IsDelete,0)=0 ";
                var __res = await conn.QueryAsync<ChannelforMechanism>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetMechanismTemplate()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __res = await conn.QueryAsync<object>("ip_mst_mechanism_tmp_list");
                    return __res.ToList();
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<BaseLP> GetMechanismLandingPage(string keyword, string sortColumn
        , string sortDirection = "ASC"
        , int dataDisplayed = 10
        , int pageNum = 0)
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
                    SubCategoryId,
                    SubCategory,
                    ActivityId,
                    Activity,
                    SubActivityId,
                    SubActivity,
                    ProductId,
                    Product,
                    Requirement,
                    Discount,
                    Mechanism,
                    ChannelId,
                    Channel
                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM tbmst_mechanism WHERE isnull(IsDelete, 0)=0 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM tbmst_mechanism
                        WHERE {0} AND isnull(IsDelete, 0)=0
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
                res.Data = __res.Read<MechanismModel>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<BaseLP> GetSubAccountLandingPage(string keyword, string sortColumn, string sortDirection = "ASC", int dataDisplayed = 10, int pageNum = 0)
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


        public async Task<BaseLP> GetProductLandingPage(string keyword, string sortColumn, string sortDirection = "ASC", int dataDisplayed = 10, int pageNum = 0)
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

        public async Task<BaseLP> GetSubActivityLandingPage(string keyword, string sortColumn, string sortDirection = "ASC", int dataDisplayed = 10, int pageNum = 0)
        {
            BaseLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = @" CONCAT_WS(' ', 
                                    CategoryId,
                                    CategoryRefId,
                                    CategoryLongDesc,
                                    CategoryShortDesc,
                                    SubCategoryId,
                                    SubCategoryRefId,
                                    SubCategoryLongDesc,
                                    SubCategoryShortDesc,
                                    ActivityId,
                                    ActivityRefId,
                                    ActivityLongDesc,
                                    ActivityShortDesc,
                                    SubActivityTypeId,
                                    SubActivityTypeRefId,
                                    SubActivityTypeLongDesc,
                                    SubActivityTypeShortDesc,
                                    SubActivityId,
                                    SubActivityRefId,
                                    SubActivityLongDesc,
                                    SubActivityShortDesc
                                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_subactivity_lp
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_subactivity_lp
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
                res.Data = __res.Read<SubActivitySelect>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
    }
}