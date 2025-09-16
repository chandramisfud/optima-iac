using System.Text.Json.Serialization;
using V7.Model.UserAccess;

namespace V7.Model.Promo
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum sortPromoReconField
    {
        refId,
        tsCode,
        entityShortDesc,
        distributorShortDesc,
        subAccountDesc,
        brandDesc,
        activityDesc,
        mechanism,
        startPromo,
        endPromo,
        investment,
        lastStatus,
        initiator_notes,
        tsCodeOn,
        tsCodeBy,
    }

    public class promoReconLPParam : LPParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int categoryId { get; set; }
        public int distributor { get; set; }
        public int budgetParent { get; set; }
        public int channel { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }

    }

    public class promoReconDownloadParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public int categoryId { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }
    }


    public class PromoReconHeaderParam
    {
        public int promoId { get; set; }
        public int promoPlanId { get; set; }
        public int allocationId { get; set; }
        public string? allocationRefId { get; set; }
        public string? categoryShortDesc { get; set; }
        public string? principalShortDesc { get; set; }
        public int budgetMasterId { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int activityId { get; set; }
        public int subActivityId { get; set; }
        public string? activityDesc { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }
        public string? mechanisme1 { get; set; }
        public string? mechanisme2 { get; set; }
        public string? mechanisme3 { get; set; }
        public string? mechanisme4 { get; set; }
        public double investment { get; set; }
        public decimal normalSales { get; set; }
        public decimal incrSales { get; set; }
        public decimal roi { get; set; }
        public decimal costRatio { get; set; }
        public string? statusApproval { get; set; }
        public string? notes { get; set; }
        public string? tsCoding { get; set; }
        public string? initiator_notes { get; set; }
        public string? modifReason { get; set; }
    }
    public class PromoReconParam
    {
        public PromoHeaderParam? PromoHeader { get; set; }
        public List<PromoAttributeParam>? Regions { get; set; }
        public List<PromoAttributeParam>? Channels { get; set; }
        public List<PromoAttributeParam>? SubChannels { get; set; }
        public List<PromoAttributeParam>? Accounts { get; set; }
        public List<PromoAttributeParam>? SubAccounts { get; set; }
        public IList<PromoAttributeParam>? Brands { get; set; }
        public IList<PromoAttributeParam>? Skus { get; set; }
        public IList<MechanismParam>? Mechanisms { get; set; }
    }
}
