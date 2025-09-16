using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Configuration
{
    public partial class ConfigController : BaseController
    {
        /// <summary>
        /// Get Data Config Late Promo Creation
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/config/latepromocreation", Name = "config_latepromocreation")]
        public async Task<IActionResult> GetConfigLatePromo()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __LatePromoRepo.GetConfigLatePromo();
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Update Late Promo Creation
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/config/latepromocreation", Name = "latepromocreation_update")]
        public async Task<IActionResult> UpdateLatePromoCreation([FromBody] V7.Model.Configuration.LatePromoCreationParam body)
        {
             DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (!String.IsNullOrEmpty(__res.ProfileID))
                {
                    Repositories.Entities.Configuration.LatePromoCreation latePromoCreationData = new()
                    {
                        configList = new List<LatePromoCreationConfig>()
                    };

                    foreach (var item in body.configList!)
                    {
                        latePromoCreationData.configList.Add(new LatePromoCreationConfig { 
                            id = item.id!,
                            daysfrom = item.daysfrom!,
                            useredit = __res.ProfileID, 
                            dateedit = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone), 
                            ModifiedEmail = __res.UserEmail! 
                        });
                    }
                    var __res2 = await __LatePromoRepo.UpdateLatePromoCreation(latePromoCreationData);
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