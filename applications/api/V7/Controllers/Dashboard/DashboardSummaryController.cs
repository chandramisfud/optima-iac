using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Dashboard
{
    public partial class DashboardController : BaseController
    {
        // kpiscoring/all
        /// <summary>
        /// Dashboard summary, Old API = "kpiscoring/all"
        /// </summary>
        /// <param name="create_from"></param>
        /// <param name="create_to"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/summary/kpiscoring", Name = "dashboard_summay_all_data")]
        public async Task<IActionResult> GetKPIScoring([FromQuery] DateTime create_from, DateTime create_to)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var __val = await __repoDashboardSummary.GetKPIScoring(
                    create_from,
                    create_to);
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
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }

            return result;
        }
        // kpiscoring/approver
        /// <summary>
        /// Dashboard summary Approver, Old API = "kpiscoring/approver"
        /// </summary>
        /// <param name="promostart"></param>
        /// <param name="promoend"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/summary/kpiscoring/approver", Name = "dashboard_summay_approver")]
        public async Task<IActionResult> GetKPIScoringApprover([FromQuery] DateTime promostart, DateTime promoend)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardSummary.GetKPIScoringApprover(
                    promostart,
                    promoend
                );
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
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }

            return result;
        }
        // kpiscoring/dashboard
        /// <summary>
        /// Dashboard summary KPI Scoring Dashboard, Old API = "kpiscoring/dashboard"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dashboard/summary/kpiscoring/dashboard", Name = "dashboard_kpiscoring_dashboard")]
        public async Task<IActionResult> GetKPIScoringDashboard([FromQuery] DateTime create_from, DateTime create_to, bool period_monitoring, DateTime date_monitoring)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardSummary.GetKPIScoringDashboard(
                    create_from,
                    create_to,
                    period_monitoring,
                    date_monitoring
                );
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
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }

            return result;
        }
        // kpiscoreboard/detail/all
        /// <summary>
        /// Dashboard summary KPI Scoring Detail, Old API = "kpiscoreboard/detail/all"
        /// </summary>
        /// <param name="create_from"></param>
        /// <param name="create_to"></param>
        /// <param name="period_monitoring"></param>
        /// <param name="date_monitoring"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/summary/kpiscoring/detail", Name = "dashboard_kpiscoring_detail")]
        public async Task<IActionResult> GetKPIScoringDetail([FromQuery] DateTime create_from, DateTime create_to, bool period_monitoring, DateTime date_monitoring)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardSummary.GetKPIScoringDetail(
                    create_from,
                    create_to,
                    period_monitoring,
                    date_monitoring
                );
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
        [HttpGet("api/dashboard/summary/kpiscoring/leagues", Name = "dashboard_kpiscoring_leagues")]
        public async Task<IActionResult> GetKPIScoringLeagues([FromQuery] DateTime create_from, DateTime create_to, string ChannelDesc)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardSummary.GetKPIScoringLeagues(
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
        [HttpGet("api/dashboard/summary/kpiscoring/standing", Name = "dashboard_kpiscoring_standing")]
        public async Task<IActionResult> GetKPIScoringStanding([FromQuery] DateTime create_from, DateTime create_to)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardSummary.GetKPIScoringStanding(
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
        // kpiscoring/summaries
        /// <summary>
        /// Dashboard summary KPI Scoring Summaries, Old API = "kpiscoring/summaries"
        /// </summary>
        /// <param name="create_from"></param>
        /// <param name="create_to"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/summary/kpiscoring/summaries", Name = "dashboard_kpiscoring_summaries")]
        public async Task<IActionResult> GetKPIScoringSummaries([FromQuery] DateTime create_from, DateTime create_to)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardSummary.GetKPIScoringSummaries(
                    create_from,
                    create_to
                );
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
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
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