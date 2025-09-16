using Repositories.Entities.Models.DN;
using Repositories.Entities.Models.DNReceivedAndApproved;
using Repositories.Entities.Models.DNReceivedbyDanone;
using Repositories.Entities.Models.DNSendtoDanone;

namespace Repositories.Contracts
{
    public interface IDNReceivedbyDanoneRepo
    {
        Task<DNReceivedbyDanoneById> GetDNReceivedDanonebyId(int id);
        Task DeletePromoAttachmentDNReceivedbyDanone(int PromoId, string DocLink);
        Task<DNReceivedbyDanoneGlobalResponse> RejectDNReceivedbyDanone(int dnid, string reason, string userid);
        Task<IList<DNReceivedbyDanone>> GetDNReceivedbyDanoneStatus(string status, string userid, int entityid, int distributorid);
        Task<DNReceivedbyDanone> DNRecivedbyDanoneApproval(DNReceivedbyDanoneApprovalParam param);
        Task<IList<DNReceivedbyDanoneEntityList>> GetEntityList();
        Task<IList<DNReceivedbyDanoneDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
      
        Task DNReceivedbyDanoneChangeStatus(string status, string userid, int dnid);
    }
}