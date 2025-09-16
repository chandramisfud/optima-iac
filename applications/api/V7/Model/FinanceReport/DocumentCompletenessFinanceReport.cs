using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.FinanceReport
{
    public class FinDocumentCompletenessRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinDocumentCompletenessSortColumn SortColumn { get; set; }
        public FinDocumentCompletenessSortDirection SortDirection { get; set; }
        public string? profileId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public string? TaxLevel { get; set; }
        public string? Status { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinDocumentCompletenessSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinDocumentCompletenessSortColumn
    {
        refId,
        promoRefId,
        activityDesc,
        totalClaim,
        lastStatus,
        lastUpdate,
        materialNumber,
        notes,
        sp_principal,
        remarkSales,
        fpNumber,
        fpDate
    }

    public class FinDocumentCompletenessDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}
