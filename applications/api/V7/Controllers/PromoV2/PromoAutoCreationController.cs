using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Model;
using V7.Model.Promo;
using V7.Services;

namespace V7.Controllers.Promo
{

    public partial class PromoV2Controller : BaseController
    {

        /// <summary>
        /// Promo Auto Generation
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/autocreation", Name = "set_promo_autocreation")]
        public async Task<IActionResult> SetPromoAutoCreation([FromBody] PromoAutoCreation param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID == null)
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

                var __val = await _repoPromoCreation.SetPromoAutoCreation(param.period, param.category, param.distributor,
                    param.brand, param.channel, param.subAccount, param.subActivity, __res.ProfileID, __res.UserEmail);

                result = Ok(new Model.BaseResponse
                {
                    error = false,
                    code = 200,
                    //values = __val,
                    message = __val.refId
                });

            }
            catch (Exception __ex)
            {
                result = Ok(new Model.BaseResponse
                {
                    error = false,
                    code = 200,
                 
                    message = __ex.Message
                });
             
            }
            return result;
        }

        [HttpGet("api/promo/autocreation/attributelist", Name = "get_promo_autocreation_attribute")]
        public async Task<IActionResult> GetPromoAutoCreationAttribute()
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID == null)
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

                var __val = await _repoPromoCreation.GetPromoAutoCreationAtribute( __res.ProfileID);

                result = Ok(new Model.BaseResponse
                {
                    error = false,
                    code = 200,
                    values = __val,
                    message = MessageService.GetDataSuccess
                });

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError,
                    new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
    }
}
