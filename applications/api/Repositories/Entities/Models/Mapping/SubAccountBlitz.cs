using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class SubAccountBlitzModel
    {
        public int id { get; set; }
        public int accountId { get; set; }
        public string? account { get; set; }
        public int subAccountId { get; set; }
        public string? subAccount { get; set; }
        public int channelId { get; set; }
        public string? channel { get; set; }
        public int subChannelId { get; set; }
        public string? subChannel { get; set; }
        public string? sapCode { get; set; }
        public string? createOn { get; set; }
        public string? createBy { get; set; }
        public string? createdEmail { get; set; }
        public int isDelete { get; set; }
        public string? deleteOn { get; set; }
        public string? deleteBy { get; set; }
        public string? deleteEmail { get; set; }
    }
    public class SubAccountBlitzCreate
    {
        public int ChannelId { get; set; }
        public int SubChannelId { get; set; }
        public int AccountId { get; set; }
        public int SubAccountId { get; set; }
        public string? SAPCode { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class SubAccountBlitzCreateReturn
    {
        public string? Id { get; set; }
        public int channelid { get; set; }
        public int subChannelId { get; set; }
        public int accountId { get; set; }
        public int subAccountId { get; set; }
        public string? sapCode { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? CreateOn { get; set; }
    }
    public class SubAccountBlitzLandingPage
    {
        public int id { get; set; }
        public int accountId { get; set; }
        public string? account { get; set; }
        public int subAccountId { get; set; }
        public string? subAccount { get; set; }
        public int channelId { get; set; }
        public string? channel { get; set; }
        public int subChannelId { get; set; }
        public string? subChannel { get; set; }
        public string? sapCode { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubAccountBlitzSortColumn
    {
        Id,
        accountId,
        account,
        subAccountId,
        subAccount,
        channelId,
        channel,
        subChannelId,
        subChannel,
        sapCode
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubAccountBlitzSortDirection
    {
        asc,
        desc
    }
    public class SubAccountBlitzListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public SubAccountBlitzSortColumn SortColumn { get; set; }
        public SubAccountBlitzSortDirection SortDirection { get; set; }
    }
    public class ChannelforSubAccountBlitz
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubChannelforSubAccountBlitz
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class AccountforSubAccountBlitz
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubAccountforSubAccountBlitz
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class SubAccountBlitzDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class SubAccountBlitzDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
    }

}

