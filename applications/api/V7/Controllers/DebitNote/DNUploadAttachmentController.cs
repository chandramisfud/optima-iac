using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
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
        //   searchDNbyRefid/post
        /// <summary>
        /// Search DN by RefId, Old API = "searchDNbyRefid/post"
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        [HttpGet("api/dn/upload-attachment/search/refid", Name = "dn_upload_attachment_search_by_RefId")]
        public async Task<IActionResult> SearchDNByRefid([FromQuery] string refId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var __val = await __repoDNUploadAttachment.SearchDNByRefid(refId);

                if (__val == null)
                {
                    return Conflict(new BaseResponse
                    {
                        code = 404,
                        error = true,
                        message = MessageService.GetDataFailed,
                        values = Empty
                    });
                }

                return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
            }
            catch (System.Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }

        }
        //   dnattachment/store
        /// <summary>
        /// DN upload attachment, Old API = "dnattachment/store".
        /// Parameter CreateOn and CreateBy tidak perlu diisi
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/upload-attachment", Name = "dn_upload_attachment")]
        public async Task<IActionResult> SaveDNAttachment([FromBody] DNCreateAttachmentParam param)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    DNCreateAttachmentParam __bodyToken = new()
                    {
                        DNId = param.DNId,
                        DocLink = param.DocLink,
                        FileName = param.FileName,
                        CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        CreateBy = __res.ProfileID
                    };

                    var __val = await __repoDNUploadAttachment.SaveDNAttachment(__bodyToken);
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
        //   debetnote/listattach
        /// <summary>
        /// Get List DN Attachment, Old API = "debetnote/listattach"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/upload-attachment/attachment", Name = "dn_upload_attachment_listattachment")]
        public async Task<IActionResult> GetDNListAttachment([FromQuery] DNGetListAttachmentParam param)
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
                    var __val = await __repoDNUploadAttachment.GetDNListAttachment(param.period!, param.distributor, __res.ProfileID!, param.isdnmanual);
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
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

    }
}