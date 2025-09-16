
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class UserGroupMenuRepository : IUserGroupMenuRepository
    {
        readonly IConfiguration __config;
        public UserGroupMenuRepository(IConfiguration config)
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

        public void InsertUserGroupHistory(userGroupHisParam param)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"                       
                        INSERT INTO [dbo].[tbhis_usergroup]
                        ([usergroupid]
                        ,[usergroupname]
                        ,[groupmenupermission]
                        ,[StatusAction]
                        ,[ActionOn]            
                        ,[ActionBy]
                        ,[CreatedEmail]
                        )    
                        SELECT usergroupid, usergroupname, groupmenupermission, '{0}',
                        '{4}', '{1}', '{2}'
                        FROM [dbo].[tbset_usergroup]
                        WHERE usergroupid = '{3}'                     
                        ";
                __query = String.Format(__query, param.statusAction, param.userID, param.userEmail, 
                        param.usergroupid, TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone));
                var __res = conn.Execute(__query);
            }
            catch (SqlException __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<UserGroupMenuCreateReturn> CreateUserGroupMenu(UserGroupMenuCreate body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                        DECLARE @message varchar(255);
                        DECLARE @return varchar(255);
                        -- Cek Jika data sudah ada
                            IF NOT EXISTS
                            (
                        SELECT usergroupid FROM tbset_usergroup 
                        WHERE usergroupname = @usergroupname
                        AND groupmenupermission = @groupmenupermission
                    --    AND isnull(IsDeleted, 0) = 0
                            )
                        BEGIN
                        INSERT INTO [dbo].[tbset_usergroup]
                        ([usergroupid]
                        ,[usergroupname]
                        ,[groupmenupermission]
                        ,[dateinput]
                        ,[userinput]
                        ,[CreatedEmail]
                        )    
                        VALUES
                        (
                        @usergroupid
                       ,@usergroupname
                       ,@groupmenupermission
                       ,@dateinput
                       ,@userinput
                       ,@CreatedEmail
                        )
                        SELECT usergroupid, usergroupname, groupmenupermission, dateinput, userinput, CreatedEmail 
                        FROM tbset_usergroup WHERE usergroupid=@usergroupid
                        END
                        ELSE
                        BEGIN
                                --	message error jika data sudah ada
                        SET @return = (SELECT usergroupid FROM tbset_usergroup WHERE usergroupname=@usergroupname)
                        SET @message= 'User Group Menu with usergroupid = ' + @return + ' is already exist'
                        RAISERROR (@message, -- Message text.
                        16, -- Severity.
                        1 -- State.
                        );
                        END";
                var __res = await conn.QueryAsync<UserGroupMenuCreateReturn>(__query, new
                {
                    usergroupid = body.usergroupid,
                    usergroupname = body.usergroupname,
                    groupmenupermission = body.groupmenupermission,
                    dateinput = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    userinput = body.userinput,
                    CreatedEmail = body.CreatedEmail
                });
                // insert history
                userGroupHisParam param = new()
                {
                    usergroupid = __res.First().usergroupid!,
                    userEmail = body.CreatedEmail!,
                    userID = body.userinput!,
                    statusAction = "Insert",
                };
                InsertUserGroupHistory(param);

                return __res.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }


        public async Task<bool> DeleteUserGroupMenu(string usergroupid, string DeletedEmail, string DeletedBy)
        {
            using IDbConnection conn = Connection;
            try
            {
                // set history
                userGroupHisParam param = new()
                {
                    usergroupid = usergroupid,
                    userEmail = DeletedEmail,
                    userID = DeletedBy,
                    statusAction = "Delete"
                };
                InsertUserGroupHistory(param);

                var __query = @"
                        DELETE tbset_usergroup WHERE usergroupid='{0}'                        
                        ";
                __query = string.Format(__query, usergroupid);
                var __res = await conn.ExecuteAsync(__query);
                return (__res > 0);
            }
            catch (Exception __ex)
            {
                return false;
                throw new Exception(__ex.Message);

            }
        }
        public async Task<UserGroupMenuModel> GetUserGroupMenuById(string usergroupid)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"
                                    SELECT * FROM tbset_usergroup 
                                    WHERE usergroupid = '{0}'";
                __query = String.Format(__query, usergroupid);
                var __res = await conn.QueryAsync<UserGroupMenuModel>(__query);
                return __res.FirstOrDefault()!;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<UserGroupMenuLandingPage> GetUserGroupMenuLandingPage(string keyword, string sortColumn, string sortDirection = "ASC",
            int dataDisplayed = 10, int pageNum = 0)
        {
            UserGroupMenuLandingPage res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = " CONCAT_WS(' ', UserGroupId,  UserGroupName, GroupMenuPermission, Category, Name, Description) LIKE '%" + keyword + "%' ";
                }
                else
                {
                    userFilter = " 1=1 ";
                }



                // pageNum= -1, show all data

                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_usergroup_lp 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_usergroup_lp
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
                res = __res.ReadSingle<UserGroupMenuLandingPage>();
                res.Data = __res.Read<UserGroupMenuSelect>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<UserGroupMenuUpdateReturn> UpdateUserGroupMenu(UserGroupMenuUpdate body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            using IDbConnection conn = Connection;
            try
            {
                userGroupHisParam param = new()
                {
                    usergroupid = body.usergroupid!,
                    userEmail = body.ModifiedEmail!,
                    userID = body.useredit!,
                    statusAction = "Modify"
                };
                InsertUserGroupHistory(param);
                var __query = @"
                    DECLARE @messageout varchar(255)
                                -- Cek Jika data sudah ada
                                IF NOT EXISTS(
                                    SELECT usergroupid FROM tbset_usergroup 
                                        WHERE usergroupname = @usergroupname
                                      --  AND isnull(IsDeleted, 0) = 0
                                        and usergroupid<>@usergroupid
                                    )
                                BEGIN
                
                    UPDATE [dbo].[tbset_usergroup]
                    SET
                    [usergroupname] = @usergroupname
                    ,[groupmenupermission] = @groupmenupermission
                    ,[dateedit] = @dateEdit
                    ,[useredit] = @useredit
                    ,[ModifiedEmail] = @ModifiedEmail

                    WHERE 
                    usergroupid=@usergroupid
                    SELECT usergroupid, usergroupname, groupmenupermission, dateedit, useredit, ModifiedEmail FROM tbset_usergroup WHERE usergroupid=@usergroupid
                                END
                                ELSE
                                BEGIN
                                --	message error jika data sudah ada
                                    SET @messageout='UserGroup Menu already exist'
                                        RAISERROR (@messageout, -- Message text.
                                            16, -- Severity.
                                            1 -- State.
                                            );

                                END
                    ";
                var __res = await conn.QueryAsync<UserGroupMenuUpdateReturn>(__query, new
                {
                    userGroupName = body.usergroupname,
                    groupMenuPermission = body.groupmenupermission,
                    dateEdit = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    useredit = body.useredit,
                    usergroupid = body.usergroupid,
                    ModifiedEmail = body.ModifiedEmail
                });
                return __res.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<List<UserAccessGroupRightMenu>> GetMenuAccesByGroupId(string usergroupid)
        {
            List<UserAccessGroupRightMenu> mainMenu = new();
            try
            {
                //SubMenu usrGroup = new SubMenu();
                using IDbConnection conn = Connection;
                conn.Open();
                var __query = @"select a.id, a.id as name, a.name as text, a.icon, coalesce(a.parent,0) as parent_id, " +
                "coalesce(b.usergroupid,'-') as akses " +
                " from tbset_menu_v4 a left join " +
                "(select usergroupid, menuid from tbset_userrights " +
                " where usergroupid='{0}') as b on a.id=b.menuid order by a.number, a.id, a.parent";

                __query = String.Format(__query, usergroupid);
                var __res = await conn.QueryAsync<UserAccessGroupRightMenuChild>(__query);

                //get by parent
                foreach (var parent in __res.Where(x => x.parent_id == "0").ToList())
                {
                    UserAccessGroupRightMenu mainParent = new()
                    {
                        icon = parent.icon,
                        id = parent.id,
                        name = parent.name,
                        parent_id = parent.parent_id,
                        text = parent.text,
                        children = GetSubMenuByParent(__res.ToList(), parent.id!).ToArray()
                    };
                    mainMenu.Add(mainParent);
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return mainMenu;
        }

        private static List<object> GetSubMenuByParent(List<UserAccessGroupRightMenuChild> lsMenu, string parentid)
        {
            List<object> __result = new();
            //get the parent first
            List<UserAccessGroupRightMenuChild> result = lsMenu.Where(x => x.parent_id == parentid && x.id != parentid).ToList();
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    var lsChild = GetSubMenuByParent(lsMenu, item.id!);
                    if (lsChild.Count > 0)
                    {
                        var res = new
                        {
                            id = item.id,
                            name = item.name,
                            parent_id = item.parent_id,
                            text = item.text,
                            icon = item.icon,
                            akses = item.akses,
                            children = lsChild
                        };
                        __result.Add(res);
                    }
                    else
                    {
                        var res = new
                        {
                            id = item.id,
                            name = item.name,
                            parent_id = item.parent_id,
                            text = item.text,
                            icon = item.icon,
                            akses = item.akses,
                            children = lsChild,
                            state = new UserAccessGroupRightMenuState
                            {
                                selected = item.akses != "-"
                            }
                        };
                        __result.Add(res);
                    }
                }
            }
            return __result;
        }

        public async Task<bool> InsertUserRights(string byUser, UserRightPostDto userRightPostDto)
        {
            bool res = true;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                using var transaction = conn.BeginTransaction();
                var sqlDelete = @"DELETE FROM tbset_userrights where usergroupid = @usergroupid";
                await conn.ExecuteAsync(sqlDelete, userRightPostDto, transaction);
                foreach (var item in userRightPostDto.userRightArrays!)
                {
                    var sqlInsert = @"INSERT INTO tbset_userrights (usergroupid,menuid, ModifiedEmail) 
                                                VALUES (@usergroupid, @menuid, @email)";
                    await conn.ExecuteAsync(sqlInsert,
                        new { usergroupid = item.usergroupid, menuid = item.menuid, email = byUser }, transaction);
                }
                transaction.Commit();
            }
            catch (Exception __ex)
            {
                res = false;
                throw new Exception(__ex.Message);

            }
            return res;
        }

        public async Task<IList<UserGroupListUserLevel>> GetUserLevelByUserGroupId(string usergroupid)
        {
            using IDbConnection conn = Connection;
            try
            {
                var sql = @"SELECT userlevel, levelname
                            FROM tbset_userlevel
                            WHERE usergroupid = @UserGroupId";
                var __res = await conn.QueryAsync<UserGroupListUserLevel>(sql, new { UserGroupId = usergroupid });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<List<UserAccessGroupRightMenuByUserLevel>> GetMenuByLevelId(string userlevelid)
        {
            List<UserAccessGroupRightMenuByUserLevel> mainMenu = new();
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var __query = @"select a.id, a.id as name, a.name as text, a.icon, coalesce(a.parent,0) as parent_id" +
                " from tbset_menu_v4 a inner join " +
                " (select x.userlevel, z.usergroupid, z.menuid from tbset_userlevel x " +
                " inner join tbset_usergroup y on x.usergroupid=y.usergroupid " +
                " inner join tbset_userrights z on y.usergroupid=z.usergroupid " +
                " where x.userlevel='{0}') as b on a.id=b.menuid order by a.number, a.id, a.parent";

                __query = String.Format(__query, userlevelid);
                var __res = await conn.QueryAsync<UserAccessGroupRightMenuChildByUserLevel>(__query);

                //get by parent
                foreach (var parent in __res.Where(x => x.parent_id == "0").ToList())
                {
                    UserAccessGroupRightMenuByUserLevel mainParent = new()
                    {
                        icon = parent.icon,
                        id = parent.id,
                        name = parent.name,
                        parent_id = parent.parent_id,
                        text = parent.text,
                        children = GetSubMenuUserLevelByParent(__res.ToList(), parent.id!).ToArray()
                    };
                    mainMenu.Add(mainParent);
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return mainMenu;
        }

        private static List<UserAccessGroupRightMenuChildByUserLevel> GetSubMenuUserLevelByParent(List<UserAccessGroupRightMenuChildByUserLevel> lsMenu, string parentid)
        {
            try
            {
                List<UserAccessGroupRightMenuChildByUserLevel> result = new();
                //get the parent first
                result = lsMenu.Where(x => x.parent_id == parentid && x.id != parentid).ToList();
                //Console.WriteLine("Get " + parentid + " Child");
                if (result != null && result.Count > 0)
                {
                    foreach (var mn in result)
                    {
                        mn.children = GetSubMenuUserLevelByParent(lsMenu, mn.id!).ToArray();

                    }
                }

                return result!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<CRUDMenuDto> GetLevelMenuById(string usergroupid, int userlevel, string menuid)
        {
            try
            {
                using IDbConnection dbConnection = Connection;
                var sql = @"SELECT a.flag, a.crud, a.approve, isnull(b.create_rec, 0) 
                as create_rec, isnull(b.read_rec, 0) as read_rec, isnull(b.update_rec, 0) 
                as update_rec, isnull(b.delete_rec, 0) as delete_rec 
                FROM tbset_menu_v4 a 
                left join (select menuid, create_rec, read_rec, update_rec, delete_rec 
                FROM tbset_userlevel_access 
                WHERE usergroupid=@usergroupid 
                and userlevel=@userlevel) as b on a.id=b.menuid where a.id=@menuid";
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<CRUDMenuDto>(sql, new { UserGroupId = usergroupid, UserLevel = userlevel, MenuId = menuid });
                return result.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task SaveLevelMenu(UserLevelAccess userLevelAccess)
        {
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                using var transaction = conn.BeginTransaction();
                var sqlDelete = @"DELETE FROM tbset_userlevel_access where userlevel=@userlevel and usergroupid=@usergroupid and menuid=@menuid";
                await conn.ExecuteAsync(sqlDelete, userLevelAccess, transaction);
                var sqlInsert = @"INSERT INTO tbset_userlevel_access(userlevel,usergroupid,menuid,create_rec,read_rec,update_rec,delete_rec) VALUES (@userlevel, @usergroupid, @menuid, @create_rec, @read_rec, @update_rec, @delete_rec)";
                await conn.ExecuteAsync(sqlInsert,
                    new
                    {
                        userLevel = userLevelAccess.userlevel,
                        UserGroupId = userLevelAccess.usergroupid,
                        MenuId = userLevelAccess.menuid,
                        Create_rec = userLevelAccess.create_rec,
                        Read_rec = userLevelAccess.read_rec,
                        Update_rec = userLevelAccess.update_rec,
                        Delete_rec = userLevelAccess.delete_rec
                    },
                        transaction);

                transaction.Commit();

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}