using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public int SubChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public int IsActive { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? RefId { get; set; }
        public string? SAPCode { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeletedEmail { get; set; }
    }

    public class AccountLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<AccountSelect>? Data { get; set; }

    }
    public class AccountById
    {
        public int Id { get; set; }
    }

    public class AccountSelect
    {
        public int ChannelId { get; set; }
        public string? ChannelLongDesc { get; set; }
        public string? ChannelShortDesc { get; set; }
        public string? ChannelRefId { get; set; }
        public int SubChannelId { get; set; }
        public string? SubChannelLongDesc { get; set; }
        public string? SubChannelShortDesc { get; set; }
        public string? SubChannelRefid { get; set; }
        public int AccountId { get; set; }
        public string? AccountLongDesc { get; set; }
        public string? AccountShortDesc { get; set; }
        public string? AccountRefId { get; set; }
    }

    public class AccountListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public AccountSortColumn SortColumn { get; set; }
        public AccountSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AccountSortColumn
    {
        ChannelId,
        ChannelLongDesc,
        ChannelShortDesc,
        ChannelRefId,
        SubChannelId,
        SubChannelLongDesc,
        SubChannelShortDesc,
        SubChannelRefid,
        AccountId,
        AccountLongDesc,
        AccountShortDesc,
        AccountRefId
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AccountSortDirection
    {
        asc,
        desc
    }

    public class AccountCreateBody
    {
        public int SubChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
    }

    public class AccountCreate
    {
        public int SubChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class AccountCreateReturn
    {
        public int Id { get; set; }
        public int SubChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class AccountUpdate
    {
        public int Id { get; set; }
        public int SubChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }
    public class AccountUpdateBody
    {
        public int Id { get; set; }
        public int SubChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
    }

    public class AccountUpdateReturn
    {
        public int Id { get; set; }
        public int SubChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class AccountDeleteBody
    {
        public int Id { get; set; }

    }

    public class AccountDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class AccountDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class AccountByAccess
    {
        public string? userid { get; set; }

        public int channelid { get; set; }
    }

    public class AccountByAccessReturn
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class AccountSubChannel
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class AccountChannel
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }



}