using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.Services;

namespace V7.Controllers.Configuration
{
    public partial class ConfigController : BaseController
    {
        /// <summary>
        /// Run task again after 5am
        /// </summary>
        /// <param name="reminderType"></param>
        /// <returns></returns>
        [HttpGet("api/config/restarttimer", Name = "get_timer_target")]
        public async Task<IActionResult> GetTimerTarget()
        {
            try
            {
                var __val = new TimerService(__config, new TimerTask(__config, __ScopeFactory), __ScopeFactory).GetTargetTime();
          
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = "Run Task again"
                       // values = __val
                    });
             
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
       
       

    }
}