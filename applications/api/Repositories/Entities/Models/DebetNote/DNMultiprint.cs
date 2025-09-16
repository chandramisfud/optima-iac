namespace Repositories.Entities.Models.DN
{
    public class SubAccountforDNMultiprint
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class DNMultiPrintLandingPage
    {
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
        public IList<DNMultiprintList>? Data { get; set; }
    }
    public class UserForDNMultiPrint
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class PromoforDN
    {
        public int PromoId { get; set; }
        public string? RefId { get; set; }
        public string? ActivityDesc { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public string? Allocation { get; set; }
        public decimal Investment { get; set; }
        public string? LastStatus { get; set; }
        public bool IsCancelLocked { get; set; }
        public string? TsCoding { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? CancelNotes { get; set; }
        public string? skp_flagging_status { get; set; }
        public int entityId { get; set; }
        public string? entityName { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public int skpstatus { get; set; }
        public string? skp_notes { get; set; }
        public string? CreateBy { get; set; }
        public double dnclaim { get; set; }
        public string? creationdate { get; set; }
        public string? laststatusdate { get; set; }
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }
    }
    public class DNMultiPrintRecordTotal
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
    public class DNMultiprintList
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int PromoId { get; set; }
        public string? PromoRefId { get; set; }
        public string? ActivityDesc { get; set; }
        public decimal DPP { get; set; }
        public decimal PPNAmt { get; set; }
        public decimal TotalClaim { get; set; }
        public decimal TotalPaid { get; set; }
        public string? LastStatus { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsFreeze { get; set; }
        public int IsOverBudget { get; set; }
        public bool IsDNManual { get; set; }
        public bool IsDNPromo { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? approver_userid { get; set; }
        public string? approver_username { get; set; }
        public string? approver_email { get; set; }
        public string? InternalOrderNumber { get; set; }
        public string? sp_ho { get; set; }
        public string? sp_principal { get; set; }
        public string? EntityId { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? MemDocNo { get; set; }
        public string? IntDocNo { get; set; }
        public string? TaxLevel { get; set; }
        public int docCount { get; set; }
        public string? Notes { get; set; }
        public string? MaterialNumber { get; set; }
        public double pphPct { get; set; }
        public double pphAmt { get; set; }
        public string? StatusSalesCode { get; set; }
        public string? StatusSales { get; set; }
        public DateTime StatusSalesOn { get; set; }
        public string? StatusSalesBy { get; set; }
        public DateTime StatusSalesDistOn { get; set; }
        public string? StatusSalesDistBy { get; set; }
        public string? StatusSalesNotes { get; set; }
        public string? remarkSales { get; set; }
        public string? SalesValidationStatus { get; set; }
        public DateTime SalesValidationStatusOn { get; set; }
        public int AccountId { get; set; }
        public string? Account { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? validate_by_finance_by { get; set; }
        public DateTime validate_by_finance_on { get; set; }
        public string? validate_by_sales_by { get; set; }
        public DateTime validate_by_sales_on { get; set; }
        public string? confirm_paid_by { get; set; }
        public DateTime confirm_paid_on { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public bool VATExpired { get; set; }
        public bool IsCancel { get; set; }
        public string? dnCategory { get; set; }
        public string? overBudgetStatus {get; set;}
    }
}