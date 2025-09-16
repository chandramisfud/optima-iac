using Repositories.Entities.Configuration;

namespace Repositories.Contracts
{
    public interface IMajorChangesRepository
    {
        Task<List<MajorChangesResp>> Select();
        Task<List<MajorChangesResp>> GetHistory(string year, string catShortDesc = "RC");
        Task<bool> UpdateMajorChanges(MajorChangesReq changes);
        Task<List<MajorChangesResp>> SelectDC();
        Task<bool> UpdateMajorChangesDC(MajorChangesReq changes);
    }
}