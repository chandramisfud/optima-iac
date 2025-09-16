using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IPICDNManualRepo
    {
        Task<BaseLP> GetPICDNManualLandingPage(
             string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<PICDNManualCreateReturn> CreatePICDNManual(PICDNManualCreate body);
        Task<IList<PICDNMAnualModel>> GetPICDNManualDownload();
        Task<PICDNManualDeleteReturn> DeletePICDNManual(PICDNManualDelete body);
        Task<IList<ChannelforPICDNManual>> GetChannelforPICDNManual();
        Task<IList<UserIdPICDNManual>> GetUserIdPICDNManual();
        Task<IList<SubChannelforPICDNManual>> GetSubChannelforPICDNManual(int ChannelId);
        Task<IList<AccountforPICDNManual>> GetAccountforPICDNManual(int SubChannelId);
        Task<IList<SubAccountforPICDNManual>> GetSubAccountforPICDNManual(int AccountId);
    }
}