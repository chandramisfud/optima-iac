using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class PICDNMAnualModel
    {
        public int ChannelId { get; set; }
        public string? Channel { get; set; }
        public int SubChannelId { get; set; }
        public string? SubChannel { get; set; }
        public int AccountId { get; set; }
        public string? Account { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public string? Pic1 { get; set; }
        public string? Pic2 { get; set; }
        public int IsActive { get; set; }
        public string? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? Id { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class PICDNManualCreate
    {
        public int ChannelId { get; set; }
        public int SubChannelId { get; set; }
        public int AccountId { get; set; }
        public int SubAccountId { get; set; }
        public string? Pic1 { get; set; }
        public string? Pic2 { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class PICDNManualCreateReturn
    {
        public string? Id { get; set; }
        public int ChannelId { get; set; }
        public int SubChannelId { get; set; }
        public int AccountId { get; set; }
        public int SubAccountId { get; set; }
        public string? Pic1 { get; set; }
        public string? Pic2 { get; set; }
        public string? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class PICDNManualLandingPage
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public string? Channel { get; set; }
        public int SubChannelId { get; set; }
        public string? SubChannel { get; set; }
        public int AccountId { get; set; }
        public string? Account { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public string? Pic1 { get; set; }
        public string? Pic2 { get; set; }
        public string? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PICDNManualSortColumn
    {
        Id,
        ChannelId,
        Channel,
        SubChannelId,
        SubChannel,
        AccountId,
        Account,
        SubAccountId,
        SubAccount,
        Pic1,
        Pic2,
        CreateOn,
        CreateBy,
        CreatedEmail
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PICDNManualSortDirection
    {
        asc,
        desc
    }
    public class PICDNManualListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public PICDNManualSortColumn SortColumn { get; set; }
        public PICDNManualSortDirection SortDirection { get; set; }
    }
    public class ChannelforPICDNManual
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubChannelforPICDNManual
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class AccountforPICDNManual
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubAccountforPICDNManual
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class UserIdPICDNManual
    {
        public string? Id { get; set; }
    }

    public class PICDNManualDelete
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class PICDNManualDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public int ParentId { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }

}

