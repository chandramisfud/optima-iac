using System.Data;

namespace Repositories.Contracts
{
    public interface IConfigRepository
    {
        Task<List<Entities.Dtos.Config>> GetConfig(string category);
        Task<object> GetMechanismInput();
        Task<object> UploadMechanismInput(DataTable data, string fileName, string profile, string email);
    }
}
