using Repositories.Entities.Configuration;

namespace V7.Model.Configuration
{
    public class ConfigPromoCalculator
    {
        public string mainActivity { get; set; }
        public int channelId { get; set; }
        public int baseline { get; set; }
        public int totalSales { get; set; }
        public int uplift { get; set; }
        public int salesContribution { get; set; }
        public int storesCoverage { get; set; }
        public int redemptionRate { get; set; }
        public int cr { get; set; }
        public int cost { get; set; }
        public int baselineRecon { get; set; }
        public int totalSalesRecon { get; set; }
        public int upliftRecon { get; set; }
        public int salesContributionRecon { get; set; }
        public int storesCoverageRecon { get; set; }
        public int redemptionRateRecon { get; set; }
        public int crRecon { get; set; }
        public int costRecon { get; set; }

    }

    public class ConfigPromoCalculatorCreateParam
    {
        public ConfigPromoCalculator[] configCalculator { get; set; }
        public int[] subActivity { get; set; }

    }

    public class ConfigPromoCalculatorUpdateParam
    {
        public ConfigPromoCalculatorUpdate[] configCalculator { get; set; }
        public int[] subActivity { get; set; }

    }

    public class ConfigPromoCalculatorUpdate
    {
        public int mainActivityId { get; set; }
        public string mainActivity { get; set; }
        public int channelId { get; set; }
        public int baseline { get; set; }
        public int totalSales { get; set; }
        public int uplift { get; set; }
        public int salesContribution { get; set; }
        public int storesCoverage { get; set; }
        public int redemptionRate { get; set; }
        public int cr { get; set; }
        public int cost { get; set; }
        public int baselineRecon { get; set; }
        public int totalSalesRecon { get; set; }
        public int upliftRecon { get; set; }
        public int salesContributionRecon { get; set; }
        public int storesCoverageRecon { get; set; }
        public int redemptionRateRecon { get; set; }
        public int crRecon { get; set; }
        public int costRecon { get; set; }

    }
}


