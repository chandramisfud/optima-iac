using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
//using Repositories.Entities.Models;
using Repositories.Entities.Models.Promo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos.Promo
{
    public class PromoClosureRepository : IPromoClosureRepository
    {
        readonly IConfiguration __config;
        public PromoClosureRepository(IConfiguration config)
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
        public async Task<BaseLP2> GetPromoClosureLP(int entity, int distributor, int channel, DateTime start_from, DateTime start_to, int remaining_budget, string userid, int start, int length, string filter, string txtsearch)
        {
            BaseLP2 res = new();
            List<PromoCloseListDtoPagination> __promolistclose_p = new();
            using (IDbConnection conn = Connection)
            {
                var __param = new DynamicParameters();
                __param.Add("@entity", entity);
                __param.Add("@distributor", distributor);
                __param.Add("@remaining_budget", remaining_budget);
                __param.Add("@channel", channel);
                __param.Add("@userid", userid);
                __param.Add("@start_from", start_from);
                __param.Add("@start_to", start_to);
                __param.Add("@start", start);
                __param.Add("@length", length);
                __param.Add("@filter", filter);
                __param.Add("@txtsearch", txtsearch);


                using var __re = await conn.QueryMultipleAsync("ip_promo_list_close_p", __param, commandTimeout: 180, commandType: CommandType.StoredProcedure);
                PromoCloseListDtoPagination __result = new()
                {
                    Data = new List<PromoCloseListDto>()
                };
                __result.Data = __re.Read<PromoCloseListDto>().ToList();

                //foreach (PromoCloseListDto i in __re.Read<PromoCloseListDto>())
                //    __result.Data.Add(new PromoCloseListDto()
                //    {
                //        PromoId = i.PromoId,
                //        PromoNumber = i.PromoNumber,
                //        Entity = i.Entity,
                //        Distributor = i.Distributor,
                //        Initiator = i.Initiator,
                //        CreateOn = i.CreateOn,
                //        StartPromo = i.StartPromo,
                //        EndPromo = i.EndPromo,
                //        ChannelDesc = i.ChannelDesc,
                //        SubChannelDesc = i.SubChannelDesc,
                //        AccountDesc = i.AccountDesc,
                //        SubAccountDesc = i.SubAccountDesc,
                //        BrandDesc = i.BrandDesc,
                //        SubCategory = i.SubCategory,
                //        ActivityDesc = i.ActivityDesc,
                //        Mechanisme1 = i.Mechanisme1,
                //        Mechanisme2 = i.Mechanisme2,
                //        Mechanisme3 = i.Mechanisme3,
                //        Mechanisme4 = i.Mechanisme4,
                //        PromoStatus = i.PromoStatus,
                //        ReconStatus = i.ReconStatus,
                //        Investment = i.Investment,
                //        DNPaid = i.DNPaid,
                //        DNClaim = i.DNClaim,
                //        aging = i.aging,
                //        LastDNCreationDate = i.LastDNCreationDate,
                //        RemainingInvestment_DN = i.RemainingInvestment_DN,
                //        ClosureStatus = i.ClosureStatus,
                //        CloseBy = i.CloseBy,
                //        CloseOn = i.CloseOn
                //    });
                __result.RecordsTotal = __re.Read<Entities.Models.RecordTotal>().FirstOrDefault()!;

                __promolistclose_p.Add(__result);
                res.recordsTotal = __result.RecordsTotal!.recordsTotal;
                res.recordsFiltered = __result.RecordsTotal.recordsFiltered;

                res.Data = __result.Data.Cast<object>().ToList();

            }
            return res;
        }

        public async Task<List<PromoImportResponse>> ImportPromoClosure(DataTable data, string userid, string useremail)
        {
            try
            {
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var result = await conn.QueryAsync<PromoImportResponse>("ip_import_promo_closure",
                     new
                     {
                         promo = data.AsTableValuedParameter("CharAttributeType "),
                         userid = userid,
                         useremail = useremail
                     },
                commandTimeout: 180, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ResponseMultipleDocDto>> ClosingPromo(int[] lsPromoId, string userId, string userEmail)
        {
            using IDbConnection conn = Connection;

            DataTable __pr = Helper._castToDataTable(new DetailPromoToClose(), null!);

            foreach (int v in lsPromoId)
                __pr.Rows.Add(v);

            var __param = new DynamicParameters();
            __param.Add("@promoid", __pr.AsTableValuedParameter("ArrayintType"));
            __param.Add("@userid", userId);
            __param.Add("@useremail", userEmail);
            var result = await conn.QueryAsync<ResponseMultipleDocDto>("ip_promo_closure", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return result.ToList();
        }
        public async Task ReOpenPromo(int[] lsPromoId, string userId, string userEmail)
        {
            using IDbConnection conn = Connection;

            DataTable __pr = Helper._castToDataTable(new DetailPromoToClose(), null!);

            foreach (int v in lsPromoId)
                __pr.Rows.Add(v);

            var __param = new DynamicParameters();
            __param.Add("@promoid", __pr.AsTableValuedParameter("ArrayintType"));
            __param.Add("@userid", userId);
            __param.Add("@useremail", userEmail);

            var res = await conn.QueryAsync<string>("ip_promo_closure_open", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            //return res.FirstOrDefault().ToString();
        }
    }
}
