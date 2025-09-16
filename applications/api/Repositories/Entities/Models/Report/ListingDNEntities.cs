using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class ListingDNLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<ListingDNData>? Data { get; set; }
    }

    public class ListingDNData
    {
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
        public string? BudgetSource { get; set; }
        public string? Initiator { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? PromoNumber { get; set; }
        public string? SubCategory { get; set; }
        public string? SubActivity { get; set; }
        public string? Activity { get; set; }
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
        public double Target { get; set; }
        public double Investment { get; set; }
        public string? DNNumber { get; set; }
        public string? ActivityDescDN { get; set; }
        public string? LastStatusDN { get; set; }
        public double DPP { get; set; }
        public string? DNCreator { get; set; }
        public double DNClaim { get; set; }
        public string? SalesValidationStatus { get; set; }
    }

    public class ListingDNEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class ListingDNDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

}
