using System.Data;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNUploadFakturRepo
    {
        // debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}
        Task<IList<DNFilter>> DNFilterUploadFaktur(string userid, string status, int entity, string TaxLevel, DataTable dn);
        // debetnote/create_by_batch/updatefp
        Task<IList<DNUpload>> DNUploadUpdateFP(DataTable dn, string userId);
    }
}