using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Report
{
    public class SKPValidationRecordCount
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }

    public class SKPValidationLandingPage
    {
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
        public IList<SKPValidationData>? Data { get; set; }
    }

    public class SKPValidationData
    {
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
        public string? PrincipalDesc { get; set; }
        public int PromoId { get; set; }
        public string? RefId { get; set; }
        public string? BudgetSource { get; set; }
        public string? Initiator { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? PromoNumber { get; set; }
        public string? SubCategory { get; set; }
        public string? Activity { get; set; }
        public string? SubActivity { get; set; }
        public string? ActivityDesc { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public string? RegionDesc { get; set; }
        public string? ChannelDesc { get; set; }
        public string? SubChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? LastStatusDate { get; set; }
        public string? ApprovalNotes { get; set; }
        public decimal Target { get; set; }
        public double Investment { get; set; }
        public decimal NormalSales { get; set; }
        public decimal IncrSales { get; set; }
        public decimal Roi { get; set; }
        public decimal CostRatio { get; set; }
        public decimal DnClaim { get; set; }
        public decimal RemainingBalance { get; set; }
        public decimal DnPaid { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public int Gap { get; set; }
        public bool OnTime { get; set; }
        public string? TsCoding { get; set; }
        public string? BrandDesc { get; set; }
        public string? SKUDesc { get; set; }
        public int PromoPlanId { get; set; }
        public string? PromoPlanRefId { get; set; }
        public bool ClosureStatus { get; set; }
        public string? ReconStatus { get; set; }
        public string? LastReconStatus { get; set; }
        public int SubactivityTypeId { get; set; }
        public string? SubactivityTypeRefId { get; set; }
        public string? SubactivityType { get; set; }
        public string? CancelReason { get; set; }
        public string? LastStatus { get; set; }
        public string? Allocation { get; set; }
        public bool IsCancelLocked { get; set; }
        public string? CancelNotes { get; set; }
        public string? SKPDraftAvail { get; set; }
        public string? SKPDraftAvailBfrAct60 { get; set; }
        public string? SKPEntityDraft { get; set; }
        public string? SKPBrandDraft { get; set; }
        public string? SKPPeriodDraft { get; set; }
        public string? SKPActivityDescDraft { get; set; }
        public string? SKPMechanismDraft { get; set; }
        public string? SKPInvestmentDraft { get; set; }
        public string? SKPDistributorDraft { get; set; }
        public string? SKPChannelDraft { get; set; }
        public string? SKPStoreNameDraft { get; set; }
        public string? SKPPeriodMatch { get; set; }
        public DateTime PeriodMatchon { get; set; }
        public string? PeriodMatchby { get; set; }
        public string? SKPInvestmentMatch { get; set; }
        public DateTime InvestmentMatchon { get; set; }
        public string? InvestmentMatchby { get; set; }
        public string? SKPMechanismMatch { get; set; }
        public DateTime MechanismMatchon { get; set; }
        public string? MechanismMatchby { get; set; }
        public string? SKPSign7 { get; set; }
        public DateTime SKPSign7on { get; set; }
        public string? SKPSign7by { get; set; }
        public string? SKPEntity { get; set; }
        public DateTime Entityon { get; set; }
        public string? Entityby { get; set; }
        public string? SKPBrand { get; set; }
        public DateTime Brandon { get; set; }
        public string? Brandby { get; set; }
        public string? SKPActivityDesc { get; set; }
        public DateTime ActivityDescon { get; set; }
        public string? ActivityDescby { get; set; }
        public string? SKPDistributor { get; set; }
        public DateTime Distributoron { get; set; }
        public string? Distributorby { get; set; }
        public string? SKPChannel { get; set; }
        public DateTime Channelon { get; set; }
        public string? Channelby { get; set; }
        public string? SKPStoreName { get; set; }
        public DateTime StoreNameon { get; set; }
        public string? StoreNameby { get; set; }
        public string? SKPStatus { get; set; }
        public string? SKP_Notes { get; set; }
        public string? skp_flagging_status { get; set; }
    }

    public class SKPValidationEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class SKPValidationDistributorList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class SKPValidationChannelList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }

    public class SKPValidationView
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
        public int SubAccountId { get; set; }
        public string? subAccountDesc { get; set; }
        public string? CancelNotes { get; set; }
        public string? skp_flagging_status { get; set; }
        public int entityId { get; set; }
        public string? Entity { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public int skpstatus { get; set; }
        public string? skp_notes { get; set; }
        public string? CreateBy { get; set; }
        public double dnclaim { get; set; }
        public string? creationdate { get; set; }
        public string? laststatusdate { get; set; }
        public double investmentBfrClose { get; set; }
        public double investmentClosedBalance { get; set; }
        //added: andrie Sept 29 2023
        public string? createdName { get; set; }
        public string? channelDesc { get; set; }
        //added: andrie Nov 11 2023
        public string? categoryShortDesc { get; set; }


    }
}
