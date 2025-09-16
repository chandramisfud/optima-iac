using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Repositories.Entities.Models
{
    public class DistributorModel
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? NPWP { get; set; }
        public int IsActive { get; set; }
        public string? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public string? RefId { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? NoRekening { get; set; }
        public string? BankName { get; set; }
        public string? BankCabang { get; set; }
        public string? ClaimManager { get; set; }
        public string? SAPCode { get; set; }
        public string? SAPCodex { get; set; }
        public string? CreatedEmail { get; set; }
        public string? ModifiedEmail { get; set; }
        public string? DeleteEmail { get; set; }
    }

    public class DistributorLandingPage
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IList<DistributorSelect>? Data { get; set; }

    }
    public class DistributorById
    {
        public int Id { get; set; }
    }


    public class DistributorSelect
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? RefId { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? NPWP { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? NoRekening { get; set; }
        public string? BankName { get; set; }
        public string? BankCabang { get; set; }
        public string? ClaimManager { get; set; }
    }

    public class DistributorListRequest
    {
        public string? Search { get; set; }
        [Required(ErrorMessage = "Empty PageNumber")]
        public int PageNumber { get; set; } = 1;
        [Required(ErrorMessage = "Empty PageSize")]
        public int PageSize { get; set; } = 10;
        public DistributorSortColumn SortColumn { get; set; }
        public DistributorSortDirection SortDirection { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DistributorSortColumn
    {
        ShortDesc,
        LongDesc,
        companyName, 
        address, 
        claimManager,
        RefId
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DistributorSortDirection
    {
        asc,
        desc
    }

    public class DistributorCreate
    {
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? NPWP { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? NoRekening { get; set; }
        public string? BankName { get; set; }
        public string? BankCabang { get; set; }
        public string? ClaimManager { get; set; }
        public string? SAPCode { get; set; }
        public string? SAPCodex { get; set; }
        public string? CreateBy { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class DistributorCreateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? NPWP { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? NoRekening { get; set; }
        public string? BankName { get; set; }
        public string? BankCabang { get; set; }
        public string? ClaimManager { get; set; }
        public string? SAPCode { get; set; }
        public string? SAPCodex { get; set; }
        public string? CreateBy { get; set; }
        public string? CreateOn { get; set; }
        public string? RefId { get; set; }
        public string? CreatedEmail { get; set; }
    }

    public class DistributorUpdate
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? NPWP { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? NoRekening { get; set; }
        public string? BankName { get; set; }
        public string? BankCabang { get; set; }
        public string? ClaimManager { get; set; }
        public string? SAPCode { get; set; }
        public string? SAPCodex { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }

    }

    public class DistributorUpdateReturn
    {
        public int Id { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? NPWP { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? NoRekening { get; set; }
        public string? BankName { get; set; }
        public string? BankCabang { get; set; }
        public string? ClaimManager { get; set; }
        public string? SAPCode { get; set; }
        public string? SAPCodex { get; set; }
        public string? RefId { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedEmail { get; set; }
    }

    public class DistributorDelete
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedEmail { get; set; }

    }
    public class DistributorDeleteReturn
    {
        public int Id { get; set; }
        public string? DeletedBy { get; set; }
        public int IsDeleted { get; set; }
        public string? DeletedOn { get; set; }
        public string? DeleteEmail { get; set; }
        public string? RefId { get; set; }

    }


}