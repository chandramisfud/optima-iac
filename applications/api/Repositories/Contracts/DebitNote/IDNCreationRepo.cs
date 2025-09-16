using Repositories.Entities.Models;
using Repositories.Entities.Report;

namespace Repositories.Contracts
{
    public interface IDNCreationRepo
    {
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
        // debetnote/getbyId
        Task<DNGetById> GetDebetNoteById(int id);
        // debetnote/cancel
        Task<DNGlobalResponse> CancelDN(DNCancelBody body);
        // master/getAttributeByUser
        Task<DNDistributorEntity> GetAttributeByUser(string userid);
        // promo/getPromoForDn
        Task<IList<DNApprovedPromoList>> GetApprovedPromoforDNCreation(string periode, int entity, int channel, int account, string userid);
        // master/getDistEntityByUser/
        Task<DNDistributorEntity> GetDistributorEntityByUserId(string userid);
        // sellingpoint/getByUser
        Task<IEnumerable<DNSellingPoint>> GetSellingPointByUser(string userid);
        //  mapmaterial/all
        Task<IEnumerable<DNMaterial>> GetTaxLevel();

        // dncreation/taxlevel?entityid
        //Task<List<DNCreationTaxLevel>> DNCreationTaxLevel(string entityid);

        // AND #131
        Task<List<DNCreationTaxLevel>> DNCreationTaxLevel(string entityid, string whtType = "");

        // debetnote/store
        Task<DNStoreResultSearch> GetDebetnoteStoreValidate(DNCreationParam body);
        Task<DNStoreResultSearch> GetDebetnoteUpdateValidate(DNCreationParam body);
        Task<DNCreationReturn> CreateDN(DNCreationParam body);
        // dnattachment/store
        Task<DNGlobalResponse> CreateDNAttachment(DNAttachmentBody body);
        // dnattachment/delete
        Task DeleteDNAttachment(DNAttachmentBody body);
        // debetnote/print/
        Task<DNPrint> DNPrint(int id);
        // debetnote/update
        Task<DNCreationReturn> UpdateDN(DNCreationParam body);
        // select Sub Account
        Task<IList<DNCreationSubAccountList>> GetSubAccountList(string profileId);
        // Select Entity 
        Task<IList<DNCreationEntityList>> GetEntityList();
        // Select Channel
        Task<IList<DNCreationChannelList>> GetChannelList(string userid, int[] arrayParent);
        Task<DNCreationGetWHTType> GetDNCreationGetWHTType(int promoId);
    }
}