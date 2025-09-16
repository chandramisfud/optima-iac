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
        /// Get Promo Sendback v2 by id for approval
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendbackv2/id", Name = "get_promo_sendbackv2")]
        public async Task<IActionResult> GetPromoSendbackV2byId(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoDisplayById(id);
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
        /// Get Promo DC Sendback v2 by id for approval
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendbackv2dc/id", Name = "get_promo_sendbackv2_dc_by_id")]
        public async Task<IActionResult> GetPromoSendbackV2DCbyId(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoCreationDCById(id);
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
        /// Promo Sendback v2
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/promo/sendbackv2", Name = "promoV2_sendback_update")]
        public async Task<IActionResult> SetPromoV2Sendback([FromBody] PromoCreationInsertParam param)
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
                    dtPromo.Rows[0]["createdEmail"] = __res.UserEmail;
                    dtPromo.Rows[0]["createBy"] = __res.ProfileID;
                    dtPromo.Rows[0]["createOn"] = DateTime.Now;

                    DataTable dtAttachment = Helper.ConvertParamToDataTable(new DBPromoAttachmentType(), param.attachment);
                    DataTable dtmechanism = Helper.ConvertParamToDataTable(new DBPromoMechanismType(), param.mechanism);
                    DataTable dtRegion = Helper.ArrayIntToKeyId(param.region);
                    DataTable dtSku = Helper.ArrayIntToKeyId(param.sku);
                    var __val = await _repoPromoCreation.SetPromoCreationUpdate(dtPromo, dtRegion, dtSku,
                        dtAttachment, dtmechanism);
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
        /// Promo senvback v2 update DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/promo/sendbackv2dc", Name = "promoV2dc_sendback_update_dc")]
        public async Task<IActionResult> SetPromoV2SendbackDCUpdate([FromBody] PromoCreationInsertParam param)
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
                    var __val = await _repoPromoCreation.SetPromoCreationUpdateDC(dtPromo, dtRegion, dtSku,
                        dtAttachment, dtmechanism);
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
        /// Show all promo sendback atribut
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/sendbackv2/attributlist", Name = "promo_sendbackv2_attribut_list")]
        public async Task<IActionResult> GetPromoSendbackAttributlist()
        {

            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationAttributeList(__res.ProfileID);
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
                        result = Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.GetDataFailed
                        });

                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = Conflict(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }

            return result;
        }

        /// <summary>
        /// Get Promo sendback Mechanism with available Status
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendbackv2/mechanismwithstatus", Name = "promo_sendback_get_mechanism_withStatus")]
        public async Task<IActionResult> GetPromoSendBackMechanismWithStatus([FromQuery] MechanismSourceParam param)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoCreationMechanismWithStatus(
                    param.entityId, param.subCategoryId, 
                    param.activityId, param.subActivityId, param.skuId, param.channelId, param.brandId,
                    param.startDate!, param.endDate!);
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
        /// Get SS Value for Promo sendback
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendbackv2/ssvalue", Name = "promo_sendback_get_ssValue")]
        public async Task<IActionResult> GetPromosendbackSSValue([FromQuery] PromoCreationSSValueParam param)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoCreationSSValue(param.period, param.channelId, param.subChannelId,
                    param.accountId, param.subAccountId, param.groupBrandId, param.promoStart, param.promoEnd);
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
        /// Get Promo sendback Baseline
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendbackv2/baseline", Name = "promo_sendback_get_baselinev2")]
        public async Task<IActionResult> GetPromosendbackBaseline([FromQuery] PromoCreationBaselineParam param)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoCreationBaseline(param.promoId, param.period, param.date, param.pType,
                    param.distributor, param.region, param.channel, param.subChannel, param.account, param.subAccount,
                    param.product, param.subCategory, param.subActivity, param.grpBrand, param.promoStart, param.promoEnd);
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
        /// Get Promo sendback CR
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendbackv2/cr", Name = "promo_sendback_get_cr")]
        public async Task<IActionResult> GetPromosendbackCR([FromQuery] PromoCreationCRParam param)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoCreationCR(param.period, param.subActivityId, param.subAccountId,
                    param.distributorId, param.grpBrandId);
                if (__val != null)
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.SaveSuccess
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.SaveFailed });
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
