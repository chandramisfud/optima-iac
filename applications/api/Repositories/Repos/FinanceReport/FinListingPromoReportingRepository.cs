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
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class FinListingPromoReportingRepository : IFinListingPromoReportingRepo
    {
        readonly IConfiguration __config;
        public FinListingPromoReportingRepository(IConfiguration config)
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

        public async Task<ListingPromoReportingLandingPage> GetListingPromoReportingLandingPage(
            string period,
            int entityId,
            int distributorId,
            int budgetParentId,
            int channelId,
            string profileId,
            string createFrom,
            string createTo,
            string startFrom,
            string startTo,
            int submissionParam,
            string keyword, 
            string sortColumn,
            int categoryId = 0,
            string sortDirection = "ASC", 
            int pageNum = 0, 
            int dataDisplayed = 10)
        {
            ListingPromoReportingLandingPage res = new();
            using (IDbConnection conn = Connection)
                try
                {
                    var startData = pageNum * dataDisplayed;
                    keyword = (keyword) ?? "";

                    var __param = new DynamicParameters();
                    __param.Add("@periode", period);
                    __param.Add("@category", categoryId);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@userid", profileId);
                    __param.Add("@create_from", createFrom);
                    __param.Add("@create_to", createTo);
                    __param.Add("@start_from", startFrom);
                    __param.Add("@start_to", startTo);
                    __param.Add("@submission_param", submissionParam);
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@filter", "");
                    __param.Add("@txtSearch", keyword);

                    var __object = await conn.QueryMultipleAsync("ip_promo_list_reporting_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    var data = __object.Read<ListingPromoReportingData>().ToList();

                    var count = __object.ReadSingle<ListingPromoReportingRecordCount>();

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

        public async Task<object> GetListingPromoReportingByMechaLP(
           string period,
           int entityId,
           int distributorId,
           int budgetParentId,
           int channelId,
           string profileId,
           string createFrom,
           string createTo,
           string startFrom,
           string startTo,
           int submissionParam,
           string keyword,
           string sortColumn,
           int categoryId = 0,
           string sortDirection = "ASC",
           int pageNum = 0,
           int dataDisplayed = 10)
        {
            //ListingPromoReportingLandingPage res = new();
            using (IDbConnection conn = Connection)
                try
                {
                    var startData = pageNum * dataDisplayed;
                    keyword = (keyword) ?? "";

                    var __param = new DynamicParameters();
                    __param.Add("@periode", period);
                    __param.Add("@category", categoryId);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@userid", profileId);
                    __param.Add("@create_from", createFrom);
                    __param.Add("@create_to", createTo);
                    __param.Add("@start_from", startFrom);
                    __param.Add("@start_to", startTo);
                    __param.Add("@submission_param", submissionParam);
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@filter", "");
                    __param.Add("@txtSearch", keyword);

                    var __object = await conn.QueryMultipleAsync("ip_promo_list_reporting_by_mechanism_p", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
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

       public async Task<object> GetListingPromoReportingPostRecon(
       string period,
       int entityId,
       int distributorId,
       int budgetParentId,
       int channelId,
       string profileId,
       string createFrom,
       string createTo,
       string startFrom,
       string startTo,
       int submissionParam,
       string keyword,
       string sortColumn,
       int categoryId = 0,
       string sortDirection = "ASC",
       int pageNum = 0,
       int dataDisplayed = 10)
        {
            //ListingPromoReportingLandingPage res = new();
            using (IDbConnection conn = Connection)
                try
                {
                    var startData = pageNum * dataDisplayed;
                    keyword = (keyword) ?? "";

                    var __param = new DynamicParameters();
                    __param.Add("@periode", period);
                    __param.Add("@category", categoryId);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@userid", profileId);
                    __param.Add("@create_from", createFrom);
                    __param.Add("@create_to", createTo);
                    __param.Add("@start_from", startFrom);
                    __param.Add("@start_to", startTo);
                    __param.Add("@submission_param", submissionParam);
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@filter", "");
                    __param.Add("@txtSearch", keyword);

                    var __object = await conn.QueryMultipleAsync("ip_promo_list_reporting_post_recon_p", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
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
        public async Task<IList<ListingPromoReportingEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<ListingPromoReportingEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ListingPromoReportingDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
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
                var child = await conn.QueryAsync<ListingPromoReportingDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
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

        public async Task<InvestmentNotifFinReport> CekInvestmentNotif(InvestmentNotifBodyFinReport body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@periode", body.periode);
                __param.Add("@userid", body.userid);
                var result = await conn.QueryMultipleAsync("ip_hlp_investment_notif_cek", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                var accrualDtos = result.Read<InvesmentNotifResultAccrual>();
                var promoDtos = result.Read<InvesmentNotifResultPromo>();
                var debitnoteDtos = result.Read<InvesmentNotifResultDN>();
                var GAP_listDtos = result.Read<InvesmentNotifResultGAP>();

                InvestmentNotifFinReport __res = new()
                {
                    accrual = accrualDtos.ToList(),
                    promo = promoDtos.ToList(),
                    debitnote = debitnoteDtos.ToList(),
                    GAP_list = GAP_listDtos.ToList(),

                };

                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<IList<object>> GetCategoryDropdownList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT id, shortDesc, longDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<object>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}
