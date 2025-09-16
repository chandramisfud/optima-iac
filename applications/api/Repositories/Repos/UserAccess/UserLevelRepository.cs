
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class UserLevelRepository : IUserLevelRepository
    {
        readonly IConfiguration __config;
        public UserLevelRepository(IConfiguration config)
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

        public async Task<UserLevelLP> GetUserLevelWithPaging(string keyword, string userGroupId, string sortColumn, string sortDirection = "ASC",
            int dataDisplayed = 10, int pageNum = 0)
        {
            UserLevelLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = " CONCAT_WS(' ',  usergroupname, levelname, userlevel) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }

                if (!String.IsNullOrEmpty(userGroupId))
                {
                    userFilter += string.Format(" AND  userGroupId='{0}' ", userGroupId);
                }

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_userlevel_lp 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_userlevel_lp
                        WHERE {0}
                        ORDER BY {1} {2}                     
                        ";

                // if set -1 domt use paging
                if (dataDisplayed >= 0)
                {
                    __query += String.Format(paging, pageNum, dataDisplayed);
                }


                __query = string.Format(__query, userFilter, sortColumn, sortDirection);

                using IDbConnection conn = Connection;
                var __res = await conn.QueryMultipleAsync(__query);
                res = __res.ReadSingle<UserLevelLP>();
                res.Data = __res.Read<UserLevel>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

            return res;
        }

        public async Task<bool> DeleteUserLevel(int id)
        {
            bool result = false;
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                var sql = "DELETE FROM tbset_userlevel WHERE userlevel = {0}";
                sql = String.Format(sql, id);
                var __res = await conn.ExecuteAsync(sql);
                result = __res > 0;
            }
            return result;
        }
        public async Task<UserLevel> GetUserLevelById(int id)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    SELECT * FROM vw_userlevel_lp 
                                    WHERE userlevel = {0}";
                __query = String.Format(__query, id);
                var __res = await conn.QueryAsync<UserLevel>(__query);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        /// <summary>
        /// Create and update user level
        /// </summary>
        /// <param name="userLevel"></param>
        /// <returns></returns>
        public async Task<int> CreateUserLevel(userLevelCreate userLevel, bool isUpdate)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            int res = 0;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                string sql = String.Empty;
                if (!isUpdate)
                {
                    sql = @"INSERT INTO [dbo].[tbset_userlevel]
                    ([userlevel]
                    ,[levelname]
                    ,[usergroupid]
                    ,[userinput]
                    ,[dateinput]
                    ,[CreatedEmail]
        
                    )                
                VALUES
                    (@userlevel
                    ,@levelname
                    ,@usergroupid
                    ,@byUserName
                    ,@dateinput
                    ,@byUserEmail
                    )";
                }
                else
                {
                    sql = @"UPDATE [dbo].[tbset_userlevel]
                        SET
                        levelname=@levelname
                        ,usergroupid=@usergroupid
                        ,useredit=@byUserName
                        ,dateedit=@dateedit
                        ,ModifiedEmail=@byUserEmail
                        WHERE 
                        userlevel=@userlevel";
                }
                res = await conn.ExecuteAsync(sql, new
                {
                    userLevel = userLevel.userlevel,
                    levelName = userLevel.levelname,
                    userGroupId = userLevel.usergroupid,
                    byUserName = userLevel.byUserName,
                    dateInput = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    byUserEmail = userLevel.byUserEmail,
                    dateEdit = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)
                });
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

            return res;
        }

        public async Task<List<UserRightsDto2>> GetGroupId(string usergroupid)
        {
            try
            {
                using IDbConnection dbConnection = Connection;
                var sql = "select a.id, a.id as name, a.name as text, a.icon, coalesce(a.parent,0) as parent_id, " +
                "coalesce(b.usergroupid,'-') as akses " +
                " from tbset_menu a left join " +
                "(select usergroupid, menuid from tbset_userrights " +
                " where usergroupid=@usergroupid) as b on a.id=b.menuid order by a.number, a.id, a.parent ";
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<UserRightsDto2>(sql, new { UserGroupId = usergroupid });
                return result.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}