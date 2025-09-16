using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using V7.Model.UserAccess;

namespace V7.Model.FinanceReport
{
    public class FinAccrualReportListRequestParam
    {
        public string? Search { get; set; }

        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;

        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public FinAccrualReportSortColumn SortColumn { get; set; }
        public FinAccrualReportSortDirection SortDirection { get; set; }
        public string? Period { get; set; }
        public string? profileId {get; set;}
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int BudgetParentId { get; set; }
        public int ChannelId { get; set; }
        public string? ClosingDate { get; set; }
        public int Download { get; set; }
        public int GrpBrandId { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinAccrualReportSortDirection
    {
        asc,
        desc
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FinAccrualReportSortColumn
    {
        Entity,
        Distributor,
        PrincipalDesc,
        BudgetSource,
        Initiator,
        CreateOn,
        LastUpdate,
        PromoNumber,
        SubCategory,
        Activity,
        SubActivity,
        ActivityDesc,
        Mechanisme1,
        Mechanisme2,
        Mechanisme3,
        Mechanisme4,
        ChannelDesc,
        AccountDesc,
        SubAccountDesc,
        LastStatus,
        ApprovalNotes,
        Target,
        Investment,
        DNClaim,
        DNPaid,
        StartPromo,
        EndPromo,
        AccrueMTD,
        AccrueYTD,
        TSCoding
    }
}
