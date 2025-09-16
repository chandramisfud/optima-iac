using Repositories.Entities;
using Repositories.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IPromoCreationRepository
    {
        Task<BaseLP2> GetPromoCreationLandingPage(string year, int entity, int distributor, int categoryId, string profileId,
           string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        Task<IList<PromoSourcePlanningDto>> GetPromoCreationSourcePlanning(string year, int entity, int distributor, string profileId);
        Task<IList<PromoSourceBudgetDto>> GetPromoCreationSourceBudget(
            string year, int entity, int distributor, int subCategory, int activity, int subActivity,
            int[] arrayRegion, int[] arrayChannel, int[] arraySubChannel, int[] arrayAccount, int[] arraySubAccount, int[] arrayBrand, int[] arraySKU, string profileId
        );
        Task<IList<BaseDropDownList>> GetPromoCreationChannel(string profileId);
        Task<IList<BaseDropDownList>> GetPromoCreationSubChannel(int[] arrayChannel, string profileId);
        Task<IList<BaseDropDownList>> GetPromoCreationAccount(int[] arraySubChannel, string profileId);
        Task<IList<BaseDropDownList>> GetPromoCreationSubAccount(int[] arrayAccount, string profileId);

        Task<IList<BaseDropDownList>> GetPromoCreationChannelByPromoId(int promoId, string profileId);
        Task<IList<BaseDropDownList>> GetPromoCreationSubChannelByPromoId(int promoId, int[] arrayChannel, string profileId);
        Task<IList<BaseDropDownList>> GetPromoCreationAccountByPromoId(int promoId, int[] arraySubChannel, string profileId);
        Task<IList<BaseDropDownList>> GetPromoCreationSubAccountByPromoId(int promoId, int[] arrayAccount, string profileId);
        Task<PromoCreationSKPDraftDto> GetPromoCreationSKPDraft(int id);
        Task<PromoResponseDto> PromoCreationCreate(PromoCreationDto promo);
        Task<PromoResponseDto> PromoCreationUpdate(PromoCreationDto promo);
        Task<PromoCreationByIdDto> GetPromoCreationById(int id);
        Task<bool> PromoCreationAttachment(int promoId, string docLink, string fileName, string createBy);
        Task<bool> PromoDeleteAttachment(int promoId, string docLink);
        Task<PromoExistDto> GetPromoExist(string period, string activityDesc, int[] arrayChannel, int[] arrayAccount, string startPromo, string endPromo);

        //SNTicket#125
        Task<object> GetPromoExistDC(string period, int distributor, int subActivity, int subActivityType, string startPromo, string endPromo);

        Task<LatePromoDto> GetLatePromoDays();
        Task<PromoResponseDto> PromoCancelRequest(int promoId, string profileId, string notes, string reqEmail);
        Task<IList<CancelReasonDto>> GetCancelReason();
        Task<IList<object>> GetBrandByGroupId(int grpBrandId);
        Task<IList<object>> GetGroupBrandByEntity(int entityId);
        Task<IList<object>> GetSubCategoryId(int CategoryId);
        Task<IList<object>> GetSubChannel(int[] ChannelId);
        Task<IList<object>> GetAccount(int[] SubChannelId);
        Task<IList<object>> GetSubAccount(int[] AccountId);
        Task<IList<object>> GetActivityandSubActivityId(int subCategoryId);
        Task<IList<object>> GetCategoryList();
        Task<object> GetPromoMechanismValidate(int promoId, int entityId, int subCategoryId, int activityId, int subActivityId, int skuId, int channelId, string startFrom, string startTo);
        Task<object> GetPromoCreationDownload(string year, int entity = 0, int distributor = 0, int categoryId = 0, string profileId = "");
    }
}
