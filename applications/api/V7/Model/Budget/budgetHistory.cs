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
    public enum sortBudgetHistoryField
    {
        refId,
        longDesc,
        prevAmount,
        budgetAmount,
        notes
    }
    public class budgetHistoryLPParam : LPParam
    {
        public string? year { get; set; }
        public int entity { get; set; } 
        public int distributor { get; set; }
        public int budgetParent { get; set; }
        public int channel { get; set; }

        public sortBudgetHistoryField SortColumn { get; set; } = sortBudgetHistoryField.refId;
        public sortDirection SortDirection { get; set; } = sortDirection.asc;

    }
}
