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
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum sortBudgetAllocationField
    {
        periode, 
        refId,
        longDesc, 
        budgetType, 
        budgetAmount,
        distributor

    }
    public class budgetAllocationLPParam : LPParam
    {
        public string? year { get; set; }
        public int entity { get; set; } 
        public int distributor { get; set; }
        public int budgetParent { get; set; }
        public int channel { get; set; }

        public sortBudgetAllocationField SortColumn { get; set; } = sortBudgetAllocationField.refId;
        public sortDirection SortDirection { get; set; } = sortDirection.asc;

    }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum enBudgetAllocationSource
    {
        BAL,
        BTR

    }
    public class BudgetAllocationSourceParam : LPParam
    {
        public string? year { get; set; } 
        public int entity { get; set; }
        public int distributor { get; set; }
        public enBudgetAllocationSource budgetType { get; set; }
    }

    public class BudgetAssignmentAllocationParam : LPParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        
    }

    public class BudgetAllocationSourceByIDParam
    {
       
        public int id { get; set; }        
        public enBudgetAllocationSource budgetType { get; set; }
    }
}
