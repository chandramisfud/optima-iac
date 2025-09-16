namespace Repositories.Entities.Models.DNConfirmPaid
{
    public class DNConfirmPaid
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
        public bool IsOverBudget { get; set; }
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
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? validate_by_finance_by { get; set; }
        public DateTime validate_by_finance_on { get; set; }
        public string? validate_by_sales_by { get; set; }
        public DateTime validate_by_sales_on { get; set; }
        public string? confirm_paid_by { get; set; }
        public DateTime confirm_paid_on { get; set; }
        public int AccountId { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public bool VATExpired { get; set; }
        public bool DNVATExpired { get; set; }
        public bool Original_Invoice_from_retailers { get; set; }
        public bool Tax_Invoice { get; set; }
        public bool Promotion_Agreement_Letter { get; set; }
        public bool Trading_Term { get; set; }
        public bool Sales_Data { get; set; }
        public bool Copy_of_Mailer { get; set; }
        public bool Copy_of_Photo_Doc { get; set; }
        public bool List_of_Transfer { get; set; }
        public string? validate_by_finance_username { get; set; }
        public string? FinanceValidationStatus { get; set; }
        public string? FinanceValidationStatusOn { get; set; }
        // added: March 22 by AND #868
        public double Remaining { get; set; }
        public string? dnCategory { get; set; }

    }
    public class DNChangeStatusConfirmPaid
    {
        public string? UserId { get; set; }
        public string? status { get; set; }
        public DateTime? paymentDate { get; set; }
        public IList<DNIdArray>? DNId { get; set; }
    }

    public class DNIdArray
    {
        public int DNId { get; set; }
    }
}