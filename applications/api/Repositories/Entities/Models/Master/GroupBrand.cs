using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class GroupBrandModel
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? SAPCode { get; set; }
        public int IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class GroupBrandLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<GroupBrandSelect>? Data { get; set; }
    }
    public class GroupBrandListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public GroupBrandSortColumn SortColumn { get; set; }
        public GroupBrandSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GroupBrandSortColumn
    {
        Id,
        BrandId,
        LongDesc,
        ShortDesc,
        SAPCode
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GroupBrandSortDirection
    {
        asc,
        desc
    }
    public class GroupBrandSelect
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? SAPCode { get; set; }

    }
    public class GroupBrandCreate
    {
        public int? BrandId { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
        public string? SAPCode { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }

    }

    public class GroupBrandCreateReturn
    {
        public int Id { get; set; }
        public int? BrandId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? SAPCode { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class GroupBrandUpdate
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? SAPCode { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class GroupBrandUpdateReturn
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? SAPCode { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class GroupBrandDelete
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class GroupBrandDeleteReturn
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeleteEmail { get; set; }
    }
}