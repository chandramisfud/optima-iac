using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.Report
{
    #pragma warning disable CS1591
    public class PromoHistoricalMovementRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public string? Period { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
    }

    public class PromoHistoricalMovementDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}