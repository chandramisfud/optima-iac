using Repositories.Entities.Configuration;

namespace Repositories.Contracts
{
    public interface IPromoCalculatorRepo
    {
        Task<object> GetPromoCalculatorById(int mainActivityId, int channelId);
        Task<object> GetPromoCalculatorChannel();
        Task<object> GetPromoCalculatorFilter();
        Task<object> GetPromoCalculatorFilterAndSubActivityCoverage();
        Task<object> GetPromoCalculatorLP(int mainActivityId, int channelId);
        Task<bool> SetPromoCalculatorSave(string mainActivity, int channel, int baseline, int totalSales, int uplift, int salesContribution, int storesCoverage, int redemptionRate, int cr, int cost, int baselineRecon, int totalSalesRecon, int upliftRecon, int salesContributionRecon, int storesCoverageRecon, int redemptionRateRecon, int crRecon, int costRecon, int[] subActivity, string createdBy, string createdEmail);
        Task<bool> SetPromoCalculatorUpdate(int id, string mainActivity, int channel, int baseline, int totalSales, int uplift, int salesContribution, int storesCoverage, int redemptionRate, int cr, int cost, int baselineRecon, int totalSalesRecon, int upliftRecon, int salesContributionRecon, int storesCoverageRecon, int redemptionRateRecon, int crRecon, int costRecon, int[] subActivity, string createdBy, string createdEmail);
    }
}