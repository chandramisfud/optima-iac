using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.Report
{
    #pragma warning disable CS1591
    public class SKPValidationRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public SKPValidationSortColumn SortColumn { get; set; }
        public SKPValidationSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
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
    public enum SKPValidationSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SKPValidationSortColumn
    {
        skpStatus,
        refId,
        lastStatus,
        activityDesc,
        tsCoding,
        allocation,
        investment
    }

    public class SKPValidationDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}
