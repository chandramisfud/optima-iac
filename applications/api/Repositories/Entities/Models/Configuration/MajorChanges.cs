using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Entities.Configuration
{ 
    public class MajorChangesReq
    {
        public int Id { get; set; }
        public bool SubCategory { get; set; }
        public bool Distributor { get; set; }
        public bool Activity { get; set; }
        public bool SubActivity { get; set; } 
        public bool StartPromo { get; set; }
        public bool EndPromo { get; set; }
        public bool ActivityDesc { get; set; }
        public bool InitiatorNotes { get; set; }
        public bool IncrSales { get; set; }
        public bool Investment { get; set; }
        public bool ROI { get; set; }
        public bool CR { get; set; }
        public bool Channel { get; set; }
        public bool SubChannel { get; set; }
        public bool Account { get; set; }
        public bool SubAccount { get; set; } 
        public bool Region { get; set; }
        public bool GroupBrand { get; set; }
        public bool Brand { get; set; }
        public bool SKU { get; set; }
        public bool Mechanism { get; set; }
        public bool BudgetSources { get; set; }
        public bool PromoPlan { get; set; }
        public bool Attachment { get; set; }
        public string? userid { get; set; }
        public string? useremail { get; set; }

    }

    public class MajorChangesResp
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
//        public bool Year { get; set; }
//        public bool Entity { get; set; }
        public bool Distributor { get; set; }
        public bool Activity { get; set; }
        public bool SubActivity { get; set; }
        public bool SubCategory { get; set; }
        public bool StartPromo { get; set; }
        public bool EndPromo { get; set; }
        public bool ActivityDesc { get; set; }
        public bool InitiatorNotes { get; set; }
        public bool IncrSales { get; set; }
        public bool Investment { get; set; }
        public bool ROI { get; set; }
        public bool CR { get; set; }
        public bool Channel { get; set; }
        public bool SubChannel { get; set; }
        public bool Account { get; set; }
        public bool SubAccount { get; set; }
        public bool Region { get; set; }
        public bool GroupBrand { get; set; }
        public bool Brand { get; set; }
        public bool SKU { get; set; }
        public bool Mechanism { get; set; }
        public bool BudgetSources { get; set; }
        public bool PromoPlan { get; set; }
        public bool Attachment { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
}
