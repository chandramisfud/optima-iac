using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Model;
using V7.Model.DebitNote;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    /// <summary>
    /// DN Manual Assignment
    /// </summary>
    public partial class DebitNoteController : BaseController
    {
        // debetnote/getbyId
        /// <summary>
        /// Get DN by Id for DN Manual Assignment, Old API = "debetnote/getbyId"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/manual-assignment/id", Name = "dn_get_byid_for_dnmanualassignment")]
        public async Task<IActionResult> GetDNbyIdforDNManualAssignment([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNManualAssignment.GetDNbyIdforDNManualAssignment(id);
                if (__val == null)
                {
                    return Ok(new BaseResponse { code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
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

        //    debetnote/assigndn
        /// <summary>
        /// DN Manual Assignment post, Old API = "debetnote/assigndn"
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/dn/manual-assignment/assign", Name = "dn_manualassignment_assign")]
        public async Task<IActionResult> AssignDNforDNManualAssignment([FromBody] DNAssignParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                   
                    var __val = await __repoDNManualAssignment.AssignDN(param.DNId, param.PromoId, __res.ProfileID);
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
        // promo/getPromoForDn/
        /// <summary>
        /// DN list approved promo for DN Manual Assignment, old API = "promo/getPromoForDn"
        /// </summary>
        /// <param name="periode"></param>
        /// <param name="entity"></param>
        /// <param name="channel"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("api/dn/manual-assignment/approvedpromo-for-dn", Name = "dn_manual_assignment_get_approved_promo")]
        public async Task<IActionResult> GetApprovedPromoforDNManualAssignment([FromQuery] string periode, int entity, int channel, int account)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNManualAssignment.GetApprovedPromoforDN(
                        periode,
                        entity,
                        channel,
                        account,
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
                        return Ok(new { error = false, code = 200, message = MessageService.DataNotFound, values = __val });
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

        // master/getAttributeByUser
        /// <summary>
        /// Get attribute by user, Old API = "master/getAttributeByUser"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/manual-assignment/attribute/user", Name = "dn_manualassignment_attribute_byuser")]
        public async Task<IActionResult> GetAttributeByUserforDNManualAssignment()
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
                    var __val = await __repoDNManualAssignment.GetAttributeByUser(__bodytoken.userid);
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

        //    debetnote/dnmanualassignlist
        /// <summary>
        /// Get DN Manual Assignment List, Old API = "debetnote/dnmanualassignlist"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/manual-assignment", Name = "dnmanual_assignment_list")]
        public async Task<IActionResult> GetDNManualAssignList([FromQuery] DNManualAssignmentLPParam param)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    
                    var __val = await __repoDNManualAssignment.GetDNManualAssignList(__res.ProfileID, param.SortColumn,
                        param.SortDirection, param.PageSize, param.PageNumber, param.Search);
                        
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        // debetnote/assignpromo_request
        /// <summary>
        /// DN Manual Assignment Promo, Old API = "debetnote/assignpromo_request"
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/dn/manual-assignment/assign-promo", Name = "dn_manualassignment_assign_promo")]
        public async Task<IActionResult> ForwardAssignment([FromBody] DNAssignPromoRequestParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNManualAssignment.ForwardAssignment(param.dnid, param.approver_userid!, param.internal_order_number!);
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

    }
}