using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;
using Repositories.Entities.Models.DNSendtoDanone;

namespace Repositories.Repos
{
    public class DNSendtoDanoneRepo : IDNSendtoDanoneRepo
    {
        readonly IConfiguration __config;
        public DNSendtoDanoneRepo(IConfiguration config)
        {
            __config = config;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }
        public async Task DeletePromoAttachmentForSendtoDanone(int PromoId, string DocLink)
        {
            try
            {
                using IDbConnection conn = Connection;
                await conn.ExecuteAsync(@"DELETE FROM tbtrx_promo_doclink where PromoId=@PromoId and DocLink=@DocLink"
                , new
                {
                    PromoId = PromoId,
                    DocLink = DocLink

                });
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task DNChangeStatusSendtoDanone(
            string status,
            string userid,
            int dnid
        )
        {
            try
            {
                using (IDbConnection conn = Connection)
                {

                    var __param = new DynamicParameters();
                    __param.Add("@status", status);
                    __param.Add("@userid", userid);
                    __param.Add("@id", dnid);

                    var result = await conn.ExecuteAsync("ip_dn_send_to_danone", __param, commandTimeout: 180, 
                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNChangeStatusSendtoDanone>> GetDebetNoteByStatusValidatebyDistributorHO(string status, string userid, int entityid, int distributorid)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@status", status);
                __param.Add("@userid", userid);
                __param.Add("@entityid", entityid);
                __param.Add("@distributorid", distributorid);

                var result = await conn.QueryAsync<DNChangeStatusSendtoDanone>("ip_debetnote_list_bystatus", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<GetDNbyIdForDNSendtoDanone> GetDNbyIdforSendtoDanone(int id)
        {
            try
            {
                GetDNbyIdForDNSendtoDanone? __res = null;
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __obj = await conn.QueryMultipleAsync("ip_debetnote_select", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        __res = __obj.Read<GetDNbyIdForDNSendtoDanone>().FirstOrDefault()!;
                        if (__res == default)
                        {
                            return __res = null!;
                        }
                        var __sellPoint = __obj.Read<DNSellpoint>().ToList();
                        var __dnAttachment = __obj.Read<DNAttachment>().ToList();
                        var __dnDocCompletness = __obj.Read<DNDocCompleteness>().ToList();

                        __res.sellpoint = __sellPoint;
                        __res.dnattachment = __dnAttachment;
                        __res.DNDocCompletenessHeader = __dnDocCompletness;
                    }
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DNSendtoDanoneStandardResponse> RejectDNSendtoDanone(int dnid, string reason, string userid)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@dnid", dnid);
                __param.Add("@reason", reason);
                __param.Add("@userid", userid);

                var result = await conn.QueryAsync<DNSendtoDanoneStandardResponse>("ip_debetnote_reject", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> DNGenerateSuratJalantoDanone(
         string userid,
         List<DNId> dnid)
        {
            try
            {
                DataTable __dT = new();

                __dT.Columns.Add("keyid", typeof(int));
                foreach (DNId v in dnid)
                    __dT.Rows.Add(v.dnid);

                using (IDbConnection conn = Connection)
                {
                    var sql = @"SELECT TOP 1 DistributorId 
                                FROM tbset_user_distributor 
                                where UserId=@userid ";
                    int distributorid = conn.QuerySingleOrDefault<int>(sql, new { userid = userid });

                    var __param = new DynamicParameters();
                    __param.Add("@distributorid", distributorid);
                    __param.Add("@userid", userid);
                    __param.Add("@id", __dT.AsTableValuedParameter());

                    var result = await conn.QueryAsync<object>("ip_dn_create_ttd_ho", __param, commandTimeout: 180, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}