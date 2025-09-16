using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using V7.MessagingServices;
using V7.Model.Promo;
using V7.Services;

namespace V7.Controllers.Promo
{

    public partial class PromoV2Controller : BaseController
    {

        /// <summary>
        /// Get Promo Display by id
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/displayv2/id", Name = "get_promo_displayV2")]
        public async Task<IActionResult> GetPromoDisplayV2byId(int id)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {

                    var __val = await _repoPromoCreation.GetPromoDisplayById(id, __res.ProfileID);
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
                        result = NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
                } else
                {
                    result = NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Promo Display by id for email
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/promo/displayv2email/id", Name = "get_promo_displayV2email")]
        public async Task<IActionResult> GetPromoDisplayV2EmailbyId(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoDisplayEmailById(id);
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
        /// Get Promo Display by id for generating pdf
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/displayv2pdf/id", Name = "get_promo_displayV2pdf")]
        public async Task<IActionResult> GetPromoDisplayV2PdfbyId(int id)
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
                }
                else {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Promo Display by id for generating budget email
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/displayv2budgetemail/id", Name = "get_promo_displayV2budgetemail")]
        public async Task<IActionResult> GetPromoDisplayBudgetEmailbyId(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoDisplayById(id);
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
        /// Get Promo Display by id for generating promo creation email
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/displayv2creationemail/id", Name = "get_promo_displayV2Creationemail")]
        public async Task<IActionResult> GetPromoCreationDisplayEmailbyId(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoDisplayById(id);
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
        /// Get Promo Display by id for generating promo approval email
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/displayv2approvalemail/id", Name = "get_promo_displayV2approvalEmail")]
        public async Task<IActionResult> GetPromoApprovalDisplayEmailbyId(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoDisplayById(id);
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
        /// Get Promo Display by id for generating promo sendback email
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/displayv2sendbackemail/id", Name = "get_promo_displayV2SendbackEmail")]
        public async Task<IActionResult> GetPromoSendbackDisplayEmailbyId(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoDisplayById(id);
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
        /// Get Promo Search Display by id for generating pdf
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/displayv2searchpdf/id", Name = "get_promo_displayV2SearchPdf")]
        public async Task<IActionResult> GetPromoDisplaySearchPdfbyId(int id)
        {
            IActionResult result;
            try
            {

                var __val = await _repoPromoCreation.GetPromoDisplayById(id);
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

    }
}
