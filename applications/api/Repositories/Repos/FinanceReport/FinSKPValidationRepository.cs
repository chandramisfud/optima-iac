using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Contracts;
using Dapper;
using Repositories.Entities.Report;

namespace Repositories.Repos
{
    public class FinSKPValidationRepository : IFinSKPValidationRepo
    {
        readonly IConfiguration __config;
        public FinSKPValidationRepository(IConfiguration config)
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

        public async Task<SKPValidationLandingPage> GetSKPValidationLandingPage(string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId,
            int cancelStatus, string startFrom, string startTo, int submissionParam, int status,
            string keyword, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10)
        {
            SKPValidationLandingPage res = new();
            using (IDbConnection conn = Connection)
                try
                {
                    var startData = pageNum * dataDisplayed;
                    keyword = (keyword) ?? "";

                    var __param = new DynamicParameters();
                    __param.Add("@periode", period);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@userid", profileId);
                    __param.Add("@cancelstatus", cancelStatus);
                    __param.Add("@start_from", startFrom);
                    __param.Add("@start_to", startTo);
                    __param.Add("@submission_param", submissionParam);
                    __param.Add("@status", status);
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@filter", "");
                    __param.Add("@txtSearch", keyword);

                    var __object = await conn.QueryMultipleAsync("ip_skp_validate_report_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    var data = __object.Read<SKPValidationData>().ToList();

                    var count = __object.ReadSingle<SKPValidationRecordCount>();

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

        public async Task<IList<SKPValidationEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<SKPValidationEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SKPValidationDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
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
                var child = await conn.QueryAsync<SKPValidationDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SKPValidationChannelList>> GetChannelList(string userid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);
                var __query = new DynamicParameters();
                __query.Add("@userid", userid);
                __query.Add("@attribute", "channel");
                __query.Add("@parent", __parent.AsTableValuedParameter());
                conn.Open();
                var child = await conn.QueryAsync<SKPValidationChannelList>("ip_getattribute_bymapping", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}
