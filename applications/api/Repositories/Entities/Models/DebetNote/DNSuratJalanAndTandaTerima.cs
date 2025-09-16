namespace Repositories.Entities.Models.DN
{
    public class DNSuratJalanAndTandaTerimaDto
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorDesc { get; set; }
        public int EntityId { get; set; }
        public string? EntityDesc { get; set; }
        public string? UserId { get; set; }
        public string? CreateOn { get; set; }
        public IList<DetailDNForSuratJalanAndTandaTerima>? DnId { get; set; }

    }

    public class DetailDNForSuratJalanAndTandaTerima
    {
        public string? PromoNumber { get; set; }
        public string? DNNumber { get; set; }
        public string? MemDocNo { get; set; }
        public string? AccountDesc { get; set; }
        public string? ActivityDesc { get; set; }
        public decimal TotalClaim { get; set; }

    }
}