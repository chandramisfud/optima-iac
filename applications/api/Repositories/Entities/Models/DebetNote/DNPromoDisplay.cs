namespace Repositories.Entities.Models.DN
{
    public class DNPromoDisplayDist
    {
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
        public string? PrincipalDesc { get; set; }
        public string? CategoryShortDesc { get; set; }
        public int PromoId { get; set; }
        public string? RefId { get; set; }
        public string? ActivityDesc { get; set; }
        public string? LastStatus { get; set; }
        public string? Allocation { get; set; }
        public double Investment { get; set; }
        public int IsCancelLocked { get; set; }
        public string? TsCoding { get; set; }
        public string? SubAccountId { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? CancelNotes { get; set; }
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public string? initiator_notes { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? dnclaim { get; set; }
        public string? CreateOn { get; set; }
        public string? LastStatusDate { get; set; }
        public int reconciled { get; set; }
    }

    public class DNPromoDisplayDistPaging
    {
        public IList<DNPromoDisplayDist>? Data { get; set; }
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
    }
    public class DNPromoDisplayRecord
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
    public class DNPromoDisplayGetDistributorId
    {
        public int id { get; set; }
        public int DistributorId { get; set; }
    }

    public class DNPromoDisplayMainData
    {
        public int PromoPlanId { get; set; }
        public string? RefId { get; set; }
        public int Id { get; set; }
        public string? PromoPlanRefId { get; set; }
        public string? CreateOn { get; set; }
        public string? ModifiedOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ActivityDesc { get; set; }
        public double Investment { get; set; }
        public double PrevInvestment { get; set; }
        public double NormalSales { get; set; }
        public double PrevNormalSales { get; set; }
        public double IncrSales { get; set; }
        public double PrevIncrSales { get; set; }
        public double PrevTotSales { get; set; }
        public double Roi { get; set; }
        public double PrevRoi { get; set; }
        public double CostRatio { get; set; }
        public double PrevCostRatio { get; set; }
        public string? Mechanism { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public int AllocationId { get; set; }
        public string? Periode { get; set; }
        public string? AllocationRefId { get; set; }
        public string? BudgetType { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorName { get; set; }
        public int PrincipalId { get; set; }
        public string? PrincipalName { get; set; }
        public string? OwnerId { get; set; }
        public string? BudgetOwner { get; set; }
        public string? FromOwnerId { get; set; }
        public string? FromOwnerName { get; set; }
        public int BudgetMasterId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategoryLongDesc { get; set; }
        public int ActivityId { get; set; }
        public string? ActivityLongDesc { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivityLongDesc { get; set; }
        public string? CategoryDesc { get; set; }
        public int BudgetSourceId { get; set; }
        public double BudgetAmount { get; set; }
        public double RemainingBudget { get; set; }
        public string? AllocationDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? StatusApproval { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public string? UserApprover1 { get; set; }
        public string? UserApprover1Name { get; set; }
        public string? LastStatus1 { get; set; }
        public string? ApprovalDate1 { get; set; }
        public string? UserApprover2 { get; set; }
        public string? UserApprover2Name { get; set; }
        public string? LastStatus2 { get; set; }
        public string? ApprovalDate2 { get; set; }
        public string? UserApprover3 { get; set; }
        public string? UserApprover3Name { get; set; }
        public string? LastStatus3 { get; set; }
        public string? ApprovalDate3 { get; set; }
        public string? UserApprover4 { get; set; }
        public string? UserApprover4Name { get; set; }
        public string? LastStatus4 { get; set; }
        public string? ApprovalDate4 { get; set; }
        public string? UserApprover5 { get; set; }
        public string? UserApprover5Name { get; set; }
        public string? LastStatus5 { get; set; }
        public string? ApprovalDate5 { get; set; }
        public string? RegionDesc { get; set; }
        public string? ChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? BrandDesc { get; set; }
        public string? ProductDesc { get; set; }
        public string? CutOffClaim { get; set; }
        public string? CancelReason { get; set; }
        public string? Notes { get; set; }
        public string? ApprovalNotes { get; set; }
        public string? TsCoding { get; set; }
        public bool Reconciled { get; set; }
        public double Invoiced { get; set; }
        public string? prevMechanism { get; set; }
        public string? prevMechanism1 { get; set; }
        public string? prevMechanism2 { get; set; }
        public string? prevMechanism3 { get; set; }
        public string? prevMechanism4 { get; set; }
        public string? UserApprover1prev { get; set; }
        public string? UserApprover1prevName { get; set; }
        public string? LastStatus1prev { get; set; }
        public string? ApprovalDate1prev { get; set; }
        public string? UserApprover2prev { get; set; }
        public string? UserApprover2prevName { get; set; }
        public string? LastStatus2prev { get; set; }
        public string? ApprovalDate2prev { get; set; }
        public string? UserApprover3prev { get; set; }
        public string? UserApprover3prevName { get; set; }
        public string? LastStatus3prev { get; set; }
        public string? ApprovalDate3prev { get; set; }
        public string? UserApprover4prev { get; set; }
        public string? UserApprover4prevName { get; set; }
        public string? LastStatus4prev { get; set; }
        public string? ApprovalDate4prev { get; set; }
        public string? UserApprover5prev { get; set; }
        public string? UserApprover5prevName { get; set; }
        public string? LastStatus5prev { get; set; }
        public string? ApprovalDate5prev { get; set; }
        public string? initiator_notes { get; set; }
        public string? sendback_notes { get; set; }
        public string? userid_Approver { get; set; }
        public string? sendback_notes_date { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public int late_submission_day { get; set; }
        //added: AND 20221109
        public string? ModifReason { get; set; }
        //added: AND 20221114
        public double investmentDraft { get; set; }
        // added by AND May 22 2023 #875
        public bool allowedit { get; set; }

    }

    public class DNPromoDisplayRegionRes
    {
        public bool flag { get; set; }
        public int RegionPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayChannelRes
    {
        public bool flag { get; set; }
        public int ChannelPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class DNPromoDisplaySubChannelRes
    {
        public bool flag { get; set; }
        public int SubChannelPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayAccountRes
    {
        public bool flag { get; set; }
        public int AccountPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class DNPromoDisplaySubAccountRes
    {
        public bool flag { get; set; }
        public int SubAccountPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayBrandRes
    {
        public bool flag { get; set; }
        public int BrandPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class DNPromoDisplayProductRes
    {
        public bool flag { get; set; }
        public int ProductPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayActivityRes
    {
        public int ActivityAllocId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplaySubActivityRes
    {
        public int SubActivityAllocId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayAttachment
    {
        public int PromoId { get; set; }
        public string? DocLink { get; set; }
        public string? FileName { get; set; }
    }

    public class DNPromoDisplayApprovalRes
    {

        public string? StatusCode { get; set; }
        public string? StatusDesc { get; set; }
    }
    public class DNPromoDisplaySKPValidationRes
    {
        public int PromoId { get; set; }
        public int SKPDraftAvail { get; set; }
        public string? SKPDraftAvailOn { get; set; }
        public string? SKPDraftAvailBy { get; set; }
        public int SKPDraftAvailBfrAct60 { get; set; }
        public string? SKPDraftAvailBfrAct60On { get; set; }
        public string? SKPDraftAvailBfrAct60By { get; set; }
        public int PeriodMatch { get; set; }
        public string? PeriodMatchOn { get; set; }
        public string? PeriodMatchBy { get; set; }
        public int InvestmentMatch { get; set; }
        public string? InvestmentMatchOn { get; set; }
        public string? InvestmentMatchBy { get; set; }
        public int MechanismMatch { get; set; }
        public string? MechanismMatchOn { get; set; }
        public string? MechanismMatchBy { get; set; }
        public int SKPSign7 { get; set; }
        public string? SKPSign7On { get; set; }
        public string? SKPSign7By { get; set; }
        public int EntityDraft { get; set; }
        public string? EntityDraftOn { get; set; }
        public string? EntityDraftBy { get; set; }
        public int BrandDraft { get; set; }
        public string? BrandDraftOn { get; set; }
        public string? BrandDraftBy { get; set; }
        public int PeriodDraft { get; set; }
        public string? PeriodDraftOn { get; set; }
        public string? PeriodDraftBy { get; set; }
        public int ActivityDescDraft { get; set; }
        public string? ActivityDescDraftOn { get; set; }
        public string? ActivityDescDraftBy { get; set; }
        public int MechanismDraft { get; set; }
        public string? MechanismDraftOn { get; set; }
        public string? MechanismDraftBy { get; set; }
        public int InvestmentDraft { get; set; }
        public string? InvestmentDraftOn { get; set; }
        public string? InvestmentDraftBy { get; set; }
        public int Entity { get; set; }
        public string? EntityOn { get; set; }
        public string? EntityBy { get; set; }
        public int Brand { get; set; }
        public string? BrandOn { get; set; }
        public string? BrandBy { get; set; }
        public int ActivityDesc { get; set; }
        public string? ActivityDescOn { get; set; }
        public string? ActivityDescBy { get; set; }
        public int DistributorDraft { get; set; }
        public string? DistributorDraftOn { get; set; }
        public string? DistributorDraftBy { get; set; }
        public int Distributor { get; set; }
        public string? DistributorOn { get; set; }
        public string? DistributorBy { get; set; }
        public int ChannelDraft { get; set; }
        public string? ChannelDraftOn { get; set; }
        public string? ChannelDraftBy { get; set; }
        public int Channel { get; set; }
        public string? ChannelOn { get; set; }
        public string? ChannelBy { get; set; }
        public int StoreNameDraft { get; set; }
        public string? StoreNameDraftOn { get; set; }
        public string? StoreNameDraftBy { get; set; }
        public int StoreName { get; set; }
        public string? StoreNameOn { get; set; }
        public string? StoreNameBy { get; set; }
        public int skpstatus { get; set; }
        public string? skp_notes { get; set; }
    }
    public class DNPromoDisplayReconInvestmentRes
    {
        public double Investment { get; set; }
        public double NormalSales { get; set; }
        public double IncrSales { get; set; }
        public double TotSales { get; set; }
        public double Roi { get; set; }
        public double CostRatio { get; set; }
    }
    public class DNPrommoDisplayMechanismRes
    {
        public int MechanismId { get; set; }
        public string? Mechanism { get; set; }
        public string? Notes { get; set; }
        public int ProductId { get; set; }
        public string? Product { get; set; }
        public int BrandId { get; set; }
        public string? Brand { get; set; }
    }
    public class DNPromoDisplayPromoRecon
    {
        public DNPromoDisplayMainData? PromoHeader { get; set; }
        public IList<DNPromoDisplayRegionRes>? Regions { get; set; }
        public IList<DNPromoDisplayChannelRes>? Channels { get; set; }
        public IList<DNPromoDisplaySubChannelRes>? SubChannels { get; set; }
        public IList<DNPromoDisplayAccountRes>? Accounts { get; set; }
        public IList<DNPromoDisplaySubAccountRes>? SubAccounts { get; set; }
        public IList<DNPromoDisplayBrandRes>? Brands { get; set; }
        public IList<DNPromoDisplayProductRes>? Skus { get; set; }
        public IList<DNPromoDisplayActivityRes>? Activity { get; set; }
        public IList<DNPromoDisplaySubActivityRes>? SubActivity { get; set; }
        public IList<DNPromoDisplayAttachment>? Attachments { get; set; }
        public IList<DNPromoDisplayApprovalRes>? ListApprovalStatus { get; set; }
        public DNPromoDisplaySKPValidationRes? SKPValidations { get; set; }
        public IList<DNPromoDisplayReconInvestmentRes>? Investments { get; set; }
        public IList<DNPrommoDisplayMechanismRes>? Mechanisms { get; set; }
        public IList<object>? GroupBrand { get; set; }
    }

    public class DNPromoDisplayIdHeader
    {
        public int PromoPlanId { get; set; }
        public string? RefId { get; set; }
        public int Id { get; set; }
        public string? PromoPlanRefId { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ActivityDesc { get; set; }
        public string? Investment { get; set; }
        public decimal PrevInvestment { get; set; }
        public decimal NormalSales { get; set; }
        public decimal PrevNormalSales { get; set; }
        public decimal IncrSales { get; set; }
        public decimal PrevIncrSales { get; set; }
        public decimal PrevTotSales { get; set; }
        public decimal Roi { get; set; }
        public decimal PrevRoi { get; set; }
        public decimal CostRatio { get; set; }
        public decimal PrevCostRatio { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public int AllocationId { get; set; }
        public string? Periode { get; set; }
        public string? AllocationRefId { get; set; }
        public string? BudgetType { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorName { get; set; }
        public int PrincipalId { get; set; }
        public string? PrincipalName { get; set; }
        public string? OwnerId { get; set; }
        public string? BudgetOwner { get; set; }
        public string? FromOwnerId { get; set; }
        public string? FromOwnerName { get; set; }
        public int BudgetMasterId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ActivityId { get; set; }
        public string? ActivityLongDesc { get; set; }
        public int SubactivityId { get; set; }
        public string? SubActivityLongDesc { get; set; }
        public string? CategoryDesc { get; set; }
        public string? BudgetSourceId { get; set; }
        public string? BudgetAmount { get; set; }
        public decimal RemainingBudget { get; set; }
        public string? AllocationDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? StatusApproval { get; set; }
        public int IsActive { get; set; }
        public int IsLocked { get; set; }
        public string? UserApprover1 { get; set; }
        public string? LastStatus1 { get; set; }
        public string? ApprovalDate1 { get; set; }
        public string? UserApprover2 { get; set; }
        public string? LastStatus2 { get; set; }
        public string? ApprovalDate2 { get; set; }
        public string? UserApprover3 { get; set; }
        public string? LastStatus3 { get; set; }
        public string? ApprovalDate3 { get; set; }
        public string? UserApprover4 { get; set; }
        public string? LastStatus4 { get; set; }
        public string? ApprovalDate4 { get; set; }
        public string? UserApprover5 { get; set; }
        public string? LastStatus5 { get; set; }
        public string? ApprovalDate5 { get; set; }
        public string? RegionDesc { get; set; }
        public string? ChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? BrandDesc { get; set; }
        public string? ProductDesc { get; set; }
        public DateTime CutOffClaim { get; set; }
        public string? CancelReason { get; set; }
        public string? Notes { get; set; }
        public string? ApprovalNotes { get; set; }
        public string? TsCoding { get; set; }
        public int IsClose { get; set; }
        public decimal PlanInvestment { get; set; }
        public decimal PlanNormalSales { get; set; }
        public decimal PlanIncrSales { get; set; }
        public decimal PlanTotSales { get; set; }
        public decimal PlanRoi { get; set; }
        public decimal PlanCostRatio { get; set; }
        public string? prevMechanisme1 { get; set; }
        public string? prevMechanisme2 { get; set; }
        public string? prevMechanisme3 { get; set; }
        public string? prevMechanisme4 { get; set; }
        public string? initiator_notes { get; set; }
        public string? sendback_notes { get; set; }
        public string? userid_Approver { get; set; }
        public string? sendback_notes_date { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public int late_submission_day { get; set; }
        public string? SubCategoryDesc { get; set; }
        public string? LastStatus { get; set; }
        public string? Initiator { get; set; }
        public decimal investmentBfrClose { get; set; }
        public decimal investmentClosedBalance { get; set; }
        public string? ModifReason { get; set; }
        public string? StatusApprovalCode { get; set; }
        public int isCancel { get; set; }
        public int IsCancelLocked { get; set; }
    }

    public class DNPromoDisplayIdRegionRes
    {
        public bool flag { get; set; }
        public int RegionPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayIdChannelRes
    {
        public bool flag { get; set; }
        public int ChannelPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayIdSubChannelRes
    {
        public bool flag { get; set; }
        public int SubChannelPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayIdAccountRes
    {
        public bool flag { get; set; }
        public int AccountPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayIdSubAccountRes
    {
        public bool flag { get; set; }
        public int SubAccountPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayIdBrandRes
    {
        public bool flag { get; set; }
        public int BrandPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayIdProductRes
    {
        public bool flag { get; set; }
        public int ProductPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayIdActivityRes
    {
        public int ActivityAllocId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayIdSubActivityRes
    {
        public int SubActivityAllocId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class DNPromoDisplayIdAttachmentRes
    {
        public int PromoId { get; set; }
        public string? DocLink { get; set; }
        public string? FileName { get; set; }
    }

    public class DNPromoDisplayIdApproval
    {
        public string? StatusCode { get; set; }
        public string? StatusDesc { get; set; }
    }

    public class DNPromoDisplayIdMechanismRes
    {
        public int MechanismId { get; set; }
        public string? Mechanism { get; set; }
        public string? Notes { get; set; }
        public int ProductId { get; set; }
        public string? Product { get; set; }
        public int BrandId { get; set; }
        public string? Brand { get; set; }
    }

    public class DNPromoDisplayIdInvestmentRes
    {
        public double Investment { get; set; }
        public double NormalSales { get; set; }
        public double IncrSales { get; set; }
        public double TotSales { get; set; }
        public double Roi { get; set; }
        public double CostRatio { get; set; }
    }

    public class DNPromoDisplayId
    {
        public DNPromoDisplayIdHeader? PromoHeader { get; set; }
        public List<DNPromoDisplayIdRegionRes>? Regions { get; set; }
        public List<DNPromoDisplayIdChannelRes>? Channels { get; set; }
        public List<DNPromoDisplayIdSubChannelRes>? SubChannels { get; set; }
        public List<DNPromoDisplayIdAccountRes>? Accounts { get; set; }
        public List<DNPromoDisplayIdSubAccountRes>? SubAccounts { get; set; }
        public IList<DNPromoDisplayIdBrandRes>? Brands { get; set; }
        public IList<DNPromoDisplayIdProductRes>? Skus { get; set; }
        public List<DNPromoDisplayIdActivityRes>? Activities { get; set; }
        public List<DNPromoDisplayIdSubActivityRes>? SubActivities { get; set; }
        public IList<DNPromoDisplayIdAttachmentRes>? Attachments { get; set; }
        public IList<DNPromoDisplayIdApproval>? ListApprovalStatus { get; set; }
        public IList<DNPromoDisplayIdMechanismRes>? Mechanisms { get; set; }
        public IList<DNPromoDisplayIdInvestmentRes>? Investments { get; set; }
        public IList<object>? GroupBrand { get; set; }

    }
}