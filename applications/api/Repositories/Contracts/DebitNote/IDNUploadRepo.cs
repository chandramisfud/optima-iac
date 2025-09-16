using System.Data;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNUploadRepo
    {
        // debetnote/create_by_batch
        Task<DNUploadReturn> DNUpload(DataTable dn, string userId);
        // debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}
        Task<IList<DNFilter>> DNUploadFilter(string userid, string status, int entity, string TaxLevel, DataTable dn);  

    }
}