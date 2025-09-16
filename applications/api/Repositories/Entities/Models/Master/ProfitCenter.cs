
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class ProfitCenterModel
    {
        public string? ProfitCenter { get; set; }
        public string? ProfitCenterDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeletedEmail { get; set; }
    }

    public class ProfitCenterLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<ProfitCenterSelect>? Data { get; set; }

    }
    public class ProfitCenterById
    {
        public string? ProfitCenter { get; set; }
    }

    public class ProfitCenterSelect
    {
        public string? ProfitCenter { get; set; }
        public string? ProfitCenterDesc { get; set; }
    }

    public class ProfitCenterListRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public ProfitCenterSortColumn SortColumn { get; set; }
        public ProfitCenterSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProfitCenterSortColumn
    {
        ProfitCenter,
        ProfitCenterDesc,
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProfitCenterSortDirection
    {
        asc,
        desc
    }

    public class ProfitCenterCreate
    {
        public string? ProfitCenter { get; set; }
        public string? ProfitCenterDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }


    public class ProfitCenterCreateReturn
    {

        public string? ProfitCenter { get; set; }
        public string? ProfitCenterDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }

    }

    public class ProfitCenterUpdate
    {
        public string? ProfitCenter { get; set; }
        public string? ProfitCenterDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class ProfitCenterUpdateReturn
    {

        public string? ProfitCenter { get; set; }
        public string? ProfitCenterDesc { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class ProfitCenterDelete
    {
        public string? ProfitCenter { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedEmail { get; set; }

    }
    public class ProfitCenterDeleteReturn
    {
        public string? ProfitCenter { get; set; }
        public string? DeletedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeletedEmail { get; set; }


    }
}