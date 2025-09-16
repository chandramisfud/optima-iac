using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Promo
{
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// List promo to print LP
        /// </summary>
        /// <param name="year"></param>
        /// <param name="entity"></param>
        /// <param name="distributor"></param>
        /// <param name="budgetparent"></param>
        /// <param name="channel"></param>
        /// <param name="cancelstatus"></param>
        /// <returns></returns>
        [HttpGet("api/promo/multiprint", Name = "promo_multiprint_lp")]
        public async Task<IActionResult> GetAllPromoByStatus(string year, int entity, int distributor, int budgetparent, 
            int channel, bool cancelstatus)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoDisplay.GetPromoListByStatus(
                        year,
                        entity,
                        distributor,
                        budgetparent,
                        channel,
                        __res.ProfileID,
                        cancelstatus
                    );
                    if (__val != null)
                    {
                        result = Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            message = MessageService.GetDataSuccess,
                            values = __val
                        });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse { error = false, code = 200, message = MessageService.DataNotFound});
                    }
                }
                else
                {
                    return result = Ok(new Model.BaseResponse
                    {
                        error = true,
                        code = 500,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (Exception __ex)
            {
                result = Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }
    }
}
