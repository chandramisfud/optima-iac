using Repositories.Entities.Configuration;

namespace Repositories.Contracts
{
    public interface IPromoInitiatorReminderRepo
    {
        Task<IList<ConfigPromoInitiatorReminderList>> ConfigPromoInitiatorReminderList();
        Task<bool> UpdateConfigReminderPromoInitiator(ConfigPromoInitiatorReminderListUpdate body);

    }
}