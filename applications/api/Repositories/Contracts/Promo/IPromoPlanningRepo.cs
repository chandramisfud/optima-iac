using Repositories.Entities;
using Repositories.Entities.Models;
using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repositories.Entities.PromoPlanningView;

namespace Repositories.Contracts
{
    public interface IPromoPlanningRepository
    {
        Task<BaseLP2> GetPromoPlanningLandingPage(string year, int entity, int distributor,
            string createFrom, string createTo, string startFrom, string startTo, string profileId,
           string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize);

        Task<IEnumerable<BaseDropDownList>> GetAllEntity();
        Task<IList<BaseDropDownList>> GetDistributorList(int budgetid, int[] arrayParent);

        Task<PromoResponseDto> PromoPlanningCreate(PromoPlanningDto promo);
        Task<PromoResponseDto> PromoPlanningUpdate(PromoPlanningDto promo);
        Task<IList<PromoPlanningDownloadDto>> GetPromoPlanningDownload(
                string year, int entity, int distributor,
                string createFrom, string createTo, string startFrom, string startTo, string profileId
            );
        Task<IList<BaseDropDownList>> GetAttributeByParent(int budgetid, string attribute, int[] arrayParent, string isDeleted);
        Task<IEnumerable<SubCategoryDropDownList>> GetSubCategory();
        Task<PromoPlanningByIdDto> GetPromoPlanningById(int id);
        Task<IList<MechanisSourceDto>> GetPromoMechanism(
                int entityId, int subCategoryId, int activityId, int subActivityId, int skuId, int channelId,
                string startFrom, string startTo
            );
        Task<IList<BaseDropDownList>> GetPromoAttributeRegion();

        Task<IList<BaseDropDownList>> GetPromoPlanningChannel(string profileId);
        Task<IList<BaseDropDownList>> GetPromoPlanningSubChannel(int[] arrayChannel, string profileId);
        Task<IList<BaseDropDownList>> GetPromoPlanningAccount(int[] arraySubChannel, string profileId);
        Task<IList<BaseDropDownList>> GetPromoPlanningSubAccount(int[] arrayAccount, string profileId);
        Task<IList<BaseDropDownList>> GetPromoPlanningChannelByPlanningId(int promoPlanningId, string profileId);
        Task<IList<BaseDropDownList>> GetPromoPlanningSubChannelByPlanningId(int promoPlanningId, int[] arrayChannel, string profileId);
        Task<IList<BaseDropDownList>> GetPromoPlanningAccountByPlanningId(int promoPlanningId, int[] arraySubChannel, string profileId);
        Task<IList<BaseDropDownList>> GetPromoPlanningSubAccountByPlanningId(int promoPlanningId, int[] arrayAccount, string profileId);
        Task<PromoBaselineDto> GetBaselineSales(
            int promoId, int period, string dateCreation, int typePromo, int subCategoryId, int subActivityId, int distributorId, string startPromo, string endPromo,
            int[] arrayRegion, int[] arrayChannel, int[] arraySubChannel, int[] arrayAccount, int[] arraySubAccount, int[] arrayBrand, int[] arraySKU
           );
        Task<PromoConfigROICRDto> GetPromoConfigROICR(int subActivityId);
        Task<PromoPlanningExistDto> GetPromoPlanningExist(string period, string activityDesc, int[] arrayChannel, int[] arrayAccount, string startPromo, string endPromo);
        
        Task<IList<PromoInvestmentTypeDto>> GetPromoPlanningInvestmentType(int subActivityId);
        Task<PromoResponseDto> PromoPlanningCancel(PromoPlanningCancelDto promoPlanningCancel);
        // api/promoplanning/byconditions/{year}/{entity}/{distributor}/{userid}/{create_from}/{create_to}/{start_from}/{start_to}
        Task<IList<PromoPlanningView>> GetPromoPlanningByConditions(string periode, int entityId, int distributorId, string create_from, string create_to, string start_from, string start_to, string userId);
        // api/promoplan/approval
        Task<PlanningApprovalResult> PromoPlannningApproval(DataTable importPromoPlan, string userId);
        Task<BaseLP2> GetPromoTobeCreated(string profileId, int pageNumber, int pageSize, string keyword);


    }
}
