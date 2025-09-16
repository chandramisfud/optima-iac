using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IInvestmentReportRepo
    {
        Task<InvestmentReportLandingPage> GetInvestmentReportLandingPage(
            string search, string sortColumn, 
            string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId,
            int pageNum = 0, int dataDisplayed = 10, string sortDirection = "ASC"
        );

        Task<IList<InvestmentBudgetAllocationList>> GetBudgetAllocationList(string year, int entityId, int distributorId, int budgetParentId, int channelId, string userId);

        Task<IList<InvestmentEntityList>> GetEntityList();

        Task<IList<InvestmentDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);

    }
}
