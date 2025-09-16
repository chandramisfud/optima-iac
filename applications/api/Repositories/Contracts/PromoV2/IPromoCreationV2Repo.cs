using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using System.Data;

namespace Repositories.Contracts
{
    public interface IPromoCreationV2Repository
    {
        Task<object> GetPromoAutoCreationAtribute(string profileId);
        Task<object> GetPromoBudget(int period, int categoryId, int subCategoryId, int channelId, int subChannelId, int accountId, int subAccountId, int distributorId, int groupBrandId, int subActivityTypeId, int activityId, int subActivityId);
        Task<object> GetPromoCancelRequestLP(string period, int entity, int distributor, int budgetParent, int channel, string profileId);
        Task<object> GetPromoCreationAttributeList(string profileId);
        Task<object> GetPromoCreationBaseline(int promoid, int period, DateTime date, int pType, int distributor, int[] region, int channel, int subChannel, int account, int subaccount, int[] product, int subCategory, int subActivity, int grpBrand, DateTime promostart, DateTime promoend);
        Task<object> GetPromoCreationById(int id);
        Task<object> GetPromoCreationCR(int period, int subactivityid, int subaccountid, int distributorid, int groupbrandid);
        Task<object> GetPromoCreationDCById(int id);
        Task<object> GetPromoCreationMechanismWithStatus(int entityId, int subCategoryId, int activityId, int subActivityId, 
            int skuId, int channelId, int brandId, string startFrom, string startTo);
        Task<object> GetPromoCreationPSValue(int period, int distributorId, int groupBrandId, DateTime promoStart, DateTime promoEnd);
        Task<object> GetPromoCreationSSValue(int period, int channelid, int subchannelid, int accountid, int subaccountid, 
            int groupbrandid, DateTime promostart, DateTime promoend);
        Task<object> GetPromoDisplayById(int id, string profile="");
        Task<PromoDisplayList> GetPromoDisplayByIdForSendEmail(int id);
        Task<object> GetPromoDisplayDCById(int id);
        Task<object> GetPromoDisplayEmailById(int id);
        Task<object> GetPromoDisplayWorkflow(string refid);
        Task<object> GetPromoDisplayWorkflowpdf(int id, string profile = "");
        Task<object> GetPromoReconDisplayById(int id, string profile = "");
        Task<object> GetPromoReconDisplayDCById(int id, string profile = "");
        Task SendEmail(EmailBody emailBodyDto);
        Task<promoCreationResult> SetPromoAutoCreation(int period, int category, int distributor, int brand, int channel, int subAccount, int subActivity, string profileId, string userEmail);
        Task<object> SetPromoCreationInsert(DataTable promo, DataTable region, DataTable sku, DataTable attachment, DataTable mechanism);
        Task<object> SetPromoCreationInsertDC(DataTable promo, DataTable region, DataTable sku, DataTable attachment, DataTable mechanism);
        Task<object> SetPromoCreationUpdate(DataTable promo, DataTable region, DataTable sku, DataTable attachment, DataTable mechanism);
        Task<object> SetPromoCreationUpdateDC(DataTable promo, DataTable region, DataTable sku, DataTable attachment, DataTable mechanism);
    }
}
