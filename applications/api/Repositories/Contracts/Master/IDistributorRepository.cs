using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IDistributorRepository
    {
        Task<BaseLP> GetDistributorLandingPage(string keyword, string sortColumn,
            string sortDirection, int dataDisplayed, int pageNum);
        Task<DistributorModel> GetDistributorById(DistributorById body);
        Task<DistributorCreateReturn> CreateDistributor(DistributorCreate body);
        Task<DistributorUpdateReturn> UpdateDistributor(DistributorUpdate body);
        Task<DistributorDeleteReturn> DeleteDistributor(DistributorDelete body);
    }
}