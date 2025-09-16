using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface ISubChannelRepository
    {
        Task<BaseLP> GetSubChannelLandingPage(string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum);
        Task<SubChannelModel> GetSubChannelById(SubChannelById body);
        Task<SubChannelCreateReturn> CreateSubChannel(SubChannelCreate body);
        Task<SubChannelUpdateReturn> UpdateSubChannel(SubChannelUpdate body);
        Task<SubChannelDeleteReturn> DeleteSubChannel(SubChannelDelete body);
        Task<IList<SubChanneltoChannel>> GetChannelLongDescforSubChannel();

    }
}
