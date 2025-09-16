using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Model;
using V7.Model.DebitNote;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    /// <summary>
    /// DebitNote Controller
    /// </summary>
    public partial class DebitNoteController : BaseController
    {
        // debetnote/assigndn
        /// <summary>
        /// DN Reassignment post, Old API = "debetnote/assigndn"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/reassignment-finance/assign", Name = "dn_reassignment_finance_assign")]
        public async Task<IActionResult> AssignDNforReassignmentFinance([FromBody] Model.DebitNote.DNAssignParam param)
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
                    var __val = await __repoDNReassignmentFinance.AssignDN(__bodytoken);
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
        //  master/getAttributeByUser
        /// <summary>
        /// Get attribute by user, Old API = "master/getAttributeByUser"
        /// </summary>

        /// <returns></returns>
        [HttpGet("api/dn/reassignment-finance/attribute", Name = "dn_reassignment_finance_all_attribute_byuser")]
        public async Task<IActionResult> GetAttributeByUserforDNARessignmentFinance()
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
                    var __val = await __repoDNReassignmentFinance.GetAttributeByUser(__bodytoken.userid);
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
        // debetnote/getbyId/
        /// <summary>
        /// Get DN by Id for DN Reassignent Finance, Old API = "debetnote/getbyId"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/reassignment-finance/id", Name = "dn_reassignment_finance_get_by_dn_id")]
        public async Task<IActionResult> GetDNbyIdforDNReassignmentFinance([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNReassignmentFinance.GetDNbyIdforDNReassignmentFinance(id);
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
        // debetnote/assignpromo_request
        /// <summary>
        /// DN reAssignment Finance Promo, Old API = "debetnote/assignpromo_request"
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/dn/reassignment-finance/assign-promo", Name = "dn_reassignment_finance_assign_promo")]
        public async Task<IActionResult> ForwardAssignmentforDNReassignmentFinance([FromBody] DNAssignPromoRequestParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNReassignmentFinance.ForwardAssignment(param.dnid, param.approver_userid!, param.internal_order_number!);
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
        // promo/getPromoForDn
        /// <summary>
        /// DN list approved promo for DN reAssignment Finance, old API = "promo/getPromoForDn"
        /// </summary>
        /// <param name="periode"></param>
        /// <param name="entityId"></param>
        /// <param name="channelId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("api/dn/reassignment-finance/approvedpromo-for-dn", Name = "dn_assignment_finance_get_approved_promo")]
        public async Task<IActionResult> GetApprovedPromoforDNAssignmentFinance([FromQuery] string periode, int entityId, int channelId, int accountId)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNReassignmentFinance.GetApprovedPromoforDN(
                        periode,
                        entityId,
                        channelId,
                        accountId,
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
        // debetnoteassign/list/finance
        /// <summary>
        /// DN list reassignment Finance, old API = "debetnoteassign/list/finance"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/reassignment-finance/", Name = "gn_get_reassignment_finance_list")]
        public async Task<IActionResult> GetDNAssignListFinance([FromQuery] LPPagingParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNReassignmentFinance.GetDNAssignListFinance(
                         "0",
                        0,
                        0,
                        "0",
                        "0",
                        __res.ProfileID,
                        false, param.sortColumn, param.sortDirection, param.pageSize, param.pageNumber, param.search
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
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
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
    }
}

