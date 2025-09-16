using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IActivityRepository
    {
        Task<BaseLP> GetActivityLandingPage
        (
            string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<ActivityModel> GetActivityById(ActivityById body);
        // Task<ActivityByAccessReturn> GetActivityByAccess(ActivityByAccess body);
        Task<ActivityCreateReturn> CreateActivity(ActivityCreate body);
        Task<ActivityUpdateReturn> UpdateActivity(ActivityUpdate body);
        Task<ActivityDeleteReturn> DeleteActivity(ActivityDelete body);
        Task<IList<ActivitySubCategory>> GetSubCategoryforActivity(int CategoryId);
        Task<IList<ActivityCategory>> GetCategoryforActivity();

    }
}