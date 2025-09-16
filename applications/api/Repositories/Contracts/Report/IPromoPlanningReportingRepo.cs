using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IPromoPlanningReportingRepo
    {
        Task<PromoPlanningReportingLandingPage> GetPromoPlanningReportingLandingPage(
            string period, int entityId, int distributorId, int channelId, string profileId, string createForm, string createTo, string startFrom, string startTo,
            string search, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10
        );

        Task<IList<PromoPlanningReportingEntityList>> GetEntityList();

        Task<IList<PromoPlanningReportingDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);

        Task<IList<PromoPlanningReportingChannelList>> GetChannelList(string userid, int[] arrayParent);
    }
}
