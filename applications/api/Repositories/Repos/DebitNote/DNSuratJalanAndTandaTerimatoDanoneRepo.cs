using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNSuratJalanAndTandaTerimatoDanoneRepo : IDNSuratJalanAndTandaTerimatoDanoneRepo
    {
        readonly IConfiguration __config;
        public DNSuratJalanAndTandaTerimatoDanoneRepo(IConfiguration config)
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

        public async Task<DNSuratJalanAndTandaTerimaDto> GetSuratPengantarHOtoDanonebyId(int id)
        {
            try
            {
                DNSuratJalanAndTandaTerimaDto? __res = null;
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);
                    var __obj = await conn.QueryMultipleAsync("ip_suratpengantar_ho_select", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        __res = __obj.Read<DNSuratJalanAndTandaTerimaDto>().FirstOrDefault()!;
                        if (__res == default)
                        {
                            return __res = null!;
                        }
                        var __dnId = __obj.Read<DetailDNForSuratJalanAndTandaTerima>().ToList();
                        __res.DnId = __dnId;
                    }
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public IList<DNSuratJalanAndTandaTerimaDto> GetSuratPengantarHOtoDanoneList(string senddate, string profileId)
        {
            try
            {
                List<DNSuratJalanAndTandaTerimaDto> __debetnote = new();
                var orderDictionary = new Dictionary<int, DNSuratJalanAndTandaTerimaDto>();
                using IDbConnection conn = Connection;
                var sql = @"select a.Id,
                                a.RefId,
                                a.DistributorId,
                                b.LongDesc DistributorDesc, 
                                b.[Address]
                                ,convert(varchar(10),a.CreateOn,120) CreateOn
                                ,iif(d.PromoId=0,'DN Manual',f.RefId) PromoNumber
                                ,d.RefId DNNumber
                                ,d.MemDocNo
                                ,e.LongDesc AccountDesc
                                ,d.ActivityDesc
                                ,d.TotalClaim
                                from tbtrx_debetnote_sp_header_ho a
                                inner join tbmst_distributor b on a.DistributorId=b.id
                                inner join tbtrx_debetnote_sp_detail_ho c on a.Id=c.parentid
                                inner join tbtrx_debetnote d on c.DnId=d.Id
                                LEFT join tbmst_subaccount e on d.AccountId=e.Id 
                                LEFT join tbtrx_promo f on d.PromoId=f.Id
                                where convert(varchar(10),a.CreateOn,120)=@senddate
                                and isnull(c.IsCancel, 0) = 0
                                AND d.distributorid IN 
                                    (select distributorid from tbset_user_distributor where UserId=@profileid)
                                ";

                var result = conn.Query<DNSuratJalanAndTandaTerimaDto, DetailDNForSuratJalanAndTandaTerima, DNSuratJalanAndTandaTerimaDto>(sql,
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
                 profileid = profileId
             },
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