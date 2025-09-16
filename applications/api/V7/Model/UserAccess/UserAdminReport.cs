using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.UserAccess
{
    public class UserAdminReportListRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserAdminReportSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserAdminReportSortColumn
    {
        id,
        username,
        email,
        department,
        jobtitle,
        contactinfo,
        distributorid,
        usergroupid,
        usergroupname,
        userlevel,
        levelname,
        menuid,
        submenu,
        flag,
        crud,
        approve,
        create_rec,
        read_rec,
        update_rec,
        delete_rec,
        parent
    }
}
