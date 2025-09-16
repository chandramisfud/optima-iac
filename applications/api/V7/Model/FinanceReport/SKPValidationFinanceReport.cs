using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.FinanceReport
{
    public class FinSKPValidationRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinSKPValidationSortColumn SortColumn { get; set; }
        public FinSKPValidationSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public string? profileId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
        public int CancelStatus { get; set; }
        public string? StartFrom { get; set; }
        public string? StartTo { get; set; }
        public int SubmissionParam { get; set; }
        public int Status { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinSKPValidationSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinSKPValidationSortColumn
    {
        skpStatus,
        refId,
        lastStatus,
        activityDesc,
        tsCoding,
        allocation,
        investment
    }

    public class FinSKPValidationDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }

    public class ChannelSKPValidationParam
    {
        public string? userid { get; set; }

    }
}
