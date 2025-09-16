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
        /// Get Promo v2 by id for approval
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/approvalv2/id", Name = "get_promo_approvalv2")]
        public async Task<IActionResult> GetPromoApporovalV2byId(int id)
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
        /// Get Promo Recon v2 by id for approval
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/approvalv2recon/id", Name = "get_promo_approvalv2_recon")]
        public async Task<IActionResult> GetPromoApporovalV2ReconbyId(int id)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoReconDisplayById(id, __res.ProfileID);
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
        /// Get Promo DC v2 by id for approval
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/approvalv2dc/id", Name = "get_promo_approvalv2dc")]
        public async Task<IActionResult> GetPromoApporovalV2DCbyId(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoDisplayDCById(id);
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
        /// Get Promo Recon DC v2 by id for approval
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/approvalv2dcrecon/id", Name = "get_promo_approvalv2dc_recon")]
        public async Task<IActionResult> GetPromoApporovalV2DCReconbyId(int id)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoReconDisplayDCById(id, __res.ProfileID);
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
