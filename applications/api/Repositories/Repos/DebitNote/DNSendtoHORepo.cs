using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{
    public class DNSendtoHORepo : IDNSendtoHORepo
    {
        readonly IConfiguration __config;
        public DNSendtoHORepo(IConfiguration config)
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
        public async Task DeletePromoAttachmentForSendtoHO(int PromoId, string DocLink)
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

        public async Task DNChangeStatusDistributortoHO(
            string status,
            string userid,
            int dnid)
        {
            try
            {

                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@status", status);
                __param.Add("@userid", userid);
                __param.Add("@id", dnid);


                var result = await conn.ExecuteAsync("ip_dn_send_to_dist_ho", __param, commandTimeout: 180, commandType: CommandType.StoredProcedure);

            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> DNGenerateSuratJalantoHO(
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

                    var result = await conn.QueryAsync<object>("ip_dn_create_ttd", __param, commandTimeout: 180, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<DNDto>> GetDNSendtoHO(string status, string userid, int entityid, int distributorid)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@status", status);
                __param.Add("@userid", userid);
                __param.Add("@entityid", entityid);
                __param.Add("@distributorid", distributorid);

                var result = await conn.QueryAsync<DNDto>("ip_debetnote_list_bystatus", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }

}