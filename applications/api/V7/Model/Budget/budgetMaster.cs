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
    public enum sortBudgetMasterField
    {
        principalLongDesc,
        distributorLongDesc,
        periode,
        refid,
        budgetMasterLongDesc,
        amount
    }
    public class budgetMasterLPParam : LPParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }

        public sortBudgetMasterField SortColumn { get; set; } = sortBudgetMasterField.refid;
        public sortDirection SortDirection { get; set; } = sortDirection.asc;

    }

    public class DistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }

    public class BudgetMasterDeleteParam
    {
        public int Id { get; set; }
    }
    public class BudgetMasterSaveParam
    {
        public string? Periode { get; set; }
        public int DistributorId { get; set; }
        public int EntityId { get; set; }
        public double BudgetAmount { get; set; }
        public string? LongDesc { get; set; }
        public int CategoryId { get; set; }
    }
    public class BudgetMasterUpdateParam
    {
        public int Id { get; set; }
        public string? Periode { get; set; }
        public int DistributorId { get; set; }
        public int EntityId { get; set; }
        public double BudgetAmount { get; set; }
        public string? LongDesc { get; set; }
        public int CategoryId { get; set; }
    }
}
