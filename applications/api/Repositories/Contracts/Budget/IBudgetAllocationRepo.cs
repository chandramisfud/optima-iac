using Repositories.Entities.BudgetAllocation;
using Repositories.Entities.Models;
using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBudgetAllocationRepository
    {
        Task<List<BudgetAllocationView>> GetBudgetAllocationLandingPage(string year, int entity, int distributor, 
            int BudgetParent, int channel, string userid);
        
    }
}
