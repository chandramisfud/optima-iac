using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Repositories.Entities.Models
{
    public class ActivityModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeletedd { get; set; }
        public DateTime DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? RefId { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class ActivityLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<ActivitySelect>? Data { get; set; }

    }
    public class ActivityById
    {
        public int Id { get; set; }
    }

    public class ActivitySelect
    {
        public int CategoryId { get; set; }
        public string? CategoryLongDesc { get; set; }
        public string? CategoryShortDesc { get; set; }
        public string? CategoryRefId { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategoryLongDesc { get; set; }
        public string? SubCategoryShortDesc { get; set; }
        public string? SubCategoryRefid { get; set; }
        public int ActivityId { get; set; }
        public string? ActivityLongDesc { get; set; }
        public string? ActivityShortDesc { get; set; }
        public string? ActivityRefId { get; set; }
    }

    public class ActivityListRequest : LPParamModel
    {
        public new string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public new int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public new int PageSize { get; set; } = 10;
        public ActivitySortColumn SortColumn { get; set; }
        public ActivitySortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActivitySortColumn
    {
        CategoryId,
        CategoryLongDesc,
        CategoryShortDesc,
        CategoryRefId,
        SubCategoryId,
        SubCategoryLongDesc,
        SubCategoryShortDesc,
        SubCategoryRefid,
        ActivityId,
        ActivityLongDesc,
        ActivityShortDesc,
        ActivityRefId
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActivitySortDirection
    {
        asc,
        desc
    }

    public class ActivityCreateBody
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
    }

    public class ActivityCreate
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class ActivityCreateReturn
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class ActivityUpdate
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }
    public class ActivityUpdateBody
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
    }

    public class ActivityUpdateReturn
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class ActivityDeleteBody
    {
        public int Id { get; set; }

    }

    public class ActivityDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class ActivityDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class ActivityByAccess
    {
        public string? userid { get; set; }

        public int Categoryid { get; set; }
    }

    public class ActivityByAccessReturn
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class ActivitySubCategory
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class ActivityCategory
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class LPParamModel
    {
        /// <summary>
        /// search text in every string column
        /// </summary>
        public string? Search { get; set; }
        /// <summary>
        /// Page number start form 0, to show all set -1
        /// </summary>
        public int PageNumber { get; set; } = 0;
        /// <summary>
        /// default value is 10
        /// </summary>
        public int PageSize { get; set; } = 10;

    }
}

