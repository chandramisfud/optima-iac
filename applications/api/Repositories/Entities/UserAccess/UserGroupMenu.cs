using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class UserGroupMenuModel
    {
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public string? userinput { get; set; }
        public string? dateinput { get; set; }
        public string? useredit { get; set; }
        public string? dateedit { get; set; }
        public int groupmenupermission { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeletedEmail { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedOn { get; set; }
    }

    public class UserGroupMenuLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<UserGroupMenuSelect>? Data { get; set; }
    }

    public class UserGroupMenuById
    {
        public string? usergroupid { get; set; }
    }

    public class UserGroupMenuSelect
    {
        public string? UserGroupId { get; set; }
        public string? UserGroupName { get; set; }
        public int GroupMenuPermission { get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CreatedEmail { get; set; }
        public string? userinput { get; set; }
        public DateTime? dateinput { get; set; }
        public string? useredit { get; set; }
        public DateTime? dateedit { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedEmail { get; set; }
        public int IsDeleted { get; set; }
    }

    public class UserGroupMenuListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public UserGroupMenuSortColumn SortColumn { get; set; }
        public UserGroupMenuSortDirection SortDirection { get; set; }
        //public enShowDeleted ShowDeleted { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserGroupMenuSortColumn
    {
        UserGroupId,
        UserGroupName,
        GroupMenuPermission,
        Category,
        Name,
        Description
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserGroupMenuSortDirection
    {
        asc,
        desc
    }

    public class UserGroupMenuCreate
    {
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public string? userinput { get; set; }
        public int groupmenupermission { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class UserGroupMenuCreateBody
    {
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public int groupmenupermission { get; set; }
    }

    public class UserGroupMenuCreateReturn
    {
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public string? userinput { get; set; }
        public string? dateinput { get; set; }
        public int groupmenupermission { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class UserGroupMenuUpdate
    {
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public int groupmenupermission { get; set; }
        public string? useredit { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class UserGroupMenuUpdateBody
    {
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public int groupmenupermission { get; set; }
    }

    public class UserGroupMenuUpdateReturn
    {
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public int groupmenupermission { get; set; }
        public string? useredit { get; set; }
        public string? dateedit { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class UserGroupMenuDelete
    {
        public string? usergroupid { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedEmail { get; set; }
    }
    public class UserGroupMenuDeleteReturn
    {
        public string? usergroupid { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeletedEmail { get; set; }
    }

    public class UserGroupListUserLevel
    {
        public int userlevel { get; set; }
        public string? levelname { get; set; }
    }
}