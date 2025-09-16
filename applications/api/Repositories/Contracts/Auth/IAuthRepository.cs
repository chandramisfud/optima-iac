using Repositories.Entities.Dtos;

namespace Repositories.Contracts
{
    public interface IAuthMenuRepository
    {
        Task<object> GetByGroup(string UserGroup, string UserLevel);
    }
    public interface IAuthUserRepository
    {
        Task<User> DoLogin(string id, string password);
        Task<UserbyEmail> GetByEmail(string email);
        int GetTokenAge();
        Task<bool> ChangePassword(UserPasswordChange userPasswordChangeDto);
        Task<bool> ChangeOldPassword(UserOldPasswordChange userPasswordChangeDto, string email);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(int userId);
        Task<int> UpdateIsLogin(string email);
        Task<bool> ResetIsLogin(string email);
        Task<bool> UpdateProfilePicture(string email, string filecode);
        Task<List<UserProfileCategory>> GetProfile(string id);
        Task DeleteUserEmail(string? email);
    }
}