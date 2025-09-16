using Repositories.Entities.Dtos;
using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IUserProfileRepository
    {
        Task<UserProfileLP> GetUserProfileWithPaging(string keyword, string usergroupid, int userlevel, string fltStatus, string sortColumn, string sortDirection = "ASC",
            int dataDisplayed = 10, int pageNum = 0);
        Task<UserProfileDataById> GetById(string id);
        //Task CreateUser(Entities.Dtos.User user);
        //Task<bool> UpdateUser(Entities.Dtos.User user);
        Task<bool> DeleteUser(UserUpdateDto userUpdateDto);
        Task<List<DistributorSelect>> GetUserDistributor(string profileID);
        Task<IList<ListUserGroupMenu>> GetUserGroupMenuList();
        Task<IList<ListUserGroupRights>> GettUserRightsList(string UserGroupMenuId);
        Task CreateUser(UserProfileInsertDto user);
        Task<bool> UpdateUser(UserProfileInsertDto user);
        Task<IList<ProfileCategory>> GetProfileListCategory();
        Task<object> GetChannelList();
    }

} 