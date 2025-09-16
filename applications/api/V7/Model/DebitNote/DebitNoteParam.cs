using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Repositories.Entities.Models.DN;
using V7.Model.FinanceReport;
using V7.Model.Promo;

namespace V7.Model.DebitNote
{
    public class DNAttributebyUserParam
    {
        public string? userid { get; set; }
    }

    public class DNGetUserbyIdParam
    {
        public int userId { get; set; }
    }

    public class DebitNoteReportParam
    {
        public string? year { get; set; }
        public int entity { get; set; }
        public int distributor { get; set; }
        public int channel { get; set; }
        public int account { get; set; }
    }

    public class DNDistributorParam
    {
        public int id { get; set; }
        public string? user { get; set; }
    }
    public class DNAttachmentParam
    {
        public int DNId { get; set; }
        public string? DocLink { get; set; }
        public string? FileName { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
    }

    public class DNLandingPageParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public DNSortColumn SortColumn { get; set; }
        public DNSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int ChannelId { get; set; }
        public int AccountId { get; set; }
        public bool IsDNManual { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DNSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DNSortColumn
    {
        PromorefId,
        refId,
        EntityLongDesc,
        EntityShortDesc,
        ActivityDesc,
        SubAccount,
        MaterialNumber,
        MemDocNo,
        IntDocNo,
    }

    public class DebitNoteSendtoParam
    {
        public int entityid { get; set; }
        public int distributorid { get; set; }
    }

    public class DNValidateByDistributorHOParam
    {
        public int entityid { get; set; }
        public int distributorid { get; set; }
    }
    public class DNRejectParam
    {
        public int dnid { get; set; }
        public string? reason { get; set; }

    }
    public class DNDistributorGlobalParam
    {
        public int budgetId { get; set; }
        public int[]? entityId { get; set; }
    }
    public class DNGetStatusParamwithTaxLevel
    {
        public int entityid { get; set; }
        public int distributorid { get; set; }
        public string? TaxLevel { get; set; }
        public string? dnPeriod { get; set; }
        public int categoryId { get; set; }
    }

    public class DNGetStatusValidateByFinanceParamwithTaxLevel
    {
        public int entityid { get; set; }
        public int distributorid { get; set; }
        public string? TaxLevel { get; set; }
    }
    public class DNGetStatusParam
    {
        public int entityid { get; set; }
        public int distributorid { get; set; }

    }
    public class CreateInvoiceParam
    {
        //public int Id { get; set; }
        public string? Desc { get; set; }
        public decimal DPPAmount { get; set; }
        public decimal PPNpct { get; set; }
        public decimal InvoiceAmount { get; set; }
        public int DistributorId { get; set; }
        public int EntityId { get; set; }
        public IList<DNIdReadytoInvoiceArray>? DNId { get; set; }
        public string? TaxLevel { get; set; }
        public string? dnPeriod { get; set; }
        public int categoryId { get; set; }
    }
    public class UpdateInvoiceParam
    {
        public int InvoiceId { get; set; }
        public string? Desc { get; set; }
        public decimal DPPAmount { get; set; }
        public decimal PPNpct { get; set; }
        public decimal InvoiceAmount { get; set; }
        public int DistributorId { get; set; }
        public int EntityId { get; set; }
        public IList<DNIdReadytoInvoiceArray>? DNId { get; set; }
        public string? TaxLevel { get; set; }
        public string? dnPeriod { get; set; }
        public int categoryId { get; set; }
    }

    public class DNIdArrayParam
    {
        public int DNId { get; set; }
    }
    public class DNApprovedPromoforMultiprintDNParam
    {
        public string? period { get; set; }
        public int entity { get; set; }
        public int channel { get; set; }
        public int account { get; set; }
    }
    public class DNUploadparam
    {
        public string? userId { get; set; }
    }
    public class DNGetListAttachmentParam
    {
        public string? period { get; set; }
        public int distributor { get; set; }
        public bool isdnmanual { get; set; }
    }
    public class DNListingPromoDistributorRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinListingPromoReportingSortColumn SortColumn { get; set; }
        public FinListingPromoReportingSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int CategoryId { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
        public string? CreateFrom { get; set; }
        public string? CreateTo { get; set; }
        public string? StartFrom { get; set; }
        public string? StartTo { get; set; }
        public int SubmissionParam { get; set; }
    }
    public class GetApprovedPromoforDNParam
    {
        public string? periode { get; set; }
        public int entity { get; set; }
        public int channel { get; set; }
        public int account { get; set; }
    }
    public class GetApprovedPromoforbyInitiatorDNParam
    {
        public string? periode { get; set; }
        public int entityId { get; set; }
        public int channelId { get; set; }
        public int accountId { get; set; }
    }

    public class DNReAssignListParam : LPPagingParam
    {
        public string? periode { get; set; }
        public int entityId { get; set; } = 0;
        public int distributorId { get; set; } = 0;
        public int channelId { get; set; } = 0;
        public int accountId { get; set; } = 0;
        public bool isDNManual { get; set; }
    }

    public class DNSendbackParam : LPPagingParam
    {
        public string? periode { get; set; }
        public int subAccount { get; set; } = 0;
    }

    public class DNVATExpiredLPParam : LPPagingParam
    {
        public int entityId { get; set; } = 0;
        public int distributorId { get; set; } = 0;
        public string? taxLevel { get; set; } 

    }

    public class DNAssignPromoRequestParam
    {
        public int dnid { get; set; }
        public string? approver_userid { get; set; }
        public string? internal_order_number { get; set; }
    }

    public class DNUpdateTaxlevelParam
    {
        public int id { get; set; }
        public string? periode { get; set; }
        public int entityId { get; set; }
        public int distributorId { get; set; }
        public string? taxlevel { get; set; }
        public int promoId { get; set; }
        public bool isDNPromo { get; set; }
    }

    public class DNPromoDisplayRequestParam
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public DNPromoDisplaySortColumn SortColumn { get; set; }
        public DNPromoDisplaySortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
        public bool CancelStatus { get; set; }

    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DNPromoDisplaySortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DNPromoDisplaySortColumn
    {
        Entity,
        Distributor,
        PrincipalDesc,
        RefId,
        ActivityDesc,
        Allocation

    }

    public class DNReceivedAndApproveMultiParam
    {
        public int DNId { get; set; }
        public string? status { get; set; }
        public string? notes { get; set; }
    }

    public class DNChangeStatusParam
    {
        public List<DNId>? dnid { get; set; }
    }

    public class DNChangeSingleStatusParam
    {
        public int dnId { get; set; }
    }

    public class DNFilterParam
    {
        public IFormFile? formFile { get; set; }
        public string? status { get; set; }
        public int entity { get; set; }
        public string? TaxLevel { get; set; }
    }
    public class DNInvoiceFilterParam
    {
        public IFormFile? formFile { get; set; }
        public string? status { get; set; }
        public int entity { get; set; }
        public string? TaxLevel { get; set; }
        public int invoiceId { get; set; }
        public string? dnPeriod { get; set; }
        public int categoryId { get; set; }

    }


    public class DNValidationCompletenessParam
    {
        public int DNId { get; set; }
        public string? status { get; set; }
        public string? notes { get; set; }
        public string? taxlevel { get; set; }
        public int entityId { get; set; }
        public int promoId { get; set; }
        public bool isDNPromo { get; set; }
        public string? wHTType { get; set; }
        public string? statusPPH { get; set; }
        public double pphPct { get; set; }
        public double pphAmt { get; set; }
        public DNDocCompletenessforValidationbyFinance? DNDocCompletenessHeader { get; set; }
    }

    public class DNGetStatusParamforValidatebySales
    {
        public int entityid { get; set; }
        public int distributorid { get; set; }
        public string? TaxLevel { get; set; }
        public string? period { get; set; }
    }

    public class DNVATExpiredUpdateParam
    {
        public int id { get; set; }
        public int VATExpired { get; set; }
    }

    public class DNFinValidationOnDocCompletenessParam
    {
        public int DNId { get; set; }
        public DNDocCompletenessforValidationbyFinance? DNDocCompletenessHeader { get; set; }
    }

    public class DNManualAssignmentLPParam : LPParam
    {
        public string SortColumn { get; set; } = string.Empty;
        /// <summary>
        /// Sort Direction
        /// </summary>
        public string SortDirection { get; set; } = string.Empty;

    }
    public class DNAssignParam
    {
        public int DNId { get; set; }
        public int PromoId { get; set; }
        public string? UserId { get; set; }
    }

    public class DNGetInvoiceLPParam : LPParam
    {
        public string CreateDate { get; set; } = string.Empty;
        public int Entity { get; set; }
        public int Distributor { get; set; }
        public string SortColumn { get; set; } = string.Empty;
        /// <summary>
        /// Sort Direction
        /// </summary>
        public string SortDirection { get; set; } = string.Empty;

    }


   
}

