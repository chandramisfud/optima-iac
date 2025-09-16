namespace Repositories.Entities.Models
{
    public class DNDistributorEntity
    {
        public IList<EntityDropDownDto>? Entity { get; set; }
        public IList<MasterGlobalData>? Distributor { get; set; }
        public IList<MasterGlobalData>? Channel { get; set; }
        public IList<MasterGlobalData>? SubChannel { get; set; }
        public IList<MasterGlobalData>? Account { get; set; }
        public IList<MasterGlobalData>? SubAccount { get; set; }
    }
    public class EntityDropDownDto
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
    }
    public class MasterGlobalData
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
    }
    public class DNLandingPage
    {
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
        public IList<DNList>? Data { get; set; }
        // public DNRecordTotal RecordsTotal { get; set; }
    }
    public class DNRecord
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
    public class DNList
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
        public string? overBudgetStatus { get; set; }
    }
    public class DNRecordTotal
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
    public class DNCreationReturn
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
        public string? dnCategory { get; set; }

    }
    public class DNGetById
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
    public class DNAttachmentBody
    {
        public int DNId { get; set; }
        public string? DocLink { get; set; }
        public string? FileName { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
    }
    public class DNLandingPageBody
    {
        public string? periode { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public string? channel { get; set; }
        public string? account { get; set; }
        public string? userid { get; set; }
        public bool isdnmanual { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string? filter { get; set; }
        public string? txtsearch { get; set; }
    }
    public class DNGlobalResponse
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
    }
    public class DNCancelBody
    {
        public int dnid { get; set; }
        public string? reason { get; set; }
        public string? userid { get; set; }
    }
    public class DNApprovedPromoList
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

    public class GetApprovedPromoforDNBody
    {
        public string? periode { get; set; }
        public int entity { get; set; }
        public int channel { get; set; }
        public int account { get; set; }
        public string? userid { get; set; }
    }
    public class DNSellingPoint
    {
        public string? Id { get; set; }
        public string? RefId { get; set; }
        public int RegionId { get; set; }
        public string? RegionDesc { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
    }
    public class DNMaterial
    {
        public string? MaterialNumber { get; set; }
        public string? Description { get; set; }
        public string? WHT_Type { get; set; }
        public string? WHT_Code { get; set; }
        public string? Purpose { get; set; }
        public string? Entity { get; set; }
        public int EntityId { get; set; }
        public double PPNPct { get; set; }

    }
    public class DNCreationTaxLevel
    {
        public int id { get; set; }
        public string? taxLevel { get; set; }
        public string? Description { get; set; }
        public string? WHT_Type { get; set; }
        public string? WHT_Code { get; set; }
        public string? Purpose { get; set; }
        public string? EntityId { get; set; }
        public string? Entity { get; set; }
        public string? PPNPct { get; set; }
        public string? PPHPct { get; set; }

    }

    public class DNCreationParam
    {
        public bool IsDNPromo { get; set; }
        public int Id { get; set; }
        public string? Periode { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public string? ActivityDesc { get; set; }
        public int AccountId { get; set; }
        public int PromoId { get; set; }
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
        public IList<DNSellpoint>? sellpoint { get; set; }
        public IList<DNAttachment>? dnattachment { get; set; }
        public string? TaxLevel { get; set; }
        public double pphPct { get; set; }
        public double pphAmt { get; set; }
        public string? statusPPH { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public string? statusPPN { get; set; }
        public string? WHTType { get; set; }
    }

    public class DNPrint
    {
        public string? ProfitCenter { get; set; }
        public string? ProfitCenterDesc { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? Periode { get; set; }
        public int EntityId { get; set; }
        public string? AccountDesc { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? EntityLongDesc { get; set; }
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
        public string? Terbilang { get; set; }
        public decimal PPNPct { get; set; }
        public DateTime DeductionDate { get; set; }
        public string? UserId { get; set; }
        public string? BankName { get; set; }
        public string? BankCabang { get; set; }
        public string? NoRekening { get; set; }
        public string? ClaimManager { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public bool IsOverBudget { get; set; }
        public bool IsDNPromo { get; set; }
        public bool IsDNmanual { get; set; }
        public double PPNAmt { get; set; }
    }
    public class DNCreationSubAccountList
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class DNCreationEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class DNCreationChannelList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class DNStoreResultSearch
    {
        public int id { get; set; }
        public string? refid { get; set; }
        public int DistributorId { get; set; }
        public string? ActivityDesc { get; set; }
        public string? DPP { get; set; }
        public string? IntDocNo { get; set; }
        public int errorcode { get; set; }
        public string? messageout { get; set; }
        // added andrie Juli 13 2023 #882
        public string? entity { get; set; }
        public string? distributor { get; set; }
    }

    public class DNCreationGetWHTType
    {
        public string? WHTType { get; set; }
    }
}