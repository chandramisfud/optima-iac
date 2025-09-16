using Repositories.Entities;
using Repositories.Entities.Models.PromoApproval;
using Repositories.Entities.Models.PromoSendback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts.Promo
{
    public interface IPromoSendbackRepository
    {
        Task<IList<InitiatorView>> GetPromoSendbackLP(string year, int entity, int distributor,
            int BudgetParent, int channel, string userid, int categoryId = 0);
        Task<IEnumerable<BaseDropDownList>> GetAllEntity();
        Task<IList<BaseDropDownList>> GetDistributorList(int budgetid, int[] arrayParent);
        Task<PromoReviseV3Dto> GetPromoSendbackById(int id);
        Task<List<object>> GetAttributeByParent(int budgetid, int[] arrayParent, string attribute, string status);
        Task<List<object>> GetAttributeByPromo(string userid, int[] arrayParent, string attribute, int promoId);
        /// <summary>
        /// Save Promo sendback
        /// 
        /// (ip_promo_v4_insert)
        /// </summary>
        /// <param name="promo"></param>
        /// <returns></returns>
        Task<PromoResponseDto> PromoSendback(PromoCreationDto promo);
        Task<IList<object>> GetCategoryListforPromoSendBack();
        Task<IList<PromoSourceBudgetDto>> GetPromoSendbackSourceBudget(
       string year, int entity, int distributor, int subCategory, int activity, int subActivity,
       int[] arrayRegion, int[] arrayChannel, int[] arraySubChannel, int[] arrayAccount, int[] arraySubAccount, int[] arrayBrand, int[] arraySKU, string profileId
   );


        //sendback Recon
        Task<PromoReconV3> GetPromoSendbackReconById(int Id, string LongDesc);
        Task<IList<InitiatorView>> GetPromoSendbackReconLP(string year, int entity, int distributor,
            int BudgetParent, int channel, string userid, int categoryId = 0);
        Task<ErrorMessageDto> PromoSendbackRecon(PromoReconCreationDto promo);
        Task<IList<object>> GetCategoryListforPromoReconSendBack();

    }
}
