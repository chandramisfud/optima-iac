using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Repositories.Entities.Report;

namespace Repositories.Repos
{
    public class FinDNDisplayRepository : IFinDNDisplayRepo
    {
        readonly IConfiguration __config;
        public FinDNDisplayRepository(IConfiguration config)
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

              public async Task<DNDisplayLandingPage> GetDNDisplayLandingPage(string period, int entityId, int distributorId, int channelId, int accountId, string profileId, string isDNManual,
            string keyword, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10)
        {
            DNDisplayLandingPage res = new();
            using (IDbConnection conn = Connection)
                try
                {
                    var startData = pageNum * dataDisplayed;
                    keyword = (keyword) ?? "";
                    var __param = new DynamicParameters();
                    __param.Add("@periode", period);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@channel", channelId);
                    __param.Add("@account", accountId);
                    __param.Add("@userid", profileId);
                    __param.Add("@isdnmanual", isDNManual);
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@filter", "");
                    __param.Add("@txtSearch", keyword);

                    var __object = await conn.QueryMultipleAsync("ip_debetnote_list_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    var data = __object.Read<DNDisplayData>().ToList();
                    var count = __object.ReadSingle<DNDisplayRecordCount>();

                    res.totalCount = count.recordsTotal;
                    res.filteredCount = count.recordsFiltered;

                    res.Data = data;
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
            return res;
        }

        public async Task<DNDisplayDataById> GetDNDisplayData(int id)
        {
            try
            {
                DNDisplayDataById __res = null!;
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __obj = await conn.QueryMultipleAsync("ip_debetnote_select", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    {
                        __res = __obj.Read<DNDisplayDataById>().FirstOrDefault()!;
                        if (__res == default)
                        {
                            return __res = null!;
                        }
                        var __sellPoint = __obj.Read<DNDisplaySellpoint>().ToList();
                        var __dnAttachment = __obj.Read<DNDisplayAttachment>().ToList();
                        var __dnDocCompletness = __obj.Read<DNDisplayDocCompleteness>().ToList();

                        __res.sellPoint = __sellPoint;
                        __res.dnAttachment = __dnAttachment;
                        __res.dnDocCompletenessHeader = __dnDocCompletness;
                    }
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNDisplayEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<DNDisplayEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNDisplayDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);
                var __query = new DynamicParameters();
                __query.Add("@budgetid", budgetid);
                __query.Add("@attribute", "distributor");
                __query.Add("@parent", __parent.AsTableValuedParameter());
                conn.Open();
                var child = await conn.QueryAsync<DNDisplayDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNDisplayTaxLevelList>> GetTaxLevelList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT * FROM tbset_mapping_material";
                var __res = await conn.QueryAsync<DNDisplayTaxLevelList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNDisplaySellingPointList>> GetSellingPointList(string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = new DynamicParameters();
                __query.Add("@userid", profileId);
                conn.Open();
                var child = await conn.QueryAsync<DNDisplaySellingPointList>("ip_select_sellpoint_byuser", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<DNDisplayPrint> GetDNPrint(int id)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@id", id);
                DNDisplayPrint res = new();
                conn.Open();
                var __object = await conn.QueryMultipleAsync("ip_debetnote_print", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                res = __object.Read<DNDisplayPrint>().FirstOrDefault()!;
                return res;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}
