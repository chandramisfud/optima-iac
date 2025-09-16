using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Configuration
{
    public class ConfigReminderList
    {
        public int id { get; set; }
        public int remindertype { get; set; }
        public string? remindertypename { get; set; }
        public string? categoryname { get; set; }
        public int category { get; set; }
        public string? Description { get; set; }
        public int daysfrom { get; set; }
        public int daysto { get; set; }
        public int frequency { get; set; }
        public string? userinput { get; set; }
        public DateTime dateinput { get; set; }
        public string? useredit { get; set; }
        public DateTime dateedit { get; set; }
        public int isdeleted { get; set; }
        public string? deletedby { get; set; }
        public DateTime deletedon { get; set; }
    }

    public partial class ConfigReminderUpdate
    {
        [Key]
        public int id { get; set; }
        public int daysfrom { get; set; }
        public int daysto { get; set; }
        public int frequency { get; set; }
        public string? useredit { get; set; }
        public DateTime dateedit { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class ConfigReminderListUpdate
    {
        public IList<ConfigReminderUpdate>? configList { get; set; }
    }
}
