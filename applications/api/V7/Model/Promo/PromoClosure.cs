using System.Text.Json.Serialization;

namespace V7.Model.Promo
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum enBudgetRemaining
    {
        /// <summary>
        /// show all budget
        /// </summary>
        ALL = 0,
        /// <summary>
        /// show budget 0-10M
        /// </summary>
        _0To10M = 1,
        /// <summary>
        /// show budget 10-100M
        /// </summary>
        _10To100M = 2,
        /// <summary>
        /// show budget 100-500M
        /// </summary>
        _100To500M = 3,
        /// <summary>
        /// show budget 500-1B
        /// </summary>
        _500To1B = 4,
        /// <summary>
        /// show budget above 1B
        /// </summary>
        Above1B = 5,
    }
   

    public class promoClosureParam
    {
        public int[]? promoId { get; set; }
    }
}
