using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class RegionModel
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public int IsActive { get; set; }
        public string? CreatedOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? RefId { get; set; }
        public string? SAPCode { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class RegionLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<RegionSelect>? Data { get; set; }

    }
    public class RegionById
    {
        public int Id { get; set; }
    }

    public class RegionSelect
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
    }

    public class RegionListRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public RegionSortColumn SortColumn { get; set; }
        public RegionSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RegionSortColumn
    {
        ShortDesc,
        LongDesc,
        RefId
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RegionSortDirection
    {
        asc,
        desc
    }

    public class RegionCreate
    {
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class RegionCreateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreatedOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class RegionUpdate
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class RegionUpdateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class RegionDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class RegionDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }
}