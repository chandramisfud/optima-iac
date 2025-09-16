using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Dtos;

namespace Repositories.Repos
{
    public class ToolsBlitzRepository : IToolsBlitzRepository
    {
        readonly IConfiguration __config;
        public ToolsBlitzRepository(IConfiguration config)
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
        public async Task<IList<BlitzNotif>> BlitzTranferNotif()
        {
            List<BlitzNotif> __blitzNotif = new();
            using IDbConnection conn = Connection;
            try
            {
                var __query = new DynamicParameters();
                using (var __re = await conn.QueryMultipleAsync("[dbo].[ip_blitz_transfer_notif_c]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180))
                {
                    BlitzNotif __result = new()
                    {
                        itemtype = new List<ItemType>()
                    };
                    foreach (ItemType r in __re.Read<ItemType>())
                        __result.itemtype.Add(new ItemType()
                        {
                            type = r.type,
                            code = r.code,
                            desc = r.desc
                        });
                    __result.blitzemail = new List<BlitzEmail>();
                    foreach (BlitzEmail r in __re.Read<BlitzEmail>())
                        __result.blitzemail.Add(new BlitzEmail()
                        {
                            email_to = r.email_to,
                            email_cc = r.email_cc,
                            email_subject = r.email_subject
                        });

                    __blitzNotif.Add(__result);
                }
                return __blitzNotif.AsList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaselineRaw>> GetBaselineRaws(string refid, int promoplan)
        {
            List<BaselineRaw> __baseline = new();
            using IDbConnection conn = Connection;
            try
            {
                var __query = new DynamicParameters();
                __query.Add("@refid", refid);
                __query.Add("@promoplan", promoplan);
                using (var __re = await conn.QueryMultipleAsync("[dbo].[ip_getbaseline_raw]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180))
                {
                    BaselineRaw __result = new()
                    {
                        PromoPlanBaseline = new List<PromoPlanBaseline>()
                    };
                    foreach (PromoPlanBaseline r in __re.Read<PromoPlanBaseline>())
                        __result.PromoPlanBaseline.Add(new PromoPlanBaseline()
                        {
                            f1 = r.f1,
                            f2 = r.f2,
                            f3 = r.f3,
                            f4 = r.f4,
                            f5 = r.f5,
                            f6 = r.f6,
                            f7 = r.f7,
                            f8 = r.f8,
                            f9 = r.f9
                        });
                    __result.BaselineCalculation = new List<BaselineCalculation>();
                    foreach (BaselineCalculation r in __re.Read<BaselineCalculation>())
                        __result.BaselineCalculation.Add(new BaselineCalculation()
                        {
                            ym = r.ym,
                            sales = r.sales,
                            anomaly_pct = r.anomaly_pct,
                            bsold = r.bsold,
                            abnormal_pct = r.abnormal_pct,
                            sales_abnormal = r.sales_abnormal,
                            sales_new = r.sales_new
                        });
                    __result.BaselineRawBSCalculation = new List<BaselineRawBSCalculation>();
                    foreach (BaselineRawBSCalculation r in __re.Read<BaselineRawBSCalculation>())
                        __result.BaselineRawBSCalculation.Add(new BaselineRawBSCalculation()
                        {
                            bs = r.bs,
                            bsold = r.bsold,
                            bsnew = r.bsnew
                        });
                    __result.BaselineRawResult = new List<BaselineRawResult>();
                    foreach (BaselineRawResult r in __re.Read<BaselineRawResult>())
                        __result.BaselineRawResult.Add(new BaselineRawResult()
                        {
                            baseline_sales = r.baseline_sales,
                            actual_sales = r.actual_sales
                        });
                    __result.RawActualSales = new List<RawActualSales>();
                    foreach (RawActualSales r in __re.Read<RawActualSales>())
                        __result.RawActualSales.Add(new RawActualSales()
                        {
                            Distributor_Id = r.Distributor_Id,
                            Distributor_Name = r.Distributor_Name,
                            Region_Code = r.Region_Code,
                            Region_Desc = r.Region_Desc,
                            Year = r.Year,
                            Month_Name = r.Month_Name,
                            AccountCode = r.AccountCode,
                            AccountDesc = r.AccountDesc,
                            SubAccountCode = r.SubAccountCode,
                            SubAccountDesc = r.SubAccountDesc,
                            Product_Code = r.Product_Code,
                            Product_Name = r.Product_Name,
                            Qty_In_Ton = r.Qty_In_Ton,
                            Qty_In_Car = r.Qty_In_Car,
                            SS_DBP = r.SS_DBP,
                            SS_RBP = r.SS_RBP,
                            SOURCE = r.SOURCE,
                            month_int = r.month_int,
                            created_at = r.created_at,
                            updated_at = r.updated_at,
                            ym = r.ym
                        });
                    __result.RawBaseLine = __re.Read<RawActualSales>().ToList();
                    __baseline.Add(__result);
                }
                return __baseline.AsList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}