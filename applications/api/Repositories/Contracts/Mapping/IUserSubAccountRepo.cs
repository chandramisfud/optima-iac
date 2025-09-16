using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IUserSubAccountRepo
    {
        Task<BaseLP> GetUserSubAccountLandingPage(
             string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<UserSubAccountCreateReturn> CreateUserSubAccount(UserSubAccountCreate body);
        Task<IList<UserSubAccountModel>> GetUserSubAccountDownload();
        Task<UserSubAccountDeleteReturn> DeleteUserSubAccount(UserSubAccountDelete body);
        Task<IList<UserIdUserSubAccount>> GetUserIdUserSubAccount();
        Task<IList<ChannelforUserSubAccount>> GetChannelforUserSubAccount();
        Task<IList<SubChannelforUserSubAccount>> GetSubChannelforUserSubAccount(int ChannelId);
        Task<IList<AccountforUserSubAccount>> GetAccountforUserSubAccount(int SubChannelId);
        Task<IList<SubAccountforUserSubAccount>> GetSubAccountforUserSubAccount(int AccountId);

    }
}