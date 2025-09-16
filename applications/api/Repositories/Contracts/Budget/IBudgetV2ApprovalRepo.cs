using Repositories.Entities;
using Repositories.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBudgetV2Repository
    {
        Task<object> GetBudgetApprovalRequestFilter();
      
    
        Task<object> SetBudgetApprovalRequestApprove(string batchId, string profileId, string userEmail);
    
      
        Task<object> GetBudgetDeploymentUpdateStatus(string profileId, string userEmail, string batchId);
        Task<object> GetBudgetDeploymentRequest(DataTable promoId, string profileId, string userEmail, string batchId);
        Task<object> SetBudgetApprovalRequestReject(string batchId, string profileId, string userEmail);
        Task<object> GetBudgetDeploymentRequestList(int period, DataTable channelId, DataTable groupBrand, DataTable subactivityTypeId);
        Task<object> GetBudgetDeploymentRequestFilter();

        Task<object> GetBudgetApprovalByBatch(string batchId);
        Task<object> GetBudgetApprovalRequestReport(int period, DataTable channelId, int categoryId, DataTable groupBrand, DataTable approvalStatus);
        Task<object> GetBudgetApprovalRequestList(int period, DataTable channelId, DataTable groupBrand, DataTable approvalStatus, DataTable month, int? is5Bio, int categoryId, int start, int length, string txtSearch, string sort, string order);
        Task<object> GetBudgetApprovalRequestDataForDownload(int period, int categoryId, DataTable channelId, DataTable groupBrand, DataTable approvalStatus, DataTable month, int? is5Bio);
        Task<object> GetBudgetApprovalRequestDataForEmail(int period, int categoryId, DataTable channelId, DataTable groupBrand, DataTable approvalStatus, DataTable month, int? is5Bio, string profileId, string userEmail);
        void CekAndRunBudgetDeployment(string batchId, string userId, string email);
    }
}
