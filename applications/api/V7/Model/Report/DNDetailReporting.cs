using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.Report
{
    public class DNReadyToPayParam
    {
        public string? period { get; set; }
        public int category { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
     
        public int subAccount { get; set; }
        public string userId { get; set; }
        public string? search { get; set; }
 
  
        public int pageNumber { get; set; } = 0;
      
        public int pageSize { get; set; } = 10;
      
    
    }

    public class DNDetailReportingListRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public DNDetailReportingSortColumn SortColumn { get; set; }
        public DNDetailReportingSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int CategoryId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int ChannelId { get; set; }
        public int AccountId { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DNDetailReportingSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DNDetailReportingSortColumn
    {
        Category,
        dnurut,
        dnurut1,
        Id,
        RefId,
        dnkontra,
        PromoId,
        EntityId,
        EntityDesc,
        DistributorId,
        DistributorDesc,
        PromoRefId,
        ActivityDesc,
        Account,
        SubAccount,
        Sellingpoint,
        ProfitCenter,
        DPP,
        PPNPct,
        PPNAmt,
        TotalClaim,
        TotalPaid,
        PaymentDate,
        LastStatus,
        SuratPengantarCabang,
        SuratPengantarHO,
        InvoiceNo,
        ReceivedByDanoneOn,
        InvoiceNotifOn,
        Aging,
        CreateOn,
        CreateBy,
        LastUpdate,
        ModifiedBy,
        IsOverBudget,
        TsCoding,
        InternalOrderNumber,
        IsDNPromo,
        IntDocNo,
        MemDocNo,
        DistributorName,
        FeeDesc,
        FeePct,
        FeeAmount,
        MaterialNumber,
        TaxLevel,
        Notes,
        Initiator,
        received_by_danone_by,
        ReceivedByDanone,
        validate_by_finance_by,
        validate_by_finance_on,
        validate_by_sales_by,
        validate_by_sales_on,
        InvoiceNotifBy,
        InvoiceNotif,
        invoice_by,
        invoice_on,
        confirm_paid_by,
        confirm_paid_on,
        validate_by_finance,
        validate_by_sales,
        invoiced,
        confirm_paid,
        SalesValidationStatus,
        PPHPct,
        PPHAmt,
        StatusSalesOn,
        StatusSalesNotes,
        StatusSalesDistOn,
        validate_by_finance_by_username,
        FPNumber,
        FPDate,
        VATExpired,
        ponumber,
        channel,
        PIC,
        batchname,
        send_to_danone_on,
        send_to_danone_by,
        mechanism,
        StartPromo,
        EndPromo,
        sku,
        promo_activity_name,
        ApprovalStatus,
        isClose,
        remainingBalance,
        subActivityType,
        categoryId,
        categoryShortDesc,
        categoryLongDesc,
        groupBrandDesc,
        dnCategory,
        overBudgetStatus
    }

    public class DNDetailReportingDistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }

}
