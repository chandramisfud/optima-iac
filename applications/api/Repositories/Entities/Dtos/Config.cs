namespace Repositories.Entities.Dtos
{
    public class Config
    {

        public string? ID { get; set; }
        public string? Name { get; set; }
        public int NoUrut { get; set; }
    }

    public class BlitzNotif
    {
        public IList<ItemType>? itemtype { get; set; }
        public IList<BlitzEmail>? blitzemail { get; set; }
    }

    public class ItemType
    {
        public string? type { get; set; }
        public string? code { get; set; }
        public string? desc { get; set; }

    }

    public class BlitzEmail
    {
        public string? email_to { get; set; }
        public string? email_cc { get; set; }
        public string? email_subject { get; set; }

    }

    public class ConfigLatePromoCreation
    {
        public int id { get; set; }
        public int remindertype { get; set; }
        public int category { get; set; }
        public string? description { get; set; }
        public int days { get; set; }

    }

}
