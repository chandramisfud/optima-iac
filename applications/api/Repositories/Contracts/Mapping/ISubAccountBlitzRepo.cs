using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface ISubAccountBlitzRepo
    {
        // Task<IEnumerable<MapMaterial>> GetMapMaterials();
        Task<BaseLP> GetSubAccountBlitzLandingPage(
             string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<SubAccountBlitzCreateReturn> CreateSubAccountBlitz(SubAccountBlitzCreate body);
        Task<IList<SubAccountBlitzModel>> GetSubAccountBlitzDownload();
        Task<SubAccountBlitzDeleteReturn> DeleteSubAccountBlitz(SubAccountBlitzDelete body);
        Task<IList<ChannelforSubAccountBlitz>> GetChannelforSubAccountBlitz();
        Task<IList<SubChannelforSubAccountBlitz>> GetSubChannelforSubAccountBlitz(int ChannelId);
        Task<IList<AccountforSubAccountBlitz>> GetAccountforSubAccountBlitz(int SubChannelId);
        Task<IList<SubAccountforSubAccountBlitz>> GetSubAccountforSubAccountBlitz(int AccountId);

    }
}