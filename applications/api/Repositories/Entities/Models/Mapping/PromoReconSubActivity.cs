
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class PromoReconSubActivity
    {
        public int SubActivityId { get; set; }
        public string? SubActivityLongDesc { get; set; }
        public int AllowEdit { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
        public int IsDeleted { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class PromoReconSubActivityLP
    {
        public int Id { get; set; }
        public string? Refid { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
        public string? Activity { get; set; }
        public string? SubActivityType { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivity { get; set; }
        public int AllowEdit { get; set; }
    }
    public class PromoReconSubActivityGetById
    {
        public int Id { get; set; }
        public string? Refid { get; set; }
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategory { get; set; }
        public int ActivityId { get; set; }
        public string? Activity { get; set; }
        public string? SubActivityType { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivity { get; set; }
        public int AllowEdit { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PromoReconSubActivitySortColumn
    {
        id,
        RefId,
        Category,
        SubCategory,
        Activity,
        SubActivityType,
        SubActivity,
        AllowEdit
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PromoReconSubActivitySortDirection
    {
        asc,
        desc
    }
    public class PromoReconSubActivityListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public PromoReconSubActivitySortColumn SortColumn { get; set; }
        public PromoReconSubActivitySortDirection SortDirection { get; set; }
    }
    public class PromoReconSubActivityDelete
    {
        public int SubActivityId { get; set; }
        public int IsDeleted { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class PromoReconSubActivityDeleteReturn
    {
        public int SubActivityId { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class PromoReconSubActivityUpdate
    {
        public int SubActivityId { get; set; }
        public bool AllowEdit { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class PromoReconSubActivityUpdateReturn
    {
        public int SubActivityId { get; set; }
        public int AllowEdit { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class PromoReconSubActivityCreate
    {
        public int SubActivityId { get; set; }
        public bool AllowEdit { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class PromoReconSubActivityCreateReturn
    {
        public int SubActivityId { get; set; }
        public bool AllowEdit { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class SubActivityDropDownMapping
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class SubActivityTemplate
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategory { get; set; }
        public string? ActivityId { get; set; }
        public string? TypeId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ActivityLongDesc { get; set; }
        public string? SubActivityTypeId { get; set; }
        public string? SubActivityTypeLongDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? RefId { get; set; }
    }
}





