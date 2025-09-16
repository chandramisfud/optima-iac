using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface ICategoryRepository
    {
        Task<BaseLP> GetCategoryLandingPage
        (
            string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<CategoryModel> GetCategoryById(CategoryById body);
        Task<CategoryCreateReturn> CreateCategory(CategoryCreate body);
        Task<CategoryUpdateReturn> UpdateCategory(CategoryUpdate body);
        Task<CategoryDeleteReturn> DeleteCategory(CategoryDelete body);
        Task<object> GetCategoryByShortDesc(string categoryShortDesc);
    }

}