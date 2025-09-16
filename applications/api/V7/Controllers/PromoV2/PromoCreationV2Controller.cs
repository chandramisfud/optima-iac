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
        /// Show all promo creation atribut
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/creation/attributlist", Name = "promo_creation_attribut_list")]
        public async Task<IActionResult> GetPromoCreationattributlist()
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
        /// Get Promo Mechanism with available Status
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/mechanismwithstatus", Name = "promo_get_mechanism_withStatus")]
        public async Task<IActionResult> GetPromoMechanismWithStatus([FromQuery] MechanismSourceParam param)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoCreationMechanismWithStatus(param.entityId, param.subCategoryId, param.activityId, 
                    param.subActivityId, param.skuId, param.channelId, param.brandId, param.startDate!, param.endDate!);
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
        /// Get SS Value for Promo Creation
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/ssvalue", Name = "promo_get_ssValue")]
        public async Task<IActionResult> GetPromoSSValue([FromQuery] PromoCreationSSValueParam param)
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
        /// Get PS Value for Promo Creation
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/psvalue", Name = "promo_get_psValue")]
        public async Task<IActionResult> GetPromoPSValue([FromQuery] PromoCreationPSValueParam param)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoCreationPSValue(param.period, param.distributorId, 
                    param.groupBrandId, param.promoStart, param.promoEnd);
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
        /// Get Promo Baseline
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/baseline", Name = "promo_creation_get_baselinev2")]
        public async Task<IActionResult> GetPromoCreationBaseline([FromQuery] PromoCreationBaselineParam param)
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
        /// Get Promo CR
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/cr", Name = "promo_creation_get_cr")]
        public async Task<IActionResult> GetPromoCreationCR([FromQuery] PromoCreationCRParam param)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoCreationCR( param.period, param.subActivityId, param.subAccountId,
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

        /// <summary>
        /// Get Promo v2 by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creationv2/id", Name = "get_promo_creation_byid")]
        public async Task<IActionResult> GetPromoCreationById(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoCreationById(id);
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
        /// Get Promo DC v2 by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creationv2dc/id", Name = "get_promo_creation_dc_byid")]
        public async Task<IActionResult> GetPromoCreationDCById(int id)
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
        /// Promo creation V2 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/creationv2", Name = "promoV2_creation_insert")]
        public async Task<IActionResult> SetPromoV2CreationInsert([FromBody] PromoCreationInsertParam param)
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
                    var __val = await _repoPromoCreation.SetPromoCreationInsert(dtPromo, dtRegion, dtSku, 
                        dtAttachment, dtmechanism);
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
                        return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
                } else
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
        /// Promo creation V2 DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/creationv2dc", Name = "promoV2dc_creation_insert")]
        public async Task<IActionResult> SetPromoV2CreationDCInsert([FromBody] PromoCreationInsertParam param)
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
                    var __val = await _repoPromoCreation.SetPromoCreationInsertDC(dtPromo, dtRegion, dtSku,
                        dtAttachment, dtmechanism);
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
        /// Promo creation v2 update
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/promo/creationv2", Name = "promoV2_creation_update")]
        public async Task<IActionResult> SetPromoV2CreationUpdate([FromBody] PromoCreationInsertParam param)
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
        /// Promo creation v2 update DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/promo/creationv2dc", Name = "promoV2dc_creation_update")]
        public async Task<IActionResult> SetPromoV2CreationDCUpdate([FromBody] PromoCreationInsertParam param)
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
        /// Get budget for promo
        /// </summary>
        /// <param name="period"></param>
        /// <param name="categoryId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="channelId"></param>
        /// <param name="subChannelId"></param>
        /// <param name="accountId"></param>
        /// <param name="subAccountId"></param>
        /// <param name="distributorId"></param>
        /// <param name="groupBrandId"></param>
        /// <param name="subActivityTypeId"></param>
        /// <param name="activityId"></param>
        /// <param name="subActivityId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creationv2/budget", Name = "promo_creation_get_budget")]
        public async Task<IActionResult> GetPromoCreationBudget(int period, int categoryId, int subCategoryId, int channelId,
            int subChannelId, int accountId, int subAccountId, int distributorId, int groupBrandId, int subActivityTypeId,
            int activityId, int subActivityId)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoBudget(period, categoryId, subCategoryId, channelId,
            subChannelId, accountId, subAccountId, distributorId, groupBrandId, subActivityTypeId,
            activityId, subActivityId);
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
    }
}
