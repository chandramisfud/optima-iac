using Repositories.Entities.Configuration;

namespace Repositories.Contracts
{
    public interface IPromoItemRepo
    {
        Task<GetPromoItem> GetConfigPromoItems(string categoryShortDesc);
        Task<IList<object>> GetConfigPromoItemsHistory(int year, string? categoryShortDesc);
        Task UpdateConfigPromoItem(int categoryId, string? userId, string? userEmail, PromoItem configPromoItem);
    }
}