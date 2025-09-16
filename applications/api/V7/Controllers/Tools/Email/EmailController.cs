using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using V7.MessagingServices;

namespace V7.Controllers.Tools
{
    public partial class ToolsController : BaseController
    {

        [AllowAnonymous]
        [HttpGet("api/tools/xva/resend_email_approval", Name = "xva_resend_email_approval")]
        public async Task<IActionResult> ReSendEmailApproval()
        {
            try
            {

                var __res = await __emailRepo.resendEmailApproval();

                return Ok(new { error = false, code = 200, values = __res, Message = MessageService.GetDataSuccess });
            }
            catch (Exception __ex)
            {
                return Conflict(new { error = true, code = 400, message = __ex.Message });
            }
        }


            /// <summary>
            /// Sending Email NoAuth
            /// </summary>
            /// <param name="param"></param>
            /// <returns></returns>
            [AllowAnonymous]
        [HttpPost("api/tools/email/", Name = "email_storebody")]
        public async Task<IActionResult> SendEmail([FromForm] Model.EmailParam param)
        {
            try
            {
                EmailBody emailBodyDto = new()
                {
                    attachment = param.attachment!,
                    body = param.body!,
                    email = param.email!,
                    subject = param.subject!,

                    cc = param.cc!,
                    bcc = param.bcc!
                };

                await __emailRepo.SendEmail(emailBodyDto);

                return Ok(new { error = false, code = 200, Message = "Email send successfully" });
            }
            catch (Exception __ex)
            {
                return BadRequest(new { error = true, code = 400, message = __ex.Message });
            }
        }

        [HttpGet("api/tools/email/getconfig", Name = "email_getconfig")]
        public async Task<IActionResult> GetEmailConfig([FromQuery] EmailBodyReq body)
        {
            IActionResult result;
            try
            {
                var __res = await __emailRepo.GetEmailConfig(body);

                if (__res != null && __res.Count > 0)
                {
                    result = Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __res });
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                }

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
    }
}
