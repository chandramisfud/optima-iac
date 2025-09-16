using Repositories.Entities;
using Repositories.Entities.Models;
using Repositories.Entities.BudgetAllocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBudgetMasterRepository
    {
        Task<BaseLP> GetBudgetMasterLandingPage(string year, int entity, int distributor, 
            string userid, string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        Task<IEnumerable<BaseDropDownList>> GetAllEntity();
        Task<IList<BaseDropDownList>> GetDistributorList(int budgetid, int[] arrayParent);
        Task<IList<BaseDropDownList>> GetCategoryList();
        Task<BudgetMasterDto> BudgetMasterById(int Id);
        Task<bool>CreateBudgetMaster(BudgetMasterSaveDto budgetMaster);
        Task<bool> UpdateBudgetMaster(BudgetMasterSaveDto budgetMaster);
        Task<bool> DeleteBudgetMaster(BudgetMasterDeleteDto budgetMasterDeleteDto);

        // Budget ALlocation part
        #region budget allocation
        Task<List<BudgetAllocationView>> GetBudgetAllocationLandingPage(string year, int entity, int distributor,
            int BudgetParent, int channel, string userid);
        Task<List<BudgetAllocatedSource>> GetAllocatedBudgetSource(string year, int entity, int distributor,
            string userid, string budgetType);
        //Task<List<BudgetAllocatedSource>> GetAllocatedBudgetSourceById(int id, string budgetType);
        Task<ErrorMessageDto> CreateBudgetAllocation(BudgetAllocationStoreDto budgetallocation);
        Task<ErrorMessageDto> UpdateBudgetAllocation(BudgetAllocationUpdateDto budgetallocation);
        Task<BudgetAllocationDto> GetBudgetAllcationByPrimaryId(int id);
        Task<object> GetAllocatedBudgetSourceById(int id, string budgetType);
        #endregion

        //Budget Allocation Attribut
        #region Budget Allocaton Attribute
        Task<IEnumerable<Entities.BudgetAllocation.Region>> GetAllRegion();
        Task<IEnumerable<Entities.BudgetAllocation.Channel>> GetAllChannel();
        Task<IEnumerable<Entities.BudgetAllocation.SubChannel>> GetAllSubChannel(int channelId);
        Task<IEnumerable<Entities.BudgetAllocation.Account>> GetAllAccount(int subChannelId);
        Task<List<UserAllDto>> GetAllActiveUser();
        Task<IList<MasterIdDesc>> GetBrandAttributeByParent(int budgetid, int[] arrayParent);
        Task<IEnumerable<Entities.BudgetAllocation.Product>> GetAllProductByBrandId(int brandId);
        Task<IList<MasterIdDesc>> GetSubCategoryAttributeByParent(int budgetid, int[] arrayParent);
        Task<IList<MasterIdDesc>> GetActivityAttributeByParent(int budgetid, int[] arrayParent);
        Task<IList<MasterIdDesc>> GetSubActivityAttributeByParent(int budgetid, int[] arrayParent);
        Task<IEnumerable<ViewSubAccount>> GetAllSubAccount(int accountId);
        #endregion

        // budget Assignment
        #region budget Assignment
        Task<IList<BudgetAssignmentView>> GetBudgetAssignmentLP(string year, int entity, int distributor, int BudgetParent, int channel, string userid);
        Task<IList<BudgetAllocationView>> GetBudgetAllocationByConditions(string year, int entity, int distributor, int BudgetParent, int channel, string userid);
        Task<BudgetAllocationDto> GetByPrimaryId(int id);
        Task<ErrorMessageDto> CreateBudgetAssignment(BudgetAssignmentStoreDto budgetAssignment);
        Task<ErrorMessageDto> UpdateBudgetAssignment(BudgetAssignmentUpdateDto budgetAssignment);
        Task<BudgetAssignmentDto> GetAssignmentById(int Id);
        #endregion
    }
}
