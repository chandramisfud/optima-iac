
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class UserRepository : IUserRepository
    {
        readonly IConfiguration __config;
        public UserRepository(IConfiguration config)
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

        public async Task<UserLP> GetUserWithPaging(string keyword, string fltStatus, string sortColumn, string sortDirection="ASC", 
            int dataDisplayed=10, int pageNum = 0)
        {
            UserLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = " CONCAT_WS(' ', email, userName, contactInfo) LIKE '%" + keyword + "%' AND ";
                }

                // status filtered
                if (fltStatus != "ALL")
                {
                    userFilter += String.Format("status = '{0}' ", fltStatus);
                }
                else
                {
                    userFilter += "1=1 ";
                }

                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_user_profile_lp 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_user_profile_lp
                        WHERE {0}
                        ORDER BY {1} {2}                     
                        ";

                // if set -1 domt use paging
                if (dataDisplayed >= 0) {
                    __query += String.Format(paging, pageNum, dataDisplayed);
                }

                __query = string.Format(__query, userFilter, sortColumn, sortDirection);

                using IDbConnection conn = Connection;
                var __res = await conn.QueryMultipleAsync(__query);
                res = __res.ReadSingle<UserLP>();
                res.Data = __res.Read<UserLPData>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

            return res;
        }

        public async Task<UserByID> GetUserById(string userid)
        {
            UserByID res = null!;
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    SELECT * FROM vw_user_profile_lp 
                                    WHERE id = {0}
                                    
                                    select id, profileid from tbset_user_login_profile where id={0}
                    ";
                __query = String.Format(__query, userid);
                var __res = await conn.QueryMultipleAsync(__query);
                res = __res.ReadSingle<UserByID>();
                res.profiles = __res.Read<UserByIDProfile>().ToList();

            }
            catch (Exception __ex)
            {
                Console.WriteLine(__ex.Message);
            }

            return res;
        }

        public async Task<IList<UserProfileList>> GetListUserProfile()
        {
            using IDbConnection conn = Connection;
            try
            {
                var sql = @"SELECT id FROM vw_userprofile_lp where status='Active'";
                var __res = await conn.QueryAsync<UserProfileList>(sql);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task UserProfileInsert(UserProfileInsert body)
        {
            using IDbConnection conn = Connection;

            DataTable __pr = new("UserType");
            __pr.Columns.Add("UserType");
            foreach (string v in body.profile!)
                __pr.Rows.Add(v);

            var passwordHash = Utils.HashString.GetSha256FromString(body.password!);

            var __param = new DynamicParameters();
            if (body.id > 0)
            {
                __param.Add("@isnew", 2);
            }
            else // new rec
            {
                __param.Add("@isnew", 1);
            }
            __param.Add("@id", body.id);
            __param.Add("@username", body.username);
            __param.Add("@password", passwordHash);
            __param.Add("@email", body.email);
            __param.Add("@contactinfo", body.contactinfo);
            __param.Add("@userid", body.userid);
            __param.Add("@profile", __pr.AsTableValuedParameter());
            var __res = await conn.QueryAsync<Entities.Models.User>("ip_user_management_ins", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <param name="action">3=Delet; 4=activate</param>
        /// <returns></returns>
        public async Task<bool> UserProfileDeleteStatus(string id, string userid, int action)
        {
            using IDbConnection conn = Connection;

            DataTable __pr = new("UserType");
            __pr.Columns.Add("UserType");

            var __param = new DynamicParameters();
            __param.Add("@isnew", action);
            __param.Add("@id", id);
            __param.Add("@userid", userid);
            __param.Add("@username", "");
            __param.Add("@password", "");
            __param.Add("@email", "");
            __param.Add("@contactinfo", "");
            __param.Add("@profile", __pr.AsTableValuedParameter());
            var __res = await conn.QueryAsync<Entities.Models.User>("ip_user_management_ins", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return true;
        }
        public async Task<UserProfileResult> GetUserLoginProfilebyId(string id)
        {
            List<UserProfileResult> __userprofile = new();
            using IDbConnection conn = Connection;
            var __param = new DynamicParameters();
            __param.Add("@id", id);
            using (var __re = await conn.QueryMultipleAsync("ip_user_management_getbyid", __param, commandType: CommandType.StoredProcedure, commandTimeout:180))
            {
                UserProfileResult __result = new()
                {
                    userprofileuserresult = __re.Read<UserProfileResultHeader>().FirstOrDefault(),

                    profileresult = new List<Profile>()
                };
                foreach (Profile r in __re.Read<Profile>())
                    __result.profileresult.Add(new Profile()
                    {
                        id = r.id,
                        profileid = r.profileid
                    });
                __userprofile.Add(__result);
            }
            return __userprofile.FirstOrDefault()!;
        }
    }
}