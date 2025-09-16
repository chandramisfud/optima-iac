
using Repositories.Entities.Configuration;

namespace Repositories.Entities.Models
{
    public class PromoReconciliationPage
    {
        public int promoId { get; set; }
        public string? RefId { get; set; }
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
        public string? ActivityDesc { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public string? CreateBy { get; set; }
        public string? AccountDesc { get; set; }
        public string? SubAccountDesc { get; set; }
        public double bfrInvestment { get; set; }
        public double bfrRoi { get; set; }
        public double bfrCostRatio { get; set; }
        public double Investment { get; set; }
        public double Roi { get; set; }
        public double CostRatio { get; set; }
        public double Invoiced { get; set; }
        public string? ReconStatus { get; set; }
        public string? LastStatus { get; set; }
        public bool IsCancelLocked { get; set; }
        public string? TsCoding { get; set; }
        public string? CancelNotes { get; set; }
        public string? initiator_notes { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }
        // Added March 10 2023 by AND
        public bool isClose { get; set; }
        // Added May 24 2023 by AND #875
        public bool allowedit { get; set; }

        // Added, andrie Oct 9 2023 E2#38
        public int categoryId { get; set; }
        public string? categoryShortDesc { get; set; }

    }
    public class RecordTotal
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
    public class PromoReconciliationPagination
    {
        public IList<PromoReconciliationPage>? Data { get; set; }
        public RecordTotal? RecordsTotal { get; set; }
    }

    //}
    public class PromoReconByIdDto
    {
        public PromoReconHeader? PromoHeader { get; set; }
        public List<PromoAttibuteById>? Regions { get; set; }
        public List<PromoAttibuteById>? Channels { get; set; }
        public List<PromoAttibuteById>? SubChannels { get; set; }
        public List<PromoAttibuteById>? Accounts { get; set; }
        public List<PromoAttibuteById>? SubAccounts { get; set; }
        public List<PromoAttibuteById>? Activity { get; set; }
        public List<PromoAttibuteById>? SubActivity { get; set; }
        public IList<PromoAttibuteById>? Brands { get; set; }
        public IList<PromoAttibuteById>? Skus { get; set; }
        public IList<PromoAttachmentById>? attachments { get; set; }
        public List<ListApprovalStatusById>? listApprovalStatus { get; set; }
        public IList<SKPValidation>? SKPValidations { get; set; }
        public IList<PromoApproval.PromoReconInvestment>? Investments { get; set; }
        public IList<MechanismById>? Mechanisms { get; set; }
        public List<object>? GroupBrand { get; set; }
        public IList<object>? PromoConfigItem { get; set; }

    }

    public class PromoReconHeader
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

        public string? PrincipalShortDesc { get; set; }
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

        public decimal investmentBfrClose { get; set; }
        public decimal investmentClosedBalance { get; set; }
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

        //added by AND Sept 16 2023
        public string? statusApprovalCode { get; set; }
        public bool isCancel { get; set; }
        public bool isClose { get; set; }
        public bool isCancelLocked { get; set; }
        public double totalClaim { get; set; }
        public double totalPaid { get; set; }
    }
}
