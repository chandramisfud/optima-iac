using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.FinanceReport
{
    public class FinMatrixApprovalListingRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinMatrixApprovalListingSortColumn SortColumn { get; set; }
        public FinMatrixApprovalListingSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int CategoryId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinMatrixApprovalListingSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinMatrixApprovalListingSortColumn
    {
        initiator,
        entity,
        distributor,
        subActivityType,
        channel,
        subChannel,
        minInvestment,
        maxInvestment,
        dnPaid,
        matrixApprover,
        modifiedOn,
        categoryId,
        categoryShortDesc,
        categoryLongDesc
    }

    public class FinMatrixApprovalListingDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}
