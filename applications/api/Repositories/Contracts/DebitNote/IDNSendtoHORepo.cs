using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNSendtoHORepo
    {
        Task<IList<DNDto>> GetDNSendtoHO(
            string status,
            string userid,
            int entityid,
            int distributorid
        );
        Task DeletePromoAttachmentForSendtoHO(int PromoId, string DocLink);
        Task<object> DNGenerateSuratJalantoHO(string userid, List<DNId> dnid);
        Task DNChangeStatusDistributortoHO(string status, string userid, int dnid);
    }

}