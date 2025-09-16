using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DNConfirmPaid;
using V7.MessagingServices;
using V7.Model.DebitNote;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    /// <summary>
    /// DebitNote Controller
    /// </summary>
    public partial class DebitNoteController : BaseController
    {
        // debetnote/confirmpaid/ [GET]
        /// <summary>
        /// Get DN status "invoice", Old API = "debetnote/confirmpaid/ [GET]"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/confirm-paid/invoice", Name = "dn_confirm-paid-invoice")]
        public async Task<IActionResult> GetDNStatusConfirmPaid([FromQuery] DNGetStatusParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                // get token from request header
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    var __val = await __repoConfirmPaid.GetDNStatusConfirmPaid("invoice", __res.ProfileID!, param.entityid, param.distributorid);
                    result = Ok(
                        new BaseResponse
                        {
                            error = false,
                            code = 200,
                            message = MessageService.GetDataSuccess,
                            values = __val
                        });
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                }
            }
            catch (System.Exception __ex)
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
        //debetnote/confirm_paid [POST]
        /// <summary>
        /// DN change status to "Confirm Paid"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/confirm-paid/changestatus/confirm-paid", Name = "dn_changestatus_confirm_paid_changestatus")]
        public async Task<IActionResult> DNConfirmPaid([FromBody] DNChangeStatusConfirmPaid param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    DNChangeStatusConfirmPaid __bodyToken = new()
                    {
                        DNId = new List<DNIdArray>(),
                        UserId = __res.ProfileID,
                        status = "confirm_paid",
                        paymentDate = param.paymentDate
                    };

                    foreach (var item in param.DNId!)
                    {
                        __bodyToken.DNId.Add(new DNIdArray
                        {
                            DNId = item.DNId
                        });
                    }

                    var __val = await __repoConfirmPaid.DNConfirmPaid(__bodyToken);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess });
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