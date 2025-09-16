using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNSuratJalanAndTandaTerimatoDanoneRepo
    {
        IList<DNSuratJalanAndTandaTerimaDto> GetSuratPengantarHOtoDanoneList(string senddate, string profileId);
        Task<DNSuratJalanAndTandaTerimaDto> GetSuratPengantarHOtoDanonebyId(int id);

    }
}