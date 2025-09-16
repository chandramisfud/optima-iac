using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Repositories.Entities.Report;
using Repositories.Contracts;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Repositories.Repos
{
    public class PromoHistoricalMovementRepository : IPromoHistoricalMovementRepo
    {
        readonly IConfiguration __config;
        public PromoHistoricalMovementRepository(IConfiguration config)
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

        public async Task<PromoHistoricalMovementLandingPage> GetPromoHistoricalMovementLandingPage(string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId,
            string search,int pageNum = 0, int dataDisplayed = 10)
        {
            PromoHistoricalMovementLandingPage res = null!;
            using (IDbConnection conn = Connection)
                try
                {
                    var startData = pageNum * dataDisplayed;
                    search = (search) ?? "";
                    var __param = new DynamicParameters();
                    __param.Add("@userid", profileId);
                    __param.Add("@periode", period);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@txtSearch", search);


                    var __object = await conn.QueryMultipleAsync("ip_promo_historical_movement_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);

                    res = __object.ReadSingle<PromoHistoricalMovementLandingPage>();

                    var data = __object.Read<PromoHistoricalMovementData>().ToList();

                    res.Data = data;
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
            return res;
        }

        public async Task<IList<PromoHistoricalMovementEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<PromoHistoricalMovementEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<PromoHistoricalMovementDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;

            DataTable __parent = new("ArrayIntType");
            __parent.Columns.Add("keyid");
            foreach (int v in arrayParent)
                __parent.Rows.Add(v);

            var __query = new DynamicParameters();

            __query.Add("@budgetid", budgetid);
            __query.Add("@attribute", "distributor");
            __query.Add("@parent", __parent.AsTableValuedParameter());


            conn.Open();
            var child = await conn.QueryAsync<PromoHistoricalMovementDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return child.ToList();
        }
    }
}
