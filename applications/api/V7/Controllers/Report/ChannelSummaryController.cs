using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Models;
using System.Data;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers
{
    /// <summary>
    /// Accrual Report handler
    /// </summary>
    public partial class ReportController : BaseController
    {
        /// <summary>
        /// Get Channel Summary with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/report/channelsummary", Name = "accrual_rpt_get_channelsummary_lp")]
        public async Task<IActionResult> GetchannelsummaryLandingPage([FromQuery] TTControlRCDCParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                DataTable dtChannel = Helper.ListIntToKeyId(param.channelId!);
                DataTable dtBrand = Helper.ListIntToKeyId(param.groupBrandId!);
                DataTable dtCategory = Helper.ListIntToKeyId(param.categoryId!);
                DataTable dtSubActivity = Helper.ListIntToKeyId(param.subActivityTypeId!);
                // get token from request header
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID == null)
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
                else
                {
                    var __val = await __repoAccrualReport.GetChannelSummary(param.period,
                            dtCategory, dtBrand, dtChannel, dtSubActivity, __res.ProfileID,
                            param.pageNumber, param.pageSize, param.filter!, param.search!);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get Channel Summary Download with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/report/channelsummary/download", Name = "accrual_rpt_get_channelsummary")]
        public async Task<IActionResult> GetchannelsummaryDownload([FromQuery] TTControlRCDCParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                DataTable dtChannel = Helper.ListIntToKeyId(param.channelId!);
                DataTable dtBrand = Helper.ListIntToKeyId(param.groupBrandId!);
                DataTable dtCategory = Helper.ListIntToKeyId(param.categoryId!);
                DataTable dtSubActivity = Helper.ListIntToKeyId(param.subActivityTypeId!);
                // get token from request header
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID == null)
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
                else
                {
                    var __val = await __repoAccrualReport.GetChannelSummaryDownload(param.period,
                            dtCategory, dtBrand, dtChannel, dtSubActivity, __res.ProfileID,
                            param.pageNumber, param.pageSize, param.filter!, param.search!);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Report TT Control RC DC Filter
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        [HttpGet("api/report/channelsummary/filter", Name = "rpt_channelSumamry_filter")]
        public async Task<IActionResult> GetTTControlFilter()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var __val = await __repoAccrualReport.GetTTControlRCDCFilter();
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });

            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
    }
}