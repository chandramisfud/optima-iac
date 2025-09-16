using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class BudgetApprovalView
    {
        public int id { get; set; }
        public int periode { get; set; }
        public string? refId { get; set; }
        public string? longDesc { get; set; }
        public string? budgetAmount { get; set; }
        public string? distributorName { get; set; }
        public string? ownerId { get; set; }
        public string? ownerName { get; set; }
        public string? fromOwnerId { get; set; }
        public string? fromOwnerName { get; set; }
        public string? statusApproval { get; set; }
        public DateTime? createOn { get; set; }
        public string? createBy { get; set; }
        public DateTime? modifiedOn { get; set; }
        public string? modifiedBy { get; set; }
    }

    public partial class BudgetApprovalApproveDto
    {
        public int budgetId { get; set; }
        public string? statsuApproval { get; set; }
        public string? notes { get; set; }
        public string? profileId { get; set; }        
    }

}