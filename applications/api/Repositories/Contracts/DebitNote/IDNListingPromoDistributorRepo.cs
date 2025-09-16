using Repositories.Entities.Models;
using Repositories.Entities.Report;

namespace Repositories.Contracts
{
    public interface IDNListingPromoDistributorRepo
    {
        Task<ListingPromoReportingLandingPage> GetListingPromoReportingLandingPage(
           string period, int categoryId, int entityId, int distributorId, int budgetParentId, int channelId, string profileId,
           string createFrom, string createTo, string startFrom, string startTo, int submissionParam,
           string keyword, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10
       );

        Task<IList<ListingPromoReportingEntityList>> GetEntityList();

        Task<IList<ListingPromoReportingDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);

        Task<IList<ListingPromoReportingChannelList>> GetChannelList(string userid, int[] arrayParent);

        Task<UserProfileDataById> GetById(string id);

        Task<IList<object>> GetCategoryDropdownList();
    }
}