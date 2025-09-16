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
        // promov3/display/distributor
        /// <summary>
        /// Get Listing Promo Display with pagination, Old API = "promov3/display/distributor"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/dn/promodisplay", Name = "dn_promo_display_landingpage")]
        public async Task<IActionResult> GetFinPromoDisplayLandingPage([FromQuery] DNPromoDisplayRequestParam param)
        {
            IActionResult result;
            try
            {

                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __resDist = await __repoDNPromoDisplay.GetDistributorId(__res.ProfileID);
                    if (__resDist != null)
                    {
                        var __val = await __repoDNPromoDisplay.GetDNPromoDisplayLandingPage(
                            param.Period!,
                            param.EntityId,
                            __resDist.DistributorId,
                            param.BudgetParentId,
                            param.ChannelId,
                            __res.ProfileID,
                            param.CancelStatus,
                            param.Search!,
                            param.SortColumn.ToString(),
                            param.SortDirection.ToString(),
                            param.PageNumber,
                            param.PageSize);
                        if (__val != null)
                        {
                            result = Ok(new BaseResponse
                            {
                                error = false,
                                code = 200,
                                values = __val,
                                message = MessageService.GetDataSuccess
                            });
                        }
                        else
                        {
                            return Ok(new BaseResponse { error = false, code = 200, message = MessageService.DataNotFound, values = __val });
                        }
                    }
                    else
                    {
                        return Ok(new BaseResponse { error = false, code = 200, message = MessageService.DataNotFound, values = __resDist });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new BaseResponse { error = true, code = 401, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;

        }
        // promov3/getrecon
        /// <summary>
        /// Get Promo Reconciliation by Id, Old API = "promov3/getrecon"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/promodisplay/export-pdf", Name = "dn_promodisplay_exportpdf_get_id")]
        public async Task<IActionResult> GetPromoReconPromoDisplay([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDNPromoDisplay.GetPromoReconPromoDisplay(id);
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
        // promov3/getbyprimaryid
        /// <summary>
        /// Get Promo by Promo Id, Old API = "promov3/getbyprimaryid"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/promodisplay/id", Name = "dn_promodisplay_by_id")]
        public async Task<IActionResult> GetDNPromoDisplaybyId([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDNPromoDisplay.GetDNPromoDisplaybyId(id);
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
        //Select Entity
        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/promodisplay/entity", Name = "dn_promodisplay_entity_list")]
        public async Task<IActionResult> GetEntityDNPromoDisplay()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNPromoDisplay.GetEntityList();
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
    }
}