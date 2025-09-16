using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.Promo;
using V7.Services;

namespace V7.Controllers.Promo
{
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// Get List promo cancel request by pagination
        /// </summary>
        /// <param name="year"></param>
        /// <param name="entity"></param>
        /// <param name="distributor"></param>
        /// <param name="budgetparent"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/cancel/request", Name = "promo_cancel_request_LP")]
        public async Task<IActionResult> GetPromoCancelRequestLP([FromQuery] string year, int entity, int distributor, int budgetparent,
            int channel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var __acc = await _repoPromoCancel.GetPromoCancelRequestLP(year, entity, distributor, budgetparent,
                channel, "0");
            if (__acc != null)
            {
                return Ok(new Model.BaseResponse
                {
                    error = false,
                    code = 200,
                    values = __acc,
                    message = MessageService.GetDataSuccess
                });
            }
            else
            {
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 404,
                    message = MessagingServices.MessageService.GetDataFailed
                });
            }
        }

        [HttpGet("api/promo/cancel/request/id", Name = "promo_cancel_request_byId")]
        public async Task<IActionResult> GetPromoCancelRequestById(int id)
        {

            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __acc = await _repoPromoDisplay.GetPromoDisplayById(id);
                if (__acc != null)
                {
                    return Ok(new Model.BaseResponse { error = false, code = 200, values = __acc, message = MessageService.GetDataSuccess });
                }
                else
                {
                    return Ok(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Set Promo Cancel Request approve
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/cancel/request/approve", Name = "promo_cancel_request_approve")]
        public async Task<IActionResult> PromoCancelRequestApprove([FromBody] PromoCancelRequestParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    if (!ModelState.IsValid) return BadRequest(ModelState);
                    var x = await _repoPromoCancel.SetPromoCancelRequest(param.promoId, param.planningId, "TP2", param.notes,
                        __res.ProfileID, __res.UserEmail!);

                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = !string.IsNullOrEmpty(x.message),
                        message = string.IsNullOrEmpty(x.message) ? MessageService.SaveSuccess : x.message,
                        values = new
                        {
                            PromoId = x.Id,
                            RefId = x.RefId
                        }
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message,
                });
            }
        }

        /// <summary>
        /// Set Promo Cancel Request sendback
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/cancel/request/sendback", Name = "promo_cancel_request_sendback")]
        public async Task<IActionResult> PromoCancelRequestSendback([FromBody] PromoCancelRequestParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var x = await _repoPromoCancel.SetPromoCancelRequest(param.promoId, param.planningId, "TP3", param.notes,
                        __res.ProfileID, __res.UserEmail!);

                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = !string.IsNullOrEmpty(x.message),
                        message = string.IsNullOrEmpty(x.message) ? MessageService.SaveSuccess : x.message,
                        values = new
                        {
                            PromoId = x.Id,
                            RefId = x.RefId
                        }
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message
                });
            }
        }
    }
}
