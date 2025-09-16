using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IBrandRepository
    {
        Task<BaseLP> GetBrandLandingPage(string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum);
        Task<BrandModel> GetBrandById(BrandById body);
        Task<BrandCreateReturn> CreateBrand(BrandCreate body);
        Task<BrandUpdateReturn> UpdateBrand(BrandUpdate body);
        Task<BrandDeleteReturn> DeleteBrand(BrandDelete body);
        Task<IList<EntityforBrand>> GetEntityforBrand();

    }

}