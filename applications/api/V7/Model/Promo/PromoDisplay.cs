using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace V7.Model.Promo
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum sortPromoDisplayField
    {
        promoId,
        lastStatus,
        activityDesc,
        mechanism,
        tsCode,
        allocation,
        investment
    }

    public class promoDisplayLPParam : LPParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int budgetParent { get; set; }
        public int channel { get; set; }
        public int distributor { get; set; }

    }


    public class PromoDisplayLandingPageParam
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public string? year { get; set; }
        public int entity { get; set; }
        public int budgetParent { get; set; }
        public int channel { get; set; }
        public int distributor { get; set; }

    }
}
