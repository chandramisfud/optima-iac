using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class SKUBlitzModel
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public int BrandId { get; set; }
        public string? Brand { get; set; }
        public int SKUId { get; set; }
        public string? SKU { get; set; }
        public int IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public int isDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }
        public string? SAPCode { get; set; }
    }
    public class SKUBlitzCreate
    {
        public int SKUId { get; set; }
        public string? SAPCode { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class SKUBlitzCreateReturn
    {
        public string? Id { get; set; }
        public int SKUId { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class SKUBlitzLandingPage
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public int BrandId { get; set; }
        public string? Brand { get; set; }
        public int SKUId { get; set; }
        public string? SKU { get; set; }
        public int IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? SAPCode { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SKUBlitzSortColumn
    {
        Id,
        EntityId,
        Entity,
        BrandId,
        Brand,
        SKUId,
        SKU,
        IsActive,
        CreateOn,
        CreateBy,
        CreatedEmail,
        SAPCode
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SKUBlitzSortDirection
    {
        asc,
        desc
    }
    public class SKUBlitzListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public SKUBlitzSortColumn SortColumn { get; set; }
        public SKUBlitzSortDirection SortDirection { get; set; }
    }
    public class EntityforSKUBlitz
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class BrandforSKUBlitz
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SKUforSKUBlitz
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class SKUBlitzDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class SKUBlitzDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public int ParentId { get; set; }
    }

}

