using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface ISummaryAgingApprovalRepo
    {
        Task<SummaryAgingApprovalLandingPage> GetSummaryAgingApprovalLandingPage(
            string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId,
            string search, int pageNum = 0, int dataDisplayed = 10
        );

        Task<IList<SummaryAgingApprovalEntityList>> GetEntityList();

        Task<IList<SummaryAgingApprovalDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
    }
}
