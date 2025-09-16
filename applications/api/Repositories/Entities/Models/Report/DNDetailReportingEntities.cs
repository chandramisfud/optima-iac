using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class DNDetailReportingRecordCount
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }

    public class DNDetailReportingLandingPage
    {
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
        public IList<DNDetailReportingData>? Data { get; set; }
    }

    public class DNDetailReportingData
    {
        public string? Category { get; set; }
        public string? dnurut { get; set; }
        public string? dnurut1 { get; set; }
        public string? Id { get; set; }
        public string? RefId { get; set; }
        public string? dnkontra { get; set; }
        public string? PromoId { get; set; }
        public string? EntityId { get; set; }
        public string? EntityDesc { get; set; }
        public string? DistributorId { get; set; }
        public string? DistributorDesc { get; set; }
        public string? PromoRefId { get; set; }
        public string? ActivityDesc { get; set; }
        public string? Account { get; set; }
        public string? SubAccount { get; set; }
        public string? Sellingpoint { get; set; }
        public string? ProfitCenter { get; set; }
        public double DPP { get; set; }
        public double PPNPct { get; set; }
        public double PPNAmt { get; set; }
        public double TotalClaim { get; set; }
        public double TotalPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? LastStatus { get; set; }
        public string? SuratPengantarCabang { get; set; }
        public string? SuratPengantarHO { get; set; }
        public string? InvoiceNo { get; set; }
        public string? ReceivedByDanoneOn { get; set; }
        public string? InvoiceNotifOn { get; set; }
        public string? Aging { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsOverBudget { get; set; }
        public string? TsCoding { get; set; }
        public string? InternalOrderNumber { get; set; }
        public int IsDNPromo { get; set; }
        public string? IntDocNo { get; set; }
        public string? MemDocNo { get; set; }
        public string? DistributorName { get; set; }
        public string? FeeDesc { get; set; }
        public double FeePct { get; set; }
        public double FeeAmount { get; set; }
        public string? MaterialNumber { get; set; }
        public string? TaxLevel { get; set; }
        public string? Notes { get; set; }
        public string? Initiator { get; set; }
        public string? received_by_danone_by { get; set; }
        public string? ReceivedByDanone { get; set; }
        public string? validate_by_finance_by { get; set; }
        public DateTime? validate_by_finance_on { get; set; }
        public string? validate_by_sales_by { get; set; }
        public DateTime? validate_by_sales_on { get; set; }
        public string? InvoiceNotifBy { get; set; }
        public string? InvoiceNotif { get; set; }
        public string? invoice_by { get; set; }
        public DateTime? invoice_on { get; set; }
        public string? confirm_paid_by { get; set; }
        public DateTime? confirm_paid_on { get; set; }
        public string? validate_by_finance { get; set; }
        public string? validate_by_sales { get; set; }
        public string? invoiced { get; set; }
        public string? confirm_paid { get; set; }
        public string? SalesValidationStatus { get; set; }
        public double PPHPct { get; set; }
        public double PPHAmt { get; set; }
        public DateTime? StatusSalesOn { get; set; }
        public string? StatusSalesNotes { get; set; }
        public DateTime? StatusSalesDistOn { get; set; }
        public string? validate_by_finance_by_username { get; set; }
        public string? FPNumber { get; set; }
        public DateTime? FPDate { get; set; }
        public int VATExpired { get; set; }
        public string? ponumber { get; set; }
        public string? channel { get; set; }
        public string? PIC { get; set; }
        public string? batchname { get; set; }
        public DateTime? send_to_danone_on { get; set; }
        public string? send_to_danone_by { get; set; }
        public string? mechanism { get; set; }
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public string? sku { get; set; }
        public string? promo_activity_name { get; set; }
        public string? ApprovalStatus { get; set; }
        public int isClose { get; set; }
        public string? remainingBalance { get; set; }
        public string? subActivityType { get; set; }
        public int categoryId { get; set; }
        public string? categoryShortDesc { get; set; }
        public string? categoryLongDesc { get; set; }
        public string? groupBrandDesc { get; set; }
        public string? dnCategory { get; set; }
        public string? overBudgetStatus { get; set; }
        //#141 Maintenance Support 2024 - Item 156
        public string? subActivityDesc { get; set; }
        //July25 - AND
        public string? whtType { get; set; }
    }


    public class DNDetailReportingEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class DNDetailReportingDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class DNDetailReportingSubAccountList
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
}
