using Repositories.Entities.Dtos;
using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<UserLP> GetUserWithPaging(string keyword, string fltStatus, string sortColumn, string sortDirection = "ASC",
            int dataDisplayed=10, int pageNum = 0);
        Task<UserByID> GetUserById(string userid);
        Task<bool> UserProfileDeleteStatus(string id, string userid, int action);
        Task UserProfileInsert(UserProfileInsert body);
        //Task UserProfileUpdate(UserProfileInsert body);
        Task<UserProfileResult> GetUserLoginProfilebyId(string id);
        Task<IList<UserProfileList>> GetListUserProfile();
    }

} 