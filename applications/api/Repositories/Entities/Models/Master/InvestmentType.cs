using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class InvestmentTypeModel
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? LongDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class InvestmentTypeLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<InvestmentTypeSelect>? Data { get; set; }
    }
    public class InvestmentTypeById
    {
        public int Id { get; set; }
    }
    public class InvestmentTypeSelect
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? RefId { get; set; }
    }
    public class InvestmentTypeListRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public InvestmentTypeSortColumn SortColumn { get; set; }
        public InvestmentTypeSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InvestmentTypeSortColumn
    {
        LongDesc,
        RefId
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InvestmentTypeSortDirection
    {
        asc,
        desc
    }
    public class InvestmentTypeCreate
    {
        public string? RefId { get; set; }
        public string? LongDesc { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class InvestmentTypeCreateReturn
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? LongDesc { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class InvestmentTypeUpdate
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? LongDesc { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class InvestmentTypeUpdateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class InvestmentTypeDelete
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteEmail { get; set; }
    }
    public class InvestmentTypeDeleteReturn
    {
        public int Id { get; set; }
        public string? DeleteBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }
    }

    public class InvestmentTypeMappingReturn
    {
        public int Id { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivityTypeId { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedEmail { get; set; }
    }
    public class InvestmentTypeMappingCreate
    {
        public IList<InvestmentDataType>? investment { get; set; }
        public string? userid { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class InvestmentDataType
    {
        public int id { get; set; }
        public int SubActivityId { get; set; }
        public int InvestmentTypeId { get; set; }
    }
    public class InvestmentTypeMappingDelete
    {
        public IList<InvestmentDataType>? investment { get; set; }
        public string? userid { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class InvestmentTypeMappingDeleteReturn
    {
        public int Id { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivityTypeId { get; set; }
        public DateTime DeleteOn { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class CategoryInvestmentMap
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }

    public class SubCategoryInvestmentMap
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class ActivityInvestmentMap
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class SubActivityInvestmentMap
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
    }
    public class InvestmentTypeforInvestmentMap
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? LongDesc { get; set; }
    }
    public class InvestmentTypeActivate
    {
        public int Id { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
    public class InvestmentTypeActivateReturn
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? LongDesc { get; set; }
        public int IsDeleted { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }
}