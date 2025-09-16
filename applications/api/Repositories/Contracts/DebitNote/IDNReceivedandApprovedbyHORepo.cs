using System.Data;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;
using Repositories.Entities.Models.DNReceivedAndApproved;

namespace Repositories.Contracts
{
    public interface IDNReceivedandApprovedbyHORepo
    {
        Task<IList<DNValidateByDistributorHO>> GetDNValidateByDistributorHO(string status, string userid, int entityid, int distributorid);
        Task<DNValidateByDistributorHO> DNMultiApprovalParalel(int DNId, string status, string notes, string userId);
        Task DeletePromoAttachmentDNReceivedandApproved(int PromoId, string DocLink);
        Task<DNGetById> GetDNbyId(int id);
        Task<DNPromoDisplayGetDistributorId> GetDistributorId(string UserId);
        // debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}
        Task<IList<DNFilter>> DNFilterReceivedandApprovedbyHO(string userid, string status, int entity, string TaxLevel, DataTable dn);
        Task DNChangeStatusDistributorMultiApproval(string status, string userid, string useremail, int dnid);
    }
}