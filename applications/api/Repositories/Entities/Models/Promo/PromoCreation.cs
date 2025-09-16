using Microsoft.AspNetCore.Components.Web;
using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{

    public class PromoCreationLPDto
    {
        public string? entity { get; set; }
        public string? distributor { get; set; }
        public string? principalDesc { get; set; }
        public int promoId { get; set; }
        public string? refId { get; set; }
        public string? activityDesc { get; set; }
        public string? lastStatus { get; set; }
        public string? allocation { get; set; }
        public double investment { get; set; }
        public bool isCancelLocked { get; set; }
        public string? tsCoding { get; set; }
        public string? cancelNotes { get; set; }
        public string? initiator_notes { get; set; }
        public int investmentTypeId { get; set; }
        public string? investmentTypeRefId { get; set; }
        public string? investmentTypeDesc { get; set; }
        public bool isClose { get; set; }
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }

        // Added, andrie Oct 9 2023 E2#38
        public int categoryId { get; set; }
        public string? categoryShortDesc { get; set; }
        public bool? isOldPromo
        {
            get
            {
                return startPromo.Year < 2025;
            }
            set { }
        }

        // Added Oct 22 2024
        public bool reconciled { get; set; }
        public bool editable { get; set; }
        // Added Oct 29 2024
        public string? reconciledStatus { get; set; }
        // Added Nov 20 2024
        public string? approvalReconStatus { get; set; }

    }

    public class PromoCreationDownloadDto
    {
        public string? entity { get; set; }
        public string? distributor { get; set; }
        public string? principalDesc { get; set; }
        public int promoId { get; set; }
        public string? refId { get; set; }
        public string? activityDesc { get; set; }
        public string? lastStatus { get; set; }
        public string? allocation { get; set; }
        public double investment { get; set; }
        public bool isCancelLocked { get; set; }
        public string? tsCoding { get; set; }
        public string? cancelNotes { get; set; }
        public string? initiator_notes { get; set; }
        public int investmentTypeId { get; set; }
        public string? investmentTypeRefId { get; set; }
        public string? investmentTypeDesc { get; set; }
        public bool isClose { get; set; }
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }
        public DateTime startPromo { get; set; }
        public DateTime endPromo { get; set; }
    }

    public class PromoCreationSKPDraftDto
    {
        public int id { get; set; }
        public string? refId { get; set; }
        public int channelId { get; set; }
        public string? channel { set; get; } 
        public int subChannelId { get; set; }
        public string? subChannel { set; get; }
        public int accountId { get; set; }
        public string? account { get; set; }
        public int subAccountId { set; get; }
        public string? subAccount { set; get; }
        public string? initiator { set; get; }  
        public string? initiatorName { set; get; }
        public string? jobTitle { set; get; }
        public string? email { set; get; }
        public string? contactInfo { set; get; }
        public int entityId { set; get; }
        public string? entity { set; get; }
        public DateTime startPromo { set; get; }
        public DateTime endPromo { set; get; }
        public int subActivityId { set; get; }
        public string? subActivity { set; get; }
        public string? mechanisme1 { set; get; }
        public string? brandDesc { set; get; }
        public string? skuDesc { set; get; }
        public int distributorId { set; get; }
        public string? distributor { set; get; }
        public double investment { set; get; }
        public double normalSales { set; get; }
        public double incrSales { set; get; }
        public double totalSales { set; get; }
        public double costRatio { set; get; }
        public string? tsCoding { set; get; }
    }

    public class Promo
    {
        public int promoId { get; set; }
        public int promoPlanId { get; set; }
        public int allocationId { get; set; }
        public string? allocationRefId { get; set; }
        public string? categoryShortDesc { get; set; }
        public string? principalShortDesc { get; set; }
        public int budgetMasterId { get; set; }
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
        public double investment { get; set; }
        public decimal normalSales { get; set; }
        public decimal incrSales { get; set; }
        public decimal roi { get; set; }
        public decimal costRatio { get; set; }
        public string? statusApproval { get; set; }
        public string? notes { get; set; }
        public string? tsCoding { get; set; }
        public DateTime createOn { get; set; }
        public string? createBy { get; set; }
        public string? initiator_notes { get; set; }
        public string? createdEmail { get; set; }
        public string? modifReason { get; set; }
    }

    public class PromoAttachmentStore
    {
        public string? DocLink { get; set; }
        public string? FileName { get; set; }

    }
    public class PromoCreationDto
    {
        public Promo? PromoHeader { get; set; }
        public List<Region>? Regions { get; set; }
        public List<Channel>? Channels { get; set; }
        public List<SubChannel>? SubChannels { get; set; }
        public List<Account>? Accounts { get; set; }
        public List<SubAccount>? SubAccounts { get; set; }
        public IList<Brand>? Brands { get; set; }
        public IList<Product>? Skus { get; set; }
        public IList<MechanismType>? Mechanisms { get; set; }
        public IList<PromoAttachmentStore>? promoAttachment { get; set; }
        public double budgetAmount { set; get; }

    }

    public class PromoReconCreationDto
    {
        public PromoReconV4TypeDto? PromoHeader { get; set; }
        public List<Region>? Regions { get; set; }
        public List<Channel>? Channels { get; set; }
        public List<SubChannel>? SubChannels { get; set; }
        public List<Account>? Accounts { get; set; }
        public List<SubAccount>? SubAccounts { get; set; }
        public IList<Brand>? Brands { get; set; }
        public IList<Product>? Skus { get; set; }
        public IList<MechanismType>? Mechanisms { get; set; }
       
        public bool Reconciled { get; set; }
        public bool ReconciledUpd { get; set; }

    }
    public class PromoTypeDto
    {
        public int promoId { get; set; }
        public int promoPlanId { get; set; }
        public int allocationId { get; set; }
        public string? allocationRefId { get; set; }
        public string? principalShortDesc { get; set; }
        public string? categoryShortDesc { get; set; }
        public int budgetMasterId { get; set; }
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
        public string? statusApproval { get; set; }
        public string? notes { get; set; }
        public string? tsCoding { get; set; }
        public DateTime createOn { get; set; }
        public string? createBy { get; set; }
        public string? initiator_notes { get; set; }
        public string? createdEmail { get; set; }
        public string? modifReason { get; set; }
    }

    public class PromoReconV4TypeDto
    {
        public int PromoId { get; set; }
        public int PromoPlanId { get; set; }
        public int AllocationId { get; set; }
        public string? AllocationRefId { get; set; }
        public string? PrincipalShortDesc { get; set; }
        public string? CategoryShortDesc { get; set; }
        public int BudgetMasterId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ActivityId { get; set; }
        public int SubActivityId { get; set; }
        public string? ActivityDesc { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public double Investment { get; set; }
        public decimal NormalSales { get; set; }
        public decimal IncrSales { get; set; }
        public decimal Roi { get; set; }
        public decimal CostRatio { get; set; }
        public string? StatusApproval { get; set; }
        public string? Notes { get; set; }
        public string? TsCoding { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? initiator_notes { get; set; }
        public double actual_sales { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifReason { get; set; }

    }
}