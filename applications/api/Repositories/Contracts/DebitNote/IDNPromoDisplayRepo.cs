using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNPromoDisplayRepo
    {
        // promov3/display/distributor
        Task<DNPromoDisplayDistPaging> GetDNPromoDisplayLandingPage(
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
        // promov3/getbyprimaryid
        Task<DNPromoDisplayId> GetDNPromoDisplaybyId(int id);

        // select Entity List
        Task<IList<FinPromoDisplayEntityList>> GetEntityList();

        // promov3/getrecon
        Task<DNPromoDisplayPromoRecon> GetPromoReconPromoDisplay(int Id);
        Task<DNPromoDisplayGetDistributorId> GetDistributorId(string UserId);


    }
}