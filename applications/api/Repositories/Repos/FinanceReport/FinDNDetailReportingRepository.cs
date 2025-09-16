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
using static Dapper.SqlMapper;
using System.Security.Principal;
using System.Threading.Channels;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class FinDNDetailReportingRepository : IFinDNDetailReportingRepo
    {
        readonly IConfiguration __config;
        public FinDNDetailReportingRepository(IConfiguration config)
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

        public async Task<DNDetailReportingLandingPage> GetDNDetailReportingLandingPage(string period, int categoryId, int entityId, int distributorId, int channelId, int accountId, string profileId,
            string keyword, string sortColumn, int pageNum = 0, int dataDisplayed = 10, string sortDirection = "ASC")
        {
            DNDetailReportingLandingPage res = new();
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
                    __param.Add("@channel", channelId);
                    __param.Add("@account", accountId);
                    __param.Add("@userid", profileId);
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@filter", "");
                    __param.Add("@txtSearch", keyword);

                    var __object = await conn.QueryMultipleAsync("ip_debetnote_report_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    var data = __object.Read<DNDetailReportingData>().ToList();

                    var count = __object.ReadSingle<DNDetailReportingRecordCount>();

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

        public async Task<IList<DNDetailReportingEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<DNDetailReportingEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetDNOutStanding(string period, DataTable distributorId, string userProfile,
          int pageNumber, int pageSize, string search)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = new DynamicParameters();

                __query.Add("@period", period);
                __query.Add("@distributorId", distributorId.AsTableValuedParameter());
                __query.Add("@profile", userProfile);
                __query.Add("@start", pageNumber);
                __query.Add("@length", pageSize);
                __query.Add("@txtSearch", search);


                conn.Open();
                var __res = await conn.QueryMultipleAsync("ip_debetnote_report_outstanding", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                var _dataAll = __res.Read<object>().ToList();
                var _stat = __res.ReadSingle<BaseLPStats>();
                return new
                {
                    data = _dataAll,
                    totalCount = _stat.recordsTotal,
                    filteredCount = _stat.recordsFiltered,
                };
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetDistributorList()
        {
            using IDbConnection conn = Connection;
            try
            {

                var __query = @"select id distributorId, LongDesc distributorLongDesc
                    from tbmst_distributor
                    WHERE ISNULL(IsDeleted,0)=0 ";

                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNDetailReportingDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
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
                var child = await conn.QueryAsync<DNDetailReportingDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DNDetailReportingSubAccountList>> GetSubAccountList(string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@userid", profileId);

                conn.Open();
                var __query = @"select c.Id, c.LongDesc  from tbset_map_distributor_account a
                        inner join tbset_user_distributor b on a.DistributorId=b.DistributorId and IIF(@userid='0','',b.Userid) =IIF(@userid='0','',@userid)
                        inner join tbmst_subaccount c on a.SubAccountId=c.Id 
                        group by b.UserId,c.Id, c.LongDesc
                        order by c.LongDesc";
                var child = await conn.QueryAsync<DNDetailReportingSubAccountList>(__query, __param);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<InvestmentNotifFinReport> CekInvestmentNotifDN(InvestmentNotifBodyFinReport body)
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
