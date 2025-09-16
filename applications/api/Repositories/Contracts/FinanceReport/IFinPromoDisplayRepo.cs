using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IFinPromoDisplayRepo
    {
        Task<FinPromoDisplayLandingPage> GetFinPromoDisplayLandingPage(
        string period,
        int entityId,
        int distributorId,
        int budgetParentId,
        int channelId,
        string profileId,
        bool cancelstatus,
        string keyword,
        string sortColumn,
        string sortDirection = "ASC",
        int pageNum = 0,
        int dataDisplayed = 10
        );

        Task<IList<FinPromoDisplayEntityList>> GetEntityList();
        Task<IList<FinPromoDisplayDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
        Task<PromoDisplayList> GetPromoDisplaybyId(int id);
        Task<PromoRecon> GetPromoReconPromoDisplay(int Id);
        Task<object> GetPromoDisplaypdfbyId(int id, string profile = "");
    }
}