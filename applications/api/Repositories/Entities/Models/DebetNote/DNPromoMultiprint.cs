namespace Repositories.Entities.Models.DN
{
    public class PromoMultiprint
    {
        public int? Period { get; set; }
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
        public string? PrincipalDesc { get; set; }
        public int PromoId { get; set; }
        public string? RefId { get; set; }
        public string? ActivityDesc { get; set; }
        public string? LastStatus { get; set; }
        public string? Allocation { get; set; }
        public string? Investment { get; set; }
        public int IsCancelLocked { get; set; }
        public string? TsCoding { get; set; }
        public string? CancelNotes { get; set; }
    }
    public class PromoView
    {
        public int PromoId { get; set; }
        public string? RefId { get; set; }
        public string? ActivityDesc { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public string? Allocation { get; set; }
        public decimal Investment { get; set; }
        public string? LastStatus { get; set; }
        public bool IsCancelLocked { get; set; }
        public string? TsCoding { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? CancelNotes { get; set; }
        public string? skp_flagging_status { get; set; }
        public int entityId { get; set; }
        public string? entityName { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public int skpstatus { get; set; }
        public string? skp_notes { get; set; }
        public string? CreateBy { get; set; }
        public double dnclaim { get; set; }
        public string? creationdate { get; set; }
        public string? laststatusdate { get; set; }
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }
        // Added, andrie Nov 11 2024 
        public bool mechanismInputMethod { get;set; }
    }
}