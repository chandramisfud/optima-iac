using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.FinanceReport;
using V7.Model.Tools;
using V7.Services;

namespace V7.Controllers.FinanceReport
{
    public partial class FinanceReportController : BaseController
    {
      

        /// <summary>
        /// Get Promo approval reminder list LP
        /// </summary>
        /// <param name="year"></param>
        /// <param name="monthStart"></param>
        /// <param name="monthEnd"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromoreporting/promoapprovalreminder", Name = "finance_report_promo_approval_reminder")]
        public async Task<IActionResult> PromoApprovalReminder([FromQuery]string year, string monthStart = "1", string monthEnd = "2")
        {
            try
            {
                var __res = await __repoFinPromoApprovalReminder.GetPromoApprovalReminder(year, monthStart, monthEnd);
                if (__res != null)
                {
                    return Ok(new Model.BaseResponse { 
                        error = false, message = MessagingServices.MessageService.GetDataSuccess, code = 200, values = __res 
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
        /// get Data email send regular
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromoreporting/promoapprovalreminder/regularemail", Name = "finance_report_promo_approval_reminder_regular_send")]
        public async Task<IActionResult> PromoApprovalReminderRegularSend()
        {
            try
            {
                var __res = await __repoFinPromoApprovalReminder.GetPromoApprovalReminderRegularSend();
                if (__res != null)
                {
                    return Ok(new Model.BaseResponse { 
                        error = false, message = MessageService.GetDataSuccess, code = 200, values = __res });

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
        /// get Data auto config
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromoreporting/promoapprovalreminder/autoconfig", Name = "finance_report_promo_approval_reminder_config")]
        public async Task<IActionResult> PromoApprovalReminderConfig()
        {
            try
            {
                var __res = await __repoFinPromoApprovalReminder.GetPromoApprovalReminderSettingById(1);
                if (__res != null)
                {
                    return Ok(new Model.BaseResponse { 
                        error = false, message = MessageService.GetDataSuccess, code = 200, values = __res 
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
        /// save Data auto config
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/finance-report/listingpromoreporting/promoapprovalreminder/autoconfig", Name = "finance_report_update_promo_approval_reminder_config")]
        public async Task<IActionResult> UpdatePromoApprovalReminderConfig([FromBody] PromoApprovalReminderSettingUpdateParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    List<Entities.PromoApprovalReminderConfigEmail> lsEmail = new();
                    foreach (var email in param.configEmail!)
                    {
                        lsEmail.Add(new Entities.PromoApprovalReminderConfigEmail
                        {
                            email = email.email!,
                            statusName = email.statusName!,
                            userGroupName = email.userGroupName!,
                            userName = email.userName!,
                        });
                    }

                    var result = await __repoFinPromoApprovalReminder.UpdatePromoApprovalReminderSetting(param.id, param.dt1, param.dt2, param.eod, param.autoRun,
                        lsEmail, __res.ProfileID, __res.UserEmail);
                    if (result)
                    {
                        return Ok(new Model.BaseResponse 
                        { 
                            error = false, message = MessageService.SaveSuccess, code = 200, values = result }
                        );

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
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Ok(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Save Data email send regular (manual)
        /// </summary>
        /// <returns></returns>
        [HttpPut("api/finance-report/listingpromoreporting/promoapprovalreminder/regularemail", Name = "finance_report_update_promo_approval_reminder_manual_email_config")]
        public async Task<IActionResult> UpdatePromoApprovalReminderManualEmailConfig([FromBody] PromoApprovalReminderManualEmailParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    List<Entities.PromoApprovalReminderConfigEmail> lsEmail = new();
                    foreach (var email in param.configEmail!)
                    {
                        lsEmail.Add(new Entities.PromoApprovalReminderConfigEmail
                        {
                            email = email.email!,
                            statusName = email.statusName!,
                            userGroupName = email.userGroupName!,
                            userName = email.userName!,
                        });
                    }
                    var res = await __repoFinPromoApprovalReminder
                        .UpdatePromoApprovalReminderManualEmailConfig(lsEmail, __res.ProfileID, __res.UserEmail);
                    if (res)
                    {
                        return Ok(new Model.BaseResponse { error = false, message = MessageService.SaveSuccess, code = 200, values = res });

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
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Ok(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// get Data source email
        /// </summary>
        /// <param name="usergroupid"></param>
        /// <param name="userlevel"></param>
        /// <param name="isdeleted"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromoreporting/promoapprovalreminder/sourceemail", Name = "finance_report_promo_approval_reminder_sourceemail")]
        public async Task<IActionResult> GetPromoApprovalReminderSourceEmail([FromQuery] string usergroupid, int userlevel, int isdeleted)
        {
            try
            {
                var __res = await __repoFinPromoApprovalReminder.GetUserList(usergroupid, userlevel, isdeleted);
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
        /// get Data Email regular
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromoreporting/promoapprovalreminder/regularemail/data", Name = "finance_report_promo_approval_reminder_manual_email_config_data")]
        public async Task<IActionResult> GetPromoApprovalReminderManualEmailConfigData()
        {
            try
            {
                var __res = await __repoFinPromoApprovalReminder.GetPromoApprovalReminderManualEmailConfig();
                if (__res != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        message = MessageService.SaveSuccess,
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

    }
}
