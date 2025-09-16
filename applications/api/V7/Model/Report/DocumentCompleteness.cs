using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.Report
{
    public class DocumentCompletenessRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public DocumentCompletenessSortColumn SortColumn { get; set; }
        public DocumentCompletenessSortDirection SortDirection { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public string? TaxLevel { get; set; }
        public string? Status { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DocumentCompletenessSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DocumentCompletenessSortColumn
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

    public class DocumentCompletenessDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }

    public class DNOutStandingLPParam : LPParam
    {
        public string period { get; set; }

        public int[] distributorId { get; set; }

    }
}
