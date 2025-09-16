using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.Report
{
    public class ListingDNRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public ListingDNSortColumn SortColumn { get; set; }
        public ListingDNSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ListingDNSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ListingDNSortColumn
    {
        entity,
        distributor,
        budgetSource,
        initiator,
        createOn,
        lastUpdate,
        promoNumber,
        subCategory,
        subActivity,
        activity,
        activityDesc,
        mechanisme1,
        mechanisme2,
        mechanisme3,
        mechanisme4,
        channelDesc,
        accountDesc,
        startPromo,
        endPromo,
        lastStatus,
        approvalNotes,
        dnNumber,
        activityDescDN,
        lastStatusDN,
        dpp,
        dnCreator,
        dnClaim,
        salesValidationStatus
    }

    public class ListingDNDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}
