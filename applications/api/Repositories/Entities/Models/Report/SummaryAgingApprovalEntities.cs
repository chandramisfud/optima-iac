using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class SummaryAgingApprovalLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<SummaryAgingApprovalData>? Data { get; set; }
    }

    public class SummaryAgingApprovalData
    {
        public int line_index { get; set; }
        public string? user_name { get; set; }
        public string? user_id { get; set; }
        public string? promo_ref_id { get; set; }
        public int aging { get; set; }
        public double avgAging { get; set; }
    }

    public class SummaryAgingApprovalEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class SummaryAgingApprovalDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
}
