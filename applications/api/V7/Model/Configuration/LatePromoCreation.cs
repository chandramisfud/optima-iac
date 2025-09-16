namespace V7.Model.Configuration
{

    public class LatePromoCreationParam
    {
        public ConfigList[]? configList { get; set; }
    }

    public class ConfigList
    {
        public string? id { get; set; }
        public string? daysfrom { get; set; }
    }
}
