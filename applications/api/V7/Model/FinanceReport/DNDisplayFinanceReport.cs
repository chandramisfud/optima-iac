using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.FinanceReport
{
    public class FinDNDisplayRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinDNDisplaySortColumn SortColumn { get; set; }
        public FinDNDisplaySortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public string? profileId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int ChannelId { get; set; }
        public int AccountId { get; set; }
        public string? IsDNManual { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinDNDisplaySortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinDNDisplaySortColumn
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

    public class FinDNDisplayDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }

}
