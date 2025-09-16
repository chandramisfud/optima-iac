using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    /// <summary>
    /// General model to handle data for dropdown list
    /// </summary>
    public class BaseDropDownList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class SubCategoryDropDownList
    {
        public int id { get; set; }
        public string? shortDesc { get; set; }
        public string? longDesc { get; set; }
        public int? categoryId { get; set; }
        public string? categoryShortDesc { get; set; }
        public string? categoryLongDesc { get; set; }
    }

}
