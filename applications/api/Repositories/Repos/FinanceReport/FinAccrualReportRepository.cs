using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Report;

namespace Repositories.Repos
{
    public class FinAccrualReportRepository : IFinAccrualReportRepo
    {
        readonly IConfiguration __config;
        public FinAccrualReportRepository(IConfiguration config)
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

        public async Task<AccrualReportLandingPage> GetAccrualReportLandingPage(int download, string period, int entityId, int distributorId,
            int budgetParentId, int channelId, int grpBrandId, string profileId, string closingDate,
            string search, string sortColumn, int pageNum = 0, int dataDisplayed = 10, string sortDirection = "ASC")
        {
            AccrualReportLandingPage res = null!;
            using (IDbConnection conn = Connection)
                try
                {
                    var __param = new DynamicParameters();
                    __param.Add("@download", download);
                    __param.Add("@userid", profileId);
                    __param.Add("@periode", period);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@closingdt", closingDate);
                    __param.Add("@start", pageNum);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@order", sortColumn);
                    __param.Add("@sort", sortDirection);
                    __param.Add("@txtSearch", search);
                    __param.Add("@grpBrandId", grpBrandId);



                    var __object = await conn.QueryMultipleAsync("ip_promo_accrual_report_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);

                    res = __object.ReadSingle<AccrualReportLandingPage>();

                    var data = __object.Read<object>().ToList();

                    res.Data = data;
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
            return res;
        }

        public async Task<IList<AccrualEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<AccrualEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetGroupBrandList(int entity)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT id grpBrandId, longdesc grBrandDesc 
                    FROM tbmst_brand_group
                    WHERE PrincipalId=@pEntity AND ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<object>(__query, new { pEntity = entity });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<AccrualReportHeaderList>> GetPromoAccrualReportHeader(AccrualReportHeaderBody body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@periode", body.periode);
                __param.Add("@entity", body.entity);
                __param.Add("@closingdt", body.closingdt);

                var result = await conn.QueryAsync<AccrualReportHeaderList>("ip_promo_accrual_report_hdr", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<IList<AccrualReportList>> GetPromoAccrualReportById(int id)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __param = new DynamicParameters();
                __param.Add("@id", id);
                var __res = await conn.QueryAsync<AccrualReportList>("ip_promo_accrual_report_byId", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return __res.ToList();
            }
            catch (System.Exception __ex)
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
    }
}