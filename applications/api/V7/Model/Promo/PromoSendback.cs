namespace V7.Model.Promo
{
    public class PromoSendbackResponse
    {
        public int id { get; set; }
        public string? refId { get; set; }
        public string? userid_approver { get; set; }
        public string? username_approver { get; set; }
        public string? email_approver { get; set; }
        public bool IsFullyApproved { get; set; }
    }
}
