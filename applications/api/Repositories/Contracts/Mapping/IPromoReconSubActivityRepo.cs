using System.Data;
using Repositories.Entities.Models;
using Repositories.Entities.Models.Promo;

namespace Repositories.Contracts
{
    public interface IPromoReconSubActivityRepo
    {
        Task<BaseLP> GetPromoReconSubActivityLandingPage(
                string keyword
                , string sortColumn
                , string sortDirection
                , int dataDisplayed
                , int pageNum);
        Task<bool> CreatePromoReconSubActivity(PromoReconSubActivityCreate body);
        Task<PromoReconSubActivityUpdateReturn> UpdatePromoReconSubActivity(PromoReconSubActivityUpdate body);
        Task<IList<PromoReconSubActivityLP>> GetPromoReconSubActivityDownload();
        Task<PromoReconSubActivityGetById> GetPromoReconSubActivitybySubActivityId(int Id);
        Task<PromoReconSubActivityDeleteReturn> DeletePromoReconSubActivity(PromoReconSubActivityDelete body);
        Task<IList<SubActivityDropDownMapping>> GetSubActivityDropdown(int ActivityId);
        Task<List<PromoImportResponse>> ImportPromoReconSubactivity(DataTable data, string userid, string useremail);
        Task<IList<SubCategoryforSubActivity>> SubCategoryforSubActivity(int CategoryId);
        Task<IList<CategoryforSubActivity>> CategoryforSubActivityPromoRecon();
        Task<IList<ActivityTypeforSubActivity>> ActivityTypeforSubActivityPromoRecon();
        Task<IList<ActivityforSubActivity>> ActivityforSubActivityPromoRecon(int SubCategoryId);
        Task<IList<SubActivityTemplate>> GetSubActivityTemplate();
    }
}