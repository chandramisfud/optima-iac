using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IMatrixPromoRepository
    {
        Task<IList<MatrixPromoModel>> GetMatrixPromoAproval(MatrixPromoApprovalBodyReq body);
        
        
        Task<GetMatrixPromoAprovalbyIdResult> GetMatrixPromoAprovalbyId(GetMatrixPromoAprovalbyIdBody body);
        Task<IList<EntityforMatrixPromo>> GetEntityForMatrixPromo();
        Task<IList<DistributorforMatrixPromo>> GetDistributorforMatrixPromo(int PrincipalId);
        Task<IList<SubActivityTypeforMatrixPromo>> GetSubActivityTypeforMatrixPromo();
        Task<IList<ChannelforMatrixPromo>> GetChannelforMatrixPromo();
        Task<IList<SubChannelforMatrixPromo>> GetSubChannelforMatrixPromo(int ChannelId);
        Task<IList<InitiatorforMatrixPromo>> GetInitiatorforMatrixPromo();
        Task<IList<CategoryforMatrixPromo>> GetCategoryforMatrixPromo();
        Task<IList<object>> GetSubActivityTypebyCategoryId(int categoryId);
        
        Task<object> CreateMatrixPromoAproval(MatrixPromoApprovalInsert body);
        Task<object> UpdateMatrixPromoAproval(MatrixPromoApprovalUpdate body);
    
        Task<object> GetMatrixPromoAprovalHistory(int category, int entity, int distributor, string userid, int start, int length, string txtSearch, string order, string sort);
    }
}