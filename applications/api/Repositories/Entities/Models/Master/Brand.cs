using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class BrandModel
    {
        public int Id { get; set; }
        public int PrincipalId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public string? RefID { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class BrandLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<BrandSelect>? Data { get; set; }
    }
    public class BrandById
    {
        public int Id { get; set; }
    }

    public class BrandSelect
    {
        public int EntityId { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? EntityRefId { get; set; }
        public int BrandId { get; set; }
        public string? BrandShortDesc { get; set; }
        public string? BrandLongDesc { get; set; }
        public string? BrandRefId { get; set; }
    }

    public class BrandListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public BrandSortColumn SortColumn { get; set; }
        public BrandSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BrandSortColumn
    {
        EntityId,
        EntityLongDesc,
        EntityShortDesc,
        EntityRefid,
        BrandId,
        BrandLongDesc,
        BrandShortDesc,
        BrandRefid
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BrandSortDirection
    {
        asc,
        desc
    }

    public class BrandCreate
    {
        public int PrincipalId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class BrandCreateReturn
    {
        public int Id { get; set; }
        public int PrincipalId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class BrandUpdate
    {
        public int Id { get; set; }
        public int PrincipalId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class BrandUpdateReturn
    {
        public int Id { get; set; }
        public int PrincipalId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class BrandDelete
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class BrandDeleteReturn
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }
    }

    public class BrandByAccess
    {
        public string? userid { get; set; }

        public int Entityid { get; set; }
    }

    public class BrandByAccessReturn
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class EntityforBrand
    {
        public int id { get; set; }
        public string? LongDesc { get; set; }
    }
}
