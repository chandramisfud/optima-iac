using Repositories.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class DNDisplayRecordCount
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }

    public class DNDisplayLandingPage
    {
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
        public IList<DNDisplayData>? Data { get; set; }
    }

    public class DNDisplayData
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
        public DateTime? LastUpdate { get; set; }
        public bool IsFreeze { get; set; }
        public int IsOverBudget { get; set; }
        public bool IsDNManual { get; set; }
        public bool IsDNPromo { get; set; }
        public DateTime? CreateOn { get; set; }
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
        public DateTime? StatusSalesOn { get; set; }
        public string? StatusSalesBy { get; set; }
        public DateTime StatusSalesDistOn { get; set; }
        public string? StatusSalesDistBy { get; set; }
        public string? StatusSalesNotes { get; set; }
        public string? remarkSales { get; set; }
        public string? SalesValidationStatus { get; set; }
        public DateTime? SalesValidationStatusOn { get; set; }
        // Added: #865 March 14 23 by AND 
        public int AccountId { get; set; }
        public string? Account { get; set; }

        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? validate_by_finance_by { get; set; }
        public DateTime? validate_by_finance_on { get; set; }
        public string? validate_by_sales_by { get; set; }
        public DateTime? validate_by_sales_on { get; set; }
        public string? confirm_paid_by { get; set; }
        public DateTime? confirm_paid_on { get; set; }
        public string? FPNumber { get; set; }
        public DateTime? FPDate { get; set; }
        public bool VATExpired { get; set; }
        //Added, AND March 17 2023, #867
        public bool IsCancel { get; set; }
        public string? dnCategory { get; set; }
        public string? overBudgetStatus { get; set; }
    }

    public class DNDisplayDataById
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
        public IList<DNDisplaySellpoint>? sellPoint { get; set; }
        public IList<DNDisplayAttachment>? dnAttachment { get; set; }
        public IList<DNDisplayDocCompleteness>? dnDocCompletenessHeader { get; set; }
    }

    public class DNDisplaySellpoint
    {
        public bool flag { get; set; }
        public string? sellpoint { get; set; }
        public string? LongDesc { get; set; }
    }
    public class DNDisplayAttachment
    {
        public string? DocLink { get; set; }
        public string? FileName { get; set; }
    }
    public class DNDisplayDocCompleteness
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

    public class DNDisplayEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class DNDisplayDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class DNDisplayTaxLevelList
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

    public class DNDisplaySellingPointList
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

    public class DNDisplayPrint
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
}
