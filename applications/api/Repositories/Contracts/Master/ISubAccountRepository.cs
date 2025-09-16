using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface ISubAccountRepository
    {
        Task<BaseLP> GetSubAccountLandingPage
        (
            string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<SubAccountModel> GetSubAccountById(SubAccountById body);
        Task<SubAccountCreateReturn> CreateSubAccount(SubAccountCreate body);
        Task<SubAccountUpdateReturn> UpdateSubAccount(SubAccountUpdate body);
        Task<SubAccountDeleteReturn> DeleteSubAccount(SubAccountDelete body);
        Task<IList<AccountSubChannel>> GetSubChannelforSubAccount(int ChannelId);
        Task<IList<AccountChannel>> GetChannelforSubAccount();
        Task<IList<SubAccountAccount>> GetAccountforSubAccount(int SubChannelId);

    }

}
