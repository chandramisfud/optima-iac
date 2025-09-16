using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IDNCreationHORepo
    {
        // debetnote/getbyId
        Task<DNGetById> GetDNHO(int id);
        // debetnote/cancel
        Task<DNGlobalResponse> CancelDN(DNCancelBody body);
        // master/getAttributeByUser
        Task<DNDistributorEntity> GetAttributeByUser(string userid);
        // debetnote_p/list/
        Task<DNLandingPage> GetDNListLandingPage(
            string period,
            int entityId,
            int distributorId,
            int channelId,
            int accountId,
            string profileId,
            bool isdnmanual,
            string search,
            string sortColumn,
            int pageNum = 0,
            int dataDisplayed = 10,
            string sortDirection = "ASC"
        );
        // promo/getPromoForDn
        Task<IList<DNApprovedPromoList>> GetApprovedPromoforDN(GetApprovedPromoforDNBody body);
        // master/getDistEntityByUser/
        Task<DNDistributorEntity> GetDistributorEntityByUserId(string userid);
        // sellingpoint/getByUser
        Task<IEnumerable<DNSellingPoint>> GetSellingPointByUser(string userid);
        Task<IList<DNReportDto>> GetDNReport(string year, int entity, int distributor, int channel, int account, string userid);
        // debetnote/store
        Task<DNStoreResultSearch> GetDebetnoteStoreValidate(DNCreationParam body);
        Task<DNCreationReturn> CreateDN(DNCreationParam body);
        // debetnote/update
        Task<DNCreationReturn> UpdateDN(DNCreationParam body);
        // dnattachment/store
        Task<DNGlobalResponse> CreateDNAttachment(DNAttachmentBody body);
        // dnattachment/delete
        Task DeleteDNAttachment(DNAttachmentBody body);
        // debetnote/print/
        Task<DNPrint> DNPrint(int id);
        // select Sub Account
        Task<IList<DNCreationSubAccountList>> GetSubAccountList(string profileId);
        // Select Entity 
        Task<IList<DNCreationEntityList>> GetEntityList();
        // Select Channel
        Task<IList<DNCreationChannelList>> GetChannelList(string userid, int[] arrayParent);
        Task<DNStoreResultSearch> GetDebetnoteUpdateValidate(DNCreationParam body);
        Task<DNCreationGetWHTType> GetDNCreationHOGetWHTType(int promoId);
    }
}