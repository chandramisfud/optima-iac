using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using V7.Model.UserAccess;

namespace V7.Model.FinanceReport
{
    public class FinInvestmentReportListRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinInvestmentReportSortColumn SortColumn { get; set; }
        public FinInvestmentReportSortDirection SortDirection { get; set; }
        public string? profileId { get; set; }
        public string? Period { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinInvestmentReportSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinInvestmentReportSortColumn
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

    public class FinBudgetAllocationListRequestParam
    {
        public string? year { get; set; }
        public string? userid { get; set; }
        public int entityId { get; set; }
        public int distributorId { get; set; }
        public int budgetParentId { get; set; }
        public int channelId { get; set; }
    }

    public class FinDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}
