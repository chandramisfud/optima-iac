using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Dashboard
{
    public partial class DashboardController : BaseController
    {
        // dashboard/calendar
        /// <summary>
        /// Get Dashboard Promo Calendar, Old API = "dashboard/calendar"
        /// </summary>
        /// <param name="promoPlanId"></param>
        /// <param name="activity_desc"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/promocalendar", Name = "dashboard_promocalendar")]
        public async Task<IActionResult> GetDashboarPromodCalendar([FromQuery] int promoPlanId = 0, string activity_desc = "")
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
                    var __val = await __repoDashboardPromoCalendar.GetDashboarPromodCalendar(__res.ProfileID!, promoPlanId, activity_desc);
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
                        return Ok(new BaseResponse { error = true, code = 204, message = MessageService.DataNotFound, values = null });
                    }
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
        // select Entity
        /// <summary>
        /// Get entity dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dashboard/promocalendar/entity", Name = "dashboard_promo_calendar_entity")]
        public async Task<IActionResult> GetEntityForDashboardPromoCalendar()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDashboardPromoCalendar.GetEntityForDashboardPromoCalendar();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
        // select Channel
        /// <summary>
        /// Get List channel, Old API = "channel/byaccess/"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dashboard/promocalendar/channel", Name = "dashboard_promocalendar_channel")]
        public async Task<IActionResult> GetDashboardMasterChannelbyAccesses()
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
                    var __val = await __repoDashboardPromoCalendar.GetDashboardMasterChannelbyAccesses(__res.ProfileID!);
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
                        return Ok(new BaseResponse { error = true, code = 204, message = MessageService.DataNotFound, values = null });
                    }
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
        // select Account
        /// <summary>
        /// Get List channel, Old API = "account/byaccess/"
        /// </summary>
        /// <param name="channelid"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/promocalendar/account", Name = "dashboard_promocalendar_account")]
        public async Task<IActionResult> GetDashboardMasterAccountbyAccesses([FromQuery] int channelid)
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
                    var __val = await __repoDashboardPromoCalendar.GetDashboardMasterAccountbyAccesses(__res.ProfileID!, channelid);
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
                        return Ok(new BaseResponse { error = true, code = 204, message = MessageService.DataNotFound, values = null });
                    }
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
        // subcategory/getbyuseraccess   
        /// <summary>
        /// Get List Sub Category, Old API = "subcategory/getbyuseraccess"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dashboard/promocalendar/subcategory", Name = "dashboard_promocalendar_subcategory")]
        public async Task<IActionResult> GetSubCategoryforDashboardPromoCalendars()
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
                    var __val = await __repoDashboardPromoCalendar.GetSubCategoryforDashboardPromoCalendars(__res.ProfileID!);
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
                        return Ok(new BaseResponse { error = true, code = 204, message = MessageService.DataNotFound, values = null });
                    }
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

        //get all categody
        /// <summary>
        /// Get category dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dashboard/promocalendar/category", Name = "dashboard_getdropdown_category")]
        public async Task<IActionResult> GetCategoryListforDashboardPromoCalendar()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDashboardPromoCalendar.GetCategoryListforDashboardPromoCalendar();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
    }
}
