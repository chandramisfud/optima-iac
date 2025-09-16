using Repositories.Entities.Models.DN;

namespace Repositories.Entities.Models
{
    public class DebetNoteReport
    {
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
        public string? PrincipalDesc { get; set; }
        public string? BudgetSource { get; set; }
        public string? Initiator { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? PromoNumber { get; set; }
        public string? SubCategory { get; set; }
        public string? Activity { get; set; }
        public string? SubActivity { get; set; }
        public string? ActivityDesc { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public string? ChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public string? LastStatus { get; set; }
        public string? ApprovalNotes { get; set; }
        public decimal Target { get; set; }
        public decimal Investment { get; set; }
        public string? DnNumber { get; set; }
        public string? activitydescdn { get; set; }
        public string? laststatusdn { get; set; }
        public double dpp { get; set; }
        public string? DNCreator { get; set; }
        public decimal DnClaim { get; set; }
        public string? SalesValidationStatus { get; set; }
    }

    public class DNReportDto
    {
        public string? category { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? dnkontra { get; set; }
        public int PromoId { get; set; }
        public int EntityId { get; set; }
        public string? EntityDesc { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorDesc { get; set; }
        public string? PromoRefId { get; set; }
        public string? ActivityDesc { get; set; }
        public string? Account { get; set; }
        public string? SubAccount { get; set; }
        public string? Sellingpoint { get; set; }
        public string? ProfitCenter { get; set; }
        public decimal DPP { get; set; }
        public decimal PPNPct { get; set; }
        public decimal PPNAmt { get; set; }
        public decimal TotalClaim { get; set; }
        public decimal TotalPaid { get; set; }
        public string? PaymentDate { get; set; }
        public string? LastStatus { get; set; }
        public string? SuratPengantarCabang { get; set; }
        public string? SuratPengantarHO { get; set; }
        public string? InvoiceNo { get; set; }
        public string? ReceivedByDanoneOn { get; set; }
        public string? InvoiceNotifOn { get; set; }
        public int Aging { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsOverBudget { get; set; }
        public string? TsCoding { get; set; }
        public string? InternalOrderNumber { get; set; }
        public bool? IsDNPromo { get; set; }
        public string? IntDocNo { get; set; }
        public string? MemDocNo { get; set; }
        public string? DistributorName { get; set; }
        public string? FeeDesc { get; set; }
        public decimal FeePct { get; set; }
        public decimal FeeAmount { get; set; }
        public string? TaxLevel { get; set; }
        public string? MaterialNumber { get; set; }
        public string? Notes { get; set; }
        public string? initiator { get; set; }
        public string? received_by_danone_by { get; set; }
        public string? ReceivedByDanone { get; set; }
        public string? validate_by_finance { get; set; }
        public string? validate_by_finance_by { get; set; }
        public DateTime validate_by_finance_on { get; set; }
        public string? validate_by_sales_by { get; set; }
        public DateTime validate_by_sales_on { get; set; }
        public string? InvoiceNotifBy { get; set; }
        public string? InvoiceNotif { get; set; }
        public string? invoice_by { get; set; }
        public DateTime invoice_on { get; set; }
        public string? confirm_paid_by { get; set; }
        public DateTime confirm_paid_on { get; set; }
        public string? validate_by_sales { get; set; }
        public string? invoiced { get; set; }
        public string? confirm_paid { get; set; }
        public string? SalesValidationStatus { get; set; }
        public double PPHPct { get; set; }
        public double PPHAmt { get; set; }
        public string? StatusSalesCode { get; set; }
        public DateTime StatusSalesOn { get; set; }
        public string? StatusSalesNotes { get; set; }
        public DateTime StatusSalesDistOn { get; set; }
        public string? validate_by_finance_by_username { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public bool VATExpired { get; set; }
        public string? ponumber { get; set; }
        public string? channel { get; set; }
        public string? PIC { get; set; }
        public string? batchname { get; set; }
        public string? send_to_danone_on { get; set; }
        public string? send_to_danone_by { get; set; }
        // modified by andRie, May 8 2023, #869
        public string? mechanism { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public string? sku { get; set; }
        public string? promo_activity_name { get; set; }
        public string? ApprovalStatus { get; set; }

    }
    public class DNReport_p
    {
        public IList<DNReportDto>? Data { get; set; }
        public DNRecordTotal? RecordsTotal { get; set; }
    }




    public class DNSellpoint
    {
        public bool flag { get; set; }
        public string? sellpoint { get; set; }
        public string? LongDesc { get; set; }
    }
    public class DNAttachment
    {
        public string? DocLink { get; set; }
        public string? FileName { get; set; }
    }
    public class DNDocCompleteness
    {
        public int DNId { get; set; }
        public bool Original_Invoice_from_retailers { get; set; }
        public bool Tax_Invoice { get; set; }
        public bool Promotion_Agreement_Letter { get; set; }
        public bool Trading_Term { get; set; }
        public bool Sales_Data { get; set; }
        public bool Copy_of_Mailer { get; set; }
        public bool Copy_of_Photo_Doc { get; set; }
        public bool List_of_Transfer { get; set; }
    }

    public class DebetNoteSuratPengantarDto
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorDesc { get; set; }
        public int EntityId { get; set; }
        public string? EntityDesc { get; set; }
        public string? UserId { get; set; }
        public string? CreateOn { get; set; }
        public IList<DetailDNDto>? DnId { get; set; }
    }

    public class DetailDNDto
    {
        public string? PromoNumber { get; set; }
        public string? DNNumber { get; set; }
        public string? MemDocNo { get; set; }
        public string? AccountDesc { get; set; }
        public string? ActivityDesc { get; set; }
        public decimal TotalClaim { get; set; }
    }
    public class UpdateDNAttachment
    {
        public int DNId { get; set; }
        public string? userid { get; set; }
        public IList<DNAttachment>? Attachment { get; set; }
    }
    public class DebetNoteAssignDto
    {
        public int DNId { get; set; }
        public int PromoId { get; set; }
        public string? UserId { get; set; }
    }


    public class DNValidationDto
    {
        public int DNId { get; set; }
        public string? StatusCode { get; set; }
        public string? Notes { get; set; }
        public string? userid { get; set; }
        public string? TaxLevel { get; set; }
    }
    public class DNValidationCompletenessDto
    {
        public int DNId { get; set; }
        public string? StatusCode { get; set; }
        public string? Notes { get; set; }
        public string? userid { get; set; }
        public string? TaxLevel { get; set; }
        public DNDocCompleteness? DNDocCompletenessHeader { get; set; }
    }
    public class DNTandaTerimaDto
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int DistributorId { get; set; }
        public int EntityId { get; set; }
        public string? UserId { get; set; }
        public IList<DNId>? DnId { get; set; }
    }


    public class DebetNoteFilterDto
    {
        public bool CheckFlag { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? PromoRefId { get; set; }
        public string? ActivityDesc { get; set; }
        public decimal DPP { get; set; }
        public decimal PPNAmt { get; set; }
        public string? memDocNo { get; set; }
        public string? intDocNo { get; set; }
        public decimal TotalClaim { get; set; }
        public string? LastStatus { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? Status { get; set; }
        public string? TaxLevel { get; set; }
        public string? MaterialNumber { get; set; }
        public string? StatusSalesNotes { get; set; }
        public string? SalesValidationStatus { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public bool VATExpired { get; set; }
        public bool DNVATExpired { get; set; }
    }

    public class DNListByPromoIdBody
    {
        public int promoid { get; set; }
    }
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
        public int categoryId { get; set; }
        public string? categoryShortDesc { get; set; }
        public string? categoryLongDesc { get; set; }
    }
    public class ToBeSettledBody
    {
        public string? userid { get; set; }
    }
    public class DNLogParentDto
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int PromoId { get; set; }
        public string? PromoRefId { get; set; }
        public string? ActivityDesc { get; set; }
        public double dpp { get; set; }
        public double PPNAmt { get; set; }
        public double TotalClaim { get; set; }
        public double TotalPaid { get; set; }
        public string? LastStatus { get; set; }
        public bool IsFreeze { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public bool IsOverBudget { get; set; }
        public string? AssignBy { get; set; }
        public string? LastUpdate { get; set; }
        public int EntityId { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? MemDocNo { get; set; }
        public string? IntDocNo { get; set; }
        public string? MaterialNumber { get; set; }
        public string? TaxLevel { get; set; }
        public bool IsDNPromo { get; set; }
        public int docCount { get; set; }
        public string? remarkSales { get; set; }
        public string? SalesValidationStatus { get; set; }
        public DateTime SalesValidationStatusOn { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public string? validate_by_finance_by { get; set; }
        public DateTime validate_by_finance_on { get; set; }
        public string? validate_by_sales_by { get; set; }
        public DateTime validate_by_sales_on { get; set; }
        public string? confirm_paid_by { get; set; }
        public DateTime confirm_paid_on { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public string? VATExpired { get; set; }
    }

    public class DNParentBody
    {
        public string? periode { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public int channel { get; set; }
        public int account { get; set; }
        public string? userid { get; set; }
        public bool isdnmanual { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string? filter { get; set; }
        public string? txtSearch { get; set; }
    }


    public class DNLogDetailBody
    {
        public int id { get; set; }
    }

    public class DNLogDetail
    {
        public int id { get; set; }
        public string? RefId { get; set; }
        public string? ActivityDesc { get; set; }
        public string? MemDocNo { get; set; }
        public string? IntDocNo { get; set; }
        public double TotalClaim { get; set; }
        public string? TaxLevel { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
    }
    public class DNParentLogPagination
    {
        public IList<DNLogParentDto>? Data { get; set; }
        public DNRecordTotal? RecordsTotal { get; set; }
    }
    public class InvoiceVoid
    {
        public int id { get; set; }
        public string? reason { get; set; }
        public string? userid { get; set; }
    }
    public class InvoiceVoidListReport
    {
        public int Id { get; set; }
        public string? invoice_number { get; set; }
        public int DNId { get; set; }
        public string? dn_number { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? username { get; set; }
        public DateTime invoicedate { get; set; }
    }

    public class InvoiceVoidListReportBody
    {
        public DateTime void_from { get; set; }
        public DateTime void_to { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public string? userid { get; set; }
    }


    public class DebetNoteResult
    {
        public int Id { get; set; }
        public bool IsDNManual { get; set; }
        public string? Periode { get; set; }
        public string? RefId { get; set; }
        public string? PromoRefId { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public int EntityId { get; set; }
        public string? AccountDesc { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public string? DistributorShortDesc { get; set; }
        public string? DistributorLongDesc { get; set; }
        public string? AreaCode { get; set; }
        public string? DNNumber { get; set; }
        public string? ActivityDesc { get; set; }
        public int SubChannelId { get; set; }
        public int AccountId { get; set; }
        public int PromoId { get; set; }
        public string? IntDocNo { get; set; }
        public string? MemDocNo { get; set; }
        public double DPP { get; set; }
        public double PPNPct { get; set; }
        public double PPNAmt { get; set; }
        public double TotalClaim { get; set; }
        public DateTime DeductionDate { get; set; }
        public string? Notes { get; set; }
        public string? LastStatus { get; set; }
        public bool IsPaid { get; set; }
        public double TotalPaid { get; set; }
        public bool IsCancel { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreateOn { get; set; }
        public string? UserId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsDelete { get; set; }
        public DateTime DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public DateTime AssignOn { get; set; }
        public string? AssignBy { get; set; }
        public bool IsFreeze { get; set; }
        public string? approver_userid { get; set; }
        public string? approver_username { get; set; }
        public string? approver_email { get; set; }
        public string? InternalOrderNumber { get; set; }
        public double DNAmount { get; set; }
        public string? FeeDesc { get; set; }
        public double FeePct { get; set; }
        public double FeeAmount { get; set; }
        public string? TaxLevel { get; set; }
        public bool isDNPromo { get; set; }
        public double PPHPct { get; set; }
        public double PPHAmt { get; set; }
        public string? statusPPH { get; set; }
        public string? StatusSalesNotes { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public int ChannelId { get; set; }
        public string? statusPPN { get; set; }
        public bool VATExpired { get; set; }

        //#835
        public string? WHTType { get; set; }
        //#130 already exist
        //public double totalClaim { get; set; }
    }
    public class SellingPointResult
    {
        public int flag { get; set; }
        public string? sellpoint { get; set; }
        public string? LongDesc { get; set; }
    }

    public class FileAttachResult
    {
        public int DNId { get; set; }
        public string? FileName { get; set; }
        public string? Doclink { get; set; }
    }

    public class DocumentCompletenessResult
    {
        public bool Original_Invoice_from_retailers { get; set; }
        public bool Tax_Invoice { get; set; }
        public bool Promotion_Agreement_Letter { get; set; }
        public bool Trading_Term { get; set; }
        public bool Sales_Data { get; set; }
        public bool Copy_of_Mailer { get; set; }
        public bool Copy_of_Photo_Doc { get; set; }
        public bool List_of_Transfer { get; set; }
    }
}