using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Models
{
    public class PromoCreationV2AttributeList
    {
        public  object distibutor { get; set; }
        public  object entity { get; set; }
        public  object grpBrand { get; set; }
        public  object sku { get; set; }
        public  object category { get; set; }
        public  object subCategory { get; set; }
        public  object activity { get; set; }
        public  object subActivity { get; set; }
        public  object channel { get; set; }
        public  object subChannel { get; set; }
        public  object account { get; set; }
        public  object subAccount { get; set; }
        public  object region { get; set; }
        public  object configCalculator { get; set; }
    }

    public class promoCreationResult
    {
        public int id { get; set; }
        public string refId { get; set; }
        public bool isSendEmail { get; set; }
        public object dataEmail { get; set; }
    }
}
