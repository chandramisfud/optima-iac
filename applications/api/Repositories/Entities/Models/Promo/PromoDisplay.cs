using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Models.Promo
{
    public class PromoCreationData
    {
        public int PromoId { get; set; }
        public string? RefId { get; set; }
        public string? ActivityDesc { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public string? Allocation { get; set; }
        public decimal Investment { get; set; }
        public string? LastStatus { get; set; }
        public bool IsCancelLocked { get; set; }
        public string? TsCoding { get; set; }

        //modif by AND Nov 25 '23, change int to string?, DC can hold multi subaccountid, 
        public string? SubAccountId { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? CancelNotes { get; set; }
        public string? skp_flagging_status { get; set; }
        public int entityId { get; set; }
        public string? entityName { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public string? initiator_notes { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public string? CreateBy { get; set; }
        public double dnclaim { get; set; }
        public string? CreateOn { get; set; }
        public string? LastStatusDate { get; set; }
        public int isClose { get; set; }

        // #839
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }

        // Added, andrie Oct 9 2023 E2#38
        public int categoryId { get; set; }
        public string? categoryShortDesc { get; set; }
    }

    public class PromoCreationPagination
    {
        public IList<PromoCreationData>? Data { get; set; }
        public RecordTotal? RecordsTotal { get; set; }
    }

    public class PromoDisplayLP
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<PromoDisplayDataforLP>? Data { get; set; }
    }

    public class PromoDisplayDataforLP
    {
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
        public string? PrincipalDesc { get; set; }
        public int PromoId { get; set; }
        public string? RefId { get; set; }
        public string? ActivityDesc { get; set; }
        public string? LastStatus { get; set; }
        public string? Allocation { get; set; }
        public double Investment { get; set; }
        public int IsCancelLocked { get; set; }
        public string? TsCoding { get; set; }
        public string? SubAccountId { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? CancelNotes { get; set; }
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public string? initiator_notes { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? dnclaim { get; set; }
        public string? CreateOn { get; set; }
        public string? LastStatusDate { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryShortDesc { get; set; }

        public int reconciled { get; set; }
    }
}
