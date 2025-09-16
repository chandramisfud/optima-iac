using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNOverBudgetRepo
    {
        // debetnote/listrefreshoverbudget
        Task<IList<DNOverBudgetList>> GetDNOverBudgetList(string periode, int entityId, int distributorId, string channelId, string accountId, string userid, bool isdnmanual);

        // debetnote/dnupdateoverbudget
        Task DNUpdateOverBudget(int PromoId);        
    }
}