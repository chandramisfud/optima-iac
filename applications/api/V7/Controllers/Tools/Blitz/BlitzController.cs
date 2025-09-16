using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;

namespace V7.Controllers.Tools
{
    public partial class ToolsController : BaseController
    {
        /// <summary>
        /// Blitz Notification
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/tools/blitznotif/", Name = "blitznotif")]
        public async Task<IActionResult> BlitzTranferNotif()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __blitzRepo.BlitzTranferNotif();
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        [HttpGet("api/tools/baseline", Name = "tools_baseline")]
        public async Task<IActionResult> GetBaselineRaws([FromQuery] string refid, int promoplan)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __blitzRepo.GetBaselineRaws(refid, promoplan);
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
    }
}