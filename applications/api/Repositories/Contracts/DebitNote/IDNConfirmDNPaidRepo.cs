using Repositories.Entities.Models.DNConfirmPaid;

namespace Repositories.Contracts
{
    public interface IDNConfirmDNPaidRepo
    {
        //debetnote/confirmpaid/
        Task<IList<DNConfirmPaid>> GetDNStatusConfirmPaid(string status, string userId, int entityId, int distributorId);
        //debetnote/confirm_paid
        Task<DNConfirmPaid> DNConfirmPaid(DNChangeStatusConfirmPaid param);

    }
}