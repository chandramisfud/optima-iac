using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.Promo;

namespace Repositories.Repos
{
    public class PromoReconSubActivityRepo : IPromoReconSubActivityRepo
    {
        readonly IConfiguration __config;
        public PromoReconSubActivityRepo(IConfiguration config)
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

        public async Task<bool> CreatePromoReconSubActivity(PromoReconSubActivityCreate body)
        {
             DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                var __query = @"SELECT * FROM [dbo].[tbset_map_promorecon_period_subactivity]
                        WHERE SubActivityId={0}";
                __query = String.Format(__query, body.SubActivityId);
                var __res = await conn.QueryAsync(__query);
                if (__res.Any()) //data exist, update it
                {
                    //Added andrie June 20
                    await SaveHistory(body.SubActivityId, "Update");

                    var __queryUpdate = @"UPDATE [dbo].[tbset_map_promorecon_period_subactivity]
                        SET AllowEdit={0}, ModifiedOn='{1}', ModifiedBy='{2}', ModifiedEmail='{3}', IsDeleted = 0
                        WHERE subActivityID = {4}";
                    var __res1 = await conn.ExecuteAsync(
                        String.Format(__queryUpdate, body.AllowEdit ? 1 : 0, TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone).ToString(), body.ModifiedBy, body.ModifiedEmail, body.SubActivityId));
                    return true;
                }
                else
                {
                    var __queryInsert = @"INSERT INTO [dbo].[tbset_map_promorecon_period_subactivity]
                        (SubActivityId, AllowEdit, CreateBy, CreatedEmail, CreateOn)
                        VALUES
                        ({0}, {1}, '{2}', '{3}', '{4}')";
                    __queryInsert = String.Format(__queryInsert, body.SubActivityId, body.AllowEdit ? 1 : 0, body.CreateBy, body.CreatedEmail, TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone).ToString());
                    var __res1 = await conn.ExecuteAsync(__queryInsert);
                    await SaveHistory(body.SubActivityId, "Create");
                    return __res1 > 0;
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoReconSubActivityUpdateReturn> UpdatePromoReconSubActivity(PromoReconSubActivityUpdate body)
        {
             DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            await SaveHistory(body.SubActivityId, "Update");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                        UPDATE [dbo].[tbset_map_promorecon_period_subactivity]
                        SET AllowEdit={0}, ModifiedOn='{1}', ModifiedBy='{2}', ModifiedEmail='{3}'
                        WHERE subActivityID = {4}";
                var __res = await conn.QueryAsync<PromoReconSubActivityUpdateReturn>(
                String.Format(__query, body.AllowEdit ? 1 : 0, TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone).ToString(), body.ModifiedBy, body.ModifiedEmail, body.SubActivityId));
                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        private async Task SaveHistory(int SubActivityId, string action)
        {
            try
            {
                using IDbConnection conn = Connection;
                string __query = "INSERT INTO tbhis_map_promorecon_period_subactivity " +
                "(SubActivityId, AllowEdit, ModifiedOn, ModifiedBy,ModifiedEmail, " +
                "CreateOn, CreateBy, CreatedEmail," +
                "DeleteOn, DeleteBy, DeleteEmail, Action  )" +
                "SELECT SubActivityId, AllowEdit, ModifiedOn, ModifiedBy,ModifiedEmail, " +
                "CreateOn, CreateBy, CreatedEmail," +
                "DeleteOn, DeleteBy, DeleteEmail, '{1}' " +
                " FROM tbset_map_promorecon_period_subactivity " +
                " WHERE SubActivityId={0} ";
                __query = String.Format(__query, SubActivityId, action);
                var __res = await conn.ExecuteAsync(__query);
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<PromoReconSubActivityDeleteReturn> DeletePromoReconSubActivity(PromoReconSubActivityDelete body)
        {
             DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                await SaveHistory(body.SubActivityId, "Delete");
                using IDbConnection conn = Connection;
                var __query = @"
                        UPDATE [dbo].[tbset_map_promorecon_period_subactivity]     
                        SET IsDeleted = 1, DeleteOn = '{1}', DeleteBy ='{2}', DeleteEmail = '{3}' 
                        WHERE subActivityID = {0}
                        ";
                var __res = await conn.QueryAsync<PromoReconSubActivityDeleteReturn>(
                    String.Format(__query, body.SubActivityId, TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone).ToString(), body.DeleteBy, body.DeleteEmail));
                return __res.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoReconSubActivityGetById> GetPromoReconSubActivitybySubActivityId(int Id)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"
                        SELECT 
                        ts.id, 
                        ts.RefId, 
                        tc.id CategoryId, 
                        tc.LongDesc Category, 
                        ts2.id SubCategoryId, 
                        ts2.LongDesc SubCategory, 
                        ta.id ActivityId, 
                        ta.LongDesc Activity, 
                        tst.LongDesc Type, 
                        ts.LongDesc SubActivity, 
                        tmpps.AllowEdit 
                        FROM tbset_map_promorecon_period_subactivity tmpps 
                        INNER JOIN tbmst_subactivity ts ON tmpps.SubActivityId = ts.Id AND isnull(ts.IsDeleted, 0) = 0
                        INNER JOIN tbmst_subactivity_type tst ON ts.SubActivityTypeId = tst.Id AND isnull(tst.IsDeleted, 0) = 0
                        INNER JOIN tbmst_activity ta ON ts.ActivityId = ta.id AND isnull(ta.IsDeleted, 0) = 0
                        INNER JOIN tbmst_subcategory ts2 ON ts.SubCategoryId = ts2.Id AND isnull(ts2.IsDeleted, 0) = 0
                        INNER JOIN tbmst_category tc ON ts2.CategoryId = tc.Id AND isnull(tc.IsDeleted, 0) = 0
                    WHERE ts.id={0}";
                var __res = await conn.QueryAsync<PromoReconSubActivityGetById>(String.Format(__query, Id));
                return __res.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<PromoReconSubActivityLP>> GetPromoReconSubActivityDownload()
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"
                SELECT * FROM vw_map_promorecon_subactivity_lp";
                var __res = await conn.QueryAsync<PromoReconSubActivityLP>(__query);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BaseLP> GetPromoReconSubActivityLandingPage(string keyword, string sortColumn, string sortDirection, int dataDisplayed, int pageNum)
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
                        RefId,
                        Category,
                        SubCategory,
                        Activity,
                        SubActivityType,
                        SubActivity,
                        AllowEdit
                    ) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }
                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_map_promorecon_subactivity_lp
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_map_promorecon_subactivity_lp
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
                res.Data = __res.Read<PromoReconSubActivityLP>().Cast<object>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<IList<SubActivityDropDownMapping>> GetSubActivityDropdown(int ActivityId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subactivity ts 
                                    inner join tbmst_activity tc on ts.ActivityId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @ActivityId";
                var __res = await conn.QueryAsync<SubActivityDropDownMapping>(__query, new { ActivityId = ActivityId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<List<PromoImportResponse>> ImportPromoReconSubactivity(DataTable data, string userid, string useremail)
        {
            try
            {
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var result = await conn.QueryAsync<PromoImportResponse>("ip_import_map_promorecon_period_subactivity",
                     new
                     {
                         SubActivity = data.AsTableValuedParameter("PromoReconPeriodSubActivityType"),
                         userid = userid,
                         useremail = useremail
                     },
                commandTimeout: 180, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SubCategoryforSubActivity>> SubCategoryforSubActivity(int CategoryId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subcategory ts 
                                    inner join tbmst_category tc on ts.CategoryId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @CategoryId";
                var __res = await conn.QueryAsync<SubCategoryforSubActivity>(__query, new { CategoryId = CategoryId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<CategoryforSubActivity>> CategoryforSubActivityPromoRecon()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<CategoryforSubActivity>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ActivityTypeforSubActivity>> ActivityTypeforSubActivityPromoRecon()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_subactivity_type WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<ActivityTypeforSubActivity>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ActivityforSubActivity>> ActivityforSubActivityPromoRecon(int SubCategoryId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_activity ts 
                                    inner join tbmst_subcategory tc on ts.SubCategoryId = tc.Id and isnull(tc.IsDeleted, 0) = 0
                                    where isnull(ts.IsDeleted, 0) = 0 and  tc.id = @SubCategoryId";
                var __res = await conn.QueryAsync<ActivityforSubActivity>(__query, new { SubCategoryId = SubCategoryId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SubActivityTemplate>> GetSubActivityTemplate()
        {
            try
            {
                using IDbConnection conn = Connection;
                {
                    var __query = @"SELECT A.Id, A.CategoryId, A.SubCategoryId, A.ActivityId, A.SubActivityTypeId, A.LongDesc, A.ShortDesc, A.CreateBy, A.RefId, B.LongDesc AS ActivityLongDesc, c.LongDesc AS SubActivityTypeLongDesc, cat.LongDesc AS Category, 
                                sc.LongDesc AS SubCategory
                                FROM tbmst_subactivity A LEFT OUTER JOIN
                                tbmst_category cat ON A.CategoryId = cat.Id LEFT OUTER JOIN
                                tbmst_subcategory sc ON A.SubCategoryId = sc.Id LEFT OUTER JOIN
                                tbmst_activity B ON A.ActivityId = B.Id LEFT OUTER JOIN
                                tbmst_subactivity_type c ON A.SubActivityTypeId = c.Id
                                WHERE (ISNULL(A.IsDeleted, 0) = 0)";
                    var __res = await conn.QueryAsync<SubActivityTemplate>(__query);
                    return __res.ToList();
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}
