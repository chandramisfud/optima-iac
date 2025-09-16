using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Model;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers.FinanceReport
{
    public partial class FinanceReportController : BaseController
    {
        /// <summary>
        /// Finance Report DN Ready to pay
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/dnreadytopay", Name = "findnreadytopay_rpt_get_lp")]
        public async Task<IActionResult> GetFinDnReadytopayLP([FromQuery] DNReadyToPayParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoFinListingDN.GetDNReadyToPayLP(param.period!, param.category, param.entity, 
                    param.distributor, param.subAccount,  param.userId, param.search!,  param.pageNumber, param.pageSize);
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
                result = StatusCode(StatusCodes.Status500InternalServerError, 
                    new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Filter for DN Ready to Pay LP
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/dnreadytopay/filter", Name = "get_findnreadytopay_rpt_filter")]
        public async Task<IActionResult> GetFinDnReadytopayFilter()
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID == null)
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

                var __val = await __repoFinListingDN.GetDNReadyToPayFilter(__res.ProfileID);
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
                result = StatusCode(StatusCodes.Status500InternalServerError,
                    new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
    }
}
