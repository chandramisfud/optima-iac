using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IFinDocumentCompletenessRepo
    {
        Task<DocumentCompletenessLandingPage> GetDocumentCompletenessLandingPage(
            int entityId, int distributorId, string profileId, string status, string taxLevel,
            string search, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10
        );

        Task<IList<DocumentCompletenessEntityList>> GetEntityList();

        Task<IList<DocumentCompletenessDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
    }
}
