using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using V7.Model.UserAccess;

namespace V7.Model.Report
{
    public class InvestmentReportListRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public InvestmentReportSortColumn SortColumn { get; set; }
        public InvestmentReportSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InvestmentReportSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InvestmentReportSortColumn
    {
        entity,
        entityShortDesc,
        distributor,
        budgetAllocationName,
        activityType,
        isLastLayer,
        channel,
        budgetDeployed,
        promoCreated,
        dNClaimed,
        dnPaid,
        returnBalanceFromPromo,
        remainingBudget,
        gapBudgetDeployedvsPromoCreated,
        gapPromoCreatedvsDNClaimed,
        gapDNClaimedvsDNPaid
    }

    public class BudgetAllocationListRequestParam
    {
        public string? year { get; set; }
        public int entityId { get; set; }
        public int distributorId { get; set; }
        public int budgetParentId { get; set; }
        public int channelId { get; set; }
    }

    public class DistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}
