using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class ListingPromoReportingRecordCount
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }

    public class ListingPromoReportingLandingPage
    {
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
        public IList<ListingPromoReportingData>? Data { get; set; }
    }

    public class ListingPromoReportingData
    {
        public string? Entity { get; set; }
        public int PromoPlanId { get; set; }
        public string? PromoPlanRefId { get; set; }
        public string? Distributor { get; set; }
        public string? PrincipalDesc { get; set; }
        public string? BudgetSource { get; set; }
        public string? Initiator { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string? PromoNumber { get; set; }
        public string? SubCategory { get; set; }
        public int categoryId { get; set; }
        public string? categoryShortDesc { get; set; }
        public string? categoryLongDesc { get; set; }
        public string? Activity { get; set; }
        public string? SubActivity { get; set; }
        public string? ActivityDesc { get; set; }
        public string? mechanism { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public string? RegionDesc { get; set; }
        public string? ChannelDesc { get; set; }
        public string? SubChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public int Gap { get; set; }
        public string? submission_deadline { get; set; }
        public bool OnTime { get; set; }
        public string? LastStatus { get; set; }
        public DateTime? LastStatusDate { get; set; }
        public string? ApprovalNotes { get; set; }
        public decimal Target { get; set; }
        public decimal Investment { get; set; }
        public double NormalSales { get; set; }
        public double IncrSales { get; set; }
        public double Roi { get; set; }
        public double CostRatio { get; set; }
        public decimal DnClaim { get; set; }
        public decimal RemainingBalance { get; set; }
        public decimal DnPaid { get; set; }
        public decimal AccrueMTD { get; set; }
        public decimal AccrueYTD { get; set; }
        public string? TsCoding { get; set; }
        public string? BrandDesc { get; set; }
        public string? SKUDesc { get; set; }
        public bool ClosureStatus { get; set; }
        public string? ReconStatus { get; set; }
        public string? LastReconStatus { get; set; }
        public int SubactivityTypeId { get; set; }
        public string? SubactivityTypeRefId { get; set; }
        public string? SubactivityType { get; set; }
        public string? CancelReason { get; set; }
        public double actual_sales { get; set; }
        public string? initiator_notes { get; set; }
        public string? sendback_notes { get; set; }
        public string? sendback_notes_date { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public int InvestmentTypeId_promo { get; set; }
        public string? InvestmentTypeRefId_promo { get; set; }
        public string? InvestmentTypeDesc_promo { get; set; }
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }
        public string? groupBrandDesc { get; set; }
        // UAT Feedback 11 Oct 24
        public string? budgetApprovalStatus { get; set; }
        public string? budgetApprovalStatusOn { get; set; }
        public string? budgetApprovalStatusBy { get; set; }
        public string? budgetDeployStatus { get; set; }
        public string? budgetDeployStatusOn { get; set; }
        public string? budgetDeployStatusBy { get; set; }
        // UAT Feedback 18 Oct 24
        public string? mainActivity { get; set; }
        public string? batchId { get; set; }
    }

    public class ListingPromoReportingEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class ListingPromoReportingDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class ListingPromoReportingChannelList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
}
