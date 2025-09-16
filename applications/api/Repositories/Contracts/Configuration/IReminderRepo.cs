using Repositories.Entities.Configuration;

namespace Repositories.Contracts
{
    public interface IReminderRepo
    {
        Task<List<ConfigReminderList>> GetListReminder(int remindertype);
        Task<bool> UpdateReminder(ConfigReminderListUpdate reminder);

    }
}