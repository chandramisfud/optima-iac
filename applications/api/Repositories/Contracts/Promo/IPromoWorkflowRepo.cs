using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IPromoWorkflowRepository
    {
        Task<PromoWorkflowResult> GetPromoWorkflow(string refid);
        Task<List<PromoWorkflowDN>> GetPromoWorkflowDN(string refid);
        Task<IList<PromoWorkFlowChanges>> GetPromoWorkFlowChanges(string refid);
        Task<IList<PromoWorkflowHistory>> GetPromoWorkFlowHistory(string refid);
        Task<PromoReconByIdDto> GetPromoWorkflowById(int id);
        Task<IList<object>> GetPromoWorkflowTimeline(string refId);
    }
}
