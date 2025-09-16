using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Model;
using V7.Services;

namespace V7.Controllers.Promo
{

    public partial class PromoV2Controller : BaseController
    {

        /// <summary>
        /// Get Promo cancel request LP
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/cancelrequest/list", Name = "get_promo_cancelrequest_LP")]
        public async Task<IActionResult> GetPromoCancelRequestLP([FromQuery]string period, int entity, int distributor, 
            int budgetParent, int channel)
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
                var __val = await _repoPromoCreation.GetPromoCancelRequestLP(period, entity, distributor,
                    budgetParent,  channel, __res.ProfileID);
              
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
