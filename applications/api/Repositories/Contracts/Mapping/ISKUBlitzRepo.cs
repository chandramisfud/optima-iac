using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface ISKUBlitzRepo
    {
        // Task<IEnumerable<MapMaterial>> GetMapMaterials();
        Task<BaseLP> GetSKUBlitzLandingPage(
             string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<SKUBlitzCreateReturn> CreateSKUBlitz(SKUBlitzCreate body);
        Task<IList<SKUBlitzModel>> GetSKUBlitzDownload();
        Task<SKUBlitzDeleteReturn> DeleteSKUBlitz(SKUBlitzDelete body);
        Task<IList<EntityforSKUBlitz>> GetEntityforSKUBlitz();
        Task<IList<BrandforSKUBlitz>> GetBrandforSKUBlitz(int PrincipalId);
        Task<IList<SKUforSKUBlitz>> GetSKUforSKUBlitz(int BrandId);


    }
}