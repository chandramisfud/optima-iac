using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class PromoHistoricalMovementLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<PromoHistoricalMovementData>? Data { get; set; }
    }

    public class PromoHistoricalMovementData
    {
        public string? RefId { get; set; }
        public string? ChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? LongDesc { get; set; }
        public string? ActivityDesc { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? TotSalesBefore { get; set; }
        public string? TotSalesAfter { get; set; }
        public string? TotInvestBefore { get; set; }
        public string? TotInvestAfter { get; set; }
        public string? StartPromoBefore { get; set; }
        public string? StartPromoAfter { get; set; }
        public string? EndPromoBefore { get; set; }
        public string? EndPromoAfter { get; set; }
    }

    public class PromoHistoricalMovementEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class PromoHistoricalMovementDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
}
