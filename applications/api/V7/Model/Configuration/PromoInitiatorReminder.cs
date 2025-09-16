namespace V7.Model.Configuration
{
    public class PromoInitiatorParam
    {
        public ConfigLisPromoInitiator[]? configList { get; set; }
    }

    public class ConfigLisPromoInitiator
    {
        public int id { get; set; }
        public string? datereminder { get; set; }
    }
}
