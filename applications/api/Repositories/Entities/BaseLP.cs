using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Models
{
    public class BaseLP
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<Object>? Data { get; set; }
    }
    public class BaseLP2
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<Object>? Data { get; set; }
    }

    public class BaseLPStats
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }

    }


}