using Microsoft.AspNetCore.Mvc;
using System.Data;
using V7.MessagingServices;
using V7.Model.Promo;
using V7.Services;

namespace V7.Controllers.Promo
{

    public partial class PromoV2Controller : BaseController
    {

        /// <summary>
        /// Get Promo Recon v2 by id
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/reconv2/id", Name = "get_promo_reconv2")]
        public async Task<IActionResult> GetPromoReconV2byId(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoRecon.GetPromoReconById(id);
                if (__val != null)
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Promo Recon DC v2 by id
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/reconv2dc/id", Name = "get_promo_reconv2dc")]
        public async Task<IActionResult> GetPromoReconV2DCbyId(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoRecon.GetPromoReconDCById(id);
                if (__val != null)
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Promo Recon v2 update
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/promo/reconv2", Name = "promoV2_recon_update")]
        public async Task<IActionResult> SetPromoV2ReconUpdate([FromBody] PromoV2ReconUpdateParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    List<DBPromoFormType> data = new List<DBPromoFormType>();
                    data.Add(param.promo);

                    DataTable dtPromo = Helper.ConvertParamToDataTable(new DBPromoFormType(), data);
                    //add rest column to match DB PromoFormType
                    //                    dtPromo.Columns.Add("createdEmail");
                    dtPromo.Rows[0]["createdEmail"] = __res.UserEmail;
                    //                  dtPromo.Columns.Add("createBy");
                    dtPromo.Rows[0]["createBy"] = __res.ProfileID;
                    //dtPromo.Columns.Add("createOn");
                    dtPromo.Rows[0]["createOn"] = DateTime.Now;

                    DataTable dtAttachment = Helper.ConvertParamToDataTable(new DBPromoAttachmentType(), param.attachment);
                    DataTable dtmechanism = Helper.ConvertParamToDataTable(new DBPromoMechanismType(), param.mechanism);
                    DataTable dtRegion = Helper.ArrayIntToKeyId(param.region);
                    DataTable dtSku = Helper.ArrayIntToKeyId(param.sku);
                    var __val = await _repoPromoRecon.SetPromoReconUpdate(dtPromo, dtRegion, dtSku,
                        dtAttachment, dtmechanism,
                        param.calculatorRecon.baselineCalcRecon, param.calculatorRecon.upliftCalcRecon,
                        param.calculatorRecon.totalSalesCalcRecon, param.calculatorRecon.salesContributionCalcRecon,
                        param.calculatorRecon.storesCoverageCalcRecon, param.calculatorRecon.redemptionRateCalcRecon,
                        param.calculatorRecon.crCalcRecon, param.calculatorRecon.roiCalcRecon, param.calculatorRecon.costCalcRecon);

                    if (__val != null)
                    {
                        result = Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.UpdateSuccess
                        });
                    }
                    else
                    {
                        return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Promo Recon DC v2 update
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/promo/reconv2dc", Name = "promoV2dc_recon_update")]
        public async Task<IActionResult> SetPromoDCV2ReconUpdate([FromBody] PromoCreationInsertParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    List<DBPromoFormType> data = new List<DBPromoFormType>();
                    data.Add(param.promo);

                    DataTable dtPromo = Helper.ConvertParamToDataTable(new DBPromoFormType(), data);
                    //add rest column to match DB PromoFormType
                    //                    dtPromo.Columns.Add("createdEmail");
                    dtPromo.Rows[0]["createdEmail"] = __res.UserEmail;
                    //                  dtPromo.Columns.Add("createBy");
                    dtPromo.Rows[0]["createBy"] = __res.ProfileID;
                    //dtPromo.Columns.Add("createOn");
                    dtPromo.Rows[0]["createOn"] = DateTime.Now;

                    DataTable dtAttachment = Helper.ConvertParamToDataTable(new DBPromoAttachmentType(), param.attachment);
                    DataTable dtmechanism = Helper.ConvertParamToDataTable(new DBPromoMechanismType(), param.mechanism);
                    DataTable dtRegion = Helper.ArrayIntToKeyId(param.region);
                    DataTable dtSku = Helper.ArrayIntToKeyId(param.sku);
                    var __val = await _repoPromoRecon.SetPromoReconDCUpdate(dtPromo, dtRegion, dtSku, dtAttachment, dtmechanism);
                    if (__val != null)
                    {
                        result = Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.UpdateSuccess
                        });
                    }
                    else
                    {
                        return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
    }
}
