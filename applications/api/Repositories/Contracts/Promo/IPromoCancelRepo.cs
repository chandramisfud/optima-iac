using Repositories.Entities;
using Repositories.Entities.Models.DN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts.Promo
{
    public interface IPromoCancelRepository
    {
        Task<IList<PromoView>> GetPromoCancel(string year, int entity, int distributor, int BudgetParent, 
            int channel, string userid);
        Task<IList<object>> GetPromoCancelRequestLP(string year, int entity, int distributor, int BudgetParent, int channel, string userid);
        Task<ErrorMessageDto> SetPromoCancelRequest(int promoId, int promoPlanId, string statusCode, string notes, string userId, string emailId);
    }
}
