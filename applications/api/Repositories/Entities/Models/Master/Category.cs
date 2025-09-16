using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Repositories.Entities.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
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
    public class CategoryLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<CategorySelect>? Data { get; set; }

    }
    public class CategoryById
    {
        public int Id { get; set; }
    }


    public class CategorySelect
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
    }

    public class CategoryListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public CategorySortColumn SortColumn { get; set; }
        public SortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CategorySortColumn
    {
        ShortDesc,
        LongDesc,
        RefId

    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortDirection
    {
        asc,
        desc
    }

    public class CategoryCreate
    {
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class CategoryCreateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? RefId { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class CategoryUpdate
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class CategoryUpdateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class CategoryDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class CategoryDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }
}
