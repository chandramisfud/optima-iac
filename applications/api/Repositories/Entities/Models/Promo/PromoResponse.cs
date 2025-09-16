using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class PromoResponseDto
    {
        public bool error { get; set; }
        public string? message { get; set; }
        public int id { get; set; }
        public string? refId { get; set; }
        public int statuscode { get; set; }
        public string? userid_approver { get; set; }
        public string? username_approver { get; set; }
        public string? email_approver { get; set; }
        public string? userid_initiator { get; set; }
        public string? username_initiator { get; set; }
        public string? email_initiator { get; set; }
        public bool isFullyApproved { get; set; }
        public bool isFullyApprovedRecon { get; set; }
        public bool major_changes { get; set; }

    }

    public class PromoSourcePlanningDto
    {
        public int id { get; set; }
        public string? refId { get; set; }
        public string? tsCoding { get; set; }
        public int entityId { get; set; }
        public string? entityShortDesc { get; set; }
        public string? entityLongDesc { get; set; }
        public int distributorId { get; set; }
        public string? distributorShortDesc { get; set; }
        public string? distributorLongDesc { get; set; }
        public int categoryId { get; set; }
        public string? categoryShortDesc { get; set; }
        public string? categoryLongDesc { get; set; }
        public int subCategoryId { get; set; }
        public string? subCategoryDesc { get; set; }
        public int activityId { get; set; }
        public string? activityDesc { get; set; }
        public int subActivityId { get; set; }
        public string? subActivityDesc { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }
        public string? mechanism1 { get; set; }
        public string? mechanism2 { get; set; }
        public string? mechanism3 { get; set; }
        public string? mechanism4 { get; set; }
        public double investment { get; set; }
        public double normalSales { get; set; }
        public double incrSales { get; set; }
        public double roi { get; set; }
        public double costRatio { get; set; }
        public string? notes { get; set; }
        public DateTime createOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public string? createBy { get; set; }
        public string? initiator_notes { get; set; }

    }

    public class PromoSourceBudgetDto
    {
        public string? entity { get; set; }
        public string? distributor { get; set; }
        public int id { get; set; }
        public string? periode { get; set; }
        public string? refId { get; set; }
        public string? subCategory { get; set; }
        public string? budgetType { get; set; }
        public int distributorId { get; set; }
        public string? distributorName { get; set; }
        public string? ownerId { get; set; }
        public string? ownerName { get; set; }
        public string? fromOwnerId { get; set; }
        public string? fromOwnerName { get; set; }
        public int budgetMasterId { get; set; }
        public double budgetAmount { get; set; }
        public double remainingBudget { get; set; }
        public string? longDesc { get; set; }
        public string? shortDesc { get; set; }
        public string? statusApproval { get; set; }
        public bool isActive { get; set; }
        public bool isLocked { get; set; }
        public int activityId { get; set; }
    }

    public class PromoHeader
    {
        public int promoPlanId { get; set; }
        public string? refId { get; set; }
        public int id { get; set; }
        public string? promoPlanRefId { set; get; }
        public DateTime createOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public string? createBy { get; set; }
        public string? activityDesc { get; set; }
        public double investment { get; set; }
        public double prevInvestment { get; set; }
        public double normalSales { get; set; }
        public double prevNormalSales { get; set; }
        public double incrSales { get; set; }
        public double prevIncrSales { get; set; }
        public double prevTotSales { get; set; }
        public double roi { get; set; }
        public double prevRoi { get; set; }
        public double costRatio { get; set; }
        public double prevCostRatio { get; set; }
        public string? mechanism1 { get; set; }
        public string? mechanism2 { get; set; }
        public string? mechanism3 { get; set; }
        public string? mechanism4 { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }
        public int allocationId { get; set; }
        public string? periode { get; set; }
        public string? allocationRefId { get; set; }
        public string? budgetType { get; set; }
        public int distributorId { get; set; }
        public string? distributorName { get; set; }
        public int principalId { get; set; }
        public string? principalName { get; set; }
        public string? principalShortDesc { get; set; }
        public string? ownerId { get; set; }
        public string? budgetOwner { get; set; }
        public string? fromOwnerId { get; set; }
        public string? fromOwnername { get; set; }
        public int budgetMasterId { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int activityId { get; set; }
        public string? activityLongDesc { get; set; }
        public int subActivityId { get; set; }
        public string? subActivityLongDesc { get; set; }
        public string? categoryDesc { get; set; }
        public int budgetSourceId { get; set; }
        public double budgetAmount { get; set; }
        public double remainingBudget { get; set; }
        public string? allocationDesc { get; set; }
        public string? shortDesc { get; set; }
        public string? statusApproval { get; set; }
        public bool isActive { get; set; }
        public bool isLocked { get; set; }
        public string? userApprover1 { get; set; }
        public string? lastStatus1 { get; set; }
        public DateTime? approvalDate1 { get; set; }
        public string? userApprover2 { get; set; }
        public string? lastStatus2 { get; set; }
        public DateTime? approvalDate2 { get; set; }
        public string? userApprover3 { get; set; }
        public string? lastStatus3 { get; set; }
        public DateTime? approvalDate3 { get; set; }
        public string? userApprover4 { get; set; }
        public string? lastStatus4 { get; set; }
        public DateTime? approvalDate4 { get; set; }
        public string? userApprover5 { get; set; }
        public string? lastStatus5 { get; set; }
        public DateTime? approvalDate5 { get; set; }
        public string? regionDesc { get; set; }
        public string? channelDesc { get; set; }
        public string? accountDesc { get; set; }
        public string? brandDesc { get; set; }
        public string? productDesc { get; set; }
        public DateTime cutOffClaim { get; set; }
        public string? cancelReason { get; set; }
        public string? notes { get; set; }
        public string? approvalNotes { get; set; }
        public string? tsCoding { get; set; }
        public bool isClose { get; set; }
        public double planInvestment { get; set; }
        public double planNormalSales { get; set; }
        public double planIncrSales { get; set; }
        public double planTotSales { get; set; }
        public double planRoi { get; set; }
        public double planCostRatio { get; set; }
        public string? prevMechanisme1 { get; set; }
        public string? prevMechanisme2 { get; set; }
        public string? prevMechanisme3 { get; set; }
        public string? prevMechanisme4 { get; set; }
        public string? initiator_notes { get; set; }
        public string? sendBack_notes { get; set; }
        public string? userId_approver { get; set; }
        public string? sendBack_notes_date { get; set; }
        public int investmentTypeId { get; set; }
        public string? investmentTypeRefId { get; set; }
        public string? investmentTypeDesc { get; set; }
        public int late_submission_day { get; set; }
        public string? subCategoryDesc { get; set; }
        public string? lastStatus { get; set; }
        public string? initiator { get; set; }
        public double investmentBrfClose { get; set; }
        public double investmentClosedBalance { get; set; }
        public string? modifReason { get; set; }
        public string? statusApprovalCode { get; set; }
        public bool isCancel { get; set; }
        public bool isCancelLocked { get; set; }
    }
    public class PromoAttibuteById
    {
        public bool flag { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class PromoAttachmentById
    {
        public string? docLink { get; set; }
        public string? fileName { get; set; }

    }

    public class ListApprovalStatusById
    {
        public string? statusCode { get; set; }
        public string? statusDesc { get; set; }

    }
    public class PromoCreationByIdDto
    {
        public PromoHeader? PromoHeader { get; set; }
        public List<PromoAttibuteById>? Regions { get; set; }
        public List<PromoAttibuteById>? Channels { get; set; }
        public List<PromoAttibuteById>? SubChannels { get; set; }
        public List<PromoAttibuteById>? Accounts { get; set; }
        public List<PromoAttibuteById>? SubAccounts { get; set; }
        public List<PromoAttibuteById>? Activity { get; set; }
        public List<PromoAttibuteById>? SubActivity { get; set; }
        public IList<PromoAttibuteById>? Brands { get; set; }
        public IList<PromoAttibuteById>? Skus { get; set; }
        public IList<MechanismById>? Mechanisms { get; set; }
        public IList<PromoAttachmentById>? attachments { get; set; }
        public List<ListApprovalStatusById>? listApprovalStatus { get; set; }
        public List<Object>? Investment { get; set; }

        //Add:  AND Oct 11 2023 E2#38
        public List<Object>? GroupBrand { get; set; }

    }
    public class PromoExistDto
    {
        public int id { get; set; }
        public string? refId { get; set; }
        public string? periode { get; set; }
        public string? activityDesc { get; set; }
        public int channelId { get; set; }
        public string? channel { get; set; }
        public int accountId { get; set; }
        public string? account { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }
        public int subAccountId { get; set; }
        public string? subAccount { get; set; }
    }

    public class LatePromoDto
    {
        public int id { get; set; }
        public int reminderType { get; set; }
        public int category { get; set; }
        public string? description { get; set; }
        public int days { get; set; }
    }

    public class CancelReasonDto
    {
        public int id { get; set; }
        public string? longDesc { get; set; }
        public DateTime? createOn { get; set; }
        public string? createBy { get; set; }
        public DateTime? modifiedOn { get; set; }
        public string? modifiedBy { get; set; }
        public bool isDeleted { get; set; }
        public DateTime? deletedOn { get; set; }
        public string? deletedBy { get; set; }

    }

    public class SubCategoryPromobyCategoryId
    {
        public int id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }

    }

}