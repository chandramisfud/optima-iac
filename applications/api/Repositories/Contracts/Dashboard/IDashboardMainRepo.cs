using Repositories.Entities;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using Repositories.Entities.Models.Dashboard;

namespace Repositories.Contracts
{
    public interface IDashboardMainRepo
    {
        // api/dashboard/main/{periode}/{entity}/{subcategory}/{userid}/{viewmode}
        Task<DashboardMainDto> GetDashboardMain(
            DateTime period,
            int entityId,
            int[] channelId,
            int[] accountId,
            int[] categoryId,
            int subcategoryId,
            string userid,
            string viewmode
        );

        // channel/byaccess/
        Task<IEnumerable<DashboardMasterbyAccess>> GetAllChannelByAccess(string userId);

        // account/byaccess/
        Task<IEnumerable<DashboardMasterbyAccess>> GetAllAccountByAccess(string userid, int channelid);

        // users/dropdown
        Task<DropdownList> GetDropDownList();

        // dashboard/notifications
        Task<Notifications> GetNotification(string userid);

        // dashboard/dnmonitoring
        Task<DNMonitoring> GetDNMonitoring(
            DateTime period,
            int entityId,
            string channel,
            string account,
            string paymentstatus,
            string userid,
            int distibutorId,
            int dnpromo
        );

        // dashboard/trend
        Task<IList<DashboardTrendDto>> GetDashboardTrend(
            DateTime period,
            int entityId,
            string userid,
            int[] channelId,
            int[] accountId
        );

        // dashboard/spend 
        Task<BudgetUsage> GetBudgetUsage(DateTime periode, int entityId, string channel, string account, int subcategoryid, string userid, string viewmode);

        // dashboard/promosubmission/
        Task<decimal> GetOnTimePromoSubmission(DateTime periode, string viewmode);

        // dashboard/promoapproval
        Task<decimal> GetOnTimePromoApproval(DateTime periode, string viewmode);
        //Task<List<Entities.Dtos.DashboardSearch>> Search(string refID);
        Task<object> Search(string refID, string userId);
        Task<object> GetDNOverBudgetToBeSettled(string userId, string sortColumn, string sortDirection, int length = 10, int start = 0, string? txtSearch = null);
        Task<IList<DNListbyPromoIdDto>> GetDNListbyPromoId(int promoId);
    }
}