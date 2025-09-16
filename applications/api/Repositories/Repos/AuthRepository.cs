using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using static Repositories.Utils;

namespace Repositories.Repos
{
    public class AuthMenuRepository : IAuthMenuRepository
    {

        readonly IConfiguration __config;
        public AuthMenuRepository(IConfiguration config)
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

        public async Task<object> GetByGroup(string UserGroup, string UserLevel)
        {
            MainMenu mainMenu = new();
            try
            {
                //SubMenu usrGroup = new SubMenu();
                using IDbConnection conn = Connection;
                conn.Open();
                var __query = @"SELECT * FROM tbset_menu_v4
                    INNER JOIN tbset_userrights AS A ON A.menuid = tbset_menu_v4.id
                    INNER JOIN tbset_userlevel AS B ON A.usergroupid = B.usergroupid
                    WHERE A.usergroupid = '{0}' 
                    AND B.userlevel = {1} ORDER BY tbset_menu_v4.number, tbset_menu_v4.id, tbset_menu_v4.parent";
                __query = String.Format(__query, UserGroup, UserLevel);
                var __res = await conn.QueryAsync<SubMenu>(__query);

                mainMenu.menu = new Menu
                {
                    //get by parent
                    menu = GetSubMenuByParent(__res.ToList(), "0")
                };
                ///get useracces
                IEnumerable<UserAccess> usrAccess = new List<UserAccess>();
                __query = @"select a.usergroupid, a.menuid, ISNULL(b.read_rec,1) as read_rec, ISNULL(b.create_rec,0) as create_rec, ISNULL(b.update_rec,0) as update_rec, ISNULL(b.delete_rec,0) as delete_rec   
                            from tbset_userrights a
                            left join tbset_userlevel_access b on a.usergroupid = b.usergroupid and a.menuid = b.menuid and b.userlevel = " + UserLevel +
                            "where a.usergroupid = '" + UserGroup + "'";
                usrAccess = await conn.QueryAsync<UserAccess>(__query);
                mainMenu.user_access = usrAccess.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return mainMenu;
        }

        private static List<SubMenu> GetSubMenuByParent(List<SubMenu> lsMenu, string parentid)
        {
            List<SubMenu> result = new();
            //get the parent first
            result = lsMenu.Where(x => x.parent == parentid).ToList();
            if (result != null && result.Count > 0)
            {
                foreach (var mn in result)
                {
                    mn.submenu = GetSubMenuByParent(lsMenu, mn.id!);
                }
            }
            else result = null!;
            return result;
        }
    }
    public class AuthUserRepository : IAuthUserRepository
    {
        readonly IConfiguration __config;
        public AuthUserRepository(IConfiguration config)
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

        public async Task<User> DoLogin(string id, string password)
        {
            User result = null!;
            var passwordHash = Utils.HashString.GetSha256FromString(password);
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);
                    __param.Add("@password", passwordHash);
                    var res = await conn.QueryAsync<User>("ip_user_login_profile", __param,
                        commandType: CommandType.StoredProcedure, commandTimeout:180);
                    result = res.FirstOrDefault()!;
                }
                return result;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public int GetTokenAge()
        {
            int result = 0;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                string query = "SELECT * FROM appsetting WHERE setname='tokenage'";
                var res = conn.Query<AppSetting>(query);
                result = Convert.ToInt16(res.First().SetValue);
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return result;

        }

        public async Task<List<UserProfileCategory>> GetProfile(string id)
        {
            List<UserProfile>? result = null;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var __param = new DynamicParameters();
                __param.Add("@id", id);
                var res = await conn.QueryAsync<UserProfile>("ip_user_get_profile", __param,
                    commandType: CommandType.StoredProcedure, commandTimeout:180);
                result = res.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

            List<UserProfileCategory> userProfileCategories = result
            .GroupBy(up => up.profileid)
        .Select(group => new UserProfileCategory
        {
            profileid = group.Key,
            usergroupid = group.First().usergroupid,
            usergroupname = group.First().usergroupname,
            userlevel = group.First().userlevel,
            groupmenupermission = group.First().groupmenupermission,
            ProfileCategories = group
                .Select(up => new ProfileCategory
                {
                    categoryId = up.categoryid,
                    categoryShortDesc = up.categoryshortdesc!,
                    categoryLongDesc = up.categorylongdesc!
                })
                .ToList()
        })
        .ToList();
            return userProfileCategories;

        }


        public async Task<bool> ChangeOldPassword(UserOldPasswordChange userPasswordChangeDto, string usrEmail)
        {
            bool __res = false;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                // modified by: andrie June 6 2023
                UserPasswordChange usr = new()
                {
                    email = usrEmail,
                    password = userPasswordChangeDto.password
                };
                __res = await ChangePassword(usr);
                //}
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __res;
        }
        public async Task<bool> ChangePassword(UserPasswordChange userPasswordChangeDto)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            bool __res = false;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var sql = @"UPDATE [dbo].[tbset_user_login]
                        SET
                        password=@password
                        ,password_change=@passwordChange
                        ,usernew=0
                        WHERE
                        email=@email";

                var res = await conn.ExecuteAsync(sql
                ,
                    new
                    {
                        userPasswordChangeDto.email
                        ,
                        password = HashString.GetSha256FromString(userPasswordChangeDto.password!)
                        ,
                        passwordChange = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)

                    });
                if (res > 0)
                {
                    __res = true;
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __res;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User __res = null!;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var sql = @"SELECT * FROM [dbo].[tbset_user_login]
                        WHERE
                        email='{0}'";
                var res = await conn.QueryAsync<User>(String.Format(sql, email));
                if (res.Any())
                {
                    __res = res.First();
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __res;
        }

        public async Task<User> GetUserById(int userId)
        {
            User __res = null!;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var sql = @"SELECT * FROM [dbo].[tbset_user_login]
                        WHERE
                        id='{0}'";
                var res = await conn.QueryAsync<User>(String.Format(sql, userId));
                if (res.Any())
                {
                    __res = res.First();
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __res;
        }
        public async Task<bool> UpdateProfilePicture(string email, string filecode)
        {
            bool __res = false;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var sql = @"UPDATE [dbo].[tbset_user_login]
                        SET pictureprofilefile='{0}'
                        WHERE
                        email='{1}'";
                var res = await conn.ExecuteAsync(String.Format(sql, filecode, email));
                if (res > 0)
                    __res = true;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __res;
        }

        public async Task<bool> ResetIsLogin(string email)
        {
            bool __res = false;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var sql = @"UPDATE [dbo].[tbset_user_login]
                        SET islogin=0
                        WHERE
                        email='{0}'";
                var res = await conn.ExecuteAsync(String.Format(sql, email));
                if (res == 1)
                    __res = true;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __res;
        }
        public async Task<int> UpdateIsLogin(string email)
        {
            int __res = 0;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var __query = new DynamicParameters();
                __query.Add("@email", email);
                var res = await conn.QueryAsync<User>("ip_user_login_profile_upd_lastlogin", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                if (res.Any())
                {
                    __res = res.First().isLogin;
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return __res;
        }

        public async Task<UserbyEmail> GetByEmail(string email)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"select *, DATEDIFF(DAY, lastlogin, @getWIBDate) as expLastLogin
                                from tbset_user_login where email = @email";
                var __res = await conn.QueryAsync<UserbyEmail>(__query, new
                {
                    Email = email,
                    getWIBDate = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)
                });
                return __res.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task DeleteUserEmail(string? email)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"UPDATE tbset_user_login
                                SET
                                isdeleted = 1
                                ,deletedby = 'system'
                                ,deletedon = @deletedOn
                                WHERE email = @email";
                await conn.ExecuteAsync(__query, new
                {
                    Email = email,
                    deletedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)
                });
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}