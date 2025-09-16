namespace Repositories.Entities.Models
{
    public class PromoSubmissionLandingPage
    {
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
        public IList<PromoSubmissionData>? Data { get; set; }

    }
    public class PromoSubmissionData
    {
        public string? Entity { get; set; }
        public int PromoPlanId { get; set; }
        public string? PromoPlanRefId { get; set; }
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
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public int Gap { get; set; }
        public bool OnTime { get; set; }
        public string? LastStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public string? ApprovalNotes { get; set; }
        public decimal Target { get; set; }
        public decimal Investment { get; set; }
        public double NormalSales { get; set; }
        public double IncrSales { get; set; }
        public double Roi { get; set; }
        public double CostRatio { get; set; }
        public decimal DnClaim { get; set; }
        public decimal RemainingBalance { get; set; }
        public decimal DnPaid { get; set; }
        public decimal AccrueMTD { get; set; }
        public decimal AccrueYTD { get; set; }
        public string? TsCoding { get; set; }
        public string? BrandDesc { get; set; }
        public string? SKUDesc { get; set; }
        public bool ClosureStatus { get; set; }
        public string? ReconStatus { get; set; }
        public string? LastReconStatus { get; set; }
        public int SubactivityTypeId { get; set; }
        public string? SubactivityTypeRefId { get; set; }
        public string? SubactivityType { get; set; }
        public string? CancelReason { get; set; }
        public double actual_sales { get; set; }
        public string? initiator_notes { get; set; }
        public string? sendback_notes { get; set; }
        public string? sendback_notes_date { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public int InvestmentTypeId_promo { get; set; }
        public string? InvestmentTypeRefId_promo { get; set; }
        public string? InvestmentTypeDesc_promo { get; set; }
        public string? Reason { get; set; }
    }
    public class PromoSubmissionEmailParam
    {
        public string? period { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
    }
    public class PromoSubmissionList
    {
        public List<PromoSubmissionList1>? submissionlist1 { get; set; }
        public List<PromoSubmissionList2>? submissionlist2 { get; set; }
    }
    public class PromoSubmissionList1
    {
        public string? MonthName { get; set; }
        public string? ChannelDesc { get; set; }
        public double TotOptimaCreated { get; set; }
        public double OnTime { get; set; }
        public double OnTimePCT { get; set; }
        public double Late { get; set; }
        public double LatePCT { get; set; }
    }

    public class PromoSubmissionList2
    {
        public string? ChannelDesc { get; set; }
        public double TotOptimaCreated { get; set; }
        public double OnTime { get; set; }
        public double OnTimePCT { get; set; }
        public double Late { get; set; }
        public double LatePCT { get; set; }
    }
    public class PromoSubmissonExceptionList
    {
        public int promoid { get; set; }
        public string? promorefid { get; set; }
        public string? reason { get; set; }
    }
    public class PromoSubmissonExceptionListBodyReq
    {
        public string? idx { get; set; }
    }
    public class PromoSubmissonExceptionLClearBodyReq
    {
        public string? idx { get; set; }
    }
    public class ConfigLatePromoCreation
    {
        public int id { get; set; }
        public int remindertype { get; set; }
        public int category { get; set; }
        public string? description { get; set; }
        public int days { get; set; }
    }
    public class FinPromoSubmissionEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class FinPromoSubmissionDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class FinPromoSubmissionChannelList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class FinPromoSubmissionRecord
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
    public class PromoSubmissionUser
    {
        public string? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? usergroupid { get; set; }
        public int userlevel { get; set; }
        public string? department { get; set; }
        public string? jobtitle { get; set; }
        public string? contactinfo { get; set; }
        public string? distributorid { get; set; }
        public int registered { get; set; }
        public string? code { get; set; }
        public DateTime password_change { get; set; }
        public string? token { get; set; }
        public DateTime token_date { get; set; }
        public string? userinput { get; set; }
        public DateTime dateinput { get; set; }
        public string? useredit { get; set; }
        public DateTime dateedit { get; set; }
        public int isdeleted { get; set; }
        public string? deletedby { get; set; }
        public DateTime deletedon { get; set; }
        public string? statusname { get; set; }
        public string? statussearch { get; set; }
        public string? usergroupname { get; set; }
        public string? levelname { get; set; }
    }
    public class PromoSubmissionUserGroup
    {
        public string? usergroupid { get; set; }
        public string? usergroupname { get; set; }
        public int groupmenupermission { get; set; }
        public string? groupmenupermissionname { get; set; }
        public string? userinput { get; set; }
        public DateTime dateinput { get; set; }
        public string? useredit { get; set; }
        public DateTime dateedit { get; set; }

    }

    public class PromoSubmissionExceptionUploadParam
    {
        public string? idx { get; set; }
    }
}



