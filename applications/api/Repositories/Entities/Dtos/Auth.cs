using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Dtos
{
    public class ProfileCategory
    {
        public int categoryId { get; set; }
        public string categoryShortDesc { get; set; } = string.Empty;
        public string categoryLongDesc { get; set; } = string.Empty;
    }
    public class ProfileChannel
    {
        public int channelId { get; set; }
        public string channelShortDesc { get; set; } = string.Empty;
        public string channelLongDesc { get; set; } = string.Empty;
    }
    public class AuthClaim
    {
        public string UserEmail { get; set; } = string.Empty;
        public string ProfileID { get; set; } = string.Empty;
        public string UserGroupID { get; set; } = string.Empty;
        public string UserLevel { get; set; } = string.Empty;
        // added andrie Sept 27 2023 E2 #33
        public List<ProfileCategory>? ProfileCategories { get; set; }
    }
    public class UserbyEmail
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
        public double expLastLogin { get; set; }
        public int errCode { get; set; }
        public string? errMessage { get; set; }
    }
    public class EmailDeleteBody
    {
        public string? email { get; set; }
    }
}
