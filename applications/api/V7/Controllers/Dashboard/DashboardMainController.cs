using Microsoft.AspNetCore.Mvc;
//using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model;
using V7.Model.Dashboard;
using V7.Services;

namespace V7.Controllers.Dashboard
{
    public partial class DashboardController : BaseController
    {
        // api/dashboard/main/{periode}/{entity}/{subcategory}/{userid}/{viewmode}
        /// <summary>
        /// Dashboard main, Old API = "api/dashboard/main"
        /// </summary>
        /// <param name="period"></param>
        /// <param name="entityId"></param>
        /// <param name="channelId"></param>
        /// <param name="accountId"></param>
        /// <param name="categoryId"></param>
        /// <param name="subcategoryId"></param>
        /// <param name="viewmode"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/main", Name = "dashboard_main")]
        public async Task<IActionResult> GetDashboardMain([FromQuery] DateTime period,
        int entityId, [FromQuery] int[] channelId, [FromQuery] int[] accountId, [FromQuery] int[] categoryId,
        int subcategoryId, string viewmode
        )
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
                    var __val = await __repoDashboardMain.GetDashboardMain(
                        period,
                        entityId,
                        channelId,
                        accountId,
                        categoryId,
                        subcategoryId,
                        __res.ProfileID!,
                        viewmode
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
        // channel/byaccess/
        /// <summary>
        /// Get List channel, Old API = "channel/byaccess/"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dashboard/main/channel", Name = "dashboard_main_channel")]
        public async Task<IActionResult> GetAllChannelByAccessforDashboardMain()
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
                    var __val = await __repoDashboardMain.GetAllChannelByAccess(__res.ProfileID!);
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
        // account/byaccess/
        /// <summary>
        /// Get List channel, Old API = "account/byaccess/"
        /// </summary>
        /// <param name="channelid"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/main/account", Name = "dashboard_main_account")]
        public async Task<IActionResult> GetAllAccountByAccessDashboardMain([FromQuery] int channelid)
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
                    var __val = await __repoDashboardMain.GetAllAccountByAccess(__res.ProfileID!, channelid);
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
        // users/dropdown
        /// <summary>
        /// Get Dropdown List, Old API = "users/dropdown"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dashboard/main/dropdown", Name = "dashboard_main_dropdown")]
        public async Task<IActionResult> GetDropDownList()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardMain.GetDropDownList();
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
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });

            }
        }
        // dashboard/notifications
        /// <summary>
        /// Dashboard Notifications, Old API = "dashboard/notifications"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dashboard/main/notifications", Name = "dashboard_main_notifications")]
        public async Task<IActionResult> GetUserNotifications()
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDashboardMain.GetNotification(__res.ProfileID);
                    return Ok(new
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 204, message = MessageService.DataNotFound, values = null });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
        // dashboard/dnmonitoring
        /// <summary>
        /// Dashboard DN monitoring, Old API = "dashboard/dnmonitoring"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/main/dnmonitoring", Name = "dashboard_dnmonitoring")]
        public async Task<IActionResult> GetDNMonitoring([FromQuery] DashboardMainDNMonitoringParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDashboardMain.GetDNMonitoring(
                        param.period,
                        param.entityId,
                        param.channel!,
                        param.account!,
                        param.paymentstatus!,
                        __res.ProfileID,
                        param.distibutorId,
                        param.dnpromo
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
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }
        // api/dashboard/trend
        /// <summary>
        /// Dashboard Promo Trend, Old API = "api/dashboard/trend"
        /// </summary>
        /// <param name="period"></param>
        /// <param name="entityId"></param>
        /// <param name="channelId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/main/trend", Name = "dashboard_main_trend")]
        public async Task<IActionResult> GetDashboardTrend([FromQuery] DateTime period,
            int entityId,
            [FromQuery] int[] channelId,
            [FromQuery] int[] accountId
        )
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoDashboardMain.GetDashboardTrend(
                        period,
                        entityId,
                        __res.ProfileID,
                        channelId,
                        accountId
                        );

                    return Ok(new
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
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }

        // dashboard/spend
        /// <summary>
        /// Dashboard Promo Spend, Old API = "dashboard/spend"
        /// </summary>
        /// <param name="periode"></param>
        /// <param name="entity"></param>
        /// <param name="channel"></param>
        /// <param name="account"></param>
        /// <param name="subcategory"></param>
        /// <param name="viewmode"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/main/spend", Name = "dashboard_spend")]
        public async Task<IActionResult> GetSpendPerformance([FromQuery] DateTime periode, int entity, string channel, string account, int subcategory, string viewmode)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var spend = await __repoDashboardMain.GetBudgetUsage(periode, entity, channel, account, subcategory, __res.ProfileID, viewmode);

                    var chart = new object[3];
                    chart[0] = new object[] { spend.SubCategory!, "Budget Usage" };
                    chart[1] = new object[] { "Promo Created", spend.promo_created_pct };
                    chart[2] = new object[] { "Promo Not Created Yet", spend.promo_not_created_yet_pct };
                    return Ok(new
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = new { chart, spend.budget_deployed, spend.budget_spending, spend.promo_created, spend.dn_claim, spend.dn_paid, spend.ontime_promo, spend.promo_approved }
                    });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Ok(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }

        }
        // dashboard/promosubmission/
        /// <summary>
        /// Dashboard Promo Submission, Old API = "dashboard/promosubmission/"
        /// </summary>
        /// <param name="periode"></param>
        /// <param name="viewmode"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/main/promosubmission", Name = "dashboard_promosubmission")]
        public async Task<IActionResult> GetSpendPerformance([FromQuery] DateTime periode, string viewmode)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardMain.GetOnTimePromoSubmission(periode, viewmode);

                return Ok(new
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });

            }
            catch (Exception __ex)
            {
                return Ok(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }

        }
        // dashboard/promoapproval
        /// <summary>
        /// Dashboard Promo Approval, Old API = "dashboard/promoapproval"
        /// </summary>
        /// <param name="periode"></param>
        /// <param name="viewmode"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/main/promoapproval", Name = "dashboard_promoapproval")]
        public async Task<IActionResult> GetPromoApprovalPerformance([FromQuery] DateTime periode, string viewmode)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardMain.GetOnTimePromoApproval(periode, viewmode);

                return Ok(new
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return Ok(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// This will return refID from debet note and promo
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="keyword">Key to search</param>
        /// <returns></returns>
        [HttpPost("api/dashboard/main/searchdesktop", Name = "search_desktop")]
        public async Task<IActionResult> SearchDesktop([FromForm] string keyword)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var res = await __repoDashboardMain.Search(keyword, __res.ProfileID);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.GetDataSuccess, values = res });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Ok(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Dashboard DN OverBudget tobe Settled, Old API = "debetnote/overbudget/tobesettled"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dashboard/main/dn/overbudget/tobesettled", Name = "dashboard_main_dn_overbudget_tobesettled")]
        public async Task<IActionResult> GetDNOverBudgetToBeSettled([FromQuery]LPPagingParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoDashboardMain.GetDNOverBudgetToBeSettled(__res.ProfileID,
                        param.sortColumn, param.sortDirection, param.pageSize, param.pageNumber, param.search);
                    return Ok(new
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new Model.BaseResponse { error = true, code = 200, message = MessageService.DataNotFound, values = null });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        // debetnote/dnlistbypromoid
        /// <summary>
        /// Dashboard Main  DN OverBudget List by PromoID, Old API = "debetnote/dnlistbypromoid"
        /// </summary>
        /// <param name="promoId"></param>
        /// <returns></returns>
        [HttpGet("api/dashboard/main/dn/overbudget/tobesettled/promoid", Name = "dashboard_dn_overbudget_tobesettled_promoid")]
        public async Task<IActionResult> GetDNListbyPromoId([FromQuery] int promoId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDashboardMain.GetDNListbyPromoId(promoId);

                return Ok(new
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return Ok(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }


    }
}