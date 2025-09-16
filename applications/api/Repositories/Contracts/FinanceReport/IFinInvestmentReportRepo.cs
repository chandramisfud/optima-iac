using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IFinInvestmentReportRepo
    {
        Task<InvestmentReportLandingPage> GetInvestmentReportLandingPage(
            string search, string sortColumn, 
            string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId,
            int pageNum = 0, int dataDisplayed = 10, string sortDirection = "ASC"
        );

        Task<IList<InvestmentBudgetAllocationList>> GetBudgetAllocationList(string year, int entityId, int distributorId, int budgetParentId, int channelId, string userId);

        Task<IList<InvestmentEntityList>> GetEntityList();

        Task<IList<InvestmentDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
        Task<object> GetTTControlRCDCFilter();
        Task<object> GetBudgetSummary(int period, int category, DataTable grpBrand, DataTable channel);
        Task<object> GetBudgetSummaryFilter();
        Task<object> GetBudgetSummarySignoff(int period);
        Task<object> GetBudgetSummaryDC(int period, int category, DataTable dtGrpBrand, DataTable dtChannel);
        Task<object> GetBudgetSummaryLP(int period, int category, DataTable dtGrpBrand, DataTable dtChannel, int start, int length, string txtSearch, string sort, string order);
        Task<object> GetTTControlRCDC(string year, DataTable category, DataTable groupBrand, DataTable channel, DataTable subActivityType, string profileid, int start, int length, string filter, string search);
        Task<object> GetTTControlRCDCDownload(string year, DataTable category, DataTable groupBrand, DataTable channel, DataTable subActivityType, string profileid, int start, int length, string filter, string search);
    }
}
