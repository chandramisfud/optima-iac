using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class ListingPromoReconRecordCount
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }

    public class ListingPromoReconLandingPage
    {
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
        public IList<ListingPromoReconData>? Data { get; set; }
    }

    public class ListingPromoReconData
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
        public string? RegionDesc { get; set; }
        public string? ChannelDesc { get; set; }
        public string? SubChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? LastStatus { get; set; }
        public string? ApprovalNotes { get; set; }
        public double Target { get; set; }
        public double Investment { get; set; }
        public double NormalSales { get; set; }
        public double IncrSales { get; set; }
        public double Roi { get; set; }
        public double CostRatio { get; set; }
        public double DnClaim { get; set; }
        public double RemainingBalance { get; set; }
        public double DnPaid { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public string? TsCoding { get; set; }
        public string? BrandDesc { get; set; }
        public string? SKUDesc { get; set; }
        public string? PromoPlanId { get; set; }
        public string? PromoPlanRefId { get; set; }
        public int id { get; set; }
        public string? Entity_after { get; set; }
        public string? Distributor_after { get; set; }
        public string? PrincipalDesc_after { get; set; }
        public string? BudgetSource_after { get; set; }
        public string? Initiator_after { get; set; }
        public DateTime CreateOn_after { get; set; }
        public DateTime LastUpdate_after { get; set; }
        public string? PromoNumber_after { get; set; }
        public string? SubCategory_after { get; set; }
        public string? Activity_after { get; set; }
        public string? SubActivity_after { get; set; }
        public string? ActivityDesc_after { get; set; }
        public string? Mechanisme1_after { get; set; }
        public string? Mechanisme2_after { get; set; }
        public string? Mechanisme3_after { get; set; }
        public string? Mechanisme4_after { get; set; }
        public string? RegionDesc_after { get; set; }
        public string? ChannelDesc_after { get; set; }
        public string? SubChannelDesc_after { get; set; }
        public string? AccountDesc_after { get; set; }
        public string? SubAccountDesc_after { get; set; }
        public string? LastStatus_after { get; set; }
        public string? ApprovalNotes_after { get; set; }
        public double Target_after { get; set; }
        public double Investment_after { get; set; }
        public double NormalSales_after { get; set; }
        public double IncrSales_after { get; set; }
        public double Roi_after { get; set; }
        public double CostRatio_after { get; set; }
        public double DnClaim_after { get; set; }
        public double RemainingBalance_after { get; set; }
        public double DnPaid_after { get; set; }
        public DateTime StartPromo_after { get; set; }
        public DateTime EndPromo_after { get; set; }
        public string? TsCoding_after { get; set; }
        public string? BrandDesc_after { get; set; }
        public string? SKUDesc_after { get; set; }
        public string? PromoPlanId_after { get; set; }
        public string? PromoPlanRefId_after { get; set; }
        public int id_after { get; set; }
        public string? actual_sales { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public int InvestmentTypeId_promo { get; set; }
        public string? InvestmentTypeRefId_promo { get; set; }
        public string? InvestmentTypeDesc_promo { get; set; }
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }
    }

    public class ListingPromoReconEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class ListingPromoReconDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
}
