using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.Configuration;
using V7.Services;

namespace V7.Controllers.Configuration
{
    public partial class ConfigController : BaseController
    {
        /// <summary>
        /// Get Data Configuration Promo Initiator Reminder
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/config/promoinitiatorreminder", Name = "config_reminder_promoinitiator")]
        public async Task<IActionResult> GetPromoInitiatorReminderList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __PromoInitiatorReminderRepo.ConfigPromoInitiatorReminderList();
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
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Update Configuration Promo Initiator Reminder
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("api/config/promoinitiatorreminder", Name = "master_config_reminder_promoinitatio_upate")]
        public async Task<IActionResult> UpdateConfigReminderPromoInitiator([FromBody] PromoInitiatorParam body)
        {
             DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
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
                    ConfigPromoInitiatorReminderListUpdate data = new()
                    {
                        configList = new List<ConfigPromoInitiatorReminderUpdate>()
                    };

                    foreach (var item in body.configList!)
                    {
                        data.configList.Add(new ConfigPromoInitiatorReminderUpdate
                        {
                            id = item.id,
                            datereminder = item.datereminder,
                            useredit = __res.ProfileID,
                            dateedit = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            ModifiedEmail = __res.UserEmail
                        });
                    }
                    var __res2 = await __PromoInitiatorReminderRepo.UpdateConfigReminderPromoInitiator(data);
                    if (__res2)
                    {
                        result = Ok(new BaseResponse { error = false, code = 200, message = MessageService.UpdateSuccess });
                    }
                    else
                    {
                        result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                    }
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

    }
}