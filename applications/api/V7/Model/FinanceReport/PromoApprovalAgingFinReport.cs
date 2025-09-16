using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.FinanceReport
{
    public class FinPromoApprovalAgingRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinPromoApprovalAgingSortColumn SortColumn { get; set; }
        public FinPromoApprovalAgingSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public string? profileId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }

    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinPromoApprovalAgingSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinPromoApprovalAgingSortColumn
    {
        PromoNo,
        InitiatorName,
        ChannelDesc,
        Activity,
        SubActivity,
        PromoPeriode,
        ActivityDesc,
        LastStatus,
        Approver
      
    }

    public class FinPromoApprovalAgingDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}