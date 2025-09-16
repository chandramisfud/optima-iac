namespace Repositories.Entities.Models.DN
{
    public class DNValidationbyFinancebyId
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
        public string? CategoryShortDesc { get; set; }
        //BE #102
        public string? WHTType { get; set; }

        //#130
        public double totalClaim { get; set; }

    }
    public class DNValidationbyFinance
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
        public string? overBudgetStatus { get; set; }
        public int tickable { get; set; }
    }

    public class DNValidationbyFinanceWithTickable
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
        public string? overBudgetStatus { get; set; }
        public int tickable { get; set; }

        // added: Feb 20 by AND EP2 #126
        public string? statusPPH { get; set; }
        public string? WHTType { get; set; }

    }
    public class DNChangeStatusbyFinance
    {
        public string? UserId { get; set; }
        public string? status { get; set; }
        public IList<DNIdChangeStatusbyFinance>? dnid { get; set; }
    }

    public class DNIdChangeStatusbyFinance
    {
        public int dnid { get; set; }
    }

    public class PromobyIdforValidationbyFinanceRC
    {
        public PromoDisplayData? PromoHeader { get; set; }
        public List<PromoRegionRes>? Regions { get; set; }
        public List<PromoChannelRes>? Channels { get; set; }
        public List<PromoSubChannelRes>? SubChannels { get; set; }
        public List<PromoAccountRes>? Accounts { get; set; }
        public List<PromoSubAccountRes>? SubAccounts { get; set; }
        public List<PromoActivityRes>? Activities { get; set; }
        public List<PromoSubActivityRes>? SubActivities { get; set; }
        public IList<PromoBrandRes>? Brands { get; set; }
        public IList<PromoProductRes>? Skus { get; set; }
        public IList<PromoAttachment>? Attachments { get; set; }
        public IList<ApprovalRes>? ListApprovalStatus { get; set; }
        public IList<MechanismData>? Mechanism { get; set; }
        public IList<PromoReconInvestmentData>? Investments { get; set; }
        public IList<object>? GroupBrand { get; set; }

    }
    public class PromobyIdforValidationbyFinanceDC
    {
        public PromoDisplayData? PromoHeader { get; set; }
        public List<PromoRegionRes>? Regions { get; set; }
        public List<PromoChannelRes>? Channels { get; set; }
        public List<PromoSubChannelRes>? SubChannels { get; set; }
        public List<PromoAccountRes>? Accounts { get; set; }
        public List<PromoSubAccountRes>? SubAccounts { get; set; }
        public List<PromoActivityRes>? Activities { get; set; }
        public List<PromoSubActivityRes>? SubActivities { get; set; }
        public IList<PromoBrandRes>? Brands { get; set; }
        public IList<PromoProductRes>? Skus { get; set; }
        public IList<PromoAttachment>? Attachments { get; set; }
        public IList<ApprovalRes>? ListApprovalStatus { get; set; }
        public IList<MechanismData>? Mechanisms { get; set; }
        public IList<PromoReconInvestmentData>? Investments { get; set; }
        public IList<object>? GroupBrand { get; set; }

    }
    public class DNValidationbyFinanceParam
    {
        public int DNId { get; set; }
        public string? StatusCode { get; set; }
        public string? Notes { get; set; }
        public string? userid { get; set; }
        public string? TaxLevel { get; set; }
    }
    public class DNValidationbyFinanceEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class DNValidationbyFinanceDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class DNFilter
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
        //Added by andrie, July 22  2023
        public string? remarkSales { get; set; }
        //added by andrie July 27 2023
        public double Remaining { get; set; }
        public bool IsOverBudget { get; set; }
        public int EntityId { get; set; }
        public int PromoId { get; set; }
        public string? sp_ho { get; set; }
    }

    public class DNValidationCompletenessDto
    {
        public int DNId { get; set; }
        public string? StatusCode { get; set; }
        public string? Notes { get; set; }
        public string? userid { get; set; }
        public string? TaxLevel { get; set; }
        //Added by andrie, May 26 2023 #878
        public string? entityId { get; set; }
        public string? promoId { get; set; }
        public bool isDNPromo { get; set; }
        public bool IsWHTDeductable { get; set; }
        public DNDocCompleteness? DNDocCompletenessHeader { get; set; }
    }

    public class DNDocCompletenessforValidationbyFinance
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

}