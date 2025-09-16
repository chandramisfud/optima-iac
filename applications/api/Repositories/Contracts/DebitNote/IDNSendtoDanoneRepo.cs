using Repositories.Entities.Models.DN;
using Repositories.Entities.Models.DNSendtoDanone;

namespace Repositories.Contracts
{
    public interface IDNSendtoDanoneRepo
    {
        Task DeletePromoAttachmentForSendtoDanone(int PromoId, string DocLink);       
        Task DNChangeStatusSendtoDanone(string status, string userid, int dnid);
        Task<object> DNGenerateSuratJalantoDanone(string userid, List<DNId> dnid);
        Task<IList<DNChangeStatusSendtoDanone>> GetDebetNoteByStatusValidatebyDistributorHO(string status, string userid, int entityid, int distributorid);
        Task<GetDNbyIdForDNSendtoDanone> GetDNbyIdforSendtoDanone(int id);
        Task<DNSendtoDanoneStandardResponse> RejectDNSendtoDanone(int dnid, string reason, string userid);
    }
}