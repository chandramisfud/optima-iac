using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNSuratJalanAndTandaTerimatoHORepo
    {
        IList<DNSuratJalanAndTandaTerimaDto> GetSuratPengantarList(string senddate, string userid);
        Task<DNSuratJalanAndTandaTerimaDto> GetSuratPengantarById(int id);

    }
}