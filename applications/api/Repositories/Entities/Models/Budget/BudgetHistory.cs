using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class BudgetHistoryDto
    {
        public string? refId { get; set; }
        public int  allocationId { get; set; }
        public string? longDesc { get; set; }
        public decimal prevAmount { get; set; }
        public decimal budgetAmount { get; set; }
        public string? notes { get; set; }
    }

}