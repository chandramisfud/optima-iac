using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNListingOverBudgetRepo
    {
        // debetnote/listOverBudgetByUser
        Task<object> GetDNOverBudgetList(string periode, int entity, int distributor, int channel, int account, 
            string userid, bool isdnmanual, 
            string sortColumn, string sortDirection, int length = 10, int start = 0, string? txtSearch = null);

        // debetnote/getbyId/
        Task<DNGetById> GetDNbyIdforDNListingOverBudget(int id);

        // debetnote/assigndn
        Task<DNReassignmentList> AssignDN(DNAssignParam param);

        //  promo/getPromoForDn
        Task<IList<PromoforDN>> GetApprovedPromoforDN(string periode, int entity, int channel, int account, string userid);

        // master/getAttributeByUser
        Task<DNDistributorEntity> GetAttributeByUser(string userid);
    }
}