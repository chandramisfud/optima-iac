using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers
{
    /// <summary>
    /// Promo Planniong Reporting handler
    /// </summary>
    /// 
    public partial class ReportController : BaseController
    {
        /// <summary>
        /// Get Promo Planning Reporting with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/report/promoplanningreporting", Name = "promoplanningreporting_rpt_get_lp")]
        public async Task<IActionResult> GetPromoPlanningReportingLandingPage([FromQuery] PromoPlanningReportingRequestParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoPromoPlanningReporting.GetPromoPlanningReportingLandingPage(param.Period!, param.EntityId, param.DistributorId, param.ChannelId, __res.ProfileID,
                        param.CreateFrom!, param.CreateTo!, param.StartFrom!, param.StartTo!,
                        param.Search!, param.SortColumn.ToString(), param.SortDirection.ToString(), param.PageNumber, param.PageSize);
                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
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

        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/report/promoplanningreporting/entity", Name = "promoplanningreporting_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityPromoPlanningReporting()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoPromoPlanningReporting.GetEntityList();
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

        /// <summary>
        /// Get List Distributor
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/report/promoplanningreporting/distributor", Name = "promoplanningreporting_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributorPromoPlanningReporting([FromQuery] PromoPlanningReportingDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoPromoPlanningReporting.GetDistributorList(param.budgetId, param.entityId!);
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

        /// <summary>
        /// Get List Channel
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/report/promoplanningreporting/channel", Name = "promoplanningreporting_rpt_get_channel_list")]
        public async Task<IActionResult> GetChannelPromoPlanningReporting()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                List<int> arrayParent = new();

                if (__res.ProfileID != null)
                {
                    var __val = await __repoPromoPlanningReporting.GetChannelList(__res.ProfileID, arrayParent.ToArray());
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
    }
}
