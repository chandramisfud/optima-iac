using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using User = Repositories.Entities.Dtos.User;

namespace Repositories.Repos
{
    public class UserProfileRepository : IUserProfileRepository
    {
        readonly IConfiguration __config;
        public UserProfileRepository(IConfiguration config)
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

        public async Task<UserProfileLP> GetUserProfileWithPaging(string keyword, string usergroupid, int userlevel, string fltStatus, string sortColumn, string sortDirection = "ASC",
             int dataDisplayed = 10, int pageNum = 0)
        {
            UserProfileLP res = null!;
            try
            {
                //int offset = (currentPage-1) * dataDisplayed;
                string userFilter = "";
                if (!String.IsNullOrEmpty(keyword))
                {
                    userFilter = " CONCAT_WS(' ', id, username, email, department, jobtitle, usergroupname, levelname, profileCategory) LIKE '%" + keyword + "%' AND ";
                }
                else
                {
                    userFilter = " 1=1 AND ";
                }

                // status filtered
                if (fltStatus != "ALL")
                {
                    userFilter += String.Format(" status = '{0}' ", fltStatus);
                }
                else
                {
                    userFilter += " 1=1 ";
                }

                if (usergroupid != "all" && usergroupid != null)
                {
                    userFilter += String.Format(" AND usergroupid='{0}'", usergroupid);
                }

                if (userlevel != 0)
                {
                    userFilter += String.Format(" AND  userlevel={0}", userlevel);

                }
                var __query = @"SELECT COUNT(*) as TotalCount    
                        , SUM(case when {0} then 1 else 0 end) as FilteredCount
                        FROM vw_userprofile_lp 
                        ";

                var paging = @"
                        OFFSET {0}  * {1} ROWS
                        FETCH NEXT {1} ROWS ONLY";

                __query += @"
                        SELECT * 
                        FROM vw_userprofile_lp
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
                res = __res.ReadSingle<UserProfileLP>();
                res.Data = __res.Read<UserProfileData>().ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
        public Task<UserProfileDataById> GetById(string id)
        {
            using IDbConnection conn = Connection;
            var userDictionary = new Dictionary<string, UserProfileDataById>();
            string sql = @"SELECT A.[id]
                            ,A.[username]
                            ,A.[email]
                            ,A.[password]
                            ,A.[usergroupid]
                            ,A.[userlevel]
                            ,A.[isLogin]
                            ,A.[lastLogin]
                            ,A.[department]
                            ,A.[jobtitle]
                            ,A.[contactinfo]
                            ,B.[DistributorId]
                            ,C.categoryId
                            ,C.categoryShortDesc
                            ,C.categoryLongDesc
		                    ,UC.channelId
							,CH.ShortDesc channelShortDesc
							,CH.LongDesc channelLongDesc
                            ,A.[registered]
                            ,A.[code]
                            ,A.[password_change]
                            ,A.[token]
                            ,A.[token_date]
                            ,A.[userinput]
                            ,A.[dateinput]
                            ,A.[useredit]
                            ,A.[dateedit]
                            ,A.[isdeleted]
                            ,A.[deletedby]
                            ,A.[deletedon]
                FROM tbset_user AS A 
                LEFT JOIN tbset_user_channel_sstt UC ON UC.userId = A.id
				LEFT JOIN tbmst_channel CH on CH.Id = UC.channelId
                LEFT JOIN tbset_user_distributor AS B ON A.id = B.UserId 
                LEFT JOIN
	            (
                    SELECT
	                            profCat.ProfileID, 
                                cat.Id categoryId, 
                                cat.ShortDesc categoryShortDesc,
                                cat.LongDesc categoryLongDesc
                    FROM
	                    dbo.tbset_map_profile_category profCat
                        LEFT OUTER JOIN dbo.tbmst_category AS cat ON cat.Id = profCat.CategoryID
	           ) C on  C.ProfileID = a.id
                WHERE A.id =@id";


            var list = conn.Query<UserProfileDataById, ListDistributor, ProfileCategory, ProfileChannel, UserProfileDataById>(
            sql, (user, distributor, category, channel) =>
            {
                UserProfileDataById userEntry;

                if (!userDictionary.TryGetValue(user.id!, out userEntry!))
                {
                    userEntry = user;
                    userEntry.distributorlist = new List<ListDistributor>();
                    userEntry.categoryList = new List<ProfileCategory>();
                    userEntry.channelList = new List<ProfileChannel>();
                    userDictionary.Add(userEntry.id!, userEntry);
                }
                if (distributor != null)
                    userEntry.distributorlist!.Add(distributor);
                if (category != null)
                    userEntry.categoryList!.Add(category);
                if (channel != null)
                    userEntry.channelList!.Add(channel);

                return userEntry;
            },
                new { Id = id },
                splitOn: "id, DistributorId, categoryId, channelId")
                .Distinct()
                .FirstOrDefault();

            if (list != null)
            {
                if (list!.categoryList!.Count > 0)
                {
                    list!.categoryList = list.categoryList!.DistinctBy(x => x.categoryId).ToList();
                }
                if (list!.distributorlist!.Count > 0)
                {
                    list!.distributorlist = list.distributorlist!.DistinctBy(x => x.DistributorId).ToList();
                }
                if (list!.channelList!.Count > 0)
                {
                    list!.channelList = list.channelList!.DistinctBy(x => x.channelId).ToList();
                }
            }
            return Task.FromResult(
                list
            )!;
        }

        public async Task CreateUser(UserProfileInsertDto user)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                using var transaction = conn.BeginTransaction();
                var sql = @"INSERT INTO [dbo].[tbset_user]
                    ([id]
                    ,[username]
                    ,[email]
                    ,[password]
                    ,[usergroupid]
                    ,[userlevel]
                    ,[department]
                    ,[jobtitle]                   
                    ,[userinput]
                    ,[dateinput]        
                    ,[registered]  
                    )                
                    VALUES
                    (@id
                    , @username
                    , @email
                    , @password
                    , @usergroupid
                    , @userlevel
                    , @department
                    , @jobtitle                   
                    , @userinput
                    , @dateInput
                    , 1
                    )";
                await conn.ExecuteAsync(sql,
                new
                {
                    id = user.id
                    ,
                    username = user.username
                    ,
                    password = Utils.HashString.GetSha256FromString(user.password!)
                    ,
                    email = user.email
                    ,
                    usergroupid = user.usergroupid
                    ,
                    userlevel = user.userlevel
                    ,
                    department = user.department
                    ,
                    jobtitle = user.jobtitle
                    ,
                    userinput = user.userProfile
                    ,
                    dateInput = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)
                }, transaction
                );

                sql = @"DELETE FROM [dbo].[tbset_user_distributor]
                    WHERE UserId=@userid";
                await conn.ExecuteAsync(sql,
                new
                {
                    userid = user.id
                }, transaction
                );

                // insert distributor
                foreach (var item in user.distributorlist!)
                {
                    sql = @"INSERT INTO [dbo].[tbset_user_distributor]
                            ([UserId]
                            ,[DistributorId]                    
                            ,[CreateOn]
                            ,[CreateBy]                    
                            )                
                            VALUES
                            (@userid
                            , @distid                    
                            , @createon 
                            , @createby                    
                            )";
                    await conn.ExecuteAsync(sql,
                    new
                    {
                        userid = user.id
                        ,
                        distid = item.distributorId
                        ,
                        createby = user.userProfile
                        ,
                        createon = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)

                    }, transaction
                    );
                }
                // delete-insert user category
                sql = @"DELETE FROM [dbo].[tbset_map_profile_category]
                    WHERE ProfileId=@userid";
                await conn.ExecuteAsync(sql,
                new
                {
                    userid = user.id
                }, transaction
                );
                foreach (var item in user.categoryId!)
                {
                    sql = @"INSERT INTO [dbo].[tbset_map_profile_category]
                            ([CreatedOn]
                            ,[CreatedBy]                    
                            ,[CreatedEmail]
                            ,[ProfileId]
                            ,[CategoryId]                    
                            )                
                            VALUES
                            ( @createdon 
                            , @createdby                    
                            , @createdemail                    
                            , @userid
                            , @categoryId                    

                            )";
                    await conn.ExecuteAsync(sql,
                    new
                    {
                        userid = user.id,
                        categoryId = item,
                        createdon = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        createdby = user.userProfile,
                        createdemail = user.userEmail
                    }, transaction
                    );
                }

                // delete-insert user channel
                sql = @"DELETE FROM [dbo].[tbset_user_channel_sstt]
                    WHERE userId=@userid";
                await conn.ExecuteAsync(sql,
                new
                {
                    userid = user.id
                }, transaction
                );
                foreach (var item in user.categoryId!)
                {
                    sql = @"INSERT INTO [dbo].[tbset_user_channel_sstt]
                            ([CreateOn]
                            ,[CreateBy]                    
                            ,[CreatedEmail]
                            ,[userId]
                            ,[channelId]                    
                            )                
                            VALUES
                            ( @createdon 
                            , @createdby                    
                            , @createdemail                    
                            , @userid
                            , @channelId                    

                            )";
                    await conn.ExecuteAsync(sql,
                    new
                    {
                        userid = user.id,
                        channelId = item,
                        createdon = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        createdby = user.userProfile,
                        createdemail = user.userEmail
                    }, transaction
                    );
                }
                transaction.Commit();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<bool> UpdateUser(UserProfileInsertDto user)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            bool res = false;
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                using var transaction = conn.BeginTransaction();
                var sql = @"UPDATE [dbo].[tbset_user]
                        SET
                        username = @username
                        ,email=@email
                        ,usergroupid=@usergroupid
                        ,userlevel=@userlevel
                        ,department=@department
                        ,jobtitle=@jobtitle
                        ,useredit=@useredit
                        ,dateedit=@dateedit
                        WHERE 
                        id=@id";
                await conn.ExecuteAsync(sql,
                 new
                 {
                     id = user.id,
                     username = user.username
                        ,
                     email = user.email
                        ,
                     usergroupid = user.usergroupid
                        ,
                     userlevel = user.userlevel
                        ,
                     department = user.department
                        ,
                     jobtitle = user.jobtitle
                        ,
                     useredit = user.userProfile
                        ,
                     dateedit = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)
                 }, transaction
                );


                sql = @"DELETE FROM [dbo].[tbset_user_distributor]
                    WHERE UserId=@userid";
                var __res = await conn.ExecuteAsync(sql,
                new
                {
                    userid = user.id
                }, transaction
                );

                foreach (var item in user.distributorlist!)
                {
                    sql = @"INSERT INTO [dbo].[tbset_user_distributor]
                            ([UserId]
                            ,[DistributorId]                    
                            ,[CreateOn]
                            ,[CreateBy]                    
                            )                
                            VALUES
                            (@userid
                            , @distid                    
                            , @createon
                            , @createby                    
                            )";
                    await conn.ExecuteAsync(sql,
                    new
                    {
                        userid = user.id,
                        distid = item.distributorId,
                        createby = user.userProfile,
                        createon = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)

                    }, transaction
                    );
                }

                // delete-insert user category

                sql = @"DELETE FROM [dbo].[tbset_map_profile_category]
                    WHERE ProfileId=@userid";
                await conn.ExecuteAsync(sql,
                new
                {
                    userid = user.id
                }, transaction
                );

                foreach (var item in user.categoryId!)
                {
                    sql = @"INSERT INTO [dbo].[tbset_map_profile_category]
                            ([CreatedOn]
                            ,[CreatedBy]                    
                            ,[CreatedEmail]
                            ,[ProfileId]
                            ,[CategoryId]                    
                            )                
                            VALUES
                            ( @createdon 
                            , @createdby                    
                            , @createdemail                    
                            , @userid
                            , @categoryId                    

                            )";
                    await conn.ExecuteAsync(sql,
                    new
                    {
                        userid = user.id,
                        categoryId = item,
                        createdon = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        createdby = user.userProfile,
                        createdemail = user.userEmail
                    }, transaction
                    );
                }

                // delete-insert user channel
                sql = @"DELETE FROM [dbo].[tbset_user_channel_sstt]
                    WHERE userId=@userid";
                await conn.ExecuteAsync(sql,
                new
                {
                    userid = user.id
                }, transaction
                );
                foreach (var item in user.channelId!)
                {
                    sql = @"INSERT INTO [dbo].[tbset_user_channel_sstt]
                            ([ModifiedOn]
                            ,[ModifiedBy]                    
                            ,[ModifiedEmail]
                            ,[userId]
                            ,[channelId]                    
                            )                
                            VALUES
                            ( @createdon 
                            , @createdby                    
                            , @createdemail                    
                            , @userid
                            , @channelId                    

                            )";
                    await conn.ExecuteAsync(sql,
                    new
                    {
                        userid = user.id,
                        channelId = item,
                        createdon = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        createdby = user.userProfile,
                        createdemail = user.userEmail
                    }, transaction
                    );
                }
                transaction.Commit();
                res = true;
            }
            return res;
        }

        public async Task<bool> DeleteUser(UserUpdateDto userUpdateDto)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            bool res = false;
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                var sql = @"UPDATE [dbo].[tbset_user]
                        SET
                        isdeleted=@isdeleted
                        ,deletedby=@deletedby
                        ,deletedon=@deletedon
                        WHERE
                        id=@id";
                var __res = await conn.ExecuteAsync(sql, new
                {
                    Id = userUpdateDto.id
                    ,
                    isDeleted = userUpdateDto.isdeleted
                    ,
                    deletedBy = userUpdateDto.deletedby
                    ,
                    deletedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)
                });
                res = __res > 0;
            }

            return res;
        }


        public async Task<List<DistributorSelect>> GetUserDistributor(string profileID)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT id, LongDesc, ShortDesc, RefId FROM tbmst_distributor";
                var __res = await conn.QueryAsync<DistributorSelect>(String.Format(__query, profileID));
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ListUserGroupMenu>> GetUserGroupMenuList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var sql = @"SELECT usergroupid, usergroupname FROM tbset_usergroup ORDER BY usergroupname ASC";
                var __res = await conn.QueryAsync<ListUserGroupMenu>(sql);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ProfileCategory>> GetProfileListCategory()
        {
            using IDbConnection conn = Connection;
            try
            {
                var sql = @"SELECT Id as categoryId, ShortDesc as categoryShortDesc, LongDesc as categoryLongDesc
                    FROM tbmst_category
                    WHERE isnull(isdeleted, 0)=0";
                var __res = await conn.QueryAsync<ProfileCategory>(sql);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<ListUserGroupRights>> GettUserRightsList(string UserGroupId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var sql = @"SELECT userlevel as userlevelid, levelname as userlevelname FROM tbset_userlevel where usergroupid=@UserGroupId ORDER BY levelname ASC";
                var __res = await conn.QueryAsync<ListUserGroupRights>(sql, new { UserGroupId = UserGroupId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetChannelList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @" SELECT id, refId, shortDesc, longDesc FROM tbmst_channel 
                                 WHERE ISNULL(IsDelete, 0) = 0";
                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }


    }
}
