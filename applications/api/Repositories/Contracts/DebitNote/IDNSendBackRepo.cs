using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IDNSendBackRepo
    {
        // debetnote/list/sendbackdist/
        Task<object> GetDNSendbackDistributor(string periode, int accountId, string userid, bool isdnmanual, string sortColumn, string sortDirection, int length = 10, int start = 0, string? txtSearch = null);

        // dnattachment/store/sendback
        Task<DNGlobalResponse> CreateDNAttachmentSendback(DNAttachmentBody body);

        // debetnote/getbyId/
        Task<DNGetById> GetDNbyIdforDNSendBack(int id);

        // debetnote/cancel
        Task<DNGlobalResponse> CancelDN(DNCancelBody body);

        // master/getAttributeByUser
        Task<DNDistributorEntity> GetAttributeByUser(string userid);

        // debetnote/print/
        Task<DNPrint> DNPrint(int id);

        // dnattachment/delete
        Task DeleteDNAttachment(DNAttachmentBody body);

        // debetnote/update/sendback
        Task<DNCreationReturn> UpdateDNSendback(DNCreationParam body);
    }
}