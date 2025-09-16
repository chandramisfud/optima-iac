// __repoDNVATExpiredChecklist

using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.Budget;
using V7.Model.DebitNote;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    /// <summary>
    /// DebitNote Controller
    /// </summary>
    public partial class DebitNoteController : BaseController
    {
        // debetnote/vatexpriedupdate
        /// <summary>
        /// DN VAT Expired Checklist Update, Old API = "debetnote/vatexpriedupdate"
        /// </summary>
        /// <returns></returns>
        [HttpPut("api/dn/vatexpired", Name = "dn_vatexpired_update")]
        public async Task<IActionResult> DNVATExpiredUpdate([FromBody] DNVATExpiredUpdateParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    await __repoDNVATExpiredChecklist.DNVATExpiredUpdate(__res.ProfileID, param.id, param.VATExpired);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = null,
                        message = MessageService.UpdateSuccess
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
        // Select Distributor
        /// <summary>
        /// Get List Distributor
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/dn/vatexpired/distributor", Name = "dn_vatexpired_distributor_list")]
        public async Task<IActionResult> GetDistributor([FromQuery] DistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNVATExpiredChecklist.GetDistributorList(param.budgetId, param.entityId!);
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
        // Select Entity
        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/vatexpired/entity", Name = "dn_vatexpired_entity_list")]
        public async Task<IActionResult> GetEntityforVATExpired()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNVATExpiredChecklist.GetEntityList();
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
        // debetnote/vatexpired/list/
        /// <summary>
        /// DN list VAT Expired List, old API = "debetnote/vatexpired/list"
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="distributorId"></param>
        /// <param name="TaxLevel"></param>
        /// <returns></returns>
        [HttpGet("api/dn/vatexpired", Name = "dn_vatexpired_list")]
        public async Task<IActionResult> GetVATExpiredList([FromQuery] DNVATExpiredLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNVATExpiredChecklist.GetVATExpiredList(
                        "received_by_danone",
                        __res.ProfileID,
                        param.entityId,
                        param.distributorId,
                        param.taxLevel == null ? "0" : param.taxLevel,
                        param.sortColumn, param.sortDirection, param.pageSize, param.pageNumber, param.search);
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
    }
}