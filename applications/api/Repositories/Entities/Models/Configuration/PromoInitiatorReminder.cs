using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Configuration
{
    public class ConfigPromoInitiatorReminderList
    {
        public int id { get; set; }
        public int remindertype { get; set; }
        public int category { get; set; }
        public string? Description { get; set; }
        public DateTime datereminder { get; set; }
    }

    public partial class ConfigPromoInitiatorReminderUpdate
    {
        [Key]
        public int id { get; set; }
        public string? datereminder { get; set; }
        public string? useredit { get; set; }
        public DateTime dateedit { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class ConfigPromoInitiatorReminderListUpdate
    {
        public IList<ConfigPromoInitiatorReminderUpdate>? configList { get; set; }
    }
}
