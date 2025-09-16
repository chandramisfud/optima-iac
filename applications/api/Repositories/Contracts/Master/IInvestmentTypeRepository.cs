using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IInvestmentTypeRepository
    {
        Task<BaseLP> GetInvestmentTypeLandingPage(
            string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
            );
        Task<InvestmentTypeModel> GetInvestmentTypeById(InvestmentTypeById body);
        Task<InvestmentTypeCreateReturn> CreateInvestmentType(InvestmentTypeCreate body);
        Task<InvestmentTypeUpdateReturn> UpdateInvestmentType(InvestmentTypeUpdate body);
        Task<InvestmentTypeDeleteReturn> DeleteInvestmentType(InvestmentTypeDelete body);
        Task<IList<InvestmentResultMap>> InvestmentTypeMap(int activityid, int subactivityid, int categoryid, int subcategoryid);
        Task CreateInvestmentTypeMapping(InvestmentTypeMappingCreate body);
        Task DeleteInvestmentTypeMapping(InvestmentTypeMappingDelete body);
        Task<IList<CategoryInvestmentMap>> GetCategoryforInvestmentMap();
        Task<IList<SubCategoryInvestmentMap>> GetSubCategoryInvestmentMap(int CategoryId);
        Task<IList<ActivityInvestmentMap>> GetActivityInvestmentMap(int SubCategoryId);
        Task<IList<SubActivityInvestmentMap>> GetSubActivityInvestmentMap(int ActivityId);
        Task<IList<InvestmentTypeforInvestmentMap>> InvestmentTypeInvestmentMap();
        Task<InvestmentTypeActivateReturn> ActivateInvestmentType(InvestmentTypeActivate body);


    }
}
