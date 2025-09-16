


using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class TaxLevelList
    {
        public int Id { get; set; }
        public string? MaterialNumber { get; set; }
        public string? Description { get; set; }
        public string? WHT_Type { get; set; }
        public string? WHT_Code { get; set; }
        public string? Purpose { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public decimal PPNPct { get; set; }
        public decimal PPHPct { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreateByName { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteByName { get; set; }
        public string? CreatedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaxLevelSortColumn
    {
        id,
        MaterialNumber,
        Description,
        WHT_Code,
        Purpose,
        Entity,
        PPNPct,
        PPHPct
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaxLevelSortDirection
    {
        asc,
        desc
    }
    public class TaxLevelListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public TaxLevelSortColumn SortColumn { get; set; }
        public TaxLevelSortDirection SortDirection { get; set; }
    }

    public class TaxLevelCreate
    {
        public string? MaterialNumber { get; set; }
        public string? Description { get; set; }
        public string? WHT_Type { get; set; }
        public string? WHT_Code { get; set; }
        public string? Purpose { get; set; }
        public string? Entity { get; set; }
        public int EntityId { get; set; }
        public decimal PPNPct { get; set; }
        public decimal PPHPct { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class TaxLevelCreateReturn
    {
        public int Id { get; set; }
        public string? MaterialNumber { get; set; }
        public string? Description { get; set; }
        public string? WHT_Type { get; set; }
        public string? WHT_Code { get; set; }
        public string? Entity { get; set; }
        public int EntityId { get; set; }
        public string? Purpose { get; set; }
        public decimal PPNPct { get; set; }
        public decimal PPHPct { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class TaxLevelDelete
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserLogin { get; set; }
    }

    public class TaxLevelDeleteReturn
    {
        public int? Id { get; set; }
        public string? MaterialNumber { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public int parentId { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class EntityforTaxLevel
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
}

