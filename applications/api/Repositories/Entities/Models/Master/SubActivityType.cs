using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class SubActivityTypeModel
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public int IsActive { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class SubActivityTypeLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<SubActivityTypeSelect>? Data { get; set; }

    }
    public class SubActivityTypeById
    {
        public int Id { get; set; }
    }

    public class SubActivityTypeSelect
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
    }

    public class SubActivityTypeListRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public SubActivityTypeSortColumn SortColumn { get; set; }
        public SubActivityTypeSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubActivityTypeSortColumn
    {
        ShortDesc,
        LongDesc,
        RefId
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubActivityTypeSortDirection
    {
        asc,
        desc
    }

    public class SubActivityTypeCreate
    {
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class SubActivityTypeCreateReturn
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class SubActivityTypeUpdate
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class SubActivityTypeUpdateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class SubActivityTypeDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class SubActivityTypeDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }
}