using Repositories.Entities.Models;

namespace Repositories.Entities.Configuration
{

    public class PromoDataReminder
    {
        public int period { get; set; }
        public int promoId { get; set; }
        public string promoRefId { get; set; }
        public int distributorId { get; set; }
        public string distributor { get; set; }
        public string initiatorName { get; set; }
        public string initiatorBy { get; set; }
        public int subAccountId { get; set; }
        public string subAccount { get; set; }
        public int groupBrandId { get; set; }
        public string groupBrand { get; set; }
        public string mechanism { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }
        public decimal cost { get; set; }
        public decimal cr { get; set; }
        public decimal roi { get; set; }
        public string fileAttachment { get; set; }
        public List<string> lsFileAttachment
        {
            get
            {
                return string.IsNullOrWhiteSpace(fileAttachment)
                    ? new List<string>()
                    : fileAttachment.Split('|').ToList();
            }
        }
        public string profileBy { get; set; }
        public string profileName { get; set; }
        public string profileEmail { get; set; }
        public int promoCycle { get; set; }
        public int approver {  get; set; }
    }

    public class approverParamKey
    {
        public string promoId { get; set; }
        public string refId { get; set; }
        public string profileId { get; set; }
        public string nameApprover { get; set; }
        public string sy { get; set; }
    }

    public class PromoSendBackDataReminder : PromoDataReminder
    {
        public string SendBackReason { get; set; }
    }

    public class ReminderList
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string? RefId { get; set; }
        public int PromoPlanId { get; set; }
        public int AllocationId { get; set; }
        public string? AllocationRefId { get; set; }
        public string? CategoryShortDesc { get; set; }
        public string? PrincipalShortDesc { get; set; }
        public int BudgetMasterId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ActivityId { get; set; }
        public int SubActivityId { get; set; }
        public string? ActivityDesc { get; set; }
        public DateTime? StartPromo { get; set; }
        public DateTime? EndPromo { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public string? Notes { get; set; }
        public decimal Roi { get; set; }
        public decimal CostRatio { get; set; }
        public decimal NormalSales { get; set; }
        public decimal IncrSales { get; set; }
        public decimal TotSales { get; set; }
        public decimal TotInvest { get; set; }
        public string? Status { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsCancel { get; set; }
        public decimal PrevRoi { get; set; }
        public decimal PrevCostRatio { get; set; }
        public decimal PrevNormalSales { get; set; }
        public decimal PrevIncrSales { get; set; }
        public decimal PrevTotInvest { get; set; }
        public decimal PrevTotSales { get; set; }
        public string? TsCoding { get; set; }
        public string? userapprover { get; set; }
        public DateTime? requestdate { get; set; }
        public bool isongoing { get; set; }
        public int aging { get; set; }
        public string? reminder_category { get; set; }
        public int frequency { get; set; }
        public bool send { get; set; }
        public string? initiatiorEmail { get; set; }
        public string? UserApproverEmail { get; set; }
        public int ReconStatus { get; set; }
        public string? UserApproverName { get; set; }
    }

    public class AutoCloseDto
    {
        public int id { get; set; }
        public int PromoPlanId { get; set; }
        public string? RefId { get; set; }
        public string? StatusApprovalCode { get; set; }
        public string? StatusApproval { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int dayfreeze { get; set; }
        public string? CreateBy { get; set; }
        public string? emailInitiator { get; set; }
        public string? Approver { get; set; }
        public string? emailApprover { get; set; }
    }

    public class ReminderPendingApproval
    {
        public IList<ReminderPending1>? Aging { get; set; }
        public IList<ReminderPendingPromo>? PendingPromo { get; set; }
        public IList<ReminderPendingEmail>? EmailPending { get; set; }
    }

    public class ReminderPending1
    {
        public string? num { get; set; }
        public string? category { get; set; }
        public string? UserID { get; set; }
        public string? PIC { get; set; }
        public string? qty15 { get; set; }
        public string? val15 { get; set; }
        public string? qty610 { get; set; }
        public string? val610 { get; set; }
        public string? qty10 { get; set; }
        public string? val10 { get; set; }
        public string? qtytot { get; set; }
        public string? valtot { get; set; }
    }
    public class ReminderPendingPromo
    {
        public string? promo_id { get; set; }
        public string? last_status { get; set; }
        public string? promo_initiator { get; set; }
        public string? initiator_name { get; set; }
        public string? creation_date { get; set; }
        public string? Channel { get; set; }
        public string? sub_account { get; set; }
        public string? promo_start { get; set; }
        public string? promo_end { get; set; }
        public string? activity_name { get; set; }
        public string? mechanism_1 { get; set; }
        public string? investment { get; set; }
        public string? aging { get; set; }

    }
    public class ReminderPendingEmail
    {
        public string? email_to { get; set; }
        public string? email_cc { get; set; }
    }

    public class PromoCreationPagination
    {
        public IList<PromoCreationPage>? Data { get; set; }
        public RecordTotal? RecordsTotal { get; set; }
    }

    public class PromoCreationPage
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
        public DateTime? StartPromo { get; set; }
        public DateTime? EndPromo { get; set; }
        public string? initiator_notes { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public string? CreateBy { get; set; }
        public double dnclaim { get; set; }
        public string? CreateOn { get; set; }
        public string? LastStatusDate { get; set; }
        public int isClose { get; set; }
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }
    }

    public class SchedulerStandardResult
    {
        public bool error { get; set; }
        public string? message { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int statuscode { get; set; }
        public string? userid_approver { get; set; }
        public string? username_approver { get; set; }
        public string? email_approver { get; set; }
        public string? userid_initiator { get; set; }
        public string? username_initiator { get; set; }
        public string? email_initiator { get; set; }
        public bool IsFullyApproved { get; set; }
        public bool IsFullyApprovedRecon { get; set; }
        public bool major_changes { get; set; }
    }

    public class PromoForScheduler
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

    public class PromoReconScheduler
    {
        public SchedulerPromoRecon? PromoHeader { get; set; }
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
}