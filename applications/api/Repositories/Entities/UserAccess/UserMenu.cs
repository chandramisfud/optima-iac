using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Models
{
    public class UserRightsDto2
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? text { get; set; }
        public string? icon { get; set; }
        public string? parent_id { get; set; }
        public string? akses { get; set; }

    }

    public class UserRightPostDto
    {
        public string? usergroupid { get; set; }
        public IList<UserRightArrayDto>? userRightArrays { get; set; }
    }

    public class UserRightArrayDto
    {
        public string? usergroupid { get; set; }
        public string? menuid { get; set; }

    }

    public class UserRightsLevelDto
    {

        public string? id { get; set; }
        public string? name { get; set; }
        public string? text { get; set; }
        public string? icon { get; set; }
        public string? parent_id { get; set; }
        public string? url { get; set; }

    }

    public class CRUDMenuDto
    {

        public string? usergroupid { get; set; }
        public int userlevel { get; set; }
        public string? menuid { get; set; }
        public int crud { get; set; }
        public int approve { get; set; }
        public string? flag { get; set; }
        public int create_rec { get; set; }
        public int read_rec { get; set; }
        public int update_rec { get; set; }
        public int delete_rec { get; set; }


    }

    public partial class UserLevelAccess
    {

        public string? usergroupid { get; set; }
        public string? menuid { get; set; }
        public int userlevel { get; set; }
        public int create_rec { get; set; }
        public int read_rec { get; set; }
        public int update_rec { get; set; }
        public int delete_rec { get; set; }

    }



    public class UserAccessGroupRightMenu
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? text { get; set; }
        public string? icon { get; set; }
        public string? parent_id { get; set; }
        public string? akses { get; set; }
        public object[]? children { get; set; }
    }

    public class UserAccessGroupRightMenuChild
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? text { get; set; }
        public string? icon { get; set; }
        public string? parent_id { get; set; }
        public string? akses { get; set; }
        public object[]? children { get; set; }
//        public UserAccessGroupRightMenuState? state { get; set; }
    }

    public class UserAccessGroupRightMenuChildWithState
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? text { get; set; }
        public string? icon { get; set; }
        public string? parent_id { get; set; }
        public string? akses { get; set; }
        public object[]? children { get; set; }
        public UserAccessGroupRightMenuState? state { get; set; }
    }

    public class UserAccessGroupRightMenuState
    {
        public bool selected { get; set; }
    }

    public class UserAccessGroupRightMenuByUserLevel
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? text { get; set; }
        public string? icon { get; set; }
        public string? parent_id { get; set; }
        public UserAccessGroupRightMenuChildByUserLevel[]? children { get; set; }
    }

    public class UserAccessGroupRightMenuChildByUserLevel
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? text { get; set; }
        public string? icon { get; set; }
        public string? parent_id { get; set; }
        public UserAccessGroupRightMenuChildByUserLevel[]? children { get; set; }
    }

}
