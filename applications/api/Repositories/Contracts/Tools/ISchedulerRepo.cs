using Entities;
using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;

namespace Repositories.Contracts
{
    public interface ISchedulerRepo
    {
        Task<IList<ReminderList>> GetReminderList();
        Task<IList<AutoCloseDto>> GetAutoClosing();
        Task<IList<ReminderPendingApproval>> GetReminderPendingApproval();
        Task<IList<BlitzNotif>> BlitzTranferNotif();
        Task PromoAutoClose();
        Task<SchedulerStandardResult> CancelPromo(int promoId, string userId, string statusCode, string approverEmail);
        Task<SchedulerStandardResult> CancelPromoPlanning(int promoPlanId, string userId, string notes);
        Task<PromoApprovalReminderRegularSend> GetPromoApprovalReminderRegularSend();
        Task<PromoForScheduler> GetPromoSchedulerById(int id);
        Task<PromoReconScheduler> GetPromoReconSchedulerById(int Id);
        Task<List<PromoSendBackDataReminder>> GetReminderPendingApprovalList();
        Task<bool> SendEmailApprovalReminder();
    }
}