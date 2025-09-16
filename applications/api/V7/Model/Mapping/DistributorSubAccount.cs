using V7.Model.Configuration;

namespace V7.Model.Mapping
{
    public class DIstributorSubAccountPostBody
    {
        public int id { get; set; }
        public int distributorId { get; set; }
        public int channelId { get; set; }
        public int subChannelId { get; set; }
        public int accountId { get; set; }
        public int subAccountId { get; set; }
    }

    public class DIstributorSubAccountDeleteBody
    {
        public int id { get; set; }
    }
}
