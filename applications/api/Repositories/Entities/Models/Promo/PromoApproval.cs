using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Entities.Configuration;

namespace Repositories.Entities.Models.PromoApproval
{
    public class PromoApprovalView
    {
        public int PromoId { get; set; }
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string? RefId { get; set; }
        public string? ActivityDesc { get; set; }
        public int AgingApproval { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public string? Initiator { get; set; }
        public double Investment { get; set; }
        public double TotalSales { get; set; }
        public string? TsCoding { get; set; }
        public double PlanInvestment { get; set; }
        public double PlanNormalSales { get; set; }
        public double PlanIncrSales { get; set; }
        public double PlanTotSales { get; set; }
        public decimal PlanRoi { get; set; }
        public decimal PlanCostRatio { get; set; }
        public string? descstatus { get; set; }
        public string? LastStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CancelReason { get; set; }
        public string? ChannelDesc { get; set; }
        public string? SubAccountDesc { get; set; }
        public double TotalClaim { get; set; }
        public double TotalPaid { get; set; }
        public string? CreateBy { get; set; }
        public int PromoPlanId { get; set; }

        // Added, andrie Oct 9 2023 E2#38
        public int categoryId { get; set; }
        public string? categoryShortDesc { get; set; }

        // Added, andrie Oct 23 2024 
        public int approvalCycle { get; set; }
    }
    public class PromoRevise
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
        public double BudgetAmount { get; set; }
        public double RemainingBudget { get; set; }
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
        public double Investment { get; set; }
        public double NormalSales { get; set; }
        public decimal IncrSales { get; set; }
        public decimal Roi { get; set; }
        public decimal CostRatio { get; set; }
        public double PrevInvestment { get; set; }
        public double PrevNormalSales { get; set; }
        public decimal PrevIncrSales { get; set; }
        public double PrevTotSales { get; set; }
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
        public double PlanInvestment { get; set; }
        public decimal PlanNormalSales { get; set; }
        public decimal PlanIncrSales { get; set; }
        public double PlanTotSales { get; set; }
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

        //added by AND Sept 16 2023
        public string? statusApprovalCode { get; set; }
        public bool isCancel { get; set; }
        //public bool isClose { get; set; }
        public bool isCancelLocked { get; set; }
    }
    public class PromoReviseV3Dto
    {
        public PromoRevise? PromoHeader { get; set; }
        public List<PromoRegionRes>? Regions { get; set; }
        public List<PromoChannelRes>? Channels { get; set; }
        public List<PromoSubChannelRes>? SubChannels { get; set; }
        public List<PromoAccountRes>? Accounts { get; set; }
        public List<PromoSubAccountRes>? SubAccounts { get; set; }
        public List<PromoActivityRes>? Activity { get; set; }
        public List<PromoSubActivityRes>? SubActivity { get; set; }
        public IList<PromoBrandRes>? Brands { get; set; }
        public IList<PromoProductRes>? Skus { get; set; }
        public IList<PromoAttachment>? Attachments { get; set; }
        public IList<ApprovalRes>? ListApprovalStatus { get; set; }
        public IList<MechanismSelect>? Mechanism { get; set; }

        //Add:  AND Oct 11 2023 E2#38
        public List<Object>? Investment { get; set; }
        public List<Object>? GroupBrand { get; set; }

    }
    public class MechanismSelect
    {
        public int MechanismId { get; set; }
        public string? Mechanism { get; set; }
        public string? Notes { get; set; }
        public int ProductId { get; set; }
        public string? Product { get; set; }
        public int BrandId { get; set; }
        public string? Brand { get; set; }
    }

    public class SKPValidationDto
    {
        public int PromoId { get; set; }
        public bool SKPDraftAvail { get; set; }
        public DateTime SKPDraftAvailOn { get; set; }
        public string? SKPDraftAvailBy { get; set; }
        public bool SKPDraftAvailBfrAct60 { get; set; }
        public DateTime SKPDraftAvailBfrAct60On { get; set; }
        public string? SKPDraftAvailBfrAct60By { get; set; }
        public bool PeriodMatch { get; set; }
        public DateTime PeriodMatchOn { get; set; }
        public string? PeriodMatchBy { get; set; }
        public bool InvestmentMatch { get; set; }
        public DateTime InvestmentMatchOn { get; set; }
        public string? InvestmentMatchBy { get; set; }
        public bool MechanismMatch { get; set; }
        public DateTime MechanismMatchOn { get; set; }
        public string? MechanismMatchBy { get; set; }
        public bool SKPSign7 { get; set; }
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

    public class PromoSKP
    {
        public SKPValidationDto? PromoSKPHeader { get; set; }
        public int promoid { get; set; }
        public string? statuscode { get; set; }
        public string? notes { get; set; }
        public DateTime approvaldate { get; set; }
        public string? useremail { get; set; }
    }

    public class PromoReconApproval
    {
        public int promoid { get; set; }
        public string? statuscode { get; set; }
        public string? notes { get; set; }
        public DateTime approvaldate { get; set; }
    }

    public class PromoReconV3
    {
        public PromoReconHeader? PromoHeader { get; set; }
        public List<PromoRegionRes>? Regions { get; set; }
        public List<PromoChannelRes>? Channels { get; set; }
        public List<PromoSubChannelRes>? SubChannels { get; set; }
        public List<PromoAccountRes>? Accounts { get; set; }
        public List<PromoSubAccountRes>? SubAccounts { get; set; }
        public List<PromoActivityRes>? Activity { get; set; }
        public List<PromoSubActivityRes>? SubActivity { get; set; }
        public IList<PromoBrandRes>? Brands { get; set; }
        public IList<PromoProductRes>? Skus { get; set; }
        public IList<PromoAttachment>? Attachments { get; set; }
        public IList<ApprovalRes>? ListApprovalStatus { get; set; }
        public IList<SKPValidation>? SKPValidations { get; set; }
        public IList<PromoReconInvestment>? Investments { get; set; }
        public IList<MechanismSelect>? Mechanism { get; set; }
        public IList<object>? GroupBrand { get; set; }
        public IList<object>? PromoConfigItem { get; set; }


    }

    public class PromoReconDinamic
    {
        public object? PromoHeader { get; set; }
        public List<object>? Regions { get; set; }
        public List<object>? Channels { get; set; }
        public List<object>? SubChannels { get; set; }
        public List<object>? Accounts { get; set; }
        public List<object>? SubAccounts { get; set; }
        public List<object>? Activity { get; set; }
        public List<object>? SubActivity { get; set; }
        public IList<object>? Brands { get; set; }
        public IList<object>? Skus { get; set; }
        public IList<object>? Attachments { get; set; }
        public IList<object>? ListApprovalStatus { get; set; }
        public IList<object>? SKPValidations { get; set; }
        public IList<object>? Investments { get; set; }
        public IList<object>? Mechanism { get; set; }

    }
    public class PromoSendbackDinamic
    {
        public object? PromoHeader { get; set; }
        public List<object>? Regions { get; set; }
        public List<object>? Channels { get; set; }
        public List<object>? SubChannels { get; set; }
        public List<object>? Accounts { get; set; }
        public List<object>? SubAccounts { get; set; }
        public List<object>? Activity { get; set; }
        public List<object>? SubActivity { get; set; }
        public IList<object>? Brands { get; set; }
        public IList<object>? Skus { get; set; }
        public IList<object>? Attachments { get; set; }
        public IList<object>? ListApprovalStatus { get; set; }
        public IList<object>? SKPValidations { get; set; }
        public IList<object>? Investments { get; set; }
        public IList<object>? Mechanism { get; set; }

    }
    public class PromoReconInvestment
    {
        public double Investment { get; set; }
        public double NormalSales { get; set; }
        public double IncrSales { get; set; }
        public double TotSales { get; set; }
        public double Roi { get; set; }
        public double CostRatio { get; set; }


    }

    public class PromoReconResultV3
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

        // added by AND August 22 2023 #873
        public double lastNormalSales { get; set; }
        public double lastIncrSales { get; set; }
        public double lastInvestment { get; set; }
        public double lastTotSales { get; set; }
        public double lastRoi { get; set; }
        public double lastCostRatio { get; set; }
    }
    public class DNGetByIdforPromoApproval
    {
        public bool IsDNPromo { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? Periode { get; set; }
        public int EntityId { get; set; }
        public string? AccountDesc { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorShortDesc { get; set; }
        public string? DistributorLongDesc { get; set; }
        public string? ActivityDesc { get; set; }
        public int AccountId { get; set; }
        public int PromoId { get; set; }
        public string? PromoRefId { get; set; }
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public string? IntDocNo { get; set; }
        public string? MemDocNo { get; set; }
        public decimal DPP { get; set; }
        public double DNAmount { get; set; }
        public string? FeeDesc { get; set; }
        public double FeePct { get; set; }
        public double FeeAmount { get; set; }
        public decimal PPNPct { get; set; }
        public DateTime DeductionDate { get; set; }
        public string? UserId { get; set; }
        public bool IsFreeze { get; set; }
        public string? approver_userid { get; set; }
        public string? approver_username { get; set; }
        public string? approver_email { get; set; }
        public IList<DNSellpoint>? sellpoint { get; set; }
        public IList<DNAttachment>? dnattachment { get; set; }
        public IList<DNDocCompleteness>? DNDocCompletenessHeader { get; set; }
        public string? InternalOrderNumber { get; set; }
        public string? TaxLevel { get; set; }
        public string? Notes { get; set; }
        public string? LastStatus { get; set; }
        public double pphPct { get; set; }
        public double pphAmt { get; set; }
        public string? statusPPH { get; set; }
        public string? StatusSalesNotes { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public int ChannelId { get; set; }
        public double PPNAmt { get; set; }
        public string? statusPPN { get; set; }
        //#835
        public string? DNCreator { get; set; }
    }


}
