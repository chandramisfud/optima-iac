using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IUserGroupMenuRepository
    {
        Task<UserGroupMenuLandingPage> GetUserGroupMenuLandingPage(string keyword,  string sortColumn, string sortDirection = "ASC",
            int dataDisplayed = 10, int pageNum = 0);
        Task<UserGroupMenuModel> GetUserGroupMenuById(string usergroupid);
        Task<UserGroupMenuCreateReturn> CreateUserGroupMenu(UserGroupMenuCreate body);
        Task<UserGroupMenuUpdateReturn> UpdateUserGroupMenu(UserGroupMenuUpdate body);
        Task<bool> DeleteUserGroupMenu(string usergroupid, string DeletedEmail, string DeletedBy);
        Task<List<UserAccessGroupRightMenu>> GetMenuAccesByGroupId(string usergroupid);
        Task<bool> InsertUserRights(string byUser, UserRightPostDto userRightPostDto);
        Task<IList<UserGroupListUserLevel>> GetUserLevelByUserGroupId(string usergroupid);
        Task<List<UserAccessGroupRightMenuByUserLevel>> GetMenuByLevelId(string userlevelid);
        Task<CRUDMenuDto> GetLevelMenuById(string usergroupid, int userlevel, string menuid);
        Task SaveLevelMenu(UserLevelAccess userLevelAccess);

    }

} 