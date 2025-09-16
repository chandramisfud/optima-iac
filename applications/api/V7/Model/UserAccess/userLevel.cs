using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace V7.Model.UserAccess
{
    public class userLevelCreateParam
    {
        public int userlevel { get; set; }
        public string? levelname { get; set; }
        public string? usergroupid { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum sortUsersLevelField
    {
        userlevel,
        levelname
        , usergroupname
        , usergroupid
    }

    public class usersLevelLPParam : LPParam
    {
        public new string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public new int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public new int PageSize { get; set; } = 10;
        public string? usergroupid { get; set; }
        public fltToogle Status { get; set; } = fltToogle.ALL;
        public sortUsersLevelField SortColumn { get; set; } = sortUsersLevelField.userlevel;
        public sortDirection SortDirection { get; set; } = sortDirection.asc;

    }
}
