using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IRegionRepository
    {
        Task<BaseLP> GetRegionLandingPage(string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum );
        Task<RegionModel> GetRegionById(RegionById body);
        Task<RegionCreateReturn> CreateRegion(RegionCreate body);
        Task<RegionUpdateReturn> UpdateRegion(RegionUpdate body);
        Task<RegionDeleteReturn> DeleteRegion(RegionDelete body);
    }

}