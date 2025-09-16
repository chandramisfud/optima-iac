using System.Data;
using Entities.Tools;
using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IUploadRepo
    {
      
        Task<int> ImportMatrixBudget(string userid, DataTable importMatrix);
        Task<int> ImportChannel(DataTable importChannel);
        Task<int> ImportBudget(DataTable importBudget, DataTable derBudget, string userid);
        Task<IList<ImportBudgetResponse>> ImportBudgetDC(DataTable importBudget);
        Task<IList<DCImportTableTemp>> GetDCTableTemp();
        Task<IList<ImportBudgetResponse>> ImportBudgetWithAttribute(
        DataTable importBudget
        , DataTable derBudget
        , DataTable account
        , DataTable user
        , DataTable derivativeuser
        , DataTable region
        , DataTable brand
        , string userid
        );

        Task<IList<ImportBudgetResponse>> ImportBudgetAdjustment(DataTable derBudget, string userid);
        Task<int> ImportBudgetAttribute(DataTable account, string userid);
        Task<int> ImportBudgetBrand(DataTable brand, string userid);
        Task<int> ImportMaster(
        DataTable brand,
        DataTable channel,
        DataTable subactivitytype,
        DataTable category,
        DataTable distributor,
        DataTable principal,
        string userid);

        Task<int> ImportMasterBrand(
        DataTable brand,
        string userid);


        Task<int> ImportMasterChannel(
        DataTable channel,
        string userid);

        Task<int> ImportSubactivityType(
        DataTable subactivitytype,
        string userid);

        Task<int> ImportCategory(
        DataTable category,
        string userid);

        Task<int> ImportDistributor(
        DataTable distributor,
        string userid);

        Task<int> ImportPrincipal(
        DataTable principal,
        string userid);

        Task<int> ImportRegion(
        DataTable region,
        string userid);

        Task<int> ImportSellingpoint(
        DataTable sellingpoint,
        string userid
        );
        Task<int> ImportBudgetRegion(DataTable importBudgetRegion, string userid);
        Task<int> ImportBudgetUser(DataTable importBudgetUser, string userid);
        Task CreatePromoAttachment(SavePromoAttachmentParam body);
        Task RemovePromoAttachment(
            int PromoId,
            string DocLink
        );
        Task<SearchPromobyRefidDto> SearchPromoByRefId(string refId);
        Task<IList<HierarchyResult>> GetBudgetHierarchyforAdjust(string period, int entityId, string budgetName);
        Task<IList<AllocationforAdjustResult>> GetBudgetAllocationforAdjust(string period, int entityId);
        Task<IList<ToolsUploadEntityList>> GetEntityList();
        //api/promo/listattach/{periode}/{entity}/{userid}
        Task<IList<PromoListAttachment>> GetPromoNoteListAttachment(string periode, int entity, string userid);
        Task<IList<string>> GetGroupBrand();
        Task<IList<string>> GetSubActivityTypeDC();
        Task<IList<string>> GetActiveDistributor();
        
       // Task<List<int>> GetMatrixApprovalProcessId();
       
        Task<object> GetMatrixApprovalByProcess(int id);
        Task<int> SetMatrixApprovalFinishedByProcess(int id);
       
        Task<object> GetMatrixApprovalPromoByMatrix(int id);
        Task<object> GetUploadLog(string activity);
        Task<int> InsertUploadLog(string activity, string filename, string profileId, string email, string status);
      
        Task<object> GetImportPromoUploadAttachment(DataTable data, string profileId);
        Task<object> ImportPromoUploadAttachment(int promoId, string profileId, string email);
        Task<int> UpdatePromoReconStatus(DataTable lsPromo, string userProfile, string useremail);
        Task<object> ImportMatrix(string userid, string useremail, DataTable matrix);
    }
}