using Repositories.Entities.Configuration;

namespace Repositories.Contracts
{
    public interface IROIandCRRepo
    {
        Task<IList<ConfigRoi>> GetConfigRoiList(int CategoryId, int SubCategoryId, int ActivityId);
        Task CreateConfigRoi(ConfigRoiStore body);
        Task DeleteConfigRoi(ConfigRoiDelete body);
        Task<IList<ListCategory>> GetCategoryList ();
        Task<IList<ListSubCategory>> GetSubCategoryList (int CategoryId);
        Task<IList<ListActivity>> GetActivityList (int SubCategoryId);
    }
}