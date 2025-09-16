namespace Repositories.Entities.Dtos
{
    public class BudgetUsage
    {
        public string? SubCategory { get; set; }
        public string? budget_deployed { get; set; }
        public string? budget_spending { get; set; }
        public string? promo_created { get; set; }
        public string? dn_claim { get; set; }
        public string? dn_paid { get; set; }
        public decimal promo_not_created_yet_pct { get; set; }
        public decimal promo_created_pct { get; set; }
        public int num_of_promo_created { get; set; }
        public int late_promo { get; set; }
        public int late_promo_pct { get; set; }
        public decimal ontime_promo { get; set; }
        public int ontime_promo_pct { get; set; }
        public decimal promo_approved { get; set; }
    }
    
    public class DashboardTrendDto
    {
        public int MonthNumber { get; set; }
        public string? period { get; set; }
        public decimal Late { get; set; }
        public decimal OnTime { get; set; }
    }
    public class GaugeChart
    {
        public string? param { get; set; }
        public decimal value { get; set; }
    }

   

    public class BarChart
    {
        public string? days { get; set; }
        public double SGM { get; set; }
        public double NIS { get; set; }
        public double NMN { get; set; }
    }


    public class DNMonitoringHeader
    {

        public string? total_dn_created { get; set; }
        public string? total_dn_paid { get; set; }
        public string? total_dn_remaining { get; set; }
        public string? total_dn_promo_created { get; set; }
        public string? total_dn_promo_paid { get; set; }
        public string? total_dn_promo_remaining { get; set; }
        public string? total_dn_non_promo_created { get; set; }
        public string? total_dn_non_promo_paid { get; set; }
        public string? total_dn_non_promo_remaining { get; set; }


    }

    public class FilterChannel
    {
        public int ChannelId { get; set; }
        public string? ChannelDesc { get; set; }
    }

    public class FilterAccount
    {
        public int AccountId { get; set; }
        public string? AccountDesc { get; set; }
    }

    public class MasterStatus
    {
        public string? StatusCode { get; set; }
        public string? StatusDesc { get; set; }
    }

    public class DNMonitoring
    {
        public IList<BarChart>? chart { get; set; }
        public DNMonitoringHeader? DN { get; set; }
        public IList<FilterChannel>? filter_channel { get; set; }
        public IList<FilterAccount>? filter_account { get; set; }
        public IList<MasterStatus>? filter_status { get; set; }
    }
   

    public class InvestmentNotifResult
    {
        public IList<InvesmentMotifResultAccrual>? accrual { get; set; }
        public IList<InvesmentMotifResultPromo>? promo { get; set; }
        public IList<InvesmentMotifResultDebetNote>? debetnote { get; set; }
        public IList<InvesmentMotifResultGAP>? GAP_list { get; set; }
    }

    public class InvestmentNotifBody
    {
        public string? periode { get; set; }
        public string? userid { get; set; }
    }

    public class InvesmentMotifResultPromo
    {
        public double DNClaim { get; set; }
        public double DNPaid { get; set; }
    }
    public class InvesmentMotifResultAccrual
    {
        public double DNClaim { get; set; }
        public double DNPaid { get; set; }
    }
    public class InvesmentMotifResultDebetNote
    {
        public double DNClaim { get; set; }
        public double DNPaid { get; set; }
    }
    public class InvesmentMotifResultGAP
    {
        public string? BudgetSource { get; set; }
        public string? PromoNumber { get; set; }
        public double DNClaim_promo { get; set; }
        public double DNPaid_promo { get; set; }
        public string? DNNumber { get; set; }
        public double DNPaid { get; set; }
        public double DNClaim { get; set; }
        public double GAP_DNClaim { get; set; }
        public double GAP_DNPaid { get; set; }
    }
}