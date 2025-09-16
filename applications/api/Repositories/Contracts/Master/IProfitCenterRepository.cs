using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IProfitCenterRepository
    {
        Task<BaseLP> GetProfitCenterLandingPage(string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum);
        Task<ProfitCenterModel> GetProfitCenterById(ProfitCenterById body);
        Task<ProfitCenterCreateReturn> CreateProfitCenter(ProfitCenterCreate body);
        Task<ProfitCenterUpdateReturn> UpdateProfitCenter(ProfitCenterUpdate body);
        // Task<ProfitCenterDeleteReturn> DeleteProfitCenter(ProfitCenterDelete body);
    }

}