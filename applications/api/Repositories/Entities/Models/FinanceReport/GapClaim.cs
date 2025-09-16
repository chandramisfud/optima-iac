namespace Repositories.Entities.Models
{
    public class AccrualReportHeaderList
    {
        public int Id { get; set; }
        public string? Periode { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int budgetparent { get; set; }
        public int ChannelId { get; set; }
        public string? UserId { get; set; }
        public DateTime ClosingDt { get; set; }
        public DateTime CreateOn { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
    }
    public class AccrualReportHeaderBody
    {
        public string? periode { get; set; }
        public int entity { get; set; }
        public string? closingdt { get; set; }
    }
    public class AccrualReportList
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
        public string? ChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? LastStatus { get; set; }
        public string? ApprovalNotes { get; set; }
        public double Target { get; set; }
        public double Investment { get; set; }
        public double DnClaim { get; set; }
        public double DnPaid { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public double AccrueMTD { get; set; }
        public double AccrueYTD { get; set; }
        public string? TsCoding { get; set; }
    }
    public class InvestmentNotifBodyFinReport
    {
        public string? periode { get; set; }
        public string? userid { get; set; }
    }

    public class InvesmentNotifResultPromo
    {
        public double DNClaim { get; set; }
        public double DNPaid { get; set; }
    }
    public class InvesmentNotifResultAccrual
    {
        public double DNClaim { get; set; }
        public double DNPaid { get; set; }
    }
    public class InvesmentNotifResultDN
    {
        public double DNClaim { get; set; }
        public double DNPaid { get; set; }
    }
    public class InvesmentNotifResultGAP
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

    public class InvestmentNotifFinReport
    {
        public IList<InvesmentNotifResultAccrual>? accrual { get; set; }
        public IList<InvesmentNotifResultPromo>? promo { get; set; }
        public IList<InvesmentNotifResultDN>? debitnote { get; set; }
        public IList<InvesmentNotifResultGAP>? GAP_list { get; set; }

    }

}