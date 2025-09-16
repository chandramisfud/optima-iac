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
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/invoice-notif/entity", Name = "dn_invoice-notif_entity_list")]
        public async Task<IActionResult> GetEntityListDNInvoiceNotification()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoInvoiceNotif.GetEntityList();
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

        /// <summary>
        /// Get List Distributor
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/dn/invoice-notif/distributor", Name = "dn_invoice-notif_distributor_list")]
        public async Task<IActionResult> GetDistributorListDNInvoiceNotification([FromQuery] DNDistributorGlobalParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoInvoiceNotif.GetDistributorList(param.budgetId, param.entityId!);
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
        /// <summary>
        /// Remove promo attachment
        /// </summary>
        /// <returns></returns>
        [HttpDelete("api/dn/invoice-notif/promoattachment", Name = "dn_invoice-notif_promo_attachment_remove")]
        public async Task<IActionResult> DeletePromoAttachmentDNInvoiceNotification([FromQuery] int PromoId, string DocLink)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                await __repoInvoiceNotif.DeletePromoAttachmentDNInvoiceNotification(PromoId, DocLink);
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
        /// Get DN by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/invoice-notif/id", Name = "dn_invoice-notif_by_id")]
        public async Task<IActionResult> GetDNbyIdInvoiceNotification([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoInvoiceNotif.GetDNbyIdInvoiceNotification(id);
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
        /// <summary>
        /// DN Invoice Notification by Danone Reject
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/invoice-notif/reject", Name = "dn_invoice-notif_reject")]
        public async Task<IActionResult> DNRejectInvoiceNotification([FromBody] DNRejectParam param)
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
                    var __val = await __repoInvoiceNotif.DNRejectInvoiceNotification(param.dnid, param.reason!, __res.ProfileID);
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
        // [POST]api/debetnote/ready_to_invoice
        /// <summary>
        /// DN change status from Ready to Invoice, Old API = "[POST]api/debetnote/ready_to_invoice"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/invoice-notif/changestatus/ready-to-invoice", Name = "invoice_notif_changestatus_ready-to-invoice")]
        public async Task<IActionResult> DNChangeStatusReadytoInvoice([FromBody] DNChangeStatusParam param)
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
                    var __val = await __repoInvoiceNotif.DNChangeStatusReadytoInvoice("ready_to_invoice", __res.ProfileID!, __bodyToken.dnid);
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
        // [GET]debetnote/ready_to_invoice/{userid}/{entity}/{distributor}
        /// <summary>
        /// Get DN by status "validate_by_danone", Old API = "[GET]debetnote/ready_to_invoice/{userid}/{entity}/{distributor}"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/invoice-notif", Name = "dn_get_invoice_notif_ready_to_invoice")]
        public async Task<IActionResult> GetDNValidatebyDanone([FromQuery] DebitNoteSendtoParam param)
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
                    var __val = await __repoInvoiceNotif.GetDNValidatebyDanone("validate_by_danone", __res.ProfileID!, param.entityid, param.distributorid);
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