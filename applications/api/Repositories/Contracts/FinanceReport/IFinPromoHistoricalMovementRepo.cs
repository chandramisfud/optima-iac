using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IFinPromoHistoricalMovementRepo
    {
        Task<PromoHistoricalMovementLandingPage> GetPromoHistoricalMovementLandingPage(
            string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId,
            string search, int pageNum = 0, int dataDisplayed = 10
        );

        Task<IList<PromoHistoricalMovementEntityList>> GetEntityList();

        Task<IList<PromoHistoricalMovementDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
    }
}
