using Repositories.Entities.Dtos;
using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IEntityRepository
    {
        Task<BaseLP> GetEntityLandingPage
        (
            string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum
        );
        Task<EntityModel> GetEntityById(EntityById body);
        Task<EntityCreateReturn> CreateEntity(EntityCreate body);
        Task<EntityUpdateReturn> UpdateEntity(EntityUpdate body);
        Task<EntityDeleteReturn> DeleteEntity(EntityDelete body);
    }
}