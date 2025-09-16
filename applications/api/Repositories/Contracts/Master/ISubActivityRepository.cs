using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface ISubActivityRepository
    {
        Task<BaseLP> GetSubActivityLandingPage(string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum);
        Task<SubActivityModel> GetSubActivityById(SubActivityById body);
        Task<SubActivityCreateReturn> CreateSubActivity(SubActivityCreate body);
        Task<SubActivityUpdateReturn> UpdateSubActivity(SubActivityUpdate body);
        Task<SubActivityDeleteReturn> DeleteSubActivity(SubActivityDelete body);
        Task<IList<SubCategoryforSubActivity>> SubCategoryforSubActivity(int CategoryId);
        Task<IList<CategoryforSubActivity>> CategoryforSubActivity();
        Task<IList<ActivityTypeforSubActivity>> ActivityTypeforSubActivity();
        Task<IList<ActivityforSubActivity>> ActivityforSubActivity(int SubCategoryId);
    }
}
