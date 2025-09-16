using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IChannelRepository
    {
        Task<BaseLP> GetChannelLandingPage
        (
            string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<ChannelModel> GetChannelById(ChannelById body);
        Task<ChannelCreateReturn> CreateChannel(ChannelCreate body);
        Task<ChannelUpdateReturn> UpdateChannel(ChannelUpdate body);
        Task<ChannelDeleteReturn> DeleteChannel(ChannelDelete body);
    }

}