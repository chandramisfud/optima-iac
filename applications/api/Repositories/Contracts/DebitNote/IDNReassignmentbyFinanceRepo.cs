using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNReassignmentbyFinanceRepo
    {
        // debetnoteassign/list/finance
        Task<object> GetDNAssignListFinance(string periode, int entityId, int distributorId, string channelId, string accountId, string userid, bool isdnmanual, string sortColumn, string sortDirection, int length = 10, int start = 0, string? txtSearch = null);

        // master/getAttributeByUser
        Task<DNDistributorEntity> GetAttributeByUser(string userid);

        // promo/getPromoForDn
        Task<IList<PromoforDN>> GetApprovedPromoforDN(string periode, int entityId, int channelId, int accountId, string userid);

        // debetnote/getbyId/
        Task<DNGetById> GetDNbyIdforDNReassignmentFinance(int id);

        // debetnote/assigndn
        Task<DNReassignmentList> AssignDN(DNAssignParam param);

        // debetnote/assignpromo_request
        Task<ForwardResponseDto> ForwardAssignment(int dnid, string approver_userid, string internal_order_number);
    }
}