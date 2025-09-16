using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities 
{ 
        public partial class BudgetAssignmentView
        {
            public int Id { get; set; }
            public string? RefId { get; set; }
            public int AllocationId { get; set; }
            public string? FrownId { get; set; }
            public string? FrownName { get; set; }
            public decimal BudgetAmount { get; set; }
            public bool? IsActive { get; set; }
            public bool IsLocked { get; set; }
            public DateTime CreateOn { get; set; }
            public string? CreateBy { get; set; }
            public DateTime ModifiedOn { get; set; }
            public string? ModifiedBy { get; set; }
            public bool? IsDelete { get; set; }
            public DateTime? DeleteOn { get; set; }
            public string? DeleteBy { get; set; }
        }

    public class BudgetAssignmentStoreDto
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int BudgetId { get; set; }
        public string? FrownId { get; set; }
        public int AllocationId { get; set; }
        public double BudgetAmount { get; set; }
        public string? UserId { get; set; }
        public List<BudgetAssignmentDetailCreate>? AssignmentDetail { get; set; }

    }

    public class BudgetAssignmentDetailCreate
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int AssignmentId { get; set; }
        public string? OwnId { get; set; }
        public string? Desc { get; set; }
        public double BudgetAmount { get; set; }
        public string? Periode { get; set; }
        public int BudgetSourceId { get; set; }
    }

    public partial class BudgetAssignmentDetail
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? OwnId { get; set; }
        public string? OwnName { get; set; }
        public string? Desc { get; set; }
        public bool IsAllocated { get; set; }
        public decimal BudgetAmount { get; set; }
        public bool? IsActive { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
    }
    public class BudgetAssignmentDto
    {
        public int Id { get; set; }
        public IList<BudgetAssignmentDetail>? AssignmentId { get; set; }
        public IList<string?>? OwnerId { get; set; }
        public int AllocationId { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public string? BudgetSource { get; set; }
        public string? FrownId { get; set; }
        public string? FrownName { get; set; }
        public decimal BudgetAmount { get; set; }
        public string? Periode { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
    }

    public class BudgetAssignmentUpdateDto
    {

        public string? UserId { get; set; }
        public string? AssignmentId { get; set; }
        public IList<BudgetAssignmentDetailUpdateDto>? AssignmentDetail { get; set; }

    }
    public class BudgetAssignmentDetailUpdateDto
    {
        public int Id { get; set; }
        public string? ownid { get; set; }
        public string? Desc { get; set; }
        public double BudgetAmount { get; set; }

    }
}
