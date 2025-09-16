using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface ISubCategoryRepository
    {
        Task<BaseLP> GetSubCategoryLandingPage
         (
            string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<SubCategoryModel> GetSubCategoryById(SubCategoryById body);
        Task<SubCategoryCreateReturn> CreateSubCategory(SubCategoryCreate body);
        Task<SubCategoryUpdateReturn> UpdateSubCategory(SubCategoryUpdate body);
        Task<SubCategoryDeleteReturn> DeleteSubCategory(SubCategoryDelete body);
        Task<IList<SubCategorytoCategory>> GetCategoryforSubCategory();

    }
}
