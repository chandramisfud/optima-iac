using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class AccrualReportLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public object Data { get; set; }
    }

    public class AccrualReportData
    {
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
        public string? PrincipalDesc { get; set; }
        public string? BudgetSource { get; set; }
        public string? Initiator { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? LastUpdate { get; set; }
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
        public string? SubAccountDesc { get; set; }
        public string? LastStatus { get; set; }
        public string? ApprovalNotes { get; set; }
        public double Target { get; set; }
        public double Investment { get; set; }
        public double DNClaim { get; set; }
        public double DNPaid { get; set; }
        public DateTime? StartPromo { get; set; }
        public DateTime? EndPromo { get; set; }
        public double AccrueMTD { get; set; }
        public double AccrueYTD { get; set; }
        public string? TsCoding { get; set; }
    }

    public class AccrualEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
}
