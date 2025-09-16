using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNSuratJalanAndTandaTerimatoHORepo : IDNSuratJalanAndTandaTerimatoHORepo
    {
        readonly IConfiguration __config;
        public DNSuratJalanAndTandaTerimatoHORepo(IConfiguration config)
        {
            __config = config;
        }
        //   public IDbConnection Connection => throw new NotImplementedException();
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }

        public async Task<DNSuratJalanAndTandaTerimaDto> GetSuratPengantarById(int id)
        {
            try
            {
                List<DNSuratJalanAndTandaTerimaDto> __dnSuratJalan = new();

                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@id", id);

                using (var __res = await conn.QueryMultipleAsync("ip_suratpengantar_select", __param, commandType: CommandType.StoredProcedure, commandTimeout:180))
                {
                    DNSuratJalanAndTandaTerimaDto __result = new();
                    __result = __res.ReadSingle<DNSuratJalanAndTandaTerimaDto>();
                    __result.DnId = new List<DetailDNForSuratJalanAndTandaTerima>();
                    foreach (DetailDNForSuratJalanAndTandaTerima __f in __res.Read<DetailDNForSuratJalanAndTandaTerima>())
                        __result.DnId.Add(new DetailDNForSuratJalanAndTandaTerima()
                        {
                            PromoNumber = __f.PromoNumber,
                            DNNumber = __f.DNNumber,
                            MemDocNo = __f.MemDocNo,
                            ActivityDesc = __f.ActivityDesc,
                            AccountDesc = __f.AccountDesc,
                            TotalClaim = __f.TotalClaim
                        });
                    __dnSuratJalan.Add(__result);

                }
                return __dnSuratJalan.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public IList<DNSuratJalanAndTandaTerimaDto> GetSuratPengantarList(string senddate, string userid)
        {
            try
            {
                List<DNSuratJalanAndTandaTerimaDto> __debetnote = new();
                var orderDictionary = new Dictionary<int, DNSuratJalanAndTandaTerimaDto>();
                using IDbConnection conn = Connection;



                var result = conn.Query<DNSuratJalanAndTandaTerimaDto, DetailDNForSuratJalanAndTandaTerima, DNSuratJalanAndTandaTerimaDto>("ip_suratpengantar_list",
                         (orderHeader, orderDetail) =>
                         {

                             DNSuratJalanAndTandaTerimaDto orderEntry;
                             if (!orderDictionary.TryGetValue(orderHeader.Id, out orderEntry!))
                             {
                                 orderEntry = orderHeader;
                                 orderEntry.DnId = new List<DetailDNForSuratJalanAndTandaTerima>();
                                 orderDictionary.Add(orderHeader.Id, orderEntry = orderHeader);

                             }
                             orderEntry.DnId!.Add(orderDetail);
                             return orderEntry;
                         },
             new
             {
                 senddate = senddate,
                 userid = userid
             },
             commandType: CommandType.StoredProcedure, commandTimeout:180,
             splitOn: "id,PromoNumber")
             .Distinct()
             .ToList();
                return result;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}