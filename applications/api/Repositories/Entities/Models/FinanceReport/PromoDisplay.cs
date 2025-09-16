using Microsoft.AspNetCore.Mvc;

namespace Repositories.Entities.Models
{
    public class FinPromoDisplayLandingPage
    {
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
        public IList<FinPromoDisplayData>? Data { get; set; }

    }
    public class FinPromoDisplayData
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
        //modif by AND Nov 29 '23, change SubAccountId int to string, DC can hold multi subaccountid, 
        public string? SubAccountId { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? CancelNotes { get; set; }
        public string? skp_flagging_status { get; set; }
        public int entityId { get; set; }
        public string? entityName { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public string? initiator_notes { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public string? CreateBy { get; set; }
        public double dnclaim { get; set; }
        public string? CreateOn { get; set; }
        public string? LastStatusDate { get; set; }
        public int isClose { get; set; }

        // #839
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryShortDesc { get; set; }

        public int reconciled { get; set; }
    }

    public class FinPromoDisplayRecord
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }

    public class FinPromoDisplayEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class FinPromoDisplayDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class PromoDisplayList
    {
        public PromoDisplayData? PromoHeader { get; set; }
        public List<PromoRegionRes>? Regions { get; set; }
        public List<PromoChannelRes>? Channels { get; set; }
        public List<PromoSubChannelRes>? SubChannels { get; set; }
        public List<PromoAccountRes>? Accounts { get; set; }
        public List<PromoSubAccountRes>? SubAccounts { get; set; }
        public List<PromoActivityRes>? Activities { get; set; }
        public List<PromoSubActivityRes>? SubActivities { get; set; }
        public IList<PromoBrandRes>? Brands { get; set; }
        public IList<PromoProductRes>? Skus { get; set; }
        public IList<PromoAttachment>? Attachments { get; set; }
        public IList<ApprovalRes>? ListApprovalStatus { get; set; }
        public IList<MechanismData>? Mechanisms { get; set; }
        public IList<PromoReconInvestmentData>? Investments { get; set; }
        public IList<object>? GroupBrand { get; set; }

    }

    public class PromoDisplayData
    {
        public int PromoPlanId { get; set; }
        public int AllocationId { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? PromoPlanRefId { get; set; }
        public string? AllocationRefId { get; set; }
        public string? AllocationDesc { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorName { get; set; }
        public string? PrincipalId { get; set; }
        public string? PrincipalName { get; set; }
        public string? BudgetOwner { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal RemainingBudget { get; set; }
        public string? CategoryDesc { get; set; }
        public string? CategoryShortDesc { get; set; }
        public string? PrincipalShortDesc { get; set; }
        public int BudgetMasterId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ActivityId { get; set; }
        public int SubActivityId { get; set; }
        public string? ActivityDesc { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public string? prevMechanisme1 { get; set; }
        public string? prevMechanisme2 { get; set; }
        public string? prevMechanisme3 { get; set; }
        public string? prevMechanisme4 { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public decimal Investment { get; set; }
        public decimal NormalSales { get; set; }
        public decimal IncrSales { get; set; }
        public decimal Roi { get; set; }
        public decimal CostRatio { get; set; }
        public decimal PrevInvestment { get; set; }
        public decimal PrevNormalSales { get; set; }
        public decimal PrevIncrSales { get; set; }
        public decimal PrevTotSales { get; set; }
        public decimal PrevRoi { get; set; }
        public decimal PrevCostRatio { get; set; }
        public string? StatusApproval { get; set; }
        public string? Notes { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? CreateBy { get; set; }
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
        public string? ApprovalNotes { get; set; }
        public string? TsCoding { get; set; }
        public string? ActivityLongDesc { get; set; }
        public string? SubActivityLongDesc { get; set; }
        public bool IsClose { get; set; }
        public decimal PlanInvestment { get; set; }
        public decimal PlanNormalSales { get; set; }
        public decimal PlanIncrSales { get; set; }
        public decimal PlanTotSales { get; set; }
        public decimal PlanRoi { get; set; }
        public decimal PlanCostRatio { get; set; }
        public string? initiator_notes { get; set; }
        public string? sendback_notes { get; set; }
        public string? userid_Approver { get; set; }
        public string? sendback_notes_date { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public int late_submission_day { get; set; }
        public string? SubCategoryDesc { get; set; }
        //#835
        public string? LastStatus { get; set; }
        public string? Initiator { get; set; }
        //#839
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }
        //added: AND 20221109
        public string? ModifReason { get; set; }
        public string? GroupBrandDesc { get; set; }
        public double totSales { get; set; }
    }
    public class PromoRegionRes
    {
        public bool flag { get; set; }
        public int RegionPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class PromoChannelRes
    {
        public bool flag { get; set; }
        public int ChannelPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class PromoSubChannelRes
    {
        public bool flag { get; set; }
        public int SubChannelPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class PromoAccountRes
    {
        public bool flag { get; set; }
        public int AccountPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class PromoSubAccountRes
    {
        public bool flag { get; set; }
        public int SubAccountPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }


    public class PromoSubCategoryRes
    {
        public bool flag { get; set; }
        public int SubCategoryPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class PromoActivityRes
    {
        //public bool flag { get; set; }
        public int ActivityPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
        public string? ActivityLongDesc { get; set; }
    }

    public class PromoSubActivityRes
    {
        //public bool flag { get; set; }
        public int SubActivityPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
        public string? SubActivityLongDesc { get; set; }
    }

    public class PromoBrandRes
    {
        public bool flag { get; set; }
        public int BrandPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class PromoProductRes
    {
        public bool flag { get; set; }
        public int ProductPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class ApprovalRes
    {

        public string? StatusCode { get; set; }
        public string? StatusDesc { get; set; }
    }

    public class HistoricalApproval
    {
        public int ApprovalIndex { get; set; }
        public string? Approver { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? Notes { get; set; }
    }
    public class PromoAttachment
    {
        public int PromoId { get; set; }
        public string? DocLink { get; set; }
        public string? FileName { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
    }

    public class MechanismData
    {
        public int MechanismId { get; set; }
        public string? Mechanism { get; set; }
        public string? Notes { get; set; }
        public int ProductId { get; set; }
        public string? Product { get; set; }
        public int BrandId { get; set; }
        public string? Brand { get; set; }
    }
    public class PromoRecon
    {
        public PromoReconMainData? PromoHeader { get; set; }
        public IList<PromoRegionRes>? Regions { get; set; }
        public IList<PromoChannelRes>? Channels { get; set; }
        public IList<PromoSubChannelRes>? SubChannels { get; set; }
        public IList<PromoAccountRes>? Accounts { get; set; }
        public IList<PromoSubAccountRes>? SubAccounts { get; set; }
        public IList<PromoBrandRes>? Brands { get; set; }
        public IList<PromoProductRes>? Skus { get; set; }
        public IList<PromoActivityRes>? Activity { get; set; }
        public IList<PromoSubActivityRes>? SubActivity { get; set; }
        public IList<PromoAttachment>? Attachments { get; set; }
        public IList<ApprovalRes>? ListApprovalStatus { get; set; }
        public SKPValidation? SKPValidations { get; set; }
        public IList<PromoReconInvestmentData>? Investments { get; set; }
        public IList<MechanismData>? Mechanisms { get; set; }
    }
    public class PromoReconInvestmentData
    {
        public double Investment { get; set; }
        public double NormalSales { get; set; }
        public double IncrSales { get; set; }
        public double TotSales { get; set; }
        public double Roi { get; set; }
        public double CostRatio { get; set; }
    }
    public class SKPValidation
    {
        public int PromoId { get; set; }
        public bool? SKPDraftAvail { get; set; }
        public DateTime SKPDraftAvailOn { get; set; }
        public string? SKPDraftAvailBy { get; set; }
        public bool? SKPDraftAvailBfrAct60 { get; set; }
        public DateTime SKPDraftAvailBfrAct60On { get; set; }
        public string? SKPDraftAvailBfrAct60By { get; set; }
        public bool? PeriodMatch { get; set; }
        public DateTime PeriodMatchOn { get; set; }
        public string? PeriodMatchBy { get; set; }
        public bool? InvestmentMatch { get; set; }
        public DateTime InvestmentMatchOn { get; set; }
        public string? InvestmentMatchBy { get; set; }
        public bool? MechanismMatch { get; set; }
        public DateTime MechanismMatchOn { get; set; }
        public string? MechanismMatchBy { get; set; }
        public bool? SKPSign7 { get; set; }
        public DateTime SKPSign7On { get; set; }
        public string? SKPSign7By { get; set; }
        public bool EntityDraft { get; set; }
        public DateTime EntityDraftOn { get; set; }
        public string? EntityDraftBy { get; set; }
        public bool BrandDraft { get; set; }
        public DateTime BrandDraftOn { get; set; }
        public string? BrandDraftBy { get; set; }
        public bool PeriodDraft { get; set; }
        public DateTime PeriodDraftOn { get; set; }
        public string? PeriodDraftBy { get; set; }
        public bool ActivityDescDraft { get; set; }
        public DateTime ActivityDescDraftOn { get; set; }
        public string? ActivityDescDraftBy { get; set; }
        public bool MechanismDraft { get; set; }
        public DateTime MechanismDraftOn { get; set; }
        public string? MechanismDraftBy { get; set; }
        public bool InvestmentDraft { get; set; }
        public DateTime InvestmentDraftOn { get; set; }
        public string? InvestmentDraftBy { get; set; }
        public bool Entity { get; set; }
        public DateTime EntityOn { get; set; }
        public string? EntityBy { get; set; }
        public bool Brand { get; set; }
        public DateTime BrandOn { get; set; }
        public string? BrandBy { get; set; }
        public bool ActivityDesc { get; set; }
        public DateTime ActivityDescOn { get; set; }
        public string? ActivityDescBy { get; set; }
        public bool DistributorDraft { get; set; }
        public DateTime DistributorDraftOn { get; set; }
        public string? DistributorDraftBy { get; set; }
        public bool Distributor { get; set; }
        public DateTime DistributorOn { get; set; }
        public string? DistributorBy { get; set; }
        public bool ChannelDraft { get; set; }
        public DateTime ChannelDraftOn { get; set; }
        public string? ChannelDraftBy { get; set; }
        public bool Channel { get; set; }
        public DateTime ChannelOn { get; set; }
        public string? ChannelBy { get; set; }
        public bool StoreNameDraft { get; set; }
        public DateTime StoreNameDraftOn { get; set; }
        public string? StoreNameDraftBy { get; set; }
        public bool StoreName { get; set; }
        public DateTime StoreNameOn { get; set; }
        public string? StoreNameBy { get; set; }
        public int skpstatus { get; set; }
        public string? skp_notes { get; set; }
    }
    public class PromoReconMainData
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

    public class SchedulerPromoRecon
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

        public double lastNormalSales { get; set; }
        public double lastIncrSales { get; set; }
        public double lastInvestment { get; set; }
        public double lastTotSales { get; set; }
        public double lastRoi { get; set; }
        public double lastCostRatio { get; set; }

    }

}
