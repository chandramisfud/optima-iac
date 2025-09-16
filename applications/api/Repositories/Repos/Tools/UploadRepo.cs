using System.Data;
using System.Data.SqlClient;
using Dapper;
using Entities.Tools;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public partial class UploadRepo : IUploadRepo
    {
        readonly IConfiguration _config;
        public UploadRepo(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public Task<int> InsertUploadLog(string activity, string filename, string profileId, string email, string status)
        {
            int logId = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                logId = conn.Query<int>("ip_log_upload",
                new
                {
                    activity = activity,
                    filename = filename,
                    profileId = profileId,
                    email = email,
                    logstatus = status
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180).First();

            }
            return Task.FromResult(logId);
        }

        public Task<object> GetUploadLog(string activity)
        {
            object _res = null;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                _res = conn.Query<object>("ip_log_upload_get",
                new
                {
                    activity = activity
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);

            }
            return Task.FromResult(_res);
        }
        public Task<int> ImportBudgetRegion(DataTable importBudgetRegion, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                rowAffected = conn.Execute("ip_import_alloc_region",
                new
                {
                    Region = importBudgetRegion.AsTableValuedParameter("ImportRegionType"),
                    userid = userid
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);

            }
            return Task.FromResult(rowAffected);
        }
        public Task<int> ImportBudgetAttribute(DataTable account, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                rowAffected = conn.Execute("ip_import_budget_attribute",
                new
                {
                    Account = account.AsTableValuedParameter("ImportAccountType"),
                    userid = userid
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);

            }

            return Task.FromResult(rowAffected);
        }

        public async Task<IList<ImportBudgetResponse>> ImportBudgetWithAttribute(
          DataTable importBudget
        , DataTable derBudget
        , DataTable account
        , DataTable allocationuser
        , DataTable derivativeuser
        , DataTable region
        , DataTable brand
        , string userid
        )
        {
            //int rowAffected = 0;  
            using IDbConnection conn = Connection;
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            var result = await conn.QueryAsync<ImportBudgetResponse>("ip_import_budget_with_attribute",
            new
            {
                Allocation = importBudget.AsTableValuedParameter("ImportAllocationType"),
                Derivative = derBudget.AsTableValuedParameter("ImportDerivativeType"),
                Account = account.AsTableValuedParameter("ImportAccountType"),
                allocationuser = allocationuser.AsTableValuedParameter("ImportAllocationUserType"),
                derivativeuser = derivativeuser.AsTableValuedParameter("ImportDerivativeUserType"),
                region = region.AsTableValuedParameter("ImportRegionType"),
                brand = brand.AsTableValuedParameter("ImportBrandType"),
                userid = userid
            }
                , commandType: CommandType.StoredProcedure, commandTimeout: 180);

            return result.ToList();
        }

        public Task<int> ImportBudget(DataTable importBudget, DataTable derBudget, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                rowAffected = conn.Execute("ip_import_budget_with_attribute",
                new
                {
                    Allocation = importBudget.AsTableValuedParameter("ImportAllocationType"),
                    Derivative = derBudget.AsTableValuedParameter("ImportDerivativeType"),
                    userid = userid
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);
            }
            return Task.FromResult(rowAffected);
        }

        public async Task<IList<ImportBudgetResponse>> ImportBudgetDC(DataTable importBudget)
        {
            using IDbConnection conn = Connection;
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            var __res = await conn.QueryAsync<ImportBudgetResponse>("ip_import_budget_with_attribute_dc",
            new
            {
                DCAllocation = importBudget.AsTableValuedParameter("DCBudgetUploadType")
            }
                , commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return __res.ToList();
        }

        public async Task<IList<string>> GetGroupBrand()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                SELECT LongDesc FROM tbmst_brand_group 
                                WHERE ISNULL(IsActive, 1) = 1";
                var __res = await conn.QueryAsync<string>(__query);
                return __res.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<string>> GetActiveDistributor()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                    SELECT LongDesc FROM tbmst_distributor
                    WHERE ISNULL(IsActive, 1) = 1
                ";
                var __res = await conn.QueryAsync<string>(__query);
                return __res.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<string>> GetSubActivityTypeDC()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
            SELECT ACT.LongDesc from tbmst_subactivity_type ACT
            WHERE ACT.Id in (
                SELECT SubActivityTypeId from tbmst_subactivity SA
                LEFT JOIN tbmst_category CAT on CAT.id=SA.CategoryId
                WHERE CAT.ShortDesc='DC' AND ISNULL(SA.IsActive, 1) = 1
                GROUP BY SubActivityTypeId
            )
            ";
                var __res = await conn.QueryAsync<string>(__query);
                return __res.ToList();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public Task<int> ImportChannel(DataTable importChannel)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                rowAffected = conn.Execute("ip_insert_channel", new { ChannelType = importChannel.AsTableValuedParameter("tbtype_channel") }, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            }

            return Task.FromResult(rowAffected);
        }


        public Task<int> ImportMasterBrand(
            DataTable brand,
            string userid
            )
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                rowAffected = conn.Execute("ip_import_master_brand", new
                {
                    Brand = brand.AsTableValuedParameter("BrandType"),
                    userid = userid
                }, commandType: CommandType.StoredProcedure, commandTimeout: 180
                    );

            }

            return Task.FromResult(rowAffected);
        }


        public Task<int> ImportMaster(
            DataTable brand,
            DataTable channel,
            DataTable subactivitytype,
            DataTable activity,
            DataTable distributor,
            DataTable principal,
            string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                rowAffected = conn.Execute("ip_import_master", new
                {
                    Brand = brand.AsTableValuedParameter("BrandType"),
                    Channel = channel.AsTableValuedParameter("ChannelType"),
                    Distributor = distributor.AsTableValuedParameter("DistributorType"),
                    Principal = principal.AsTableValuedParameter("PrincipalType"),
                    ActivityType = subactivitytype.AsTableValuedParameter("SubActivityType"),
                    Activity = activity.AsTableValuedParameter("ActivityType"),
                    userid = userid
                }, commandType: CommandType.StoredProcedure, commandTimeout: 180
                    );
            }

            return Task.FromResult(rowAffected);
        }

        public Task<int> ImportBudgetBrand(DataTable brand, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                rowAffected = conn.Execute("ip_import_budget_brand",
                new
                {
                    Brand = brand.AsTableValuedParameter("ImportBrandType"),
                    userid = userid
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);

            }

            return Task.FromResult(rowAffected);
        }

        public Task<object> ImportMatrix(string userid, string useremail, DataTable matrix)
        {
            object __res;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var __result = conn.QueryMultiple("ip_import_matrix",
                new
                {
                    matrix = matrix.AsTableValuedParameter("PromoApproverType"),
                    userid = userid,
                    useremail = useremail
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);
                __res = new
                {
                    processId = __result.ReadSingleOrDefault<string>(),
                    listFailed = __result.Read().ToList(),
                    totalRecords = __result.Read().ToList()
                };
                
            }

            return Task.FromResult(__res);
        }

        //public  Task<List<int>> GetMatrixApprovalProcessId ()
        //{
        //    List<MatrixApprovalPromoProcessFlat>? matrix = null;        
        //    using (IDbConnection conn = Connection)
        //    {
        //        if (conn.State == ConnectionState.Closed)
        //            conn.Open();

        //        matrix =  conn.Query<MatrixApprovalPromoProcessFlat>("ip_get_matrix_approval_process", 
        //            commandType: CommandType.StoredProcedure, commandTimeout: 180).ToList();

        //    }
        //    //MatrixApprovalPromoProcessPromo promoList = 
        //    return Task.FromResult(matrix.Select(x => x.Id).Distinct().ToList());
        //}

        public Task<int> SetMatrixApprovalFinishedByProcess(int id)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                rowAffected = conn.Execute("ip_matrix_approval_process_update",
                    new
                    {
                        processId = id                     
                    },
                    commandType: CommandType.StoredProcedure, commandTimeout: 180);

            }
            //MatrixApprovalPromoProcessPromo promoList = 
            return Task.FromResult(rowAffected);
        }
        public Task<object> GetMatrixApprovalByProcess(int id)
        {
            object matrix = null;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                matrix = conn.Query<object>("ip_get_matrix_approval_process",
                    new
                    {
                        processId = id
                    },
                    commandType: CommandType.StoredProcedure, commandTimeout: 180).ToList();

            }
            
            return Task.FromResult(matrix);
        }

        public Task<object> GetMatrixApprovalPromoByMatrix(int id)
        {
            object matrix = null;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                matrix = conn.Query<object>("ip_refresh_matrix_used_by_promoid",
                    new
                    {
                        matrixid = id
                    },
                    commandType: CommandType.StoredProcedure, commandTimeout: 180).ToList();

            }
            //MatrixApprovalPromoProcessPromo promoList = 
            return Task.FromResult(matrix);
        }
        public Task<int> ImportMasterChannel(DataTable channel, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                rowAffected = conn.Execute("ip_import_master_channel", new
                {
                    Channel = channel.AsTableValuedParameter("ChannelType"),
                    userid = userid
                }, commandType: CommandType.StoredProcedure, commandTimeout: 180
                    );
            }

            return Task.FromResult(rowAffected);
        }

        public Task<int> ImportSubactivityType(DataTable subactivitytype, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                rowAffected = conn.Execute("ip_import_master_subactivitytype", new
                {
                    Activitytype = subactivitytype.AsTableValuedParameter("SubActivityType"),
                    userid = userid
                }, commandType: CommandType.StoredProcedure, commandTimeout: 180
                    );
            }

            return Task.FromResult(rowAffected);
        }

        public Task<int> ImportCategory(DataTable category, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                rowAffected = conn.Execute("ip_import_master_activity", new
                {
                    Activity = category.AsTableValuedParameter("ActivityType"),
                    userid = userid
                }, commandType: CommandType.StoredProcedure, commandTimeout: 180
                    );
            }

            return Task.FromResult(rowAffected);
        }

        public Task<int> ImportDistributor(DataTable distributor, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                rowAffected = conn.Execute("ip_import_master_distributor", new
                {
                    Distributor = distributor.AsTableValuedParameter("DistributorType"),
                    userid = userid
                }, commandType: CommandType.StoredProcedure, commandTimeout: 180
                    );
            }

            return Task.FromResult(rowAffected);
        }

        public Task<int> ImportPrincipal(DataTable principal, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                rowAffected = conn.Execute("ip_import_master_principal", new
                {
                    Principal = principal.AsTableValuedParameter("PrincipalType"),
                    userid = userid
                }, commandType: CommandType.StoredProcedure, commandTimeout: 180
                    );
            }

            return Task.FromResult(rowAffected);
        }

        public Task<int> ImportRegion(DataTable region, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                rowAffected = conn.Execute("ip_import_master_region", new
                {
                    Region = region.AsTableValuedParameter("RegionType"),
                    userid = userid
                }, commandType: CommandType.StoredProcedure, commandTimeout: 180
                    );
            }

            return Task.FromResult(rowAffected);
        }

        public Task<int> ImportMatrixBudget(string userid, DataTable importMatrix)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                rowAffected = conn.Execute("ip_import_matrix_budget",
                new
                {
                    matrix = importMatrix.AsTableValuedParameter("BudgetApproverType"),
                    userid = userid
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);

            }

            return Task.FromResult(rowAffected);
        }

        public Task<int> ImportSellingpoint(DataTable sellingpoint, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                rowAffected = conn.Execute("ip_import_master_sellingpoint",
                new
                {
                    sellingpoint = sellingpoint.AsTableValuedParameter("SellingPointType"),
                    userid = userid
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);

            }

            return Task.FromResult(rowAffected);
        }

        public async Task<IList<ImportBudgetResponse>> ImportBudgetAdjustment(
        DataTable derBudget
        , string userid
        )
        {
            using IDbConnection conn = Connection;
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            var result = await conn.QueryAsync<ImportBudgetResponse>("ip_import_budget_adjustment",
            new
            {
                Derivative = derBudget.AsTableValuedParameter("ImportDerivativeType"),
                userid = userid
            }
                , commandType: CommandType.StoredProcedure, commandTimeout: 180);

            return result.ToList();
        }
        public Task<int> ImportBudgetUser(DataTable importBudgetUser, string userid)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                rowAffected = conn.Execute("ip_import_alloc_user",
                new
                {
                    User = importBudgetUser.AsTableValuedParameter("ImportUserType"),
                    userid = userid
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);

            }
            return Task.FromResult(rowAffected);
        }

        public async Task CreatePromoAttachment(SavePromoAttachmentParam body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"insert into tbtrx_promo_doclink
                    (
                        PromoId,
                        DocLink,
                        FileName,
                        CreateOn,
                        CreateBy
                    )
                        values
                        (
                            @PromoId,
                            @DocLink,
                            @FileName,
                            @CreateOn,
                            @CreateBy
                        )
                        ";
                await conn.ExecuteAsync(__query, new
                {
                    PromoId = body.PromoId,
                    DocLink = body.DocLink,
                    FileName = body.FileName,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    CreateBy = body.CreateBy

                });
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task RemovePromoAttachment(int PromoId, string DocLink)
        {
            try
            {
                using IDbConnection conn = Connection;
                await conn.ExecuteAsync(@"DELETE FROM tbtrx_promo_doclink where PromoId=@PromoId and DocLink=@DocLink"
                , new
                {
                    PromoId = PromoId,
                    DocLink = DocLink

                });
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<SearchPromobyRefidDto> SearchPromoByRefId(string refId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@refid", refId);
                var result = await conn.QueryAsync<SearchPromobyRefidDto>("ip_search_promobyrefid", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<HierarchyResult>> GetBudgetHierarchyforAdjust(
            string period,
            int entityId,
            string budgetName
            )
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@period", period);
                __param.Add("@entityid", entityId);
                __param.Add("@budgetname", budgetName);

                var __res = await conn.QueryAsync<HierarchyResult>("ip_hierarchybudget_foradjust", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<AllocationforAdjustResult>> GetBudgetAllocationforAdjust(string period, int entityId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@periode", period);
                __param.Add("@entity", entityId);

                var result = await conn.QueryAsync<AllocationforAdjustResult>("ip_budgetallocation_list_for_adjustment", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ToolsUploadEntityList>> GetEntityList()
        {

            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<ToolsUploadEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }
        //api/promo/listattach/{periode}/{entity}/{userid}
        public async Task<IList<PromoListAttachment>> GetPromoNoteListAttachment(string periode, int entity, string userid)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@periode", periode);
                __param.Add("@entity", entity);
                __param.Add("@userid", userid);

                var result = await conn.QueryAsync<PromoListAttachment>("ip_promo_list_upload_attach", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DCImportTableTemp>> GetDCTableTemp()
        {
            using IDbConnection conn = Connection;
            var __query = @"SELECT * FROM TempDCBudgetUploadType";
            var __res = await conn.QueryAsync<DCImportTableTemp>(__query);
            return __res.ToList();
        }

        public async Task<object> GetImportPromoUploadAttachment(DataTable data, string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var __result = await conn.QueryAsync<object>("ip_promo_upload_attchment_get",
                new
                {
                    attachment = data.AsTableValuedParameter("PromoUploadAttachmentType"),
                    profileid = profileId

                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);


                return __result;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }
        public async Task<object> ImportPromoUploadAttachment(int promoId, string profileId, string email)
        {
            using IDbConnection conn = Connection;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var __result = await conn.QueryAsync<object>("ip_promo_upload_attchment_bypromoid",
                new
                {
                    PromoId = promoId,
                    profileid = profileId,
                    email = email

                }
                    , commandType: CommandType.StoredProcedure, commandTimeout: 180);


                return __result;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }
    }
}