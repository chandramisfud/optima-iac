using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Dashboard
{
    public partial class DashboardController : BaseController
    {
        // kpiscoring/dashboard
        /// <summary>
        /// Dashboard summary KPI Scoring Dashboard, Old API = "kpiscoring/dashboard"
        /// </summary>
        /// <param name="create_from"></param>
        /// <param name="create_to"></param>
        /// <param name="userid"></param>
        /// <param name="period_monitoring"></param>
        /// <param name="date_monitoring"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/creator/kpiscoring/dashboard", Name = "dashboard_kpiscoring_creator_dashboard")]
        public async Task<IActionResult> GetKPIScoringDashboardforDashboardCreator([FromQuery] DateTime create_from, DateTime create_to, string userid, bool period_monitoring, DateTime date_monitoring)
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
                    var __val = await __repoDashboardCreator.GetKPIScoringDashboard(
                        create_from,
                        create_to,
                        __res.ProfileID!,
                        period_monitoring,
                        date_monitoring
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
        // kpiscoring/league
        /// <summary>
        /// Dashboard summary KPI Scoring Leagues, Old API = "kpiscoring/league"
        /// </summary>
        /// <param name="create_from"></param>
        /// <param name="create_to"></param>
        /// <param name="ChannelDesc"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/creator/kpiscoring/leagues", Name = "dashboard_kpiscoring_leagues_creator")]
        public async Task<IActionResult> GetKPIScoringLeaguesforDashboardCreator([FromQuery] DateTime create_from, DateTime create_to, string ChannelDesc)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardCreator.GetKPIScoringLeagues(
                    create_from,
                    create_to,
                    ChannelDesc
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
        // kpiscoring/standing
        /// <summary>
        /// Dashboard summary KPI Scoring Standing, Old API = "kpiscoring/standing"
        /// </summary>
        /// <param name="create_from"></param>
        /// <param name="create_to"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/creator/kpiscoring/standing", Name = "dashboard_kpiscoring_standing_creator")]
        public async Task<IActionResult> GetKPIScoringStandingforDashboardCreator([FromQuery] DateTime create_from, DateTime create_to)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardCreator.GetKPIScoringStanding(
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