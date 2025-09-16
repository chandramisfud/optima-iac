using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class EntityModel
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
        public string? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public string? RefId { get; set; }
        public string? DescForInvoice { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public string? CompanyName { get; set; }
        public string? EntityNPWP { get; set; }
        public string? ShortDesc2 { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class EntityLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<EntitySelect>? Data { get; set; }

    }
    public class EntityById
    {
        public int Id { get; set; }
    }


    public class EntitySelect
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
    }

    public class EntityListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public EntitySortColumn SortColumn { get; set; }
        public EntitySortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EntitySortColumn
    {
        ShortDesc,
        LongDesc,
        RefId
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EntitySortDirection
    {
        asc,
        desc
    }

    public class EntityCreate
    {
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? DescForInvoice { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public string? CompanyName { get; set; }
        public string? EntityNPWP { get; set; }
        public string? ShortDesc2 { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class EntityCreateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? DescForInvoice { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public string? CompanyName { get; set; }
        public string? EntityNPWP { get; set; }
        public string? ShortDesc2 { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? RefId { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class EntityUpdate
    {
        [Required(ErrorMessage = "Cannot be empty")]
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? DescForInvoice { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public string? CompanyName { get; set; }
        public string? EntityNPWP { get; set; }
        public string? ShortDesc2 { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class EntityUpdateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? DescForInvoice { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public string? CompanyName { get; set; }
        public string? EntityNPWP { get; set; }
        public string? ShortDesc2 { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class EntityDelete
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class EntityDeleteReturn
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }

}