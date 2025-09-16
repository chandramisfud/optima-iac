using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.FinanceReport
{
    public class FinPromoPlanningReportingRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinPromoPlanningReportingSortColumn SortColumn { get; set; }
        public FinPromoPlanningReportingSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public string? profileId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int ChannelId { get; set; }
        public string? CreateFrom { get; set; }
        public string? CreateTo { get; set; }
        public string? StartFrom { get; set; }
        public string? StartTo { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinPromoPlanningReportingSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinPromoPlanningReportingSortColumn
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

    public class FinPromoPlanningReportingDistributorListParam
    {
        public int budgetId { get; set; }
        public int[]? entityId { get; set; }
    }

    public class ChannelPromoPlanningReportingParam
    {
        public string? userid { get; set; }
    }
}
