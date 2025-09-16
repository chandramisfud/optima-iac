using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class DistributorSubAccountList
    {
        public int Id { get; set; }
        public int DistributorId { get; set; }
        public string? Distributor { get; set; }
        public int ChannelId { get; set; }
        public string? Channel { get; set; }
        public int SubChannelId { get; set; }
        public string? SubChannel { get; set; }
        public int AccountId { get; set; }
        public string? Account { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class DistributorSubAccountDownload
    {
        public int Id { get; set; }
        public int DistributorId { get; set; }
        public string? Distributor { get; set; }
        public int ChannelId { get; set; }
        public string? Channel { get; set; }
        public int SubChannelId { get; set; }
        public string? SubChannel { get; set; }
        public int AccountId { get; set; }
        public string? Account { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public bool isDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class DistributorSubAccountCreate
    {
        public int DistributorId { get; set; }
        public int ChannelId { get; set; }
        public int SubChannelId { get; set; }
        public int AccountId { get; set; }
        public int SubAccountId { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class DistributorSubAccountCreateReturn
    {
        public int Id { get; set; }
        public int DistributorId { get; set; }
        public int ChannelId { get; set; }
        public int SubChannelId { get; set; }
        public int AccountId { get; set; }
        public int SubAccountId { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class DistributorSubAccountDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class DistributorforDistributorSubAccount
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class ChannelforDistributorSubAccount
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubChannelforDistributorSubAccount
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class AccountforDistributorSubAccount
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubAccountforDistributorSubAccount
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DistributorSubAccountSortColumn
    {
        id,
        DistributorId,
        Distributor,
        ChannelId,
        Channel,
        SubChannelId,
        SubChannel,
        AccountId,
        Account,
        SubAccountId,
        SubAccount,
        CreateOn,
        CreateBy,
        CreatedEmail
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DistributorSubAccountSortDirection
    {
        asc,
        desc
    }
    public class DistributorSubAccountListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public DistributorSubAccountSortColumn SortColumn { get; set; }
        public DistributorSubAccountSortDirection SortDirection { get; set; }
    }

    public class DistributorSubAccountDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public int parentId { get; set; }
    }

}