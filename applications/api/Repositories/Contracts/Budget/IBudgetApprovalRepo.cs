using Repositories.Entities;
using Repositories.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBudgetApprovalRepository
    {
        Task<List<BudgetApprovalView>> GetBudgetApprovalLandingPage(string year, int entity, int distributor, int BudgetParent, int channel, string profileId);

        Task<IEnumerable<BaseDropDownList>> GetAllEntity();
        Task<IList<BaseDropDownList>> GetDistributorList(int budgetid, int[] arrayParent);
        Task<IEnumerable<BaseDropDownList>> GetAllChannel();
        Task<bool> BudgetApproval(BudgetApprovalApproveDto budgetApprove);
        Task<bool> BudgetUnapproval(BudgetApprovalApproveDto budgetUnapprove);
    }
}
