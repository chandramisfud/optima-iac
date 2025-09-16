using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities;
using Repositories.Entities.BudgetAllocation;
using Repositories.Entities.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Security.Claims;

namespace Repositories.Repos
{
    public  class PromoReconV2Repository : IPromoReconV2Repository
    { 
        readonly IConfiguration __config;
        public PromoReconV2Repository(IConfiguration config)
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

        public async Task<object> GetPromoReconById(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data_recon]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var _promo = __resp.Read<object>().First();
                    var _region = __resp.Read<object>().ToList();
                    var _sku = __resp.Read<object>().ToList();
                    var _attachment = __resp.Read<object>().ToList();
                    var _promoStatus = __resp.Read<object>().ToList();
                    var _mechanism = __resp.Read<object>().ToList();
                    var _prevMechanism = __resp.Read<object>().ToList();
                    var _editableConfig = __resp.ReadSingleOrDefault<object>();
                    var _calculatorRecon = __resp.Read<object>().ToList();
                    var _DNTotal = __resp.ReadSingleOrDefault<dynamic>();
                    var _DNClaim = __resp.Read<object>().ToList();
                    var _DNPaid = __resp.Read<object>().ToList();

                    var result = new
                    {
                        promo = _promo,
                        region = _region,
                        sku = _sku,
                        attachment = _attachment,
                        promoStatus = _promoStatus,
                        mechanism = _mechanism,
                        prevMechanism = _prevMechanism,
                        editableConfig = _editableConfig,
                        calculatorRecon = _calculatorRecon,
                        dnClaim = new
                        {
                            total = _DNTotal.totalClaim,
                            listDN = _DNClaim
                        },
                        dnPaid = new
                        {
                            total = _DNTotal.totalPaid,
                            listDN = _DNPaid
                        }


                    };

                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetPromoReconDCById(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@id", id);

                    var __resp = await conn.QueryMultipleAsync("[dbo].[ip_promo_data_recon_DC]", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var _promo = __resp.Read<object>().First();
                    var _region = __resp.Read<object>().ToList();
                    var _sku = __resp.Read<object>().ToList();
                    var _attachment = __resp.Read<object>().ToList();
                    var _promoStatus = __resp.Read<object>().ToList();
                    var _mechanism = __resp.Read<object>().ToList();

                    var _DNTotal = __resp.ReadSingleOrDefault<dynamic>();
                    var _DNClaim = __resp.Read<object>().ToList();
                    var _DNPaid = __resp.Read<object>().ToList();

                    var result = new
                    {
                        promo = _promo,
                        region = _region,
                        sku = _sku,
                        attachment = _attachment,
                        promoStatus = _promoStatus,
                        mechanism = _mechanism,
                        dnClaim = new
                        {
                            total = _DNTotal.totalClaim,
                            listDN = _DNClaim
                        },
                        dnPaid = new
                        {
                            total = _DNTotal.totalPaid,
                            listDN = _DNPaid
                        }

                    };

                    return result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> SetPromoReconUpdate(DataTable promo, DataTable region, DataTable sku,
            DataTable attachment, DataTable mechanism,

            decimal baselineCalcRecon, decimal upliftCalcRecon,
            decimal totalSalesCalcRecon, decimal salesContributionCalcRecon,
            decimal storesCoverageCalcRecon, decimal redemptionRateCalcRecon,
            decimal crCalcRecon, decimal roiCalcRecon, decimal costCalcRecon)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@promo", promo.AsTableValuedParameter());
                    __param.Add("@region", region.AsTableValuedParameter());
                    __param.Add("@sku", sku.AsTableValuedParameter());
                    __param.Add("@attachment", attachment.AsTableValuedParameter());
                    __param.Add("@mechanism", mechanism.AsTableValuedParameter());

                    __param.Add("@baselineCalcRecon", baselineCalcRecon);
                    __param.Add("@upliftCalcRecon", upliftCalcRecon);
                    __param.Add("@totalSalesCalcRecon", totalSalesCalcRecon);
                    __param.Add("@salesContributionCalcRecon", salesContributionCalcRecon);
                    __param.Add("@storesCoverageCalcRecon", storesCoverageCalcRecon);
                    __param.Add("@redemptionRateCalcRecon", redemptionRateCalcRecon);
                    __param.Add("@crCalcRecon", crCalcRecon);
                    __param.Add("@roiCalcRecon", roiCalcRecon);
                    __param.Add("@costCalcRecon", costCalcRecon);

                    var _qryRes = await conn.QueryMultipleAsync("ip_promo_update_recon", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    promoCreationResult _res = _qryRes.Read<promoCreationResult>().First();
                    if (_res.isSendEmail)
                    {
                        _res.dataEmail = _qryRes.Read<object>().First();
                    }
                    return _res;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> SetPromoReconDCUpdate(DataTable promo, DataTable region, DataTable sku,
           DataTable attachment, DataTable mechanism)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __param = new DynamicParameters();
                    __param.Add("@promo", promo.AsTableValuedParameter());
                    __param.Add("@region", region.AsTableValuedParameter());
                    __param.Add("@sku", sku.AsTableValuedParameter());
                    __param.Add("@attachment", attachment.AsTableValuedParameter());
                    __param.Add("@mechanism", mechanism.AsTableValuedParameter());

                    var _qryRes = await conn.QueryMultipleAsync("ip_promo_update_recon_DC", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    promoCreationResult _res = _qryRes.Read<promoCreationResult>().First();
                    if (_res.isSendEmail)
                    {
                        _res.dataEmail = _qryRes.Read<object>().First();
                    }
                    return _res;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}