using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    public partial class DebitNoteController : BaseController
    {
        /// <summary>
        /// Get surat jalan dan tanda terima to Danone by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/suratjalan-tandaterima-todanone/id", Name = "suratjalan-tandaterima_todanone")]
        public async Task<IActionResult> GetSuratPengantarHOtoDanonebyId([FromQuery] int id)
        {
            IActionResult result;
            try
            {
                var __val = await __repoSJandTTtoDanone.GetSuratPengantarHOtoDanonebyId(id);
                if (__val == null)
                {
                    return NotFound(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get surat jalan dan tanda terima to Danone semua data
        /// </summary>
        /// <param name="senddate"></param>
        /// <returns></returns>
        [HttpGet("api/dn/suratjalan-tandaterima-todanone", Name = "suratjalan__tandaterima__all_todanone")]
        public ActionResult GetSuratPengantarHOtoDanoneList([FromQuery] string senddate)
        {
            try
            {

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID == null)
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
                var __val = __repoSJandTTtoDanone.GetSuratPengantarHOtoDanoneList(senddate, __res.ProfileID);
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.DataNotFound, values = __val });
                }

            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
    }
}

