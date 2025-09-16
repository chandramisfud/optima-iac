using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface ISellingPointRepository
    {
        Task<BaseLP> GetSellingPointLandingPage(string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum);
        Task<SellingPointModel> GetSellingPointById(SellingPointById body);
        Task<SellingPointCreateReturn> CreateSellingPoint(SellingPointCreate body);
        Task<SellingPointUpdateReturn> UpdateSellingPoint(SellingPointUpdate body);
        Task<SellingPointDeleteReturn> DeleteSellingPoint(SellingPointDelete body);
        Task<IList<RegionforSellingPoint>> GetRegionforSellingPoint();
        Task<IList<ProfitCenterforSellingPoint>> GetProfitCenterforSellingPoint();
    }

}