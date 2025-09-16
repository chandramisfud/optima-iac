namespace Repositories.Entities.Models.DN
{
    // debetnoteassign/list/
    public class DNReassignmentList
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
    }
    // promo/getPromoAprrovalForDnByInitiator/
    public class ApprovalPromoforDNbyInitiator
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
    // sellingpoint/all
    public class SellingPointforDNReassignment
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    // debetnote/dnlistbypromoid
    public class DNListbyPromoIdDto
    {

        public int Id { get; set; }
        public string? RefId { get; set; }
        public int PromoId { get; set; }
        public string? PromoRefId { get; set; }
        public double dpp { get; set; }
        public double PPNAmt { get; set; }
        public double TotalClaim { get; set; }
        public double TotalPaid { get; set; }
        public string? ActivityDesc { get; set; }
        public string? LastStatus { get; set; }
        public double NormalSales { get; set; }
        public bool IsFreeze { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public bool IsOverBudget { get; set; }
        public string? AssignBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public int EntityId { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? MemDocNo { get; set; }
        public string? IntDocNo { get; set; }
        public string? MaterialNumber { get; set; }
        public string? TaxLevel { get; set; }
        public bool IsDNPromo { get; set; }
        public string? remarkSales { get; set; }
        public string? SalesValidationStatus { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public bool VATExpired { get; set; }

    }
    // debetnote/overbudget/tobesettled
    public class DNOverBudgetSettledDto
    {

        public int id { get; set; }
        public string? RefId { get; set; }
        public string? ActivityDesc { get; set; }
        public string? LastStatus { get; set; }
        public double NormalSales { get; set; }
        public double IncrSales { get; set; }
        public double Investment { get; set; }
        public double CostRatio { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public double Remaining { get; set; }
        public double dpp { get; set; }
        public string? SubAccountDesc { get; set; }

    }
    // debetnote/getbyId/
    public class DNGetByIdforReassignment
    {
        public bool IsDNPromo { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? Periode { get; set; }
        public int EntityId { get; set; }
        public string? AccountDesc { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorShortDesc { get; set; }
        public string? DistributorLongDesc { get; set; }
        public string? ActivityDesc { get; set; }
        public int AccountId { get; set; }
        public int PromoId { get; set; }
        public string? PromoRefId { get; set; }
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public string? IntDocNo { get; set; }
        public string? MemDocNo { get; set; }
        public decimal DPP { get; set; }
        public double DNAmount { get; set; }
        public string? FeeDesc { get; set; }
        public double FeePct { get; set; }
        public double FeeAmount { get; set; }
        public decimal PPNPct { get; set; }
        public DateTime DeductionDate { get; set; }
        public string? UserId { get; set; }
        public bool IsFreeze { get; set; }
        public string? approver_userid { get; set; }
        public string? approver_username { get; set; }
        public string? approver_email { get; set; }
        public IList<DNSellpoint>? sellpoint { get; set; }
        public IList<DNAttachment>? dnattachment { get; set; }
        public IList<DNDocCompleteness>? DNDocCompletenessHeader { get; set; }
        public string? InternalOrderNumber { get; set; }
        public string? TaxLevel { get; set; }
        public string? Notes { get; set; }
        public string? LastStatus { get; set; }
        public double pphPct { get; set; }
        public double pphAmt { get; set; }
        public string? statusPPH { get; set; }
        public string? StatusSalesNotes { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public int ChannelId { get; set; }
        public double PPNAmt { get; set; }
        public string? statusPPN { get; set; }
        //#835
        public string? DNCreator { get; set; }
        public string? WHTType { get; set; }

        //#130
        public double totalClaim { get; set; }

    }
    // debetnote/store
    public class DNReassignmentCreationParam
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
        public double Remaining { get; set; }
    }
    public class CreateDNReassignmentParam
    {
        public bool IsDNPromo { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? Periode { get; set; }
        public int EntityId { get; set; }
        public string? AccountDesc { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorShortDesc { get; set; }
        public string? DistributorLongDesc { get; set; }
        public string? ActivityDesc { get; set; }
        public int AccountId { get; set; }
        public int PromoId { get; set; }
        public string? PromoRefId { get; set; }
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public string? IntDocNo { get; set; }
        public string? MemDocNo { get; set; }
        public decimal DPP { get; set; }
        public double DNAmount { get; set; }
        public string? FeeDesc { get; set; }
        public double FeePct { get; set; }
        public double FeeAmount { get; set; }
        public decimal PPNPct { get; set; }
        public DateTime DeductionDate { get; set; }
        public string? UserId { get; set; }
        public bool IsFreeze { get; set; }
        public string? approver_userid { get; set; }
        public string? approver_username { get; set; }
        public string? approver_email { get; set; }
        public IList<DNSellpoint>? sellpoint { get; set; }
        public IList<DNAttachment>? dnattachment { get; set; }
        public IList<DNDocCompleteness>? DNDocCompletenessHeader { get; set; }
        public string? InternalOrderNumber { get; set; }
        public string? TaxLevel { get; set; }
        public string? Notes { get; set; }
        public string? LastStatus { get; set; }
        public double pphPct { get; set; }
        public double pphAmt { get; set; }
        public string? statusPPH { get; set; }
        public string? StatusSalesNotes { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public int ChannelId { get; set; }
        public double PPNAmt { get; set; }
        public string? statusPPN { get; set; }
        //#835
        public string? DNCreator { get; set; }

    }
    // debetnote/assigndn
    public class DNAssignParam
    {
        public int DNId { get; set; }
        public int PromoId { get; set; }
        public string? UserId { get; set; }
    }
    // distributor/delete
    public class DistributorDeleteforDNReassignment
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedEmail { get; set; }

    }
    public class DistributorDeleteReturnforDNReassignment
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }
    // promoattachment/delete
}