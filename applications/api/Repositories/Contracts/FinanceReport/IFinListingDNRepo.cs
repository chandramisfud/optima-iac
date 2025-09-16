using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IFinListingDNRepo
    {
        Task<ListingDNLandingPage> GetListingDNLandingPage(
            string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId,
            string search, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10
        );

        Task<IList<ListingDNEntityList>> GetEntityList();

        Task<IList<ListingDNDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
   
        Task<object> GetDNReadyToPayFilter(string profile);
        Task<object> GetDNReadyToPayLP(string period, int categoryId, int entityId, int distributorId, int subAccountId, string profileId, string search, int pageNum = 0, int dataDisplayed = 10);

    }
}
