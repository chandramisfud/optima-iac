using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.Report
{
    public class DNDisplayRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public DNDisplaySortColumn SortColumn { get; set; }
        public DNDisplaySortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int ChannelId { get; set; }
        public int AccountId { get; set; }
        public string? IsDNManual { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DNDisplaySortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DNDisplaySortColumn
    {
        id,
        refId,
        promoRefId,
        activityDesc,
        dpp,
        ppnAmt,
        lastStatus,
        salesValidationStatus,
        salesValidationStatusOn,
        remarkSales,
        notes,
        validate_by_finance_on,
        validate_by_sales_on,
        confirm_paid_on,
        subAccount,
        fpNumber,
        fpDate
    }

    public class DNDisplayDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }
}
