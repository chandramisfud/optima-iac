using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;
using Repositories.Entities.Models.DNReceivedAndApproved;

namespace Repositories.Repos
{
    public class DNReceivedandApprovedbyHORepo : IDNReceivedandApprovedbyHORepo
    {
        readonly IConfiguration __config;
        public DNReceivedandApprovedbyHORepo(IConfiguration config)
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

        public async Task DeletePromoAttachmentDNReceivedandApproved(int PromoId, string DocLink)
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

        public async Task DNChangeStatusDistributorMultiApproval(string status, string userid,
            string useremail, int dnid)
        {
            try
            {

                using (IDbConnection conn = Connection)
                {

                    var __param = new DynamicParameters();
                    __param.Add("@status", status);
                    __param.Add("@userid", userid);
                    __param.Add("@createdemail", useremail);
                    __param.Add("@id", dnid);

                    await conn.ExecuteAsync("ip_dn_validate_by_dist_ho", __param, commandTimeout: 180, commandType: CommandType.StoredProcedure);
                }
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DNGetById> GetDNbyId(int id)
        {
            try
            {
                DNGetById __res = new();
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __obj = await conn.QueryMultipleAsync("ip_debetnote_select", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        __res = __obj.Read<DNGetById>().FirstOrDefault()!;
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

        public async Task<DNValidateByDistributorHO> DNMultiApprovalParalel(int DNId, string status, string notes, string userId)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@dnid", DNId);
                __param.Add("@status", status);
                __param.Add("@notes", notes);
                __param.Add("@userid", userId);

                var result = await conn.QueryAsync<DNValidateByDistributorHO>("ip_dn_approval", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNValidateByDistributorHO>> GetDNValidateByDistributorHO(string status, string userid, int entityid, int distributorid)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@status", status);
                __param.Add("@userid", userid);
                __param.Add("@entityid", entityid);
                __param.Add("@distributorid", distributorid);


                var result = await conn.QueryAsync<DNValidateByDistributorHO>("ip_debetnote_list_bystatus", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<DNPromoDisplayGetDistributorId> GetDistributorId(string UserId)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"SELECT TOP 1 Id, DistributorId  
                                FROM  tbset_user_distributor WHERE UserId = @UserId AND ISNULL(IsDeleted,0)=0";

                var __obj = await conn.QueryAsync<DNPromoDisplayGetDistributorId>(__query, new { UserId = UserId });
                return __obj.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}
        public async Task<IList<DNFilter>> DNFilterReceivedandApprovedbyHO(string userid, string status, int entity, string TaxLevel, DataTable dn)
        {
            try
            {
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var result = await conn.QueryAsync<DNFilter>("ip_debetnote_filter",
                new
                {
                    userid = userid,
                    status = status,
                    entity = entity,
                    TaxLevel = TaxLevel,
                    dn = dn.AsTableValuedParameter("TemplateDebetNote")
                }
                    , commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}