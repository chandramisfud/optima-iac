using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IListingPromoReportingRepo
    {
        Task<ListingPromoReportingLandingPage> GetListingPromoReportingLandingPage(
            string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId,
            string createFrom, string createTo, string startFrom, string startTo, int submissionParam,
            string keyword, string sortColumn, int categoryId=0,
            string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10
        );

        Task<IList<ListingPromoReportingEntityList>> GetEntityList();

        Task<IList<ListingPromoReportingDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);

        Task<IList<ListingPromoReportingChannelList>> GetChannelList(string userid, int[] arrayParent);
        Task<IList<object>> GetCategoryDropdownList();
        Task<object> GetListingPromoReportingByMechanismLP(string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId, string createFrom, string createTo, string startFrom, string startTo, int submissionParam, string keyword, string sortColumn, int categoryId = 0, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10);
    }
}
