namespace V7.Model.Tools
{
    public class PromoCancelBodyRequest
    {
        public int promoId { get; set; }
        public string? userId { get; set; }
        public string? statusCode { get; set; }
        public string? approverEmail { get; set; }
    }

    public class PromoPlanningCancelBodyRequest
    {
        public int promoPlanId { get; set; }
        public string? userId { get; set; }
        public string? notes { get; set; }
    }
}