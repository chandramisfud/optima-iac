using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Repos
{

    public class DNManualAssignmentRepo : IDNManualAssignmentRepo
    {
        readonly IConfiguration __config;
        public DNManualAssignmentRepo(IConfiguration config)
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

        //    debetnote/assigndn
        public async Task<DNReassignmentList> AssignDN(int DNId, int PromoId, string? UserId)
        {
            try
            {
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                DNAssignParam param = new DNAssignParam();
                param.DNId = DNId;
                param.UserId = UserId;
                param.PromoId = PromoId;

                var result = await conn.QueryAsync<DNReassignmentList>("ip_debetnote_assign", param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        //    debetnote/assignpromo_request
        public async Task<ForwardResponseDto> ForwardAssignment(int dnid, string approver_userid, string internal_order_number)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@dnid", dnid);
                __param.Add("@approveruserid", approver_userid);
                __param.Add("@internalordernumber", internal_order_number);

                var result = await conn.QueryAsync<ForwardResponseDto>("ip_debetnote_assign_request", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        //    promo/getPromoForDn
        public async Task<IList<PromoforDN>> GetApprovedPromoforDN(string periode, int entity, int channel, int account, string userid)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@periode", periode);
                __param.Add("@entity", entity);
                __param.Add("@channel", channel);
                __param.Add("@account", account);
                __param.Add("@userid", userid);

                var result = await conn.QueryAsync<PromoforDN>("ip_promo_approved_list_for_dn", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        //    debetnote/getbyId/
        public async Task<DNGetById> GetDNbyIdforDNManualAssignment(int id)
        {
            try
            {
                DNGetById? __res = null;
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

        //    debetnote/dnmanualassignlist
        public async Task<object> GetDNManualAssignList(string userid, string sortColumn, string sortDirection,
            int length=10, int start=0, string? txtSearch = null
            )
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@userid", userid);
                __param.Add("@length", length);
                __param.Add("@start", start);
                __param.Add("@txtSearch", txtSearch);
                __param.Add("@SortColumn", sortColumn);
                __param.Add("@SortDirection", sortDirection);
                var __res = await conn.QueryMultipleAsync("ip_dnmanual_assign_list", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                List<object> __data = __res.Read<object>().ToList();

                var res = __res.ReadSingle<BaseLP2>();
                res.Data = __data.Cast<object>().ToList();
                return res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // master/getAttributeByUser
        public async Task<DNDistributorEntity> GetAttributeByUser(string userid)
        {
            try
            {
                List<DNDistributorEntity> __mapp = new();
                using IDbConnection conn = Connection;

                IList<EntityDropDownDto> __entity = new List<EntityDropDownDto>();
                IList<MasterGlobalData> __distributor = new List<MasterGlobalData>();
                IList<MasterGlobalData> __channel = new List<MasterGlobalData>();
                IList<MasterGlobalData> __subchannel = new List<MasterGlobalData>();
                IList<MasterGlobalData> __account = new List<MasterGlobalData>();
                IList<MasterGlobalData> __subaccount = new List<MasterGlobalData>();

                DNDistributorEntity __result = new();

                var __param = new DynamicParameters();
                __param.Add("@userid", userid);

                using (var __re = await conn.QueryMultipleAsync("ip_get_account_byuser", __param, commandType: CommandType.StoredProcedure, commandTimeout:180))
                {
                    __entity = __re.Read<EntityDropDownDto>().ToList();
                    __distributor = __re.Read<MasterGlobalData>().ToList();
                    __channel = __re.Read<MasterGlobalData>().ToList();
                    __subchannel = __re.Read<MasterGlobalData>().ToList();
                    __account = __re.Read<MasterGlobalData>().ToList();
                    __subaccount = __re.Read<MasterGlobalData>().ToList();

                    __result.Entity = __entity;
                    __result.Distributor = __distributor;
                    __result.Channel = __channel;
                    __result.SubChannel = __subchannel;
                    __result.Account = __account;
                    __result.SubAccount = __subaccount;
                }
                return __result;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}