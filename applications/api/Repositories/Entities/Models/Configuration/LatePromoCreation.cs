using Repositories.Entities.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Entities.Configuration
{
    public partial class LatePromoCreationConfig
    {
        [Key]
        public string? id { get; set; }
        public string? daysfrom { get; set; }
        public string? useredit { get; set; }
        public DateTime dateedit { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class LatePromoCreation
    {
        public IList<LatePromoCreationConfig>? configList { get; set; }
    }
}
