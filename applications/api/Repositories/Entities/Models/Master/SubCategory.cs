using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class SubCategoryModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
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
        public string? RefId { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class SubCategoryLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<SubCategorySelect>? Data { get; set; }
    }
    public class SubCategoryById
    {
        public int Id { get; set; }
    }

    public class SubCategorySelect
    {
        public int CategoryId { get; set; }
        public string? CategoryShortDesc { get; set; }
        public string? CategoryLongDesc { get; set; }
        public string? CategoryRefId { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategoryShortDesc { get; set; }
        public string? SubCategoryLongDesc { get; set; }
        public string? SubCategoryRefId { get; set; }
    }

    public class SubCategoryListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public SubCategorySortColumn SortColumn { get; set; }
        public SubCategorySortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubCategorySortColumn
    {
        CategoryId,
        CategoryLongDesc,
        CategoryShortDesc,
        CategoryRefid,
        SubCategoryId,
        SubCategoryLongDesc,
        SubCategoryShortDesc,
        SubCategoryRefid
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubCategorySortDirection
    {
        asc,
        desc
    }

    public class SubCategoryCreate
    {
        public int CategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class SubCategoryCreateReturn
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class SubCategoryUpdate
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class SubCategoryUpdateReturn
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class SubCategoryDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class SubCategoryDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class SubCategoryByAccess
    {
        public string? userid { get; set; }

        public int CategoryId { get; set; }
    }

    public class SubCategoryByAccessReturn
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
         public class SubCategorytoCategory
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }

}