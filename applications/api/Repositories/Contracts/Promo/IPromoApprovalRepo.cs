using Repositories.Entities;
using Repositories.Entities.Models;
using Repositories.Entities.Models.PromoApproval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Repositories.Contracts
{
    public interface IPromoApprovalRepository
    {
        Task<IList<PromoApprovalView>> GetPromoApprovalLP(string year, int category, int entity, int distributor,
            int BudgetParent, int channel, string userid);
        Task<IEnumerable<BaseDropDownList>> GetAllEntity();
        Task<IList<BaseDropDownList>> GetDistributorList(int budgetid, int[] arrayParent);
        Task<PromoReviseV3Dto> GetPromoByPrimaryId(int id);
        Task<ErrorMessageDto> ApprovalPromoWithSKP(PromoSKP promoSKP);
        Task<IList<object>> GetCategoryListforPromoApproval();

        //Promo Approval Recon
        Task<IList<PromoApprovalView>> GetPromoApprovalReconLP(string year, int category, int entity, int distributor, int BudgetParent, int channel, string userid);
        Task<ErrorMessageDto> ApprovalPromoRecon(int promoid, string statuscode, string notes, string useremail);
        Task<PromoReconV3> GetPromoReconV3(int Id, string LongDesc = "");
        Task<IList<object>> GetCategoryListforReconApproval();
        Task<DNGetByIdforPromoApproval> GetDNDetailbyIdforPromoApproval(int id);
        Task<IList<object>> GetDNPaidforPromoApprovalRecon(int id);
        Task<IList<object>> GetDNClaimforPromoApprovalRecon(int id);
        Task<IEnumerable<DNMaterial>> GetTaxLevelforPromoApprovalRecon();
    }
}

