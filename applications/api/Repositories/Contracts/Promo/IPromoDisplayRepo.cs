using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;
using Repositories.Entities.Models.Promo;
using Repositories.Entities.Models.PromoApproval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IPromoDisplayRepository
    {        
        Task<PromoDisplayList> GetPromoDisplaybyId(int id);
        Task<PromoReconByIdDto> GetPromoDisplayById(int id, string profile = "");
        Task<PromoDisplayLP> GetPromoDisplayLP(
            string year,
            int entity,
            int distributor,
            int BudgetParent,
            int channel,
            string userid,
            bool cancelstatus,
            int start = 0,
            int length = 10,
            string filter = "ASC",
            string txtsearch = ""
            );
        Task<IList<PromoView>> GetPromoListByStatus(string year, int entity, int distributor, int BudgetParent, int channel, string userid, bool cancelstatus);
    }
}