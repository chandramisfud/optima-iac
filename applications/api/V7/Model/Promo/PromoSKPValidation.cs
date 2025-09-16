using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using V7.Model.Report;

namespace V7.Model.Promo
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PromoSKPValidationSortColumn
    {
        skpstatus,
        refId,
        activityDesc,
        createBy,
        createdName,
        channelDesc,
        subAccountDesc,
        skp_notes
    }
    public class PromoSKPValidationParam
    {
        public PromoSKPValidationSortColumn SortColumn { get; set; }
        public SKPValidationSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
        public bool CancelStatus { get; set; }
        public DateTime StartFrom { get; set; }
        public DateTime StartTo { get; set; }
        public int Status { get; set; }
    }
}
