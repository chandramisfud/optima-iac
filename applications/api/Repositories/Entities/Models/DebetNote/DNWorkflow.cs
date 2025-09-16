namespace Repositories.Entities.Models.DN
{
    public class PromoWorkflowResult
    {
        public IList<PromoWorkflowDto>? Promo { get; set; }
        public IList<RegionDtos>? Region { get; set; }
        public IList<ChannelDtos>? Channel { get; set; }
        public IList<SubChannelDtos>? SubChannel { get; set; }
        public IList<AccountDtos>? Account { get; set; }
        public IList<SubAccountDtos>? SubAccount { get; set; }
        public IList<BrandDtos>? Brand { get; set; }
        public IList<SkuDtos>? Sku { get; set; }
        public IList<ActivitiesDtos>? Activity { get; set; }
        public IList<SubActivitiesDtos>? SubActivity { get; set; }
        public IList<FileAttachDtos>? FileAttach { get; set; }
        public IList<MasterStatusApprovalDtos>? StatusApproval { get; set; }
        public IList<InvestmentDtos>? Investment { get; set; }
        public IList<PromoStatusDtos>? StatusPromo { get; set; }
        public IList<MechanismSelect>? Mechanism { get; set; }
        public IList<object>? GroupBrand { get; set; }

    }
    public class PromoWorkflowDto
    {
        public int PromoPlanId { get; set; }
        public string? RefId { get; set; }
        public int Id { get; set; }
        public string? PromoPlanRefId { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
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
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public DateTime? StartPromo { get; set; }
        public DateTime? EndPromo { get; set; }
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

        //Add andrie Sept 25 2023
        public string? SubCategoryDesc { get; set; }
        public int BudgetSourceId { get; set; }
        public double BudgetAmount { get; set; }
        public double RemainingBudget { get; set; }
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
        public DateTime? CutOffClaim { get; set; }
        public string? CancelReason { get; set; }
        public string? Notes { get; set; }
        public string? ApprovalNotes { get; set; }
        public string? TsCoding { get; set; }
        public int IsClose { get; set; }
        public double PlanInvestment { get; set; }
        public double PlanNormalSales { get; set; }
        public double PlanIncrSales { get; set; }
        public double PlanTotSales { get; set; }
        public double PlanRoi { get; set; }
        public double PlanCostRatio { get; set; }
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
        public double TotalClaim { get; set; }
        public double TotalPaid { get; set; }
        public string? laststatus { get; set; }
    }

    public class RegionDtos
    {
        public int flag { get; set; }
        public int RegionPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class ChannelDtos
    {
        public int flag { get; set; }
        public int ChannelPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class SubChannelDtos
    {
        public int flag { get; set; }
        public int SubChannelPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class AccountDtos
    {
        public int flag { get; set; }
        public int AccountPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class SubAccountDtos
    {
        public int flag { get; set; }
        public int SubAccountPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class BrandDtos
    {
        public int flag { get; set; }
        public int ProductPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class SkuDtos
    {
        public int flag { get; set; }
        public string? ProductPromoId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class ActivitiesDtos
    {
        public int ActivityAllocId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class SubActivitiesDtos
    {
        public int SubActivityAllocId { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }
    public class FileAttachDtos
    {
        public int PromoId { get; set; }
        public string? FileName { get; set; }
        public string? DocLink { get; set; }
    }
    public class MasterStatusApprovalDtos
    {
        public string? StatusCode { get; set; }
        public string? StatusDesc { get; set; }
    }

    public class InvestmentDtos
    {
        public double Investment { get; set; }
        public double NormalSales { get; set; }
        public double IncrSales { get; set; }
        public double TotSales { get; set; }
        public double Roi { get; set; }
        public double CostRatio { get; set; }
    }

    public class PromoStatusDtos
    {
        public int ongoingapproval { get; set; }
        public int fullyapproved { get; set; }
        public int stsongoingapproval { get; set; }
        public int stsfullyapproved { get; set; }
        public int claimprocess { get; set; }
        public int closed { get; set; }
        public int cancelled { get; set; }
    }

    public class PromoWorkflowBodyRequest
    {
        public string? RefId { get; set; }
    }

    public class PromoWorkFlowChanges
    {
        public string? date { get; set; }
        public string? userid { get; set; }
        public string? loginemail { get; set; }
        public string? field { get; set; }
        public string? oldvalue { get; set; }
        public string? newvalue { get; set; }

    }

    public class PromoWorkflowHistory
    {
        public string? date { get; set; }
        public string? userid { get; set; }
        public string? loginemail { get; set; }
        public string? status { get; set; }
        public double investment { get; set; }
        //added: AND - 20221025
        public string? reason { get; set; }
    }

    //Added: AND 2022Nov10
    public class PromoWorkflowDN
    {
        public int id { get; set; }
        public string? refid { get; set; }
        public string? laststatus { get; set; }
        public bool vatexpired { get; set; }
        public string? fpnumber { get; set; }
        public DateTime? fpdate { get; set; }
        public string? taxlevel { get; set; }
        public double dnamount { get; set; }
        public string? feedesc { get; set; }
        public double feepct { get; set; }
        public double feeamount { get; set; }
        public double dpp { get; set; }
        public double ppnpct { get; set; }
        public double ppnamt { get; set; }
        public double pphpct { get; set; }
        public double pphamt { get; set; }
        public double totalclaim { get; set; }
        public double totalpaid { get; set; }
        public string? row1 { get; set; }
        public string? row2 { get; set; }
        public string? row3 { get; set; }
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
       public class DNIdWorkflow
    {
        public IList<StatusResult>? statusresult { get; set; }
        public IList<DebetNoteResult>? debetnoteresult { get; set; }
        public IList<SellingPointResult>? sellingpointresult { get; set; }
        public IList<FileAttachResult>? fileattactresult { get; set; }
        public IList<DocumentCompletenessResult>? doccompletenessresult { get; set; }
    }


    public class DNIdWorkflowChange
    {
        public string? dtime { get; set; }
        public string? userid { get; set; }
        public string? loginemail { get; set; }
        public string? field { get; set; }
        public string? oldvalue { get; set; }
        public string? newvalue { get; set; }
    }
    public class DNIdWorkflowHistory
    {
        public string? strdtime { get; set; }
        public string? userid { get; set; }
        public string? loginemail { get; set; }
        public string? status { get; set; }
        public double DPP { get; set; }
        public string? action { get; set; }
        public string? dtime { get; set; }
    }
    public class DebetnoteIdWorkFlowBodyReq
    {
        public string? refid { get; set; }
    }

    public class StatusResult
    {
        public bool Distributor { get; set; }
        public bool Waiting_Validation { get; set; }
        public bool Payment_Process { get; set; }
        public bool Paid { get; set; }
        public bool Cancelled { get; set; }
    }
}