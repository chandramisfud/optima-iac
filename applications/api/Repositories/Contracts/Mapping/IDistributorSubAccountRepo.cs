using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IDistributorSubAccountRepo
    {
        Task<BaseLP> GetDistributorSubAccountLandingPage(
             string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<DistributorSubAccountCreateReturn> CreateDistributorSubAccount(DistributorSubAccountCreate body);
        Task<IList<DistributorSubAccountDownload>> GetDistributorSubAccountDownload();
        Task<DistributorSubAccountDeleteReturn> DeleteDistributorSubAccount(DistributorSubAccountDelete body);
        Task<IList<DistributorforDistributorSubAccount>> GetDistributorforDistributorSubAccount();
        Task<IList<ChannelforDistributorSubAccount>> GetChannelforDistributorSubAccount();
        Task<IList<SubChannelforDistributorSubAccount>> GetSubChannelforDistributorSubAccount(int ChannelId);
        Task<IList<AccountforDistributorSubAccount>> GetAccountforDistributorSubAccount(int SubChannelId);
        Task<IList<SubAccountforDistributorSubAccount>> GetSubAccountforDistributorSubAccount(int AccountId);
    }
}