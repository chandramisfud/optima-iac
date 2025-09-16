using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class PromoPlanningReportingRecordCount
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }

    public class PromoPlanningReportingLandingPage
    {
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
        public IList<PromoPlanningReportingData>? Data { get; set; }
    }

    public class PromoPlanningReportingData
    {
        public string? Periode { get; set; }
        public int PromoPlanId { get; set; }
        public string? PromoPlanRefId { get; set; }
        public int PromoId { get; set; }
        public string? PromoRefId { get; set; }
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
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
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public decimal NormalSales { get; set; }
        public decimal IncrSales { get; set; }
        public decimal Investment { get; set; }
        public decimal Roi { get; set; }
        public decimal CostRatio { get; set; }
        public string? CancelNotes { get; set; }
        public string? LastStatus { get; set; }
        public string? TsCode { get; set; }
        public string? BrandDesc { get; set; }
        public string? SKUDesc { get; set; }
        public double TotalSales { get; set; }
        public DateTime TSCodeOn { get; set; }
        public string? TSCodeBy { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public int InvestmentTypeId_promo { get; set; }
        public string? InvestmentTypeRefId_promo { get; set; }
        public string? InvestmentTypeDesc_promo { get; set; }
        // #140
        public string? GroupBrandDesc { get; set; }
    }

    public class PromoPlanningReportingEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class PromoPlanningReportingDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class PromoPlanningReportingChannelList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
}
