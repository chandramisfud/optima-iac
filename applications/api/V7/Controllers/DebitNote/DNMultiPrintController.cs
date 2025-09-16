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
        // Select SubAccount
        /// <summary>
        /// Get sub account list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/multiprint/subaccount", Name = "dn_multiprint_subaccount")]
        public async Task<IActionResult> GetSubAccountList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMultiPrintDN.GetSubAccountList();
                if (__val != null && __val.Count > 0)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new BaseResponse
                    {
                        code = 404,
                        error = true,
                        message = MessageService.GetDataFailed,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        // user/getbyprimarykey
        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("api/dn/multiprint/user", Name = "dn_multiprint_get_usersByID")]
        public async Task<IActionResult> GetUserByIDMultiPrint([FromQuery] int userid)
        {
            try
            {
                var __val = await __repoMultiPrintDN.GetUserById(userid);
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
        // debetnote/list
        /// <summary>
        /// DN list multiprint, old API = "debetnote/list"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/dn/multiprint/", Name = "dn_multiprint_landingpage")]
        public async Task<IActionResult> GetDNListLandingPageMultiPrint([FromQuery] DNLandingPageParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoMultiPrintDN.GetDNListLandingPage(
                        param.Period!,
                        param.EntityId,
                        param.DistributorId,
                        param.ChannelId,
                        param.AccountId,
                        __res.ProfileID,
                        param.IsDNManual,
                        param.Search!,
                        param.SortColumn.ToString(),
                        param.PageNumber,
                        param.PageSize,
                        param.SortDirection.ToString()
                    );
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
                    return NotFound(new BaseResponse{ error = true, code = 404, message = MessageService.EmailTokenFailed });
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
        // promo/getPromoForDn/
        /// <summary>
        /// DN list multiprint approved promo for DN, old API = "promo/getPromoForDn"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/dn/multiprint/approvedpromo-for-dn", Name = "dn_multiprint_approved_promo_for_dn")]
        public async Task<IActionResult> GetApprovedPromoforDN([FromQuery] DNApprovedPromoforMultiprintDNParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoMultiPrintDN.GetApprovedPromoforDN(
                        param.period!,
                        param.entity,
                        param.channel,
                        param.account,
                        __res.ProfileID
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
                        return Ok(new BaseResponse{ error = false, code = 200, message = MessageService.DataNotFound, values = __val });
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
        // debetnote/print/
        /// <summary>
        /// DN multiprint, old API = "debetnote/print/"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/multiprint/print", Name = "dn_multiprint")]
        public async Task<IActionResult> DNMultiPrint([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMultiPrintDN.DNPrint(id);
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
    }
}