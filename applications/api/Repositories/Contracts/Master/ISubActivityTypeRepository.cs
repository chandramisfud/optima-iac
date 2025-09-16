using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface ISubActivityTypeRepository
    {
        Task<BaseLP> GetSubActivityTypeLandingPage(string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum);
        Task<SubActivityTypeModel> GetSubActivityTypeById(SubActivityTypeById body);
        Task<SubActivityTypeCreateReturn> CreateSubActivityType(SubActivityTypeCreate body);
        Task<SubActivityTypeUpdateReturn> UpdateSubActivityType(SubActivityTypeUpdate body);
        Task<SubActivityTypeDeleteReturn> DeleteSubActivityType(SubActivityTypeDelete body);
    }

}