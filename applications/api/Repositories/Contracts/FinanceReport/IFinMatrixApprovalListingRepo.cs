using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IFinMatrixApprovalListingRepo
    {
        Task<MatrixApprovalLandingPage> GetMatrixApprovalLandingPage(
            string period, int categoryId, int entityId, int distributorId,
            string search, string sortColumn, int pageNum = 0, int dataDisplayed = 10, string sortDirection = "ASC"
        );
        Task<IList<MatrixApprovalEntityList>> GetEntityList();
        Task<IList<MatrixApprovalDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
        Task<IList<object>> GetCategoryDropdownList();
        Task<object> GetMatrixPromoAprovalHistoryList(int category, int entity, int distributor, string userid, int start, int length, string txtSearch, string order, string sort);
    }
}
