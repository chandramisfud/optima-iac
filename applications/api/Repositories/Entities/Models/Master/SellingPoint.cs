using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class SellingPointModel
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? AreaCode { get; set; }
        public int RegionId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public int IsActive { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? ProfitCenter { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class SellingPointLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<SellingPointSelect>? Data { get; set; }
    }
    public class SellingPointById
    {
        public int Id { get; set; }
    }
    public class SellingPointSelect
    {
        public int RegionId { get; set; }
        public string? RegionRefId { get; set; }
        public string? RegionLongDesc { get; set; }
        public string? RegionShortDesc { get; set; }
        public string? ProfitCenter { get; set; }
        public string? ProfitCenterDesc { get; set; }
        public int SellingPointId { get; set; }
        public string? SellingPointRefId { get; set; }
        public string? SellingPointLongDesc { get; set; }
        public string? SellingPointShortDesc { get; set; }
        public string? AreaCode { get; set; }
    }
    public class SellingPointListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public SellingPointSortColumn SortColumn { get; set; }
        public SellingPointSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SellingPointSortColumn
    {
        RegionId,
        RegionRefId,
        RegionLongDesc,
        RegionShortDesc,
        ProfitCenter,
        ProfitCenterDesc,
        SellingPointId,
        SellingPointRefId,
        SellingPointLongDesc,
        SellingPointShortDesc,
        AreaCode
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SellingPointSortDirection
    {
        asc,
        desc
    }
    public class SellingPointCreate
    {
        public string? RefId { get; set; }
        public string? AreaCode { get; set; }
        public int RegionId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? ProfitCenter { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class SellingPointCreateReturn
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? AreaCode { get; set; }
        public int RegionId { get; set; }
        public string? ProfitCenter { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class SellingPointUpdate
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? AreaCode { get; set; }
        public int RegionId { get; set; }
        public string? ProfitCenter { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class SellingPointUpdateReturn
    {
        public int Id { get; set; }
        public string? AreaCode { get; set; }
        public int RegionId { get; set; }
        public string? ProfitCenter { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }
    public class SellingPointDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class SellingPointDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class RegionforSellingPoint
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class ProfitCenterforSellingPoint
    {
        public string? ProfitCenter { get; set; }
        public string? ProfitCenterDesc { get; set; }
    }
}