using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models.DNConfirmPaid;

namespace Repositories.Repos
{
    public class DNConfirmDNPaidRepo : IDNConfirmDNPaidRepo
    {
        readonly IConfiguration __config;
        public DNConfirmDNPaidRepo(IConfiguration config)
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
        //debetnote/confirm_paid
        public async Task<DNConfirmPaid> DNConfirmPaid(DNChangeStatusConfirmPaid param)
        {
            try
            {
                DataTable __dT = new();

                __dT.Columns.Add("keyid", typeof(int));
                foreach (DNIdArray v in param.DNId!)
                    __dT.Rows.Add(v.DNId);
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@status", param.status);
                __param.Add("@userid", param.UserId);
                __param.Add("@payment_date", param.paymentDate);
                __param.Add("@id", __dT.AsTableValuedParameter());
                var result = await conn.QueryAsync<DNConfirmPaid>("ip_dn_confirmpaid", __param, commandTimeout: 180, commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault()!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        //debetnote/confirmpaid/
        public async Task<IList<DNConfirmPaid>> GetDNStatusConfirmPaid(string status, string userId, int entityId, int distributorId)
        {
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@status", status);
                __param.Add("@userid", userId);
                __param.Add("@entityid", entityId);
                __param.Add("@distributorid", distributorId);

                var result = await conn.QueryAsync<DNConfirmPaid>("ip_debetnote_list_bystatus", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }

}