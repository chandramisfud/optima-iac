using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.Report
{
    public class ListingPromoReportingRequestParam 
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public ListingPromoReportingSortColumn SortColumn { get; set; }
        public ListingPromoReportingSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
        public string? CreateFrom { get; set; }
        public string? CreateTo { get; set; }
        public string? StartFrom { get; set; }
        public string? StartTo { get; set; }

        public int CategoryId { get; set; }
        public int SubmissionParam { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ListingPromoReportingSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ListingPromoReportingSortColumn
    {
        promoNumber,
        initiator,
        subAccountDesc,
        activityDesc,
        startPromo,
        endPromo,
        investment,
        createOn,
        lastStatus,
        lastStatusDate,
        dnClaim,
        category
    }

    public class ListingPromoReportingDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}
