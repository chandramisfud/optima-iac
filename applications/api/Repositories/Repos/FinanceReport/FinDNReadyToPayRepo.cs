using Dapper;
using Repositories.Contracts;
using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos
{
    public partial class FinListingDNRepository 
    {
        public async Task<object> GetDNReadyToPayLP(string period, int categoryId, int entityId, 
            int distributorId, int subAccountId,  string profileId,
          string search,   int pageNum = 0, int dataDisplayed = 10)
        {
            ListingDNLandingPage res = null!;
            using (IDbConnection conn = Connection)
                try
                {
                    var __param = new DynamicParameters();
                    __param.Add("@userid", profileId);
                    __param.Add("@periode", period);
                    __param.Add("@category", categoryId);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@subaccount", subAccountId);
                   
                    __param.Add("@start", pageNum);
                    __param.Add("@length", dataDisplayed);          
         
                    __param.Add("@txtSearch", search);

                    var __object = await conn.QueryMultipleAsync("ip_debetnote_report_readytopay_p", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var data = __object.Read<object>().ToList();
                    var count = __object.ReadSingle<ListingPromoReportingRecordCount>();

                    return new
                    {
                        Data = data,
                        totalCount = count.recordsTotal,
                        filteredCount = count.recordsFiltered
                    };
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }

        }

        public async Task<object> GetDNReadyToPayFilter(string profile)
        {
            ListingDNLandingPage res = null!;
            using (IDbConnection conn = Connection)
                try
                {
                    var __param = new DynamicParameters();
                    __param.Add("@profileId", profile);
                  

                    var __object = await conn.QueryMultipleAsync("ip_debetnote_report_readytopay_filter_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                
                    return new
                    {
                        category = __object.Read<object>().ToList(),
                        entityDistributor = __object.Read<object>().ToList(),
                        subAccount = __object.Read<object>().ToList()
                    };
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }

        }
    }
}
