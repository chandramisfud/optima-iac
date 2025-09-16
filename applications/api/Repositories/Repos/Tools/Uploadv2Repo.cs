using System.Data;
using System.Data.SqlClient;
using Dapper;
using Entities.Tools;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public partial class UploadRepo : IUploadRepo
    {
        public Task<int> UpdatePromoReconStatus(DataTable lsPromo, string userProfile, string useremail)
        {
            int rowAffected = 0;
            using (IDbConnection conn = Connection)
            {
                conn.Open();

                rowAffected = conn.Execute("ip_update_recon_status",
                new
                {
                    promoid = lsPromo.AsTableValuedParameter(),
                    profileid = userProfile,
                    email = useremail
                }
                , commandType: CommandType.StoredProcedure, commandTimeout: 180);
            }
            return Task.FromResult(rowAffected);
        }
    }
}