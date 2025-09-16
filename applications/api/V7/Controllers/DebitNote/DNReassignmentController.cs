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
        // debetnote/assigndn
        /// <summary>
        /// DN Reassignment post, Old API = "debetnote/assigndn"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/reassignment/assign", Name = "dn_reassignment_assign")]
        public async Task<IActionResult> AssignDN([FromBody] Model.DebitNote.DNAssignParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    Repositories.Entities.Models.DN.DNAssignParam __bodytoken = new()
                    {
                        DNId = param.DNId,
                        PromoId = param.PromoId,
                        UserId = __res.ProfileID
                    };
                    var __val = await __repoDNReassignment.AssignDN(__bodytoken);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.SaveSuccess
                    });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
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
        // promo/getPromoAprrovalForDnByInitiator/
        /// <summary>
        /// Get DN approved promo by initiator, old API = "promo/getPromoAprrovalForDnByInitiator/"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/dn/reassingment/promo-approval", Name = "dn_reassignment_get_promo_approval")]
        public async Task<IActionResult> GetApprovedPromoByInitiator([FromQuery] GetApprovedPromoforbyInitiatorDNParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNReassignment.GetApprovedPromoByInitiator(
                        param.periode!,
                        param.entityId,
                        param.channelId,
                        param.accountId,
                        __res.ProfileID
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
                    return Ok(new BaseResponse{ error = true, code = 404, message = MessageService.GetDataFailed });
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
        //  master/getAttributeByUser
        /// <summary>
        /// Get attribute by user, Old API = "master/getAttributeByUser"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/reassignment/attribute/user", Name = "dn_reassignment_all_attribute_byuser")]
        public async Task<IActionResult> GetAttributeByUserforDNAssignment()
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    DNAttributebyUserParam __bodytoken = new()
                    {
                        userid = __res.ProfileID
                    };
                    var __val = await __repoDNReassignment.GetAttributeByUser(__bodytoken.userid);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message
                });

            }
        }
        // debetnoteassign/list/
        /// <summary>
        /// DN list assign, old API = "debetnoteassign/list/"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/reassignment/", Name = "gn_get_reassignment list")]
        public async Task<IActionResult> GetDNAssignList([FromQuery] DNManualAssignmentLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNReassignment.GetDNAssignList(
                        "0",
                        0,
                        0,
                        "0",
                        "0",
                        __res.ProfileID,
                        false, param.SortColumn, param.SortDirection, param.PageSize, param.PageNumber, param.Search
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
                    return Ok(new { error = true, code = 404, message = MessageService.GetDataFailed });
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
        // debetnote/getbyId/
        /// <summary>
        /// Get DN by Id, Old API = "debetnote/getbyId"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/reassignment/id", Name = "dn_reassignment_get_by_dn_id")]
        public async Task<IActionResult> GetDNIdforReassignment([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNReassignment.GetDNIdforReassignment(id);
                if (__val == null)
                {
                    return Ok(new { code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
                }
                else
                {
                    return Ok(new
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

    }
}
