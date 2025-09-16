using Repositories.Entities.Models;
using Repositories.Entities.Models.Promo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IPromoClosureRepository
    {       
        Task<IList<ResponseMultipleDocDto>> ClosingPromo(int[] lsPromoId, string userId, string userEmail);
        Task<BaseLP2> GetPromoClosureLP(int entity, int distributor, int channel, DateTime start_from, DateTime start_to, int remaining_budget, string userid, int start, int length, string filter, string txtsearch);
        Task<List<PromoImportResponse>> ImportPromoClosure(DataTable data, string userid, string useremail);       
        Task ReOpenPromo(int[] lsPromoId, string userId, string userEmail);
    }
}
