using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    /// <summary>
    /// DebitNote Controller
    /// </summary>
    public partial class DebitNoteController : BaseController
    {
        // Select Entity
        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/multiprint-promo/entity", Name = "dn_multiprint-promo_entity_list")]
        public async Task<IActionResult> GetEntityDNMultiprintPromo()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNMultiPrintPromo.GetEntityList();
                if (__val != null)
                {
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
        // promov3/getrecon
        /// <summary>
        /// Get Promo Reconciliation by Id, Old API = "promov3/getrecon"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/multiprint-promo/recon", Name = "dn_multiprint-promo_recon_get_id")]
        public async Task<IActionResult> GetPromoReconDNMultiprintPromo([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDNMultiPrintPromo.GetPromoReconPromoDisplay(id);
                if (__val != null)
                {
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
        // promo/multiprint/
        /// <summary>
        /// DN list Promo Multiprint, old API = " promo/multiprint/"
        /// </summary>
        /// <param name="period"></param>
        /// <param name="entityId"></param>
        /// <param name="distributorId"></param>
        /// <param name="BudgetParent"></param>
        /// <param name="channelId"></param>
        /// <param name="cancelstatus"></param>
        /// <returns></returns>
        [HttpGet("api/dn/multiprint-promo", Name = "dn_multiprint-promo_list")]
        public async Task<IActionResult> GetPromoMultiprint([FromQuery] string period, int entityId, int distributorId, int BudgetParent, int channelId, bool cancelstatus)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNMultiPrintPromo.GetPromoMultiprint(
                        period,
                        entityId,
                        distributorId,
                        BudgetParent,
                        channelId,
                        __res.ProfileID,
                        cancelstatus
                    );
                    if (__val.Count != 0 && __val != null)
                    {
                        result = Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            message = MessageService.GetDataSuccess,
                            values = __val
                        });
                    }
                    else
                    {
                        return Ok(new BaseResponse { error = false, code = 200, message = MessageService.DataNotFound, values = __val });
                    }
                }
                else
                {
                    return result = StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, new BaseResponse
                    {
                        error = true,
                        code = 500,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (Exception __ex)
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
        // promo/bystatus/
        /// <summary>
        /// Promo by Status for DN Multiprint Promo, old API = "promo/bystatus/"
        /// </summary>
        /// <param name="period"></param>
        /// <param name="entityId"></param>
        /// <param name="distributorId"></param>
        /// <param name="BudgetParent"></param>
        /// <param name="channelId"></param>
        /// <param name="cancelstatus"></param>
        /// <returns></returns>
        [HttpGet("api/dn/multiprint-promo/status", Name = "dn_multiprint-promo_status")]
        public async Task<IActionResult> GetPromoByConditionsByStatus([FromQuery] string period, int entityId, int distributorId, int BudgetParent, int channelId, bool cancelstatus)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNMultiPrintPromo.GetPromoByConditionsByStatus(
                        period,
                        entityId,
                        distributorId,
                        BudgetParent,
                        channelId,
                        __res.ProfileID,
                        cancelstatus
                    );
                    if (__val.Count != 0 && __val != null)
                    {
                        result = Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            message = MessageService.GetDataSuccess,
                            values = __val
                        });
                    }
                    else
                    {
                        return Ok(new BaseResponse { error = false, code = 200, message = MessageService.DataNotFound, values = __val });
                    }
                }
                else
                {
                    return result = StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, new BaseResponse
                    {
                        error = true,
                        code = 500,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (Exception __ex)
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
        // user/getbyprimarykey/
        /// <summary>
        /// Get Data User Profile by Id, Old API = "user/getbyprimarykey/"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/multiprint-promo/userprofile/id", Name = "dn_multiprint-promo_get_userprofile_byprimarykey")]
        public IActionResult GetUserProfileByIdforDNMultiprintPromo(string id)
        {
            try
            {
                var __res = __repoDNMultiPrintPromo.GetById(id);

                if (__res == null)
                {
                    return UnprocessableEntity(
                        new BaseResponse
                        {
                            error = true,
                            code = 204,
                            message = MessageService.DataNotFound
                        }
                    );
                }
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    values = __res.Result,
                    message = MessageService.GetDataSuccess
                });
            }
            catch (System.Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
    }
}