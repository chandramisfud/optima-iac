namespace V7.Model.Mapping
{

    public class PICDNManualPostBody
    {
        public int channelId { get; set; }
        public int subChannelId { get; set; }
        public int accountId { get; set; }
        public int subAccountId { get; set; }
        public string? pic1 { get; set; }
        public string? pic2 { get; set; }
    }

    public class PICDNManualDeleteBody
    {
        public int id { get; set; }
    }
}
