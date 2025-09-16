using System.ComponentModel.DataAnnotations;

namespace Repositories.Entities.Dtos
{
    public partial class UserDistributor
    {
        [Key]
        public string? UserId { get; set; }
        public string? DistributorId { get; set; }
        public string? DistributorLongDesc { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }
    public class User
    {
        public string? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? pictureprofilefile { get; set; }
        public string? password { get; set; }
        public string? usergroupid { get; set; }
        public int userlevel { get; set; }
        public int isLogin { get; set; }
        public DateTime lastLogin { get; set; }
        public int cnt { get; set; }
        public string? groupmenupermission { get; set; }
        public string? department { get; set; }
        public string? jobtitle { get; set; }
        public string? contactinfo { get; set; }
        public IList<UserDistributor>? distributorlist { get; set; }
        public int registered { get; set; }
        public string? code { get; set; }
        public DateTime password_change { get; set; }
        public string? token { get; set; }
        public DateTime token_date { get; set; }
        public string? userinput { get; set; }
        public DateTime dateinput { get; set; }
        public string? useredit { get; set; }
        public DateTime dateedit { get; set; }
        public int isdeleted { get; set; }
        public bool usernew { get; set; }
        public int loginFailedCount { get; set; }
        public DateTime loginFailedLastTime { get; set; }
        public double loginFreezeTime { get; set; }
        public int errCode { get; set; }
        public string? errMessage { get; set; }

    }

    public class UserProfileInsertDto
    {
        public required string id { get; set; }
        public required string username { get; set; }
        public required string password { get; set; }
        public required string email { get; set; }
        public string? department { get; set; }
        public string? jobtitle { get; set; }
        public string? usergroupid { get; set; }
        public int userlevel { get; set; }
        public string? userProfile { get; set; }
        public string? userEmail { get; set; }
        public List<UserProfileDistributorlist>? distributorlist { get; set; }
        public IList<int>? categoryId { get; set; }
        public List<int>? channelId { get; set; }
    }

    public class UserProfileDistributorlist
    {
        public string? userId { get; set; }
        public string? distributorId { get; set; }
    }
    public class UserProfile
    {
        public string? profileid { get; set; }
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public int userlevel { get; set; }
        public int groupmenupermission { get; set; }
        // added andrie Sept 27 2023 E2 #33
        public int categoryid { get; set; }
        public string? categoryshortdesc { get; set; }
        public string? categorylongdesc { get; set; }
    }

    // added andrie Sept 27 2023 E2 #33
    public class UserProfileCategory
    {
        public string? profileid { get; set; }
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public int userlevel { get; set; }
        public int groupmenupermission { get; set; }

        public List<ProfileCategory>? ProfileCategories { get; set; }
    }
    public class UserBody
    {
        public string? id { get; set; }
    }

    public class UserProfileResultHeader
    {
        public string? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? contactinfo { get; set; }
        public int registered { get; set; }
        public DateTime password_change { get; set; }
        public string? userinput { get; set; }
        public DateTime dateinput { get; set; }
        public string? useredit { get; set; }
        public DateTime dateedit { get; set; }
        public int isdeleted { get; set; }
        public string? deletedby { get; set; }
        public DateTime deletedon { get; set; }
        public DateTime lastLogin { get; set; }
        public string? profileuser { get; set; }
    }
    public class UserProfileBody
    {
        public int id { get; set; }
    }

    public class UserProfileInsert
    {
        public int id { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? email { get; set; }
        public string? contactinfo { get; set; }
        public string? userid { get; set; }
        public string[]? profile { get; set; }

    }

    public class UserProfileResult
    {
        public UserProfileResultHeader? userprofileuserresult { get; set; }
        public List<Profile>? profileresult { get; set; }


    }
    public class Profile
    {
        public int id { get; set; }
        public string? profileid { get; set; }
    }

    public class GetUserBody
    {
        public int active { get; set; }
    }

    public class UserPasswordChange
    {
        /// <summary>
        /// user login email 
        /// </summary>
        public string? email { get; set; }

        /// <summary>
        /// user login password
        /// </summary>
        public string? password { get; set; }
    }

    public class UserOldPasswordChange
    {
        /// <summary>
        /// user login password
        /// </summary>
        public string? password { get; set; }
        /// <summary>
        /// user old login password
        /// </summary>

        // modified by: andrie June 6 2023
        //public string? oldPassword { get; set; }

    }
    public class SendPasswordUrlBody
    {
        public string? LinkUrl { get; set; }
        public string? UserEmail { get; set; }
        //        public DateTime passwordChange { get; set; }
    }
    public class UserUpdateDto
    {
        public string? id { get; set; }
        public int isdeleted { get; set; }
        public string? deletedby { get; set; }
        public DateTime deletedon { get; set; }
    }

    public class ListUserGroupMenu
    {
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
    }

    public class ListUserGroupRights
    {
        public int userlevelid { get; set; }
        public string? userlevelname { get; set; }
    }

}
