using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IGroupBrandRepo
    {
        Task<BaseLP> GetGroupBrandLandingPage(string keyword
           , string sortColumn
           , string sortDirection
           , int dataDisplayed
           , int pageNum);
        Task<GroupBrandModel> GetGroupBrandById(int Id);
        Task<GroupBrandCreateReturn> CreateGroupBrand(GroupBrandCreate body);
        Task<GroupBrandUpdateReturn> UpdateGroupBrand(GroupBrandUpdate body);
        Task<GroupBrandDeleteReturn> DeleteGroupBrand(GroupBrandDelete body);
    }
}