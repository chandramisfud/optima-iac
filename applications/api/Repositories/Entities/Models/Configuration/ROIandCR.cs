using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Configuration
{
    public class ConfigRoi
    {
        public int id { get; set; }
        public string? RefId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ActivityId { get; set; }
        public int SubActivityTypeId { get; set; }
        public string? LongDesc { get; set; }
        public double MinimumROI { get; set; }
        public double MaksimumCostRatio { get; set; }
        public double MaksimumROI { get; set; }
        public double MinimumCostRatio { get; set; }
    }

    public class ListCategory
    {
        public int id { get; set; }
        public string? refId { get; set; }
        public string? shortDesc { get; set; }
        public string? longDesc { get; set; }
    }

    public class ListSubCategory
    {
        public int id { get; set; }
        public string? refId { get; set; }
        public string? shortDesc { get; set; }
        public string? longDesc { get; set; }
    }

    public class ListActivity
    {
        public int id { get; set; }
        public string? refId { get; set; }
        public string? shortDesc { get; set; }
        public string? longDesc { get; set; }
    }

    public class SetRoiCrType
    {
        public int Id { get; set; }
        public double MinimumROI { get; set; }
        public double MaksimumROI { get; set; }
        public double MinimumCostRatio { get; set; }
        public double MaksimumCostRatio { get; set; }

    }

    public class ConfigRoiStore
    {
        public IList<SetRoiCrType>? Config { get; set; }
        public string? UserId { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class ConfigRoiDelete
    {
        public int id { get; set; }
        public string? UserId { get; set; }
        public string? DeletedEmail { get; set; }
    }
}
