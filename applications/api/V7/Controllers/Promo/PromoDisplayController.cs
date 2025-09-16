using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Model.Promo;
using V7.Services;

namespace V7.Controllers.Promo
{
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// Promo Display with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/display", Name = "promo_display_LP")]
        public async Task<IActionResult> GetPromoDisplayLP([FromQuery] PromoDisplayLandingPageParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    var result = await _repoPromoDisplay.GetPromoDisplayLP(
                        param.year!, 
                        param.entity, 
                        param.distributor,
                        param.budgetParent, 
                        param.channel, 
                        __res.ProfileID!, 
                        false, 
                        param.PageNumber, 
                        param.PageSize,
                        "", 
                        param.Search!
                    );

                    if (result != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = result,
                            message = MessagingServices.MessageService.GetDataSuccess
                        });

                    }
                    else
                    {
                        return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch   (Exception __ex)
            {
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Get Promo Display by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/display/id", Name = "promo_display_byid")]
        public async Task<IActionResult> GetPromoDisplayPrimaryById(int id)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    if (!ModelState.IsValid) return BadRequest(ModelState);
                    var __acc = await _repoPromoDisplay.GetPromoDisplayById(id, __res.ProfileID);
                    if (__acc != null)
                    {
                        return Ok(new Model.BaseResponse { error = false, code = 200, values = __acc, message = MessageService.GetDataSuccess });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Ok(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get Promo Display by id for Enail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/promo/display/email/id", Name = "promo_display_byid_email")]
        public async Task<IActionResult> GetPromoDisplaybyId(int id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __acc = await _repoPromoDisplay.GetPromoDisplaybyId(id);
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
                return Ok(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
    }
}
