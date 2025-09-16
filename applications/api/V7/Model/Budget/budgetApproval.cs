using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using V7.Model.UserAccess;

namespace V7.Model.Budget
{
    public class budgetApprovalLPParam
    {
        public string? year { get; set; }
        public int entity { get; set; } 
        public int distributor { get; set; }
        public int budgetParent { get; set; }
        public int channel { get; set; }
    }

    public class BudgetApprovalApproveParam
    {
        public int budgetId { get; set; }
    }

    public class BudgetApprovalUnapproveParam
    {
        public int budgetId { get; set; }
        public string? notes { get; set; }
    }

}
