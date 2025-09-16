namespace V7.Model.Configuration
{
    public class ReminderParam
    {
        public ConfigListReminder[]? configList { get; set; }
    }

    public class ConfigListReminder
    {
        public int id { get; set; }
        public int daysfrom { get; set; }
        public int daysto { get; set; }
        public int frequency { get; set; }
    }
}
