using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNManualAssignmentRepo
    {
        //    debetnote/dnmanualassignlist
        Task<object> GetDNManualAssignList(string userid, string sortColumn, string sortDirection, int length = 10, int start = 0, string? txtSearch = null);

        //    promo/getPromoForDn
        Task<IList<PromoforDN>> GetApprovedPromoforDN(string periode, int entity, int channel, int account, string userid);

        // master/getAttributeByUser
        Task<DNDistributorEntity> GetAttributeByUser(string userid);
        
        //    debetnote/getbyId/
        Task<DNGetById> GetDNbyIdforDNManualAssignment(int id);

        //    debetnote/assigndn
        Task<DNReassignmentList> AssignDN(int DNId, int PromoId, string? UserId);
        //    debetnote/assignpromo_request
        Task<ForwardResponseDto> ForwardAssignment(int dnid, string approver_userid, string internal_order_number);
    }
}