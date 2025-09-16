using System.ComponentModel;

namespace V7.Model.Promo
{
    public class PromoCreationSSValueParam
    {
        public int period { get; set; }
        public int channelId { get; set; }
        public int subChannelId { get; set; }
        public int accountId { get; set; }
        public int subAccountId { get; set; }

        public int groupBrandId { get; set; }

        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }


    }
    public class PromoCreationPSValueParam
    {
        public int period { get; set; }
        public int distributorId { get; set; }
        public int groupBrandId { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }

    }

    public class PromoCreationBaselineParam
    {
        public int promoId { get; set; }
        public int period { get; set; }
        public DateTime date { get; set; }
        public int pType { get; set; }
        public int distributor { get; set; }
        public int[] region { get; set; }
        public int channel { get; set; }
        public int subChannel { get; set; }
        public int account { get; set; }
        public int subAccount { get; set; }
        public int[] product { get; set; }
        public int subCategory { get; set; }
        public int subActivity { get; set; }
        public int grpBrand { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }


    }

    public class PromoCreationCRParam
    {
        public int period { get; set; }
        public int subActivityId { get; set; }
      
        public int subAccountId { get; set; }

        public int distributorId { get; set; }
        public int grpBrandId { get; set; }

    }

    // DB Ref: promoFormType

    public class PromoCreationFormType
    {
        public int periode { get; set; }
        public int entityId { get; set; }
        public int distributorId { get; set; }
        public int budgetMasterId { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int activityId { get; set; }
        public int subActivityId { get; set; }
        public int subActivityTypeId { get; set; }
        public string? activityDesc { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }

        public int channelId { get; set; }
        public int subChannelId { get; set; }
        public int accountId { get; set; }
        public int subAccountId { get; set; }
        public int groupBrandId { get; set; }
        public decimal baseline { get; set; }
        public decimal upLift { get; set; }
        public decimal totalSales { get; set; }
        public decimal salesContribution { get; set; }
        public decimal storesCoverage { get; set; }
        public decimal redemptionRate { get; set; }
        public decimal cr { get; set; }
        public decimal cost { get; set; }
        public string statusApproval { get; set; }
        public string notes { get; set; }
        public string initiator_notes { get; set; }

        public string modifReason { get; set; }
    }

    public class DBPromoFormType
    {
        public int promoId { get; set; }
        public int periode { get; set; }
        public int entityId { get; set; }
        public int distributorId { get; set; }
        public int budgetMasterId { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int activityId { get; set; }
        public int subActivityId { get; set; }
        public int subActivityTypeId { get; set; }
        public string? activityDesc { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }

        public int channelId { get; set; }
        public int subChannelId { get; set; }
        public int accountId { get; set; }
        public int subAccountId { get; set; }
        public int groupBrandId { get; set; }
        public decimal baseline { get; set; }
        public decimal upLift { get; set; }
        public decimal totalSales { get; set; }
        public decimal salesContribution { get; set; }
        public decimal storesCoverage { get; set; }
        public decimal redemptionRate { get; set; }
        public decimal cr { get; set; }
        public decimal cost { get; set; }
        public string statusApproval { get; set; }
        public string notes { get; set; }
        public DateTime createOn { get; set; }
        public string createBy { get; set; }
        public string initiator_notes { get; set; }

        public string createdEmail { get; set; }
        public string modifReason { get; set; }
    }

    public class PromoCreationInsertParam
    {
        public DBPromoFormType promo { get; set; }
        public List<ArrayIntType> region { get; set; }
        public List<ArrayIntType> sku { get; set; }
        public List<DBPromoMechanismType> mechanism { get; set; }
        public List<DBPromoAttachmentType> attachment { get; set; }
    }

    public class promoV2ReconUpdateCalculatorRecon
    {
        public decimal baselineCalcRecon { get; set; } = 0;
        public decimal upliftCalcRecon { get; set; } = 0;
        public decimal totalSalesCalcRecon { get; set; } = 0;
        public decimal salesContributionCalcRecon { get; set; } = 0;
        public decimal storesCoverageCalcRecon { get; set; } = 0;
        public decimal redemptionRateCalcRecon { get; set; } = 0;
        public decimal crCalcRecon { get; set; } = 0;
        public decimal roiCalcRecon { get; set; } = 0;
        public decimal costCalcRecon { get; set; } = 0;
    }

    public class PromoV2ReconUpdateParam
    {
        public DBPromoFormType promo { get; set; }
        public List<ArrayIntType> region { get; set; }
        public List<ArrayIntType> sku { get; set; }
        public List<DBPromoMechanismType> mechanism { get; set; }
        public List<DBPromoAttachmentType> attachment { get; set; }

       public promoV2ReconUpdateCalculatorRecon calculatorRecon { get; set; }

    }

    public class DBPromoAttachmentType
    {
        public string? fileName { get; set; }
        public string? docLink { get; set; }

    }
    public class ArrayIntType
    {
        public int id { get; set; }

    }
    public class DBPromoMechanismType
    {
        public int id { get; set; }
        public string? mechanism { get; set; }
        public string? notes { get; set; }
        public int productId { get; set; }
        public double baseline { get; set; }
        public double uplift { get; set; }
        public double totalSales { get; set; }
        public double salesContribution { get; set; }
        public double storesCoverage { get; set; }
        public double redemptionRate { get; set; }
        public double cr { get; set; }
        public double cost { get; set; }

    }
}
