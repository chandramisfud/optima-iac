namespace Repositories.Entities.Models.Dashboard
{
    public class DashboardMasterbyAccess
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
    }
    public class DashboardMainDto
    {
        public string? periode { get; set; }
        public double budget_deployed { get; set; }
        public string? budget_deployed_word { get; set; }
        public double promo_planning { get; set; }
        public string? promo_planning_word { get; set; }
        public double promo_created { get; set; }
        public string? promo_created_word { get; set; }
        public double total_claim { get; set; }
        public string? total_claim_word { get; set; }
        public double total_paid { get; set; }
        public string? total_paid_word { get; set; }
        public double pct_promo_created_vs_budget { get; set; }
        public int promoid_created { get; set; }
        public double pct_promo_approved { get; set; }
        public int promoid_approved { get; set; }
        public double pct_promo_reconciled { get; set; }
        public int promoid_reconciled { get; set; }
        public double pct_promo_created_ontime { get; set; }
        public double pct_promo_approved_ontime { get; set; }
        public double pct_submitted_claim { get; set; }
        public double avgdayscreated_bfr_promostart { get; set; }
        public double avgpromo { get; set; }
        public double claim_received { get; set; }
    }
    public class Notifications
    {
        public string? pending_promo_approval { get; set; }
        public int aging_promo_approval { get; set; }
        public string? promo_send_back { get; set; }
        public string? dn_manual { get; set; }
        public string? dn_over_budget { get; set; }
        public string? promo_plan { get; set; }
        public string? pending_promorecon_approval { get; set; }
        public string? promo_send_back_recon { get; set; }
        public string? dn_validate_by_sales { get; set; }
    }

    public class DropdownList
    {
        public List<UserGroupforDashboard>? UserGroup { get; set; }
        public List<UserLevelforDashboard>? UserLevel { get; set; }
        public List<DistributorforDashboard>? Distributor { get; set; }
        public List<EntityforDashboard>? Entity { get; set; }
        public List<CategoryforDashboard>? Category { get; set; }

    }
    public class UserGroupforDashboard
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

    public class DistributorforDashboard
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Npwp { get; set; }
        public string? RefId { get; set; }
    }

    public class EntityforDashboard
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public string? RefId { get; set; }
    }

    public class CategoryforDashboard
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? RefId { get; set; }
    }
    public class UserLevelforDashboard
    {
        public int userlevel { get; set; }
        public string? levelname { get; set; }
        public string? usergroupid { get; set; }
        public string? userinput { get; set; }
        public DateTime dateinput { get; set; }
        public string? useredit { get; set; }
        public DateTime dateedit { get; set; }
    }

    public class DNListbyPromoIdDto
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int PromoId { get; set; }
        public string? PromoRefId { get; set; }
        public double dpp { get; set; }
        public double PPNAmt { get; set; }
        public double TotalClaim { get; set; }
        public double TotalPaid { get; set; }
        public string? ActivityDesc { get; set; }
        public string? LastStatus { get; set; }
        public double NormalSales { get; set; }
        public bool IsFreeze { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public bool IsOverBudget { get; set; }
        public string? AssignBy { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int EntityId { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? MemDocNo { get; set; }
        public string? IntDocNo { get; set; }
        public string? MaterialNumber { get; set; }
        public string? TaxLevel { get; set; }
        public bool IsDNPromo { get; set; }
        public string? remarkSales { get; set; }
        public string? SalesValidationStatus { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public string? FPNumber { get; set; }
        public DateTime? FPDate { get; set; }
        public bool VATExpired { get; set; }

    }
}