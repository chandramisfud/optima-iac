using Repositories.Entities;
using System.Text.Json.Serialization;
using V7.Model.UserAccess;

namespace V7.Model.Promo
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum sortPromoCreationField
    {
        refId,
        lastStatus,
        activityDesc,
        startPromo,
        endPromo,
        tsCoding,
        investment,
        initiator_notes,
        investmentBfrClose,
        investmentClosedBalance
    }

    public class promoCreationLPParam : LPParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public int category { get; set; }
        public sortPromoCreationField SortColumn { get; set; } = sortPromoCreationField.refId;
        public sortDirection SortDirection { get; set; } = sortDirection.asc;

    }

    public class promoCreationDownloadParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public int category { get; set; }
    }

    public class SourceBudgetParam
    {
        public string? year { get; set; }
        public int entityId { get; set; }
        public int distributorId { get; set; }
        public int subCategoryId { get; set; }
        public int activityId { get; set; }
        public int subActivityId { get; set; }
        public int[]? arrayRegion { get; set; }
        public int[]? arrayChannel { get; set; }
        public int[]? arraySubChannel { get; set; }
        public int[]? arrayAccount { get; set; }
        public int[]? arraySubAccount { get; set; }
        public int[]? arrayBrand { get; set; }
        public int[]? arraySKU { get; set; }
    }

    public class PromoHeaderParam
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

    public class PromoAttributeParam
    {
        public int id { get; set; }
    }

    public class Promoattachment
    {
        public string? docLink { get; set; }
        public string? fileName { get; set; }
    }

    public class Mechanism
    {
        public int id { get; set; }
        public string? mechanism { get; set; }
        public string? notes { get; set; }
        public int productId { get; set; }
        public string? product { get; set; }
        public int brandId { get; set; }
        public string? brand { get; set; }
    }
    public class PromoAttachmentStoreParam
    {
        public string? docLink { get; set; }
        public string? fileName { get; set; }

    }

    public class PromoParam
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
        public IList<PromoAttachmentStoreParam>? promoAttachment { get; set; }
        public double budgetAmount { set; get; }

    }

    public class PromoReconUpdateParam
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
        public IList<PromoAttachmentStoreParam>? promoAttachment { get; set; }
        public bool Reconciled { get; set; }
        public bool ReconciledUpd { get; set; }

    }
    public class PromoExistParam
    {
        public string? period { get; set; }
        public string? activityDesc { get; set; }
        public int[]? arrayChannel { get; set; }
        public int[]? arrayAccount { get; set; }
        public string? startPromo { get; set; }
        public string? endPromo { get; set; }


    }

    public class PromoExistDCParam
    {
        public string? period { get; set; }
        public int distributorId { get; set; }
        public int subActivityTypeId { get; set; }
        public int subActivityId { get; set; }
        public string? startPromo { get; set; }
        public string? endPromo { get; set; }


    }
    public class PromoResponse
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? userid_initiator { get; set; }
        public string? username_initiator { get; set; }
        public string? email_initiator { get; set; }
        public string? userid_approver { get; set; }
        public string? username_approver { get; set; }
        public string? email_approver { get; set; }
        public bool IsFullyApproved { get; set; }
    }
    public class PromoReconResponse : PromoResponse
    {
        public bool major_changes { get; set; }

    }

    public class PromoAttachmentParam
    {
        public int promoId { get; set; }
        public string? docLink { get; set; }
        public string? fileName { get; set; }
    }
    public class PromoDeleteAttachmentParam
    {
        public int promoId { get; set; }
        public string? docLink { get; set; }
    }

    public class PromoCancelReqParam
    {
        public int promoId { get; set; }
        public string? notes { get; set; }
    }
}
