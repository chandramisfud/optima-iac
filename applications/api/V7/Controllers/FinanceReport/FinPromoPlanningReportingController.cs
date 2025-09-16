using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.FinanceReport;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers.FinanceReport
{
    /// <summary>
    /// Promo Planniong Reporting handler
    /// </summary>
    /// 
    public partial class FinanceReportController : BaseController
    {
        /// <summary>
        /// Get Promo Planning Reporting with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/promoplanningreporting", Name = "promoplanningreporting_fnc_rpt_get_lp")]
        public async Task<IActionResult> GetPromoPlanningReportingLandingPage([FromQuery] FinPromoPlanningReportingRequestParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinPromoPlanningReport.GetPromoPlanningReportingLandingPage(param.Period!, param.EntityId, param.DistributorId, param.ChannelId, param.profileId!,
                    param.CreateFrom!, param.CreateTo!, param.StartFrom!, param.StartTo!,
                    param.Search!, param.SortColumn.ToString(), param.SortDirection.ToString(), param.PageNumber, param.PageSize);
                if (__val != null)
                {
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
        [HttpGet("api/finance-report/promoplanningreporting/entity", Name = "promoplanningreporting_fnc_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityPromoPlanningReporting()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinPromoPlanningReport.GetEntityList();
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

        /// <summary>
        /// Get List Distributor
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/promoplanningreporting/distributor", Name = "promoplanningreporting_fnc_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributorPromoPlanningReporting([FromQuery] FinPromoPlanningReportingDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoFinPromoPlanningReport.GetDistributorList(param.budgetId, param.entityId!);
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

        /// <summary>
        /// Get List Channel
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/promoplanningreporting/channel", Name = "promoplanningreporting_fnc_rpt_get_channel_list")]
        public async Task<IActionResult> GetChannelPromoPlanningReporting([FromQuery] ChannelPromoPlanningReportingParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                List<int> arrayParent = new();
                var __val = await __repoFinPromoPlanningReport.GetChannelList(param.userid!, arrayParent.ToArray());
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
    }
}
