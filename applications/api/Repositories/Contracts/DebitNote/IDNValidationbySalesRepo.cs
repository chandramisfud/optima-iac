using System.Data;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNValidationbySalesRepo
    {
        Task<IList<DNValidationbySales>> GetDNByStatusForValidateBySales(string status, string userid, int entityid, int distributorid, string TaxLevel, string period);
        Task<DNValidationbySales> DNChangeStatusSalesMultiApproval(string status,
            string userid,
            List<DNId> dnid
            );
        Task<DNValidationbySalesbyId> GetDNbyIdforValidationbySales(int id);
        Task<PromobyIdforValidationbySalesDC> GetPromobyIdforDNValidationbySalesDC(int id);
        Task<PromobyIdforValidationbySalesRC> GetPromobyIdforDNValidationbySalesRC(int id);
        Task<DNValidationbySales> DNValidatebySalesApproval(int DNId, string status, string notes, string userid);
        Task DeletePromoAttachmentDNValidatebySales(int PromoId, string DocLink);
        Task<IList<DNValidationbySalesEntityList>> GetEntityList();
        Task<IList<DNValidationbySalesDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
        // debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}
        Task<IList<DNFilter>> DNFilterValidationbySales(string userid, string status, int entity, string TaxLevel, DataTable dn);
        Task<RCorDCValue> SelectPromoRCorDC(int id);

    }
}