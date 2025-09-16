using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace V7.Model.UserAccess
{
#pragma warning disable CS1591
    /// <summary>
    /// record status filter
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum fltToogle
    {
        ALL,
        InActive,
        Active
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum sortUsersField
    {
        email,
        userName,
        contactInfo,
        status
    }

    /// <summary>
    /// Sort Direction
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum sortDirection
    {
        asc,
        desc
    }
    /// <summary>
    /// user LP paramater
    /// </summary>
    public class usersLPParam
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public fltToogle Status { get; set; } = fltToogle.ALL;
        public sortUsersField SortColumn { get; set; } = sortUsersField.email;
        public sortDirection SortDirection { get; set; } = sortDirection.asc;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum sortUserProfileField
    {
        id, username, email, department, jobtitle, usergroupname, levelname, status, profileCategory
    }

    public class userProfileLPParam
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public fltToogle Status { get; set; } = fltToogle.ALL;
        public string? usergroupid { get; set; }
        public int userlevel { get; set; } = 0;
        public sortUserProfileField SortColumn { get; set; } = sortUserProfileField.id;
        public sortDirection SortDirection { get; set; } = sortDirection.asc;

    }
    public class userIdParam
    {
        public string? id { get; set; }
    }

    public class userLevelIdParam
    {
        public int userlevel { get; set; }
    }

    public class UserProfileInsertParam
    {
        public int id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? contactinfo { get; set; }
        public string?[]? profile { get; set; }

    }

    public class UserAccessProfileInsertParam
    {
        public required string id { get; set; }
        public required string username { get; set; }
        public required string email { get; set; }
        public required string department { get; set; }
        public required string jobtitle { get; set; }
        public required string usergroupid { get; set; }
        public int userlevel { get; set; }
        public required Distributorlist[] distributorlist { get; set; }
        public required int[] CategoryId { get; set; }
        public int[] channelId { get; set; }
    }

    public class Distributorlist
    {
        public required string userId { get; set; }
        public required string distributorId { get; set; }
    }

}
