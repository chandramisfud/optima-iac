using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;
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
        // debetnote/assigndn
        /// <summary>
        /// DN Listing Over Budget assign, Old API = "debetnote/assigndn"
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/dn/listing-over-budget/assign", Name = "dn_listing_over_budget_assign")]
        public async Task<IActionResult> AssignDNforListingOverBudget([FromBody] Model.DebitNote.DNAssignParam param)
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
                    var __val = await __repoDNListingOverBudget.AssignDN(__bodytoken);
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
        /// DN list approved promo for DN Listing Over Budget, old API = "promo/getPromoForDn"
        /// </summary>
        /// <param name="periode"></param>
        /// <param name="entity"></param>
        /// <param name="channel"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("api/dn/listing-over-budget/approvedpromo-for-dn", Name = "dn_listing_over_budget_get_approved_promo")]
        public async Task<IActionResult> GetApprovedPromoforDNListingOverBudget([FromQuery] string periode, int entity, int channel, int account)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNListingOverBudget.GetApprovedPromoforDN(
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

        // debetnote/getbyId/
        /// <summary>
        /// Get DN by Id for DN Listing Over Budget, Old API = "debetnote/getbyId"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/listing-over-budget/id", Name = "dn_get_byid_for_dn_listing_over_budget")]
        public async Task<IActionResult> GetDNbyIdforDNListingOverBudget([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNListingOverBudget.GetDNbyIdforDNListingOverBudget(id);
                if (__val == null)
                {
                    return Ok(new BaseResponse{ code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
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

        // dn/listOverBudgetByUser
        /// <summary>
        /// DN lisingt over budget, old API = "debetnote/listOverBudgetByUser"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/dn/listing-over-budget", Name = "gn_get_listing_over_budget_ist")]
        public async Task<IActionResult> GetDNOverBudgetList([FromQuery] DNReAssignListParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNListingOverBudget.GetDNOverBudgetList(
                        param.periode!,
                        param.entityId,
                        param.distributorId,
                        param.channelId!,
                        param.accountId!,
                        __res.ProfileID,
                        param.isDNManual,
                        param.sortColumn, param.sortDirection, param.pageSize, param.pageNumber, param.search
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

        // master/getAttributeByUser
        /// <summary>
        /// Get attribute by user, Old API = "master/getAttributeByUser"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/listing-over-budget/attribute/user", Name = "dn_listing_over_budget_attribute_byuser")]
        public async Task<IActionResult> GetAttributeByUserforDNListingOverBudget()
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
                    var __val = await __repoDNListingOverBudget.GetAttributeByUser(__bodytoken.userid);
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

    }
}