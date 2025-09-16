using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Dashboard
{
    public partial class DashboardController : BaseController
    {
        // GetAllChannel
        /// <summary>
        /// Get List Channel
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dashboard/approver/channel", Name = "dashboard_approver_channel")]
        public async Task<IActionResult> GetChannelListDashboardApprover()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                List<int> arrayParent = new();
                var __val = await __repoDashboardApprover.GetChannelList(__res.ProfileID!, arrayParent.ToArray());
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        // kpiscoring/approver
        /// <summary>
        /// Dashboard Kpi Scoring Approver, Old API = "kpiscoring/approver"
        /// </summary>
        /// <param name="promostart"></param>
        /// <param name="promoend"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/approver/kpiscoring/approver", Name = "dashboard_kpiscoring_approver")]
        public async Task<IActionResult> GetKPIScoringApproverforDashboardApprover([FromQuery] DateTime promostart, DateTime promoend)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    var __val = await __repoDashboardApprover.GetKPIScoringApprover(
                        __res.ProfileID!,
                        promostart,
                        promoend
                    );
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
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }

            return result;
        }
        // kpiscoring/standing
        /// <summary>
        /// Dashboard summary KPI Scoring Standing, Old API = "kpiscoring/standing"
        /// </summary>
        /// <param name="create_from"></param>
        /// <param name="create_to"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/approver/kpiscoring/standing", Name = "dashboard_kpiscoring_standing_approver")]
        public async Task<IActionResult> GetKPIScoringStandingforDashboardApprover([FromQuery] DateTime create_from, DateTime create_to)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardApprover.GetKPIScoringStanding(
                    create_from,
                    create_to
                );
                if (__val != null)
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });

                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound, values = null });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }

            return result;
        }
    }

}