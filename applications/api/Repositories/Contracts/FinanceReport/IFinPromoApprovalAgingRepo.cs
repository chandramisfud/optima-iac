using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IFinPromoApprovalAgingRepo
    {
        Task<FinPromoApprovalAgingLandingPage> GetFinPromoApprovalAgingLandingPage(
            string period,
            int entityId,
            int distributorId,
            int budgetParentId,
            int channelId,
            string profileId,
            string search,
            string sortColumn,
            string sortDirection = "ASC",
            int pageNum = 0,
            int dataDisplayed = 10
        );
        Task<IList<FinPromoApprovalAgingEntityList>> GetEntityList();
        Task<IList<FinPromoApprovalAgingDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
    }
}