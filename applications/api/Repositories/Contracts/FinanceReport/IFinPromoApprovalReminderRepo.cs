using Entities;
using Repositories.Entities.BudgetAllocation;

namespace Repositories.Contracts
{
    public interface IFinPromoApprovalReminderRepository
    {
        Task<PromoApprovalReminderResp> GetPromoApprovalReminder(string year, string month, string month2);
        Task<object> GetPromoApprovalReminderManualEmailConfig();
        Task<PromoApprovalReminderRegularSend> GetPromoApprovalReminderRegularSend();
        Task<PromoApprovalReminderSetting> GetPromoApprovalReminderSettingById(int id);
        Task<List<object>> GetUserList(string usergroupid, int userlevel, int isdeleted);
        Task<bool> UpdatePromoApprovalReminderConfigEmail(int id, List<PromoApprovalReminderConfigEmail> configEmail, string userId, string userEmail);
        Task<bool> UpdatePromoApprovalReminderManualEmailConfig(List<PromoApprovalReminderConfigEmail> configEmail, string userId, string userEmail);
        Task<bool> UpdatePromoApprovalReminderSetting(int id, int dt1, int dt2, bool eod, bool autoRun, List<PromoApprovalReminderConfigEmail> configEmail, string userId, string userEmail);
    }
}