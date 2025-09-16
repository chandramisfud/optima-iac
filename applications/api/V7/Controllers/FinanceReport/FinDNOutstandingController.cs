using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using System.Data;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.FinanceReport;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers.FinanceReport
{
    /// <summary>
    /// DN Detail Reporting handler
    /// </summary>
    public partial class FinanceReportController : BaseController
    {
        /// <summary>
        /// Get filter
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/dn-outstanding/filter", Name = "report_fin_dn-outstanding_filter")]
        public async Task<IActionResult> GetFinDNOutstandingFilter()
        {
            IActionResult result;
            try
            {
                var __val = await __repoFinDNDetailReporting.GetDistributorList();
                result = Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    values = __val,
                    message = MessageService.GetDataSuccess
                });
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

        [HttpGet("api/finance-report/dn-outstanding", Name = "get_fin_dn_outstanding")]
        public async Task<IActionResult> GetFinDNOutstanding([FromQuery] DNOutStandingLPParam param)
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
                    var __val = await __repoFinDNDetailReporting.GetDNOutStanding(param.period, dtDistributor, __res.ProfileID,
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
