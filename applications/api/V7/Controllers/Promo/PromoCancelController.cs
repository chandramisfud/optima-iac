using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Promo
{
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// Get List promo cancel by pagination
        /// </summary>
        /// <param name="year"></param>
        /// <param name="entity"></param>
        /// <param name="distributor"></param>
        /// <param name="budgetparent"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/cancel/", Name = "promo_cancel_LP")]
        public async Task<IActionResult> GetPromoCancelLP([FromQuery] string year, int entity, int distributor, int budgetparent,
            int channel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var __acc = await _repoPromoCancel.GetPromoCancel(year, entity, distributor, budgetparent,
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
    }
}
