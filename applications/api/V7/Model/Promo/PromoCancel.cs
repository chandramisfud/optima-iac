namespace V7.Model.Promo
{
    public class PromoCancelRequestParam
    {
        public int promoId { get; set; }
        public required int planningId { get; set; }
        public required string notes { get; set; }
    }
}
