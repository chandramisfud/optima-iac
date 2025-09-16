using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IListingDNRepo
    {
        Task<ListingDNLandingPage> GetListingDNLandingPage(
            string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId,
            string search, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10
        );

        Task<IList<ListingDNEntityList>> GetEntityList();

        Task<IList<ListingDNDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
    }
}
