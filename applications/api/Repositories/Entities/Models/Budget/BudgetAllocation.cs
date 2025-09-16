using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
//using System.Threading.Channels;
using System.Threading.Tasks;

namespace Repositories.Entities.BudgetAllocation
{

    public class BudgetAllocationView
    {
        public int id { get; set; }
        public string? refId { get; set; }
        public string? mRefId { get; set; }
        public int BudgetSourceId { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryDesc { get; set; }
        public int BudgetMasterId { get; set; }
        public int periode { get; set; }
        public string? budgetType { get; set; }
        //   public string? shortDesc { get; set; }
        public string? longDesc { get; set; }
        public string? mLongDesc { get; set; }
        public string? entity { get; set; }
        //public string? entityLongDesc { get; set; }
        //public string? entityShortDesc { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorName { get; set; }
        public int PrincipalId { get; set; }
        public string? distributor { get; set; }
        // public string? distributorShortDesc { get; set; }
        public string? ownerId { get; set; }
        public string? ownerName { get; set; }
        public string? fromOwnerId { get; set; }
        public string? fromOwnerName { get; set; }
        public decimal salesAmount { get; set; }
        public decimal budgetAmount { get; set; }
        public decimal remainingBudget { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string? statusApproval { get; set; }
        public bool? isActive { get; set; }
        public bool? isLocked { get; set; }
        public DateTime? createOn { get; set; }
        public string? createBy { get; set; }
        public DateTime? modifiedOn { get; set; }
        public string? modifiedBy { get; set; }
    }
    public class BudgetUnAllocatedList
    {
        public List<BudgetAllocatedSource>? budget { get; set; }
    }
    public class BudgetAllocatedSource    {
        public int id { get; set; }
        public string? refId { get; set; }
     //   public string? BudgetMasterLongDesc { get; set; }
        public string? Desc { get; set; }
        public decimal budgetAmount { get; set; }
        public string? periode { get; set; }
    }
    public class BudgetBalanceDto
    {
        public int AllocationId { get; set; }
        public string? LongDesc { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal ReserveAmount { get; set; }
        public decimal UseAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal AssignedFromParent { get; set; }
    }
    public class SubChannel
    {
        public bool flag { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int ChannelId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ChannelLongDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
    }
    public class SubAccount
    {
        public bool flag { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int AccountId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? AccountLongDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
    }
    public class Account
    {
        public bool flag { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int SubChannelId { get; set; }
        public string? SubChannelLongDesc { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
    }
    public class Activity
    {
        public bool flag { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? RefId { get; set; }
    }
    public class SubActivity
    {
        public bool flag { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategory { get; set; }
        public string? ActivityId { get; set; }
        public string? TypeId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ActivityLongDesc { get; set; }
        public string? SubActivityTypeId { get; set; }
        public string? SubActivityTypeLongDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? RefId { get; set; }
    }
    public class BudgetAllocationChildsTypeDto
    {
        public bool flag { get; set; }
        public int ParentId { get; set; }
        public string? Id { get; set; }
        public bool IsActive { get; set; }
    }
    public class SubCategory
    {
        public bool flag { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? CategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CategoryLongDesc { get; set; }
        public string? CategoryShortDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
    }
    public class Brand
    {
        public bool flag { get; set; }
        public int Id { get; set; }
        public int PrincipalId { get; set; }
        public string? PrincipalLongDesc { get; set; }
        public string? PrincipalShortDesc { get; set; }
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
    public class Channel
    {
        public bool flag { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
    }
    public class Product
    {
        public bool flag { get; set; }
        public int Id { get; set; }
        public int PrincipalId { get; set; }
        public string? PrincipalShortDesc { get; set; }
        public string? PrincipalLongDesc { get; set; }
        public int BrandId { get; set; }
        public string? BrandShortDesc { get; set; }
        public string? BrandLongDesc { get; set; }
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
        public int SeqNo { get; set; }
    }

    public class ViewSubAccount
    {
        public int Id { get; set; }
        public string? ChannelRefID { get; set; }
        public string? SubChannelRefID { get; set; }
        public int AccountId { get; set; }
        public string? AccountRefID { get; set; }
        public string? RefID { get; set; }
        public string? ChannelDesc { get; set; }
        public string? SubChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class BudgetAssignmentDetailDto
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int AssignmentId { get; set; }
        public string? OwnId { get; set; }
        public string? Desc { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal BudgetAssigned { get; set; }
        public decimal Spending { get; set; }
        public decimal Remaining { get; set; }
        public string? Periode { get; set; }
        public int BudgetSourceId { get; set; }
    }
    /// <summary>?
    /// 
    /// </summary>?
    public class BudgetAllocationDto
    {
        public BudgetBalanceDto? BudgetParent { get; set; }
        public BudgetAssignmentDetailDto? BudgetAssignment { get; set; }
        public BudgetAllocationView? BudgetAllocation { get; set; }
        public List<Region>? Regions { get; set; }
        public List<Channel>? Channels { get; set; }
        public List<SubChannel>? SubChannels { get; set; }
        public List<Account>? Accounts { get; set; }
        public List<SubAccount>? SubAccounts { get; set; }
        public List<SubCategory>? SubCategory { get; set; }
        public List<Activity>? Activity { get; set; }
        public List<SubActivity>? SubActivity { get; set; }
        public List<Brand>? Brand { get; set; }
        public List<Product>? Product { get; set; }
        public List<BudgetAllocationChildsTypeDto>? UserAccess { get; set; }
        public List<BudgetAllocationChildsTypeDto>? UserAssign { get; set; }
        public List<BudgetAllocationChildsTypeDto>? UserPromo { get; set; }
        public List<BudgetAllocationDetailDto>? BudgetDetail { get; set; }
    }
    public class BudgetAllocationDetailDto
    {
        public int AllocationId { get; set; }
        public int LineIndex { get; set; }
        public int subcategory { get; set; }
        public string? subcategorydesc { get; set; }
        public int activity { get; set; }
        public string? activitydesc { get; set; }
        public int subactivity { get; set; }
        public string? subactivitydesc { get; set; }
        public decimal BudgetAmount { get; set; }
        public string? LongDesc { get; set; }

    }
    public class Region
    {
        public bool flag { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
    }

    public class BudgetAllocationSourceById
    {
        public BudgetAssignmentDetailDto? BudgetAssignment { get; set; }
        public BudgetAllocationView? BudgetAllocation { get; set; }
        public List<Region>? Regions { get; set; }
        public List<Channel>? Channels { get; set; }
        public List<SubChannel>? SubChannels { get; set; }
        public List<Account>? Accounts { get; set; }
        public List<SubAccount>? SubAccounts { get; set; }
        public List<SubCategory>? SubCategory { get; set; }
        public List<Activity>? Activity { get; set; }
        public List<SubActivity>? SubActivity { get; set; }
        public List<Brand>? Brand { get; set; }
        public List<Product>? Product { get; set; }
        public List<BudgetAllocationChildsTypeDto>? UserAccess { get; set; }
        public List<BudgetAllocationChildsTypeDto>? UserAssign { get; set; }
        public List<BudgetAllocationChildsTypeDto>? UserPromo { get; set; }
        public List<BudgetAllocationDetailDto>? BudgetDetail { get; set; }
    }

    public class BudgetMasterView
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? Periode { get; set; }
        public int DistributorId { get; set; }
        public int PrincipalId { get; set; }
        public string? OwnerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal BudgetAmount { get; set; }
        public bool IsAllocated { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryLongDesc { get; set; }
        public string? CategoryShortDesc { get; set; }
        public string? DistributorLongDesc { get; set; }
        public string? DistributorShortDesc { get; set; }

        public string? PrincipalLongDesc { get; set; }
        public string? PrincipalShortDesc { get; set; }
    }

    public class UserAllDto
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

    public class MasterIdDesc 
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
    }
}
