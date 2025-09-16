using System.Text.Json.Serialization;
using V7.Model.UserAccess;

namespace V7.Model.Promo
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum sortPromoPlanningField
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
        cancelNotes,
        initiator_notes,
        tsCodeOn,
        tsCodeBy,
        PromorefId
    }

    public class promoPlanningLPParam : LPParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public string? createFrom { get; set; }
        public string? createTo { get; set; }
        public string? startFrom { get; set; }
        public string? startTo { get; set; }

        public sortPromoPlanningField SortColumn { get; set; } = sortPromoPlanningField.refId;
        public sortDirection SortDirection { get; set; } = sortDirection.asc;

    }

    public class promoPlanningDownloadParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public string? createFrom { get; set; }
        public string? createTo { get; set; }
        public string? startFrom { get; set; }
        public string? startTo { get; set; }
    }

    public class DistributorListParam
    {
        public int budgetId { get; set; }

        public int[]? entityId { get; set; }
    }

    public class PromoPlanningHeaderParam
    {
        public int promoPlanId { get; set; }
        public string? periode { get; set; }
        public int distributorId { get; set; }
        public int entityId { get; set; }
        public string? categoryShortDesc { get; set; }
        public string? principalShortDesc { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int activityId { get; set; }
        public int subActivityId { get; set; }
        public string? activityDesc { get; set; }
        public string? startPromo { get; set; }
        public string? endPromo { get; set; }
        public string? mechanisme1 { get; set; }
        public string? mechanisme2 { get; set; }
        public string? mechanisme3 { get; set; }
        public string? mechanisme4 { get; set; }
        public decimal investment { get; set; }
        public decimal normalSales { get; set; }
        public decimal incrSales { get; set; }
        public decimal roi { get; set; }
        public decimal costRatio { get; set; }
        public string? notes { get; set; }
        public string? initiator_notes { get; set; }
        public string? modifReason { get; set; }
    }

    public class RegionParam
    {
        public int id { get; set; }
    }

    public class ChannelParam
    {
        public int id { get; set; }
    }

    public class SubChannelParam
    {
        public int id { get; set; }
    }

    public class AccountParam
    {
        public int id { get; set; }
    }

    public class SubAccountParam
    {
        public int id { get; set; }
    }

    public class BrandParam
    {
        public int id { get; set; }
    }

    public class ProductParam
    {
        public int id { get; set; }
    }

    public class MechanismParam
    {
        public int id { get; set; }
        public string? mechanism { get; set; }
        public string? notes { get; set; }
        public int productId { get; set; }
        public string? product { get; set; }
        public int brandId { get; set; }
        public string? brand { get; set; }
    }

    public class PromoPlanningParam
    {
        public PromoPlanningHeaderParam? PromoPlanningHeader { get; set; }
        public List<RegionParam>? Regions { get; set; }
        public List<ChannelParam>? Channels { get; set; }
        public List<SubChannelParam>? SubChannels { get; set; }
        public List<AccountParam>? Accounts { get; set; }
        public List<SubAccountParam>? SubAccounts { get; set; }
        public IList<BrandParam>? Brands { get; set; }
        public IList<ProductParam>? Skus { get; set; }
        public IList<MechanismParam>? Mechanisms { get; set; }

    }

    public class AttributeParam
    {

        public int budgetId { get; set; }
        public string? attribute { get; set; }
        public int[]? arrayParent { get; set; }
        public string? isDeleted { get; set; }
    }

    public class MechanismSourceParam
    {
        public int entityId { get; set; }
        public int subCategoryId { get; set; }
        public int activityId { get; set; }
        public int subActivityId { get; set; }
        public int skuId { get; set; }
        public int channelId { get; set; }
        public int brandId { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }

    }

    public class ValidateMechanismParam
    {
        public int promoId { get; set; }
        public int entityId { get; set; }
        public int subCategoryId { get; set; }
        public int activityId { get; set; }
        public int subActivityId { get; set; }
        public int skuId { get; set; }
        public int channelId { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }

    }


    public class BaselineParam
    {
        public int promoId { get; set; }
        public int period { get; set; }
        public string? dateCreation { get; set; }
        public int typePromo { get; set; }
        public int subCategoryId { get; set; }
        public int subActivityId { get; set; }
        public int distributorId { get; set; }
        public string? startPromo { get; set; }
        public string? endPromo { get; set; }
        public int[]? arrayRegion { get; set; }
        public int[]? arrayChannel { get; set; }
        public int[]? arraySubChannel { get; set; }
        public int[]? arrayAccount { get; set; }
        public int[]? arraySubAccount { get; set; }
        public int[]? arrayBrand { get; set; }
        public int[]? arraySKU { get; set; }
    }

    public class PromoPlanningExistParam
    {
        public string? period { get; set; }
        public string? activityDesc { get; set; }
        public int[]? arrayChannel { get; set; }
        public int[]? arrayAccount { get; set; }
        public string? startPromo { get; set; }
        public string? endPromo { get; set; }


    }
    public class PromoPlanningCancelParam
    {
        public int promoPlanningId { get; set; }
        public string? reason { get; set; }
    }

    public class PromoPlanningViewbyConditionsParam
    {
        public string? periode { get; set; }
        public int entityId { get; set; }
        public int distributorId { get; set; }
        public string? create_from { get; set; }
        public string? create_to { get; set; }
        public string? start_from { get; set; }
        public string? start_to { get; set; }
    }
    public class promoPlanningUploadParam
    {
        public string? userId { get; set; }
    }

}
