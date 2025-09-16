using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface ICancelReasonRepository
    {
        Task<BaseLP> GetCancelReasonLandingPage(
            string keyword
            , string sortColumn
            , string sortDirection
            , int dataDisplayed
            , int pageNum            );
        Task<CancelReasonModel> GetCancelReasonById(CancelReasonById body);
        Task<CancelReasonCreateReturn> CreateCancelReason(CancelReasonCreate body);
        Task<CancelReasonUpdateReturn> UpdateCancelReason(CancelReasonUpdate body);
        Task<CancelReasonDeleteReturn> DeleteCancelReason(CancelReasonDelete body);
    }

}