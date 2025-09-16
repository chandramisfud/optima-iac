namespace V7.Model.Mapping
{
    public class UserSubAccountPostBody
    {
        public string? profileId { get; set; }
        public int subAccountId { get; set; }
    }

    public class UserSubAccountDeleteBody
    {
        public int id { get; set; }
    }
}
