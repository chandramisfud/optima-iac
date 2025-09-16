using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.FinanceReport
{
    public class FinListingPromoReconRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinListingPromoReconSortColumn SortColumn { get; set; }
        public FinListingPromoReconSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public string?[]? profileId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
        public string? CreateFrom { get; set; }
        public string? CreateTo { get; set; }
        public string? StartFrom { get; set; }
        public string? StartTo { get; set; }
        public int SubmissionParam { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinListingPromoReconSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinListingPromoReconSortColumn
    {
        promoNumber,
        initiator,
        subAccountDesc,
        activityDesc
    }

    public class FinListingPromoReconDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}
