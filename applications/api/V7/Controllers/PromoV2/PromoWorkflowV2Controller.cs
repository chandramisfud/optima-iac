using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Promo
{

    public partial class PromoV2Controller : BaseController
    {

        /// <summary>
        /// Get Promo Workflow by refid
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        
        [HttpGet("api/promo/workflowv2", Name = "get_promo_workflowV2")]
        public async Task<IActionResult> GetPromoWorkflowV2(string refid)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoDisplayWorkflow(refid);
                if (__val != null)
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Promo Workflow by refid to generate pdf
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/workflowv2pdf/id", Name = "get_promo_workflowV2_pdf")]
        public async Task<IActionResult> GetPromoWorkflowV2pdf(int id)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoDisplayWorkflowpdf(id, __res.ProfileID);
                    if (__val != null)
                    {
                        result = Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
                } else {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
    }
}
