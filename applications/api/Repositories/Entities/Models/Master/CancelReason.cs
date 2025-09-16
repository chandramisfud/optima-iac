using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class CancelReasonModel
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class CancelReasonLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<CancelReasonSelect>? Data { get; set; }

    }
    public class CancelReasonById
    {
        public int Id { get; set; }
    }


    public class CancelReasonSelect
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class CancelReasonListRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public CancelReasonSortColumn SortColumn { get; set; }
        public CancelReasonSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CancelReasonSortColumn
    {
        Id,
        LongDesc
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CancelReasonSortDirection
    {
        asc,
        desc
    }

    public class CancelReasonCreate
    {
        public string? LongDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class CancelReasonCreateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }

    }

    public class CancelReasonUpdate
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class CancelReasonUpdateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class CancelReasonDelete
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteEmail { get; set; }

    }
    public class CancelReasonDeleteReturn
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeleteEmail { get; set; }

    }
}