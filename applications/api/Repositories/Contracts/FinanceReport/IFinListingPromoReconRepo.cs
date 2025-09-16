using Repositories.Entities.Models;
using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IFinListingPromoReconRepo
    {
        Task<ListingPromoReconLandingPage> GetListingPromoReconLandingPage(
            string period, int entityId, int distributorId, int budgetParentId, int channelId, string createForm, string createTo, string startFrom, string startTo,
            int submissionParam, string[] profileId,
            string search, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10
        );

        Task<IList<ListingPromoReconEntityList>> GetEntityList();
        Task<IList<ListingPromoReconDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
        Task<IList<ListingPromoReportingChannelList>> GetChannelList(string userid, int[] arrayParent);
        Task<IList<GetUserGroupforPromoRecon>> GetUserGroupsforFinPromoRecon(string[] id);


    }
}
