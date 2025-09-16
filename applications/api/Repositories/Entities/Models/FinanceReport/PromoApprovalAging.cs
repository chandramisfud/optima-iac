namespace Repositories.Entities.Models
{
    public class FinPromoApprovalAgingLandingPage
    {
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
        public IList<FinPromoApprovalAgingData>? Data { get; set; }

    }
    public class FinPromoApprovalAgingData
    {
        public int idx { get; set; }
        public string? PromoNo { get; set; }
        public string? InitiatorName { get; set; }
        public string? ChannelDesc { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Activity { get; set; }
        public string? SubActivity { get; set; }
        public string? PromoPeriode { get; set; }
        public string? ActivityDesc { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public decimal Investment { get; set; }
        public string? LastStatus { get; set; }
        public int Initiator { get; set; }
        public int Approver1 { get; set; }
        public int Approver2 { get; set; }
        public int Approver3 { get; set; }
        public int Approver4 { get; set; }
        public int Approver5 { get; set; }
        public int Aging { get; set; }
        public string? Approver { get; set; }
        public string? ApprovalNotes { get; set; }
    }
    public class FinPromoApprovalAgingEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class FinPromoApprovalAgingDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

}