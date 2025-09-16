using Repositories.Entities.Dtos;
using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IUserRights
    {
        Task<UserLP> GetUserRightsWithPaging(string keyword, int fltStatus, int dataDisplayed=10, int pageNum = 0);

        Task<bool> UserProfileDeleteStatus(string id, string userid, int action);
        Task UserProfileInsert(UserProfileInsert body);
        //Task UserProfileUpdate(UserProfileInsert body);
        Task<UserProfileResult> GetUserLoginProfilebyId(string id);
    }

} 