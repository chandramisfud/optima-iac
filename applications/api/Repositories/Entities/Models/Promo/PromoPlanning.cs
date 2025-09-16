using Microsoft.AspNetCore.Http.HttpResults;
using Repositories.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Repositories.Entities
{

    public class PromoPlanningLPDto
    {

        public int promoPlanId { get; set; }
        public string? refId { get; set; }
        public int promoId { get; set; }
        public string? promoRefId { get; set; }
        public string? regionDesc { get; set; }
        public string? brandDesc { get; set; }
        public string? subAccountDesc { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }
        public string? activityDesc { get; set; }
        public decimal investment { get; set; }
        public string? lastStatus { get; set; }
        public string? tsCode { get; set; }
        public int entityId { get; set; }
        public string? entityLongDesc { get; set; }
        public string? entityShortDesc { get; set; }
        public int distributorId { get; set; }
        public string? distributorLongDesc { get; set; }
        public string? distributorShortDesc { get; set; }
        public string? mechanism { get; set; }
        public string? mechanism1 { get; set; }
        public string? mechanism2 { get; set; }
        public string? mechanism3 { get; set; }
        public string? mechanism4 { get; set; }
        public string? cancelNotes { get; set; }
        public bool isCancel { get; set; }
        public DateTime? tsCodeOn { get; set; }
        public string? tsCodeBy { get; set; }
        public string? initiator_notes { get; set; }
        public int investmentTypeId { get; set; }
        public string? investmentTypeRefId { get; set; }
        public string? investmentTypeDesc { get; set; }
    }

    public class PromoPlanning
    {
        public int promoPlanId { get; set; }
        public string? periode { get; set; }
        public int distributorId { get; set; }
        public int entityId { get; set; }
        public string? categoryShortDesc { get; set; }
        public string? principalShortDesc { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int activityId { get; set; }
        public int subActivityId { get; set; }
        public string? activityDesc { get; set; }
        public string? startPromo { get; set; }
        public string? endPromo { get; set; }
        public string? mechanisme1 { get; set; }
        public string? mechanisme2 { get; set; }
        public string? mechanisme3 { get; set; }
        public string? mechanisme4 { get; set; }
        public decimal investment { get; set; }
        public decimal normalSales { get; set; }
        public decimal incrSales { get; set; }
        public decimal roi { get; set; }
        public decimal costRatio { get; set; }
        public string? notes { get; set; }
        public DateTime createOn { get; set; }
        public string? createBy { get; set; }
        public string? initiator_notes { get; set; }
        public string? createdEmail { get; set; }
        public string? modifReason { get; set; }
    }

    public  class Region
    {
        public int id { get; set; }
    }

    public  class Channel
    {
        public int id { get; set; }
    }

    public  class SubChannel
    {
        public int id { get; set; }
    }

    public  class Account
    {
        public int id { get; set; }
    }

    public  class SubAccount
    {
        public int id { get; set; }
    }
    public  class Brand
    {
        public int id { get; set; }
    }

    public  class Product
    {
        public int id { get; set; }
    }

    public class MechanismType
    {
        public int id { get; set; }
        public string? mechanism { get; set; }
        public string? notes { get; set; }
        public int productId { get; set; }
        public string? product { get; set; }
        public int brandId { get; set; }
        public string? brand { get; set; }
    }

    public class PromoPlanningDto
    {
        public PromoPlanning? PromoPlanningHeader { get; set; }
        public List<Region>? Regions { get; set; }
        public List<Channel>? Channels { get; set; }
        public List<SubChannel>? SubChannels { get; set; }
        public List<Account>? Accounts { get; set; }
        public List<SubAccount>? SubAccounts { get; set; }
        public IList<Brand>? Brands { get; set; }
        public IList<Product>? Skus { get; set; }
        public IList<MechanismType>? Mechanisms { get; set; }

    }

    public class PromoChildsTypeDto
    {
        public int parentId { get; set; }
        public string? id { get; set; }
        public bool isActive { get; set; }
    }

    public class PromoPlanningTypeDto
    {
        public int promoPlanId { get; set; }
        public string? periode { get; set; }
        public int distributor { get; set; }
        public int entity { get; set; }
        public string? principalShortDesc { get; set; }
        public string? categoryShortDesc { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int activityId { get; set; }
        public int subActivityId { get; set; }
        public string? activityDesc { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }
        public string? mechanisme1 { get; set; }
        public string? mechanisme2 { get; set; }
        public string? mechanisme3 { get; set; }
        public string? mechanisme4 { get; set; }
        public decimal investment { get; set; }
        public decimal normalSales { get; set; }
        public decimal incrSales { get; set; }
        public decimal roi { get; set; }
        public decimal costRatio { get; set; }
        public string? notes { get; set; }
        public DateTime createOn { get; set; }
        public string? createBy { get; set; }
        public string? initiator_notes { get; set; }
        public string? createdEmail { get; set; }
        public string? modifReason { get; set; }
    }


    public class PromoPlanningDownloadDto
    {
        public int promoPlanId { get; set; }
        public string? refId { get; set; }
        public string? promoId { get; set; }
        public string? promoRefId { get; set; }
        public string? regionDesc { get; set; }
        public string? brandDesc { get; set; }
        public string? subAccountDesc { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }
        public string? activityDesc { get; set; }
        public decimal investment { get; set; }
        public string? lastStatus { get; set; }
        public string? tsCode { get; set; }
        public int entityId { get; set; }
        public string? entityLongDesc { get; set; }
        public string? entityShortDesc { get; set; }
        public int distributorId { get; set; }
        public string? distributorLongDesc { get; set; }
        public string? distributorShortDesc { get; set; }
        public string? mechanism { get; set; }
        public string? mechanisme1 { get; set; }
        public string? mechanisme2 { get; set; }
        public string? mechanisme3 { get; set; }
        public string? mechanisme4 { get; set; }
        public string? cancelNotes { get; set; }
        public bool isCancel { get; set; }
        public DateTime? tsCodeOn { get; set; }
        public string? tsCodeBy { get; set; }
        public string? initiator_notes { get; set; }
        public int investmentTypeId { get; set; }
        public string? investmentTypeRefId { get; set; }
        public string? investmentTypeDesc { get; set; }

        //#140
        public string? groupBrandDesc { get; set; }

    }
    public class PromoPlanningHeader
    {
        public int id { get; set; }
        public string? refId { get; set; }
        public string? tsCoding { get; set; }
        public int entityId { get; set; }
        public string? entityShortDesc { get; set; }
        public string? entityLongDesc { get; set; }
        public int distributorId { get; set; }
        public string? distributorShortDesc { get; set; }
        public string? distributorLongDesc { get; set; }
        public int categoryId { get; set; }
        public string? categoryDesc { get; set; }
        public string? categoryShortDesc { get; set; }
        public int subCategoryId { get; set; }
        public string? subCategoryDesc { get; set; }
        public int activityId { get; set; }
        public string? activityDesc { get; set; }
        public int subActivityId { get; set; }
        public string? subActivityDesc { get; set; }
        public string? startPromo { get; set; }
        public string? endPromo { get; set; }
        public string? mechanisme1 { get; set; }
        public string? mechanisme2 { get; set; }
        public string? mechanisme3 { get; set; }
        public string? mechanisme4 { get; set; }
        public decimal investment { get; set; }
        public decimal normalSales { get; set; }
        public decimal incrSales { get; set; }
        public decimal roi { get; set; }
        public decimal costRatio { get; set; }
        public string? notes { get; set; }
        public DateTime createOn { get; set; }
        public string? createBy { get; set; }
        public DateTime modifiedOn { get; set; }
        public string? initiator_notes { get; set; }
        public int late_submission_day { get; set; }
    }

    public class PromoPlanningByIdDto
    {
        public PromoPlanningHeader? PromoPlanningHeader { get; set; }
        public List<PromoPlanningAttibuteById>? Regions { get; set; }
        public List<PromoPlanningAttibuteById>? Channels { get; set; }
        public List<PromoPlanningAttibuteById>? SubChannels { get; set; }
        public List<PromoPlanningAttibuteById>? Accounts { get; set; }
        public List<PromoPlanningAttibuteById>? SubAccounts { get; set; }
        public IList<PromoPlanningAttibuteById>? Brands { get; set; }
        public IList<PromoPlanningAttibuteById>? Skus { get; set; }
        public IList<MechanismById>? Mechanisms { get; set; }

    }

    public class PromoPlanningAttibuteById
    {
        public bool flag { get; set; }
        public int id { get; set; }
        public string? longDesc { get; set; }
    }

    public class MechanismById
    {
        public int mechanismId { get; set; }
        public string? mechanism { get; set; }
        public string? notes { get; set; }
        public int productId { get; set; }
        public string? product { get; set; }
        public int brandId { get; set; }
        public string? brand { get; set; }
    }

    public class MechanisSourceDto
    {
        public int id { get; set; }
        public int entityId { get; set; }
        public string? entity { get; set; }
        public int subActivityId { get; set; }
        public string? subActivity { get; set; }
        public int productId { get; set; }
        public string? product { get; set; }
        public int brandId { get; set; }
        public string? brand { get; set; }
        public string? requirement { get; set; }
        public string? discount { get; set; }
        public string? mechanism { get; set; }
        public int channel { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime createOn { get; set; }
        public string? createBy { get; set; }
        public string? createdEmail { get; set; }
        public DateTime modifiedOn { get; set; }
        public string? modifiedBy { get; set; }
        public string? modifiedEmail { get; set; }
        public bool isDelete { get; set; }
        public DateTime deleteON { get; set; }
        public string? deleteBy { get; set; }
        public string? deleteEmail { get; set; }

    }

    public class PromoBaselineDto
    {
        public decimal baseline_sales { get; set; }
        public decimal actual_sales { get; set; }
    }

    public class PromoConfigROICRDto
    {
        public string? refId { get; set; }
        public string? subActivity { get; set; }
        public double minimumROI { get; set; }
        public double maksimumROI { get; set; }
        public double minimumCostRatio { get; set; }
        public double maksimumCostRatio { get; set; }
    }

    public class PromoPlanningExistDto
    {
        public int id { get; set; }
        public string? refId { get; set; }
        public string? periode { get; set; }
        public string? activityDesc { get; set; }
        public int channelId { get; set; }
        public string? channel { get; set; }
        public int accountId { get; set; }
        public string? account { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }
        public int subAccountId { get; set; }
        public string? subAccount { get; set; }
    }
    public class PromoInvestmentTypeDto
    {
        public int id { get; set; }
        public int subActivityId { get; set; }
        public int investmentTypeId { get; set; }
        public string? investmentTypeRefId { get; set; }
        public string? investmentTypeDesc { get; set; }
    }

    public class PromoPlanningCancelDto
    {
        public int promoPlanningId { get; set; }
        public string? reason { get; set; }
        public string? profileId { get; set; }
    }
    public class PromoPlanningView
    {
        public int PromoPlanId { get; set; }
        public string? RefId { get; set; }
        public int Promoid { get; set; }
        public string? PromoRefId { get; set; }
        public string? LastStatus { get; set; }
        public string? TSCode { get; set; }
        public string? RegionDesc { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? BrandDesc { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public int EntityId { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? EntityShortDesc { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorLongDesc { get; set; }
        public string? DistributorShortDesc { get; set; }
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public string? ActivityDesc { get; set; }
        public decimal Investment { get; set; }
        public string? CancelNotes { get; set; }
        public bool IsCancel { get; set; }
        public DateTime TSCodeOn { get; set; }
        public string? TSCodeBy { get; set; }
        public string? initiator_notes { get; set; }
        public int InvestmentTypeId { get; set; }
        public string? InvestmentTypeRefId { get; set; }
        public string? InvestmentTypeDesc { get; set; }
    }
    public class PlanningApprovalResult
    {
        public planningheader? planningheader { get; set; }
        public IList<approvaldetailresult>? planning { get; set; }

    }

    public class planningheader
    {
        public int totalplanning { get; set; }
        public double totalinvestment { get; set; }
    }
    public class approvaldetailresult
    {
        public int planningid { get; set; }
        public string? planningrefid { get; set; }
        public string? tscode { get; set; }
        public double investment { get; set; }

    }
}