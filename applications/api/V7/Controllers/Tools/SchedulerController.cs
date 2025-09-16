using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.Tools;

namespace V7.Controllers.Tools
{
    public partial class ToolsController : BaseController
    {
        /// <summary>
        /// Get reminder list
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/tools/scheduler/reminder", Name = "get_scheduler_reminderlist")]
        public async Task<IActionResult> GetReminderList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __schedulerRepo.GetReminderList();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get promo auto closing list
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/tools/scheduler/promo/autoclosing", Name = "get_scheduler_promoautoclosing")]
        public async Task<IActionResult> GetAutoClosing()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __schedulerRepo.GetAutoClosing();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get reminder pending approval
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/tools/scheduler/reminder/pendingapproval", Name = "get_scheduler_reminderpendingapproval")]
        public async Task<IActionResult> GetReminderPendingApproval()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __schedulerRepo.GetReminderPendingApproval();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Send Email Reminder pending approval and sendback
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/tools/scheduler/sendreminder/pendingapproval", Name = "set_scheduler_sendreminder_pendingapproval")]
        public async Task<IActionResult> RunSendingReminderPendingApproval()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __schedulerRepo.SendEmailApprovalReminder();
                if (__val)
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Blitz Notification
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/tools/scheduler/blitznotif", Name = "get_scheduler_blitznotif")]
        public async Task<IActionResult> BlitzTranferNotification()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __schedulerRepo.BlitzTranferNotif();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Post promo autoclose
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/tools/scheduler/promo/autoclose", Name = "get_scheduler_promo_autoclose")]
        public async Task<IActionResult> PromoAutoClose()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                await __schedulerRepo.PromoAutoClose();
                return Ok(new BaseResponse
                {
                    code = 200,
                    error = false,
                    message = MessageService.SaveSuccess,
                    values = null
                });
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Post promo cancel
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/tools/scheduler/promo/cancel", Name = "get_scheduler_promo_cancele")]
        public async Task<IActionResult> CancelPromo([FromBody] PromoCancelBodyRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __res = await __schedulerRepo.CancelPromo(body.promoId, body.userId!, body.statusCode!, body.approverEmail!);
                return Ok(new
                {
                    code = 200,
                    error = false,
                    statuscode = __res.statuscode,
                    Message = __res.message,
                    PromoId = __res.Id,
                    RefId = __res.RefId,
                    Userid_Approver = __res.userid_approver,
                    Username_Approver = __res.username_approver,
                    Email_Approver = __res.email_approver,
                    Userid_Initiator = __res.userid_initiator,
                    Username_Initiator = __res.username_initiator,
                    Email_Initiator = __res.email_initiator
                });
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Post promo planning cancel
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/tools/scheduler/promo/planning/cancel", Name = "get_scheduler_promo_planning_cancel")]
        public async Task<IActionResult> CancelPromoPlanning([FromBody] PromoPlanningCancelBodyRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __res = await __schedulerRepo.CancelPromoPlanning(body.promoPlanId, body.userId!, body.notes!);
                return Ok(new
                {
                    code = 200,
                    error = false,
                    statuscode = __res.statuscode,
                    Message = __res.message,
                    PromoPlanId = __res.Id,
                    RefId = __res.RefId
                });
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// get Data email send regular
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/tools/scheduler/listingpromoreporting/promoapprovalreminder/regularemail", Name = "scheduler_promo_approval_reminder_regular_send")]
        public async Task<IActionResult> PromoApprovalReminderRegularSend()
        {
            try
            {
                var __res = await __schedulerRepo.GetPromoApprovalReminderRegularSend();
                if (__res != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        message = MessageService.GetDataSuccess,
                        code = 200,
                        values = __res
                    });

                }
                else
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = true,
                        code = 404,
                        message = MessageService.GetDataFailed,

                    });
                }

            }
            catch (Exception __ex)
            {
                return Ok(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get Promo Display by  Id Tools Scheduler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/tools/scheduler/promo/id", Name = "tools_scheduler_promodisplay_fin_rpt_get_id")]
        public async Task<IActionResult> GetPromoDisplaybyId([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __schedulerRepo.GetPromoSchedulerById(id);
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
        /// Get Promo Recom Promo Display Tools Scheduler
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/tools/scheduler/promorecon/id", Name = "tools_scheduler_promorecon_exportpdf_fin_rpt_get_id")]
        public async Task<IActionResult> GetPromoReconPromoDisplay([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __schedulerRepo.GetPromoReconSchedulerById(id);
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