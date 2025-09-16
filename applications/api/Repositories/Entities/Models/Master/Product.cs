using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int PrincipalId { get; set; }
        public int BrandId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public int IsActive { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public string? RefId { get; set; }
        public int SeqNo { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class ProductLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<ProductSelect>? Data { get; set; }
    }
    public class ProductById
    {
        public int Id { get; set; }
    }
    public class ProductSelect
    {
        public int EntityId { get; set; }
        public string? EntityRefId { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? EntityShortDesc { get; set; }
        public int BrandId { get; set; }
        public string? BrandRefId { get; set; }
        public string? BrandLongDesc { get; set; }
        public string? BrandShortDesc { get; set; }
        public int ProductId { get; set; }
        public string? ProductRefId { get; set; }
        public string? ProductLongDesc { get; set; }
        public string? ProductShortDesc { get; set; }
        public int SeqNo { get; set; }

    }
    public class ProductListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public ProductSortColumn SortColumn { get; set; }
        public ProductSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductSortColumn
    {
        EntityId,
        EntityRefId,
        EntityLongDesc,
        EntityShortDesc,
        BrandId,
        BrandRefId,
        BrandLongDesc,
        BrandShortDesc,
        ProductId,
        ProductRefId,
        ProductLongDesc,
        ProductShortDesc,

    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductSortDirection
    {
        asc,
        desc
    }
    public class ProductCreate
    {
        public int PrincipalId { get; set; }
        public int BrandId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public int SeqNo { get; set; }

    }
    public class ProductCreateReturn
    {
        public int Id { get; set; }
        public int PrincipalId { get; set; }
        public int BrandId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? RefId { get; set; }
        public int SeqNo { get; set; }

    }
    public class ProductUpdate
    {
        public int Id { get; set; }
        public int PrincipalId { get; set; }
        public int BrandId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
        public int SeqNo { get; set; }

    }

    public class ProductUpdateReturn
    {
        public int Id { get; set; }
        public int PrincipalId { get; set; }
        public int BrandId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
        public int SeqNo { get; set; }

    }
    public class ProductDelete
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class ProductDeleteReturn
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }

    public class EntityforProduct
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class BrandforProduct
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
}