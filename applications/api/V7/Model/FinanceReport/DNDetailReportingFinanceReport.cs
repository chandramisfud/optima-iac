using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.FinanceReport
{
    public class FinDNDetailReportingListRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinDNDetailReportingSortColumn SortColumn { get; set; }
        public FinDNDetailReportingSortDirection SortDirection { get; set; }
        public string? profileId { get; set; }
        public string? Period { get; set; }
        public int CategoryId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int ChannelId { get; set; }
        public int AccountId { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinDNDetailReportingSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinDNDetailReportingSortColumn
    {
        Category,
        Id,
        RefId,
        PromoId,
        ActivityDesc,
        DPP,
        IsDNPromo,
        PromoRefId,
        IsOverBudget,
        Initiator,
        LastStatus,
        categoryId,
        categoryShortDesc,
        categoryLongDesc,
        dnCategory,
        overBudgetStatus
    }

    public class FinDNDetailReportingDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }

}
