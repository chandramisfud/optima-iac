using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Models
{
    public class BudgetMasterDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? RefId { get; set; }
        public string? Periode { get; set; }
        public int DistributorId { get; set; }
        public int PrincipalId { get; set; }
        public string? OwnerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal BudgetAmount { get; set; }
        public bool IsAllocated { get; set; }
        public string? BudgetMasterLongDesc { get; set; }
        public string? BudgetMasterShortDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public int CategoryId { get; set; }
        public string? DistributorLongDesc { get; set; }
        public string? DistributorShortDesc { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Npwp { get; set; }
        public string? PrincipalLongDesc { get; set; }
        public string? PrincipalShortDesc { get; set; }
    }

    public partial class BudgetMasterSaveDto
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? Periode { get; set; }
        public int DistributorId { get; set; }
        public int PrincipalId { get; set; }
        public string? OwnerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double BudgetAmount { get; set; }
        public bool IsAllocated { get; set; }
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
        public int CategoryId { get; set; }
    }

    public class BudgetMasterDeleteDto
    {
        public int Id { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }

    }
}
