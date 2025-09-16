using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IXMLPaymentRepository
    {
        Task<IList<XMLGenerate>> GetXMLGenerate(EntityParam body);
        Task<IList<XMLGenerateErrorMessage>> GenerateXMLFlagging(XmlFlaggingBody body);
        Task<IList<XmlGenerateAccrual>> GetXmlGenerateAccrual(XmlGenerateAccrualBody body);
        Task<IList<XMLGenerate>> GetXMLGenerateNonBatch(EntityParam body);
        Task<IList<XMLGenerateNonBatchDitributorsPayment>> GetXMLGenerateNonBatchDistributorPayment(EntityParam body);
        Task<XMLGeneratePaymentBatch> GenerateXMLPaymentBatch(XMLGeneratePaymentBatchBody body);
        // api/xmlgenerate/accrualbyId"
        Task<IList<XmlGenerateAccrualById>> GenerateXMLAccrualbyId(int id);
        Task<IList<XMLFlaggingList>> GetXMLFlaggingList(int entityId, string userProfileId, string generateOn);
        Task XMLFlaggingUpdate(XMLFlaggingUpdateBody body);
        Task<List<XMLUploadListDto>> GetXMLUploadList(XMLUploadListBody body);
        Task XMLUpload(XMLUploadBody body);
        Task<IList<XMLFlaggingList>> GetXMLFlaggingListHistory(XMLFlaggingHistoryBody body);
        Task<IList<XMLGenerateNMN>> GetXMLGenerateNMN(XMLGenerateNMNbody bodyreq);
        Task<IList<XMLGenerateBatchNameList>> GetXMLBatchName(XMLGenerateBatchNameBodyReq body);
        Task<IList<DistributorEntityXMLGenerate>> GetDistributorbyEntity(int PrincipalId);
        Task<IList<EntityforXMLGenerate>> GetEntityforXMLGenerate();
        Task<List<UserProfileXMLGenerate>> GetUserList(
            string usergroupid = "all",
            int userlevel = 0,
            int isdeleted = 2
        );
        // promo/accrualreportheader
        Task<IList<PromoAccrualReportHeader>> GetPromoAccrualHeader(string periode, int entityId, string closingDate);

    }
}