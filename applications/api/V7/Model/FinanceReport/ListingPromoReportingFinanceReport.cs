using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.FinanceReport
{
    public class FinListingPromoReportingRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinListingPromoReportingSortColumn SortColumn { get; set; }
        public FinListingPromoReportingSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public string? profileId { get; set; }
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
    public enum FinListingPromoReportingSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinListingPromoReportingSortColumn
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
        categoryId,
        categoryShortDesc,
        categoryLongDesc
    }

    public class FinListingPromoReportingDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }

    public class ChannelListingPromoReporting
    {
        public string? userid { get; set; }
    }

}
