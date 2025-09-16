using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    public partial class DebitNoteController : BaseController
    {
        /// <summary>
        /// Get surat jalan dan tanda terima to HO by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/suratjalan-tandaterima-toho/id", Name = "suratjalan-tandaterima")]
        public async Task<IActionResult> GetSuratPengantarById([FromQuery] int id)
        {
            try
            {
                var __val = await __repoSJandTTtoHO.GetSuratPengantarById(id);
                if (__val != null)
                {
                    return Ok(new BaseResponse { code = 200, error = false, values = __val, message = MessageService.GetDataSuccess });
                }
                else
                {
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get surat jalan dan tanda terima to  HO semua data
        /// </summary>
        /// <param name="senddate"></param>
        /// <returns></returns>
        [HttpGet("api/dn/suratjalan-tandaterima-toho", Name = "suratjalan__tandaterima__all")]
        public ActionResult GetSuratPengantarList([FromQuery] string senddate)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = __repoSJandTTtoHO.GetSuratPengantarList(senddate, __res.ProfileID);
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
    }
}

