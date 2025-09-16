using System.Data;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{

    public interface IDNValidationbyFinanceRepo
    {    // debetnote/validate_by_finance/
        Task<IList<DNValidationbyFinanceWithTickable>> GetDNValidationbyFinance(string status, string userid, int entityid, int distributorid, string TaxLevel);
        //debetnote/finance_multi_approval
        Task<DNValidationbyFinance> DNChangeStatusValidatebyFinanceMultiApproval(
            string status,
            string userid,
            List<DNId> dnid
        );
        //debetnote/getbyId/
        Task<DNValidationbyFinancebyId> GetDNbyIdforValidationbyFinance(int id);
        Task<RCorDCValue> SelectPromoRCorDC(int id);
        //promo/getbyprimaryid/
        Task<PromobyIdforValidationbyFinanceRC> GetPromobyIdforDNValidationbyFinanceRC(int id);
        Task<PromobyIdforValidationbyFinanceDC> GetPromobyIdforDNValidationbyFinanceDC(int id);
        //debetnote/dnvalidation
        Task<DNValidationbyFinance> DNValidationbyFinance(DNValidationbyFinanceParam param);
        //promoattachment/delete
        Task DeletePromoAttachmentForValidationbyFinance(int PromoId, string DocLink);
        Task<IList<DNValidationbyFinanceEntityList>> GetEntityList();
        Task<IList<DNValidationbyFinanceDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
        //api/debetnote/filter/validatebyfinance/{status}/{entity}/{userid}/{TaxLevel}
        Task<IList<DNFilter>> DNFilterValidateByFinance(string userid, string status, int entity, string TaxLevel, DataTable dn);
        Task<DNValidationbyFinance> DNValidationParalelCompleteness(
            int DNId,
            string status,
            string notes,
            string userid,
            string taxlevel,
            int entityId,
            int promoId,
            bool isDNPromo,
            string wHTType,
            string statusPPH,
            double pphPct,
            double pphAmt,
            DNDocCompletenessforValidationbyFinance DNDocCompletenessHeader);
        Task<DNValidationbyFinance> DNFinValidationDocCompleteness(int DNId, string userid, DNDocCompletenessforValidationbyFinance DNDocCompletenessHeader);
    }
}