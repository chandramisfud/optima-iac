using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface ITaxLevelRepo
    {
        Task<BaseLP> GetTaxLevelLandingPage(
           string keyword
          , string sortColumn
          , string sortDirection
          , int dataDisplayed
          , int pageNum
        );
        Task<TaxLevelCreateReturn> CreateTaxLevel(TaxLevelCreate body);
        Task<IList<TaxLevelList>> GetTaxLevelDownload();
        Task<TaxLevelDeleteReturn> DeleteTaxLevel(TaxLevelDelete body);
        Task<IList<EntityforTaxLevel>> GetEntityforTaxLevel();

    }
}