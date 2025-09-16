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
    public interface IPromoReconRepository
    {
        Task<BaseLP2> GetPromoReconLandingPage(string year, int entity, int distributor, string userid,
            int categoryId, int BudgetParent, int channel, DateTime start_from, DateTime start_to,
            string keyword, int pageNumber, int pageSize);
        Task<IList<Entities.Models.PromoReconciliationPage>> GetPromoReconDownload(string year,
            DateTime start_from, DateTime start_to, string profileId = "",
            int categoryId=0, int entity = 0, int channel = 0, int distributor = 0);
        Task<IList<BaseDropDownList>> GetPromoReconChannelByPromoId(int promoId, string profileId);
        Task<IList<BaseDropDownList>> GetPromoReconSubChannelByPromoId(int promoId, int[] arrayChannel, string profileId);
        Task<IList<BaseDropDownList>> GetPromoReconAccountByPromoId(int promoId, int[] arraySubChannel, string profileId);
        Task<IList<BaseDropDownList>> GetPromoReconSubAccountByPromoId(int promoId, int[] arrayAccount, string profileId);
        Task<PromoResponseDto> PromoReconUpdate(PromoReconCreationDto promo);
        Task<Entities.Models.PromoReconByIdDto> GetPromoReconById(int id);

        Task<bool> PromoReconAttachment(int promoId, string docLink, string fileName, string createBy);
        Task<bool> PromoReconDeleteAttachment(int promoId, string docLink);
        Task<IList<object>> GetCategoryListforPromoRecon();
        Task<IList<object>> GetPromoReconSubActivitybyActivityId(int activityId, string isDeleted);
        Task<List<object>> GetPromoSubactivity(string activityId);
    }
}
