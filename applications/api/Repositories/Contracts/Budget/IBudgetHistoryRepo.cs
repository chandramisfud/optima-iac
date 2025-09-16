using Repositories.Entities;
using Repositories.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBudgetHistoryRepository
    {
        Task<BaseLP> GetBudgetHistoryLandingPage(string year, int entity, int distributor, int budgetParent, int channel, string profileId,
            string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize);

        Task<IEnumerable<BaseDropDownList>> GetAllEntity();
        Task<IList<BaseDropDownList>> GetDistributorList(int budgetid, int[] arrayParent);
        
    }
}
