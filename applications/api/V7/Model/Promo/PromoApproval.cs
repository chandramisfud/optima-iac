using Repositories.Entities.Models.PromoApproval;
using System.ComponentModel.DataAnnotations;
using V7.Model.FinanceReport;

namespace V7.Model.PromoApproval
{
    public class PromoSKPHeaderParam
    {
        public bool SKPDraftAvail { get; set; }
        public bool SKPDraftAvailBfrAct60 { get; set; }
        public bool PeriodMatch { get; set; }
        public bool InvestmentMatch { get; set; }
        public bool MechanismMatch { get; set; }
        public bool SKPSign7 { get; set; }
        public bool EntityDraft { get; set; }
        public bool BrandDraft { get; set; }
        public bool PeriodDraft { get; set; }
        public bool ActivityDescDraft { get; set; }
        public bool MechanismDraft { get; set; }
        public bool InvestmentDraft { get; set; }
        public bool Entity { get; set; }
        public bool Brand { get; set; }
        public bool ActivityDesc { get; set; }
        public bool DistributorDraft { get; set; }
        public bool Distributor { get; set; }
        public bool ChannelDraft { get; set; }
        public bool Channel { get; set; }
        public bool StoreNameDraft { get; set; }
        public bool StoreName { get; set; }
        public int skpstatus { get; set; }
        public string? skp_notes { get; set; }
    }
    public class PromoApprovalWithSKPParam
    {
        public PromoSKPHeaderParam? PromoSKPHeader { get; set; }
        public int promoId { get; set; }
        public string? notes { get; set; }
    }

    public class PromoApprovalByEmailWithSKPParam
    {
        public PromoSKPHeaderParam? PromoSKPHeader { get; set; }
        public int promoId { get; set; }
        public string? notes { get; set; }
        public string? profileId { get; set; }
    }

    public class PromoApprovalReconParam
    {
        public int promoId { get; set; }
        public string? notes { get; set; }
    }
    public class PromoApprovalReconByEmailParam
    {
        public int promoId { get; set; }
        public string? notes { get; set; }
        public string? profileId { get; set; }
    }

    public class SKPValidationDownloadParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinSKPValidationSortColumn SortColumn { get; set; }
        public FinSKPValidationSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
        public int CancelStatus { get; set; }
        public string? StartFrom { get; set; }
        public string? StartTo { get; set; }
        public int SubmissionParam { get; set; }
        public int Status { get; set; }
    }
}
