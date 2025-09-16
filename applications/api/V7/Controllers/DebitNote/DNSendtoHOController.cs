
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;
using V7.MessagingServices;
using V7.Model.DebitNote;
using V7.Services;

namespace V7.Controllers.DebitNote
{

    public partial class DebitNoteController : BaseController
    {
        /// <summary>
        /// Get DN send to HO
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/send-to-ho", Name = "debetnote_send_to_ho")]
        public async Task<IActionResult> GetDNSendtoHO([FromQuery] DebitNoteSendtoParam param)
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
                    var __val = await __repoSendtoHO.GetDNSendtoHO("created", __res.ProfileID!, param.entityid, param.distributorid);
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
        /// <summary>
        /// Remove promo attachment
        /// </summary>
        /// <returns></returns>
        [HttpDelete("api/dn/send-to-ho/promoattachment", Name = "dn_send_to_ho_promo_attachment_remove")]
        public async Task<IActionResult> DeletePromoAttachmentForSendtoHO([FromQuery] int PromoId, string DocLink)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                await __repoSendtoHO.DeletePromoAttachmentForSendtoHO(PromoId, DocLink);
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    values = null,
                    message = MessageService.DeleteSucceed
                });
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }


        /// <summary>
        /// DN change status from distributor to HO
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/send-to-ho/changestatus/fromcabang-to-ho", Name = "fromdistributor-to-ho")]
        public async Task<IActionResult> DNChangeStatusDistributortoHO([FromBody] DNChangeSingleStatusParam param)
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
                    await __repoSendtoHO.DNChangeStatusDistributortoHO("send_to_dist_ho", __res.ProfileID!, param.dnId);
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

        [HttpPost("api/dn/send-to-ho/changestatus/fromcabang-to-ho/generate-sj", Name = "fromdistributor-to-ho-genSJ")]
        public async Task<IActionResult> DNGenerateSJtoHO([FromBody] DNChangeStatusParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    DNChangeStatusParam __bodyToken = new()
                    {
                        dnid = new List<DNId>()
                    };
                    foreach (var item in param.dnid!)
                    {
                        __bodyToken.dnid.Add(new DNId
                        {
                            dnid = item.dnid
                        });
                    }

                    var __val = await __repoSendtoHO.DNGenerateSuratJalantoHO(__res.ProfileID!, __bodyToken.dnid);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess, 
                    values = __val});
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