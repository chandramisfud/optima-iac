using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<BaseLP> GetProductLandingPage( 
            string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
            );
        Task<ProductModel> GetProductById(ProductById body);
        Task<ProductCreateReturn> CreateProduct(ProductCreate body);
        Task<ProductUpdateReturn> UpdateProduct(ProductUpdate body);
        Task<ProductDeleteReturn> DeleteProduct(ProductDelete body);
        Task<IList<BrandforProduct>> GetBrandforProduct(int PrincipalId);
        Task<IList<EntityforProduct>> GetEntityforProduct();
    }

}