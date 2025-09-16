using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IToolsFileRepository
    {
        Task<Files> GetFilesByCode(string filecode);
        Task<bool> UpdateNameByCode(string filecode, string filename);
        Task<string> Insert(string filename, string uniqCode);
        string GetUniqCode();
    }
}