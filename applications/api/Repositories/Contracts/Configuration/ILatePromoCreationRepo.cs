using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;

namespace Repositories.Contracts
{
    public interface ILatePromoCreationRepo
    {
        Task<IList<ConfigLatePromoCreation>> GetConfigLatePromo();
        Task<bool> UpdateLatePromoCreation(LatePromoCreation latePromoCreation);
    }
}