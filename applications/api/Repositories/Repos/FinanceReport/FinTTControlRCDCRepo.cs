using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Repositories.Contracts;
using Repositories.Entities.Report;
using System.Collections.Generic;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public partial class FinInvestmentReportRepository
    {

        public async Task<object> GetTTControlRCDC(string year, DataTable category, DataTable groupBrand, DataTable channel, DataTable subActivityType,
            string profileid,int start, int length,
            string filter, string search)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@period", year);
                    param.Add("@categoryid", category.AsTableValuedParameter());
                    param.Add("@groupbrandid", groupBrand.AsTableValuedParameter());
                    param.Add("@channelid", channel.AsTableValuedParameter());
                    param.Add("@subactivityType", subActivityType.AsTableValuedParameter());
                    param.Add("@profileid", profileid);
                    param.Add("@start", start);
                    param.Add("@length", length);
                    param.Add("@filter", filter);
                    param.Add("@txtSearch", search);
                    conn.Open();
                    var __res = await conn.QueryMultipleAsync("ip_channel_summary_lp", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var _dataAll = __res.Read<object>().ToList();
                    var _stat = __res.ReadSingle<BaseLPStats>();
                    var _brand = __res.Read<dynamic>();
                    return new
                    {
                        data = _dataAll,
                        totalCount = _stat.recordsTotal,
                        filteredCount = _stat.recordsFiltered,
                        brand = _brand.Select(x=>x.brand).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> GetTTControlRCDCDownload(string year, DataTable category, DataTable groupBrand, DataTable channel, DataTable subActivityType,
            string profileid, int start, int length,
            string filter, string search)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@period", year);
                    param.Add("@categoryid", category.AsTableValuedParameter());
                    param.Add("@groupbrandid", groupBrand.AsTableValuedParameter());
                    param.Add("@channelid", channel.AsTableValuedParameter());
                    param.Add("@subactivityType", subActivityType.AsTableValuedParameter());
                    param.Add("@profileid", profileid);
                    param.Add("@start", start);
                    param.Add("@length", length);
                    param.Add("@filter", filter);
                    param.Add("@txtSearch", search);
                    conn.Open();
                    var __res = await conn.QueryMultipleAsync("ip_channel_summary", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var _dataAll = __res.Read<object>().ToList();
                    var _stat = __res.ReadSingle<BaseLPStats>();
                    var _brand = __res.Read<dynamic>();
                    return new
                    {
                        data = _dataAll,
                        totalCount = _stat.recordsTotal,
                        filteredCount = _stat.recordsFiltered,
                        brand = _brand.Select(x => x.brand).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> GetTTControlRCDCFilter()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var sql = @" /* List Category */
        	                     SELECT id, shortDesc, longDesc
                                 FROM tbmst_category 
                                 where ISNULL(IsDeleted,0) = 0;

	                            /* List group Brand */
	                            SELECT DISTINCT groupBrandId, groupBrandDesc 
                                FROM vw_sku_active ORDER BY  groupBrandDesc;

	                            /* List group channel */
	                            SELECT id channelId, LongDesc channelDesc
                                FROM tbmst_channel
								WHERE ISNULL(isdelete, 0)=0;

                                /* List subActivityType  */
                                SELECT id as subActivityTypeId, LongDesc subActivityTypeDesc 
                                FROM tbmst_subactivity_type
	                            WHERE ISNULL(IsDeleted, 0)=0
";

                    using (var __resp = await conn.QueryMultipleAsync(sql, commandTimeout: 180))
                    {
                        var result = new
                        {
                            category = __resp.Read<object>().ToList(),
                            grpBrand = __resp.Read<object>().ToList(),
                            channel = __resp.Read<object>().ToList(),
                            subActivityType = __resp.Read<object>().ToList()
                        };

                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
