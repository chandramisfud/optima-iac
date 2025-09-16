using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IAccountRepository
    {
        Task<BaseLP> GetAccountLandingPage
        (
            string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<AccountModel> GetAccountById(AccountById body);
        // Task<AccountByAccessReturn> GetAccountByAccess(AccountByAccess body);
        Task<AccountCreateReturn> CreateAccount(AccountCreate body);
        Task<AccountUpdateReturn> UpdateAccount(AccountUpdate body);
        Task<AccountDeleteReturn> DeleteAccount(AccountDelete body);
        Task<IList<AccountSubChannel>> GetSubChannelLongDesc(int ChannelId);
        Task<IList<AccountChannel>> GetChannelLongDesc();

    }
}