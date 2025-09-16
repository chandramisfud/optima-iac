using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class BudgetHeaderStore
    {
       public int id { get; set; }
        public string? Periode { get; set; }
        public string? BudgetType { get; set; }
        public int DistributorId { get; set; }
        public string? OwnerId { get; set; }
        public string? FromOwnerId { get; set; }
        public int BudgetMasterId { get; set; }
        public int BudgetSourceId { get; set; }
        public double SalesAmount { get; set; }
        public double BudgetAmount { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? UserId { get; set; }
    }

    public class BudgetAttributeStore
    {
        public int Id { get; set; }
    }

    public class BudgetStringAttributeStore
    {
        public string? Id { get; set; }
    }


    public class BudgetAllocationStoreDto
    {
        public BudgetHeaderStore? BudgetHeader { get; set; }
        public IList<BudgetAttributeStore>? Regions { get; set; }
        public IList<BudgetAttributeStore>? Channels { get; set; }
        public IList<BudgetAttributeStore>? SubChannels { get; set; }
        public IList<BudgetAttributeStore>? Accounts { get; set; }
        public IList<BudgetAttributeStore>? SubAccounts { get; set; }
        public IList<BudgetAttributeStore>? Brands { get; set; }
        public IList<BudgetAttributeStore>? Products { get; set; }
        public IList<BudgetStringAttributeStore>? UserAccess { get; set; }
        public IList<BudgetAllocationDetailStoreDto>? BudgetDetail { get; set; }

    }
    public class BudgetAllocationDetailStoreDto
    {
        public int AllocationId { get; set; }
        public int LineIndex { get; set; }
        public int SubcategoryId { get; set; }
        public int ActivityId { get; set; }
        public int SubactivityId { get; set; }
        public double BudgetAmount { get; set; }
        public string? LongDesc { get; set; }

    }

    public class BudgetAllocationUpdateDto
    {
        public BudgetHeaderStore? BudgetHeader { get; set; }
        public IList<BudgetAttributeStore>? Regions { get; set; }
        public IList<BudgetAttributeStore>? Channels { get; set; }
        public IList<BudgetAttributeStore>? SubChannels { get; set; }
        public IList<BudgetAttributeStore>? Accounts { get; set; }
        public IList<BudgetAttributeStore>? SubAccounts { get; set; }
        public IList<BudgetAttributeStore>? Brands { get; set; }
        public IList<BudgetAttributeStore>? Products { get; set; }
        public IList<BudgetStringAttributeStore>? UserAccess { get; set; }
        public IList<BudgetAllocationDetailStoreDto>? BudgetDetail { get; set; }

    }
}
