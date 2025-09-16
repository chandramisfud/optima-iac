
using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IDNUploadAttachmentRepo
    {
        //   searchDNbyRefid/post
        Task<IList<SearchParamDNbyRefidDto>> SearchDNByRefid(string refId);
        //   dnattachment/store
        Task<DNCreateAttachmentReturn> SaveDNAttachment(DNCreateAttachmentParam param);
        //   debetnote/listattach
        Task<IList<DNListAttachment>> GetDNListAttachment(string periode, int distributor, string userId, bool isdnmanual);

    }
}