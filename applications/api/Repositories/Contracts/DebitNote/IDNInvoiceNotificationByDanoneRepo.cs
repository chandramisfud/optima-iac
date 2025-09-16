using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNInvoiceNotificationByDanoneRepo
    {
        Task<DNGetbyIdforInvoiceNotification> GetDNbyIdInvoiceNotification(int id);
        Task<DNRejectInvoiceNotificationGlobalResponse> DNRejectInvoiceNotification(int dnid, string reason, string userid);
        Task DeletePromoAttachmentDNInvoiceNotification(int PromoId, string DocLink);
        Task<IList<DNInvoiceNotificationbyDanoneEntityList>> GetEntityList();
        Task<IList<DNInvoiceNotificationbyDanoneDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
        // [GET] debetnote/ready_to_invoice/{userid}/{entity}/{distributor}
        Task<IList<DNDto>> GetDNValidatebyDanone(
            string status,
            string userid,
            int entityid,
            int distributorid
        );
        // [POST]api/debetnote/ready_to_invoice
        Task<DNChangeStatusInvoiceNotif> DNChangeStatusReadytoInvoice(
            string status,
            string userid,
            List<DNId> dnid
        );

    }
}