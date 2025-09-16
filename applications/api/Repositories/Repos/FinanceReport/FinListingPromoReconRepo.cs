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
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class FinListingPromoReconRepo : IFinListingPromoReconRepo
    {
        readonly IConfiguration __config;
        public FinListingPromoReconRepo(IConfiguration config)
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

        public async Task<ListingPromoReconLandingPage> GetListingPromoReconLandingPage(string period, int entityId, int distributorId, int budgetParentId, int channelId,
            string createForm, string createTo, string startFrom, string startTo, int submissionParam, string[] profileId,
            string keyword, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10)
        {
            ListingPromoReconLandingPage res = new();
            using (IDbConnection conn = Connection)
                try
                {
                    var startData = pageNum * dataDisplayed;
                    keyword = (keyword) ?? "";
                    DataTable __pr = new("UserType");
                    __pr.Columns.Add("userid");
                    foreach (string v in profileId)
                        __pr.Rows.Add(v);

                    var __param = new DynamicParameters();
                    __param.Add("@periode", period);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@create_from", createForm);
                    __param.Add("@create_to", createTo);
                    __param.Add("@start_from", startFrom);
                    __param.Add("@start_to", startTo);
                    __param.Add("@submission_param", submissionParam);
                    __param.Add("@userid", __pr.AsTableValuedParameter());
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@filter", "");
                    __param.Add("@txtSearch", keyword);

                    var __object = await conn.QueryMultipleAsync("ip_promo_recon_list_reporting_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    var data = __object.Read<ListingPromoReconData>().ToList();

                    var count = __object.ReadSingle<ListingPromoReconRecordCount>();

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

        public async Task<IList<ListingPromoReconEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<ListingPromoReconEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ListingPromoReconDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
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
                var child = await conn.QueryAsync<ListingPromoReconDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ListingPromoReportingChannelList>> GetChannelList(string userid, int[] arrayParent)
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
                var child = await conn.QueryAsync<ListingPromoReportingChannelList>("ip_getattribute_bymapping", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<GetUserGroupforPromoRecon>> GetUserGroupsforFinPromoRecon(string[] id)
        {
            try
            {
                using IDbConnection conn = Connection;
                DataTable __pr = new("UserType");
                __pr.Columns.Add("id");
                foreach (string v in id)
                    __pr.Rows.Add(v);

                var __param = new DynamicParameters();
                __param.Add("@id", __pr.AsTableValuedParameter());
                var result = await conn.QueryAsync<GetUserGroupforPromoRecon>("ip_getuser_bygroup", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}
