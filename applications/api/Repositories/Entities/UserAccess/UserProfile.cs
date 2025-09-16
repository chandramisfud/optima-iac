using Repositories.Entities.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Entities.Models
{
    public class UserProfileData
    {
        public string? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? department { get; set; }
        public string? jobtitle { get; set; }
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public int userlevel { get; set; }
        public string? levelname { get; set; }
        public string? status { get; set; }
        public string? ProfileCategory { get; set; }
    }

    public class UserProfileDataById
    {
        public string? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? pictureprofilefile { get; set; }
        public string? password { get; set; }
        public string? usergroupid { get; set; }
        public int userlevel { get; set; }
        public int isLogin { get; set; }
        public DateTime? lastLogin { get; set; }
        public int cnt { get; set; }
        public string? groupmenupermission { get; set; }
        public string? department { get; set; }
        public string? jobtitle { get; set; }
        public string? contactinfo { get; set; }
        public IList<ListDistributor>? distributorlist { get; set; }
        public IList<ProfileCategory>? categoryList { get; set; }
        public IList<ProfileChannel>? channelList { get; set; }
        public int registered { get; set; }
        public string? code { get; set; }
        public DateTime? password_change { get; set; }
        public string? token { get; set; }
        public DateTime? token_date { get; set; }
        public string? userinput { get; set; }
        public DateTime? dateinput { get; set; }
        public string? useredit { get; set; }
        public DateTime? dateedit { get; set; }
        public int isdeleted { get; set; }
        public bool usernew { get; set; }
        public int loginFailedCount { get; set; }
        public DateTime? loginFailedLastTime { get; set; }
        public double loginFreezeTime { get; set; }
        public int errCode { get; set; }
        public string? errMessage { get; set; }
    }
    public partial class ListDistributor
    {
        [Key]
        public string? UserId { get; set; }
        public string? DistributorId { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }
    public class UserProfileLP
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<UserProfileData>? Data { get; set; }
    }
    public class userProfileCreate
    {
        public int userlevel { get; set; }
        public string? levelname { get; set; }
        public string? usergroupid { get; set; }
        public string? byUserName { get; set; }
        public string? byUserEmail { get; set; }
    }
}