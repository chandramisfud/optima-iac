using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNWorkflowRepo
    {
        // debetnoteid/workflow
        Task<DNIdWorkflow> GetDNWorkflow(string RefId);
        // // debetnoteid/workflow_change
        Task<IList<DNIdWorkflowChange>> GetDNWorkflowChange(string RefId);
        // // debetnoteid/workflow_history
        Task<IList<DNIdWorkflowHistory>> GetDNWorkflowHistory(string RefId);
        // // promo/workflow
        Task<PromoWorkflowResult> GetPromoWorkflowforDNWorkflow(string RefId);
        // // debetnote/print/
        Task<DNPrint> DNPrintforDNWorkflow(int id);
        // DNGetById
        Task<DNGetById> GetDNbyIdforDNWorkflow(int id);

    }
}