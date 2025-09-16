using Repositories.Entities;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNVATExpiredChecklistRepo
    {
        // Select Entity
        Task<IList<DNCreationEntityList>> GetEntityList();

        // Select Distributor
        Task<IList<BaseDropDownList>> GetDistributorList(int budgetid, int[] arrayParent);

        // debetnote/vatexpired/list/
        Task<object> GetVATExpiredList(string status, string userid, int entityId, int distributorId, string TaxLevel, string sortColumn, string sortDirection, int length = 10, int start = 0, string? txtSearch = null);

        // debetnote/vatexpriedupdate
        Task DNVATExpiredUpdate(string userid, int id, int VATExpired);
    }
}