using Repositories.Entities.Dtos;
using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IUserLevelRepository
    {
        Task<UserLevelLP> GetUserLevelWithPaging(string keyword, string userGroupId, string sortColumn, string sortDirection = "ASC",
            int dataDisplayed = 10, int pageNum = 0);
        Task<int> CreateUserLevel(userLevelCreate userLevel, bool isUpdate);
        Task<UserLevel> GetUserLevelById(int id);
        Task<bool> DeleteUserLevel(int id);
        Task<List<UserRightsDto2>> GetGroupId(string usergroupid);

    }
}