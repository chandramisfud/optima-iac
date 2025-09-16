using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using System.ComponentModel.DataAnnotations;
using V7.Controllers;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Configuration
{
    public partial class ConfigController : BaseController
    {
        /// <summary>
        /// Get Data Configuration Reminder by Type
        /// </summary>
        /// <param name="reminderType"></param>
        /// <returns></returns>
        [HttpGet("api/config/reminder/type", Name = "get_config_reminder_by_type")]
        public async Task<IActionResult> GetByReminderType([FromQuery] int reminderType)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __ReminderRepo.GetListReminder(reminderType);
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
        /// Update Configuration Reminder
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/config/reminder", Name = "config_reminder_update")]
        public async Task<IActionResult> UpdateReminder([FromBody] V7.Model.Configuration.ReminderParam body)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            IActionResult? result;
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
                    ConfigReminderListUpdate configReminderData = new()
                    {
                        configList = new List<ConfigReminderUpdate>()
                    };

                    foreach (var item in body.configList!)
                    {
                        configReminderData.configList.Add(new ConfigReminderUpdate
                        {
                            id = item.id,
                            daysfrom = item.daysfrom,
                            daysto = item.daysto,
                            frequency = item.frequency,
                            useredit = __res.ProfileID,
                            dateedit = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            ModifiedEmail = __res.UserEmail
                        });
                    }
                    var __res2 = await __ReminderRepo.UpdateReminder(configReminderData);
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