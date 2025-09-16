using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class SubActivityModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ActivityId { get; set; }
        public int SubActivityTypeId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? RefId { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class SubActivityLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<SubActivitySelect>? Data { get; set; }
    }
    public class SubActivityById
    {
        public int Id { get; set; }
    }
    public class SubActivitySelect
    {
        public int CategoryId { get; set; }
        public string? CategoryRefId { get; set; }
        public string? CategoryLongDesc { get; set; }
        public string? CategoryShortDesc { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategoryRefId { get; set; }
        public string? SubCategoryLongDesc { get; set; }
        public string? SubCategoryShortDesc { get; set; }
        public int ActivityId { get; set; }
        public string? ActivityRefId { get; set; }
        public string? ActivityLongDesc { get; set; }
        public string? ActivityShortDesc { get; set; }
        public int SubActivityTypeId { get; set; }
        public string? SubActivityTypeRefId { get; set; }
        public string? SubActivityTypeLongDesc { get; set; }
        public string? SubActivityTypeShortDesc { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivityRefId { get; set; }
        public string? SubActivityLongDesc { get; set; }
        public string? SubActivityShortDesc { get; set; }
    }
    public class SubActivityListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public SubActivitySortColumn SortColumn { get; set; }
        public SubActivitySortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubActivitySortColumn
    {
        CategoryId,
        CategoryRefId,
        CategoryLongDesc,
        CategoryShortDesc,
        SubCategoryId,
        SubCategoryRefId,
        SubCategoryLongDesc,
        SubCategoryShortDesc,
        ActivityId,
        ActivityRefId,
        ActivityLongDesc,
        ActivityShortDesc,
        SubActivityTypeId,
        SubActivityTypeRefId,
        SubActivityTypeLongDesc,
        SubActivityTypeShortDesc,
        SubActivityId,
        SubActivityRefId,
        SubActivityLongDesc,
        SubActivityShortDesc
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubActivitySortDirection
    {
        asc,
        desc
    }
    public class SubActivityCreate
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ActivityId { get; set; }
        public int SubActivityTypeId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class SubActivityCreateReturn
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ActivityId { get; set; }
        public int SubActivityTypeId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? RefId { get; set; }
    }
    public class SubActivityUpdate
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ActivityId { get; set; }
        public int SubActivityTypeId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class SubActivityUpdateReturn
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ActivityId { get; set; }
        public int SubActivityTypeId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class SubActivityDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class SubActivityDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class CategoryforSubActivity
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubCategoryforSubActivity
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class ActivityforSubActivity
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class ActivityTypeforSubActivity
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
}