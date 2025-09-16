using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoApproval
{
    public class promoApprover
    {
        public int id { get; set; }
        public string refId { get; set; }
        public string userIdInitiaator { get; set; }
        public string userNameInitiaator { get; set; }
        public string emailInitiator { get; set; }
        public string userIdApprover { get; set; }
        public string userNameApprover { get; set; }
        public string emailApprover { get; set; }
        public decimal cost { get; set; }
    }


    public class approverParamKey
    {
        public string promoId { get; set; }
        public string refId { get; set; }
        public string profileId { get; set; }
        public string nameApprover { get; set; }
        public string sy { get; set; }
    }

    public class promoCostApproved
    {
        public string promoId { get; set; }
        public decimal cost { get; set; }
        public int approved { get; set; }
        public int deployed { get; set; }
        
    }
}
