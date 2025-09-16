using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.FinanceReport
{
    public class FinPromoDisplayRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinPromoDisplaySortColumn SortColumn { get; set; }
        public FinPromoDisplaySortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public string? profileId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
        public bool CancelStatus { get; set; }

    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinPromoDisplaySortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinPromoDisplaySortColumn
    {
        Entity,
        Distributor,
        PrincipalDesc,
        RefId,
        ActivityDesc,
        Allocation
      
    }

    public class FinPromoDisplayDistributorListParam
    {
        public int budgetId { get; set; }
        public int[]? entityId { get; set; }
    }
}