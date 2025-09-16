using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.Report
{
    public class PromoPlanningReportingRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public PromoPlanningReportingSortColumn SortColumn { get; set; }
        public PromoPlanningReportingSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int ChannelId { get; set; }
        public string? CreateFrom { get; set; }
        public string? CreateTo { get; set; }
        public string? StartFrom { get; set; }
        public string? StartTo { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PromoPlanningReportingSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PromoPlanningReportingSortColumn
    {
        promoPlanRefId,
        initiator,
        tsCode,
        promoNumber,
        activityDesc,
        subAccountDesc,
        startPromo,
        endPromo,
        investment,
        costRatio,
        roi,
        lastStatus
    }

    public class PromoPlanningReportingDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}
