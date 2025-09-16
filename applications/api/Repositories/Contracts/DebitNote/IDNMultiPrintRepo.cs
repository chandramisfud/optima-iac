using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNMultiPrintRepo
    {
        // Select SubAccount
        Task<IList<SubAccountforDNMultiprint>> GetSubAccountList();
        // user/getbyprimarykey
        Task<UserForDNMultiPrint> GetUserById(int userId);
        // debetnote/list
        Task<DNMultiPrintLandingPage> GetDNListLandingPage(
                           string period,
                           int entityId,
                           int distributorId,
                           int channelId,
                           int accountId,
                           string profileId,
                           bool isdnmanual,
                           string search,
                           string sortColumn,
                           int pageNum = 0,
                           int dataDisplayed = 10,
                           string sortDirection = "ASC"
                           );
       // promo/getPromoForDn/
       Task<IList<PromoforDN>> GetApprovedPromoforDN(string period, int entity, int channel, int account, string userid);
        // debetnote/print/
        Task<DNPrint> DNPrint(int id);
    }
}