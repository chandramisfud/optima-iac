using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using V7.Model.Budget;
using V7.Model.UserAccess;

namespace V7.Model.Promo
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum sortPromoReconApprovalField
    {
        RequestId,
        RequestDate,
        RefId,
        StartPromo,
        ActivityDesc,
        AgingApproval,
        Initiator,
        Investment
    }

    public class PromoReconApprovalParam : LPParam
    {

        public int entity { get; set; }
        public int distributor { get; set; }
        public int category { get; set; }
        public sortPromoReconApprovalField SortColumn { get; set; } = sortPromoReconApprovalField.RequestId;
        public sortDirection SortDirection { get; set; } = sortDirection.asc;

    }
}
