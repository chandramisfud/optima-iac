using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using System.Data;
using System.Drawing.Printing;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers
{
    /// <summary>
    /// Document Completeness handler
    /// </summary>
    public partial class ReportController : BaseController
    {
        /// <summary>
        /// Get filter
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/report/dn-outstanding/filter", Name = "report_dn-outstanding_filter")]
        public async Task<IActionResult> GetDNOutstandingFilter()
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNDetailReporting.GetDistributorList(__res.ProfileID);
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
        /// DN Outstanding LP
        /// </summary>
        /// <returns></returns>

        [HttpGet("api/report/dn-outstanding", Name = "get_dn_outstanding")]
        public async Task<IActionResult> GetDNOutstanding([FromQuery]DNOutStandingLPParam param)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    DataTable dtDistributor = Helper.ListIntToKeyId(param.distributorId!);
                    var __val = await __repoDNDetailReporting.GetDNOutStanding(param.period, dtDistributor, __res.ProfileID,
                                param.PageNumber, param.PageSize, param.Search);
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
