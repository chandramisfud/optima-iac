using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.FinanceReport
{
    public class FinPromoSubmissionRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinPromoSubmissionSortColumn SortColumn { get; set; }
        public FinPromoSubmissionSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public string? profileId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int ChannelId { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinPromoSubmissionSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinPromoSubmissionSortColumn
    {
        Entity,
        Distributor,
        PrincipalDesc,
        BudgetSource,
        PromoNumber,
        SubCategory,
        Activity,
        SubActivity,
        ActivityDesc,
        Mechanisme1,
        Mechanisme2,
        Mechanisme3,
        Mechanisme4,
        RegionDesc,
        ChannelDesc,
        SubChannelDesc,
        AccountDesc,
        SubAccountDesc,
        LastStatus,
        ApprovalNotes,
        BrandDesc,
        SKUDesc,
        PromoPlanRefId,
        ReconStatus,
        SubactivityTypeRefId,
        SubactivityType
    }
    public class FinPromoSubmissionDistributorListParam
    {
        public int budgetId { get; set; }
        public int[]? entityId { get; set; }
    }
    public class ChannelPromoSubmissionParam
    {
        public string? userid { get; set; }
    }
}