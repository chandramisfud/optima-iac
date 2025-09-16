using Repositories.Entities.Models;
using System.Data;

namespace Repositories.Contracts
{
    public interface IDistributorWHTRepo
    {
        Task<bool> CreateDistributorWHT(string distributor, string subActivity, string subAccount, string WHTType, 
            string modifiedBy, string modifiedEmail);
        Task<bool> DeleteDistributorWHT(int id, string modifiedBy, string modifiedEmail);
        Task<object> GetDistributorWHT(int id);
        Task<BaseLP> GetDistributorWHTLP(string keyword, string distributor, string subActivity, string subAccount, 
            string WHTType, int start, int length, string fieldOrder, string sort);
        Task<List<object>> ImportDistributorWHT(DataTable dt, string createdBy, string createdEmail);
        Task<IList<string>> RunQueryString(string qry);
        Task<bool> UpdateDistributorWHT(int id, string WHTType, string modifiedBy, string modifiedEmail);
    }
}