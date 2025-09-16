using System.Data;
using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IFinPromoSubmissionReportRepo
    {
        Task<PromoSubmissionLandingPage> GetFinPromoSubmissionLandingPage(
            string period,
            int entityId,
            int distributorId,
            int channelId,
            string profileId,
            string search,
            string sortColumn,
            string sortDirection = "ASC",
            int pageNum = 0,
            int dataDisplayed = 10
        );
        Task<PromoSubmissionList> PromoSubmissionEmail(string period, int entity, int distributor, string userid);
        Task<IList<PromoSubmissonExceptionList>> GetPromoSubmissonExceptionList(string idx);
        Task PromoSubmissionExceptionUpload(DataTable promo, string idx);
        Task PromoSubmissionExceptionClear(string idx);
        Task<IList<ConfigLatePromoCreation>> GetFinLatePromoSubmission();
        Task<IList<FinPromoSubmissionEntityList>> GetEntityList();
        Task<IList<FinPromoSubmissionDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
        Task<IList<FinPromoSubmissionChannelList>> GetChannelList(string userid, int[] arrayParent);
        Task<List<PromoSubmissionUser>> GetUserList(string usergroupid, int userlevel, int isdeleted);
        Task<IEnumerable<PromoSubmissionUserGroup>> GetUserGroupList();

    }
}