using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.Report
{
    public class MatrixApprovalListingRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public MatrixApprovalListingSortColumn SortColumn { get; set; }
        public MatrixApprovalListingSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int CategoryId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MatrixApprovalListingSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MatrixApprovalListingSortColumn
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
        category
    }

    public class MatrixApprovalListingDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}
