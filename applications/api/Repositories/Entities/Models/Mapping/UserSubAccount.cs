using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class UserSubAccountModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int channelid { get; set; }
        public string? channel { get; set; }
        public int subChannelid { get; set; }
        public string? subChannel { get; set; }
        public int accountid { get; set; }
        public string? account { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public int IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public int IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class UserSubAccountCreate
    {
        public string? UserId { get; set; }
        public int SubAccountId { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class UserSubAccountCreateReturn
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public int SubAccountId { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }

    }
    public class UserSubAccountLandingPage
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int channelid { get; set; }
        public string? channel { get; set; }
        public int subChannelid { get; set; }
        public string? subChannel { get; set; }
        public int accountid { get; set; }
        public string? account { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public int IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserSubAccountSortColumn
    {
        Id,
        UserId,
        channelid,
        channel,
        subChannelid,
        subChannel,
        accountid,
        account,
        SubAccountId,
        SubAccount
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserSubAccountSortDirection
    {
        asc,
        desc
    }
    public class UserSubAccountListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public UserSubAccountSortColumn SortColumn { get; set; }
        public UserSubAccountSortDirection SortDirection { get; set; }
    }

    public class ChannelforUserSubAccount
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubChannelforUserSubAccount
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class AccountforUserSubAccount
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubAccountforUserSubAccount
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class UserIdUserSubAccount
    {
        public string? Id { get; set; }
    }

    public class UserSubAccountDelete
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class UserSubAccountDeleteReturn
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeleteEmail { get; set; }
    }

}

