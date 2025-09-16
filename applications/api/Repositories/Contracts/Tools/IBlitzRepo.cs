using Repositories.Entities.Dtos;

namespace Repositories.Contracts
{
    public interface IToolsBlitzRepository
    {
        Task<IList<BlitzNotif>> BlitzTranferNotif();
        Task<IList<BaselineRaw>> GetBaselineRaws(string refid, int promoplan);
    }
}