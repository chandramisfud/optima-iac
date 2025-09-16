using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNReassignmentRepo
    {
        // debetnoteassign/list/
        Task<object> GetDNAssignList(string periode, int entityId, int distributorId, string channelId, string accountId, string userid, bool isdnmanual, string sortColumn, string sortDirection, int length = 10, int start = 0, string? txtSearch = null);
        // promo/getPromoAprrovalForDnByInitiator/
        Task<IList<ApprovalPromoforDNbyInitiator>> GetApprovedPromoByInitiator(string periode, int entityId, int channelId, int accountId, string userid);
        // debetnote/getbyId/
        Task<DNGetByIdforReassignment> GetDNIdforReassignment(int id);
        // debetnote/assigndn
        Task<DNReassignmentList> AssignDN(DNAssignParam param);
        //  master/getAttributeByUser
        Task<DNDistributorEntity> GetAttributeByUser(string userid);
    }
}