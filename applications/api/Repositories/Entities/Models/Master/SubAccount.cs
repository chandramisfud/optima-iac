using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class SubAccountModel
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public int SubChannelId { get; set; }
        public int AccountId { get; set; }
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
        public string? DeleteEmail { get; set; }
    }

    public class SubAccountLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<SubAccountSelect>? Data { get; set; }
    }
    public class SubAccountById
    {
        public int Id { get; set; }
    }
    public class SubAccountSelect
    {
        public int ChannelId { get; set; }
        public string? ChannelRefId { get; set; }
        public string? ChannelLongDesc { get; set; }
        public string? ChannelShortDesc { get; set; }
        public int SubChannelId { get; set; }
        public string? SubChannelRefId { get; set; }
        public string? SubChannelLongDesc { get; set; }
        public string? SubChannelShortDesc { get; set; }
        public int AccountId { get; set; }
        public string? AccountRefId { get; set; }
        public string? AccountLongDesc { get; set; }
        public string? AccountShortDesc { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccountRefId { get; set; }
        public string? SubAccountLongDesc { get; set; }
        public string? SubAccountShortDesc { get; set; }
    }
    public class SubAccountListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public SubAccountSortColumn SortColumn { get; set; }
        public SubAccountSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubAccountSortColumn
    {
        ChannelId,
        ChannelRefId,
        ChannelLongDesc,
        ChannelShortDesc,
        SubChannelId,
        SubChannelRefId,
        SubChannelLongDesc,
        SubChannelShortDesc,
        AccountId,
        AccountRefId,
        AccountLongDesc,
        AccountShortDesc,
        SubAccountId,
        SubAccountRefId,
        SubAccountLongDesc,
        SubAccountShortDesc
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubAccountSortDirection
    {
        asc,
        desc
    }
    public class SubAccountCreate
    {
        public int AccountId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class SubAccountCreateReturn
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? RefId { get; set; }
    }
    public class SubAccountUpdate
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class SubAccountUpdateReturn
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class SubAccountDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class SubAccountDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class SubAccountAccount
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }

    }
}