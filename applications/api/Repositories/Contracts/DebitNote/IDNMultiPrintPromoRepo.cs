using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNMultiPrintPromoRepo
    {
        // Select Entity
        Task<IList<FinPromoDisplayEntityList>> GetEntityList();

        // promov3/getrecon
        Task<PromoRecon> GetPromoReconPromoDisplay(int Id);

        // promo/multiprint/
        Task<IList<PromoMultiprint>> GetPromoMultiprint(string period, int entityId, int distributorId, int BudgetParent, int channelId, string userid, bool cancelstatus);

        // promo/bystatus/
        Task<IList<PromoView>> GetPromoByConditionsByStatus(string period, int entityId, int distributorId, int BudgetParent, int channelId, string userid, bool cancelstatus);

        // user/getbyprimarykey/
        Task<UserProfileDataById> GetById(string id);

    }
}