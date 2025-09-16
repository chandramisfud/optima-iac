using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    public partial class DebitNoteController : BaseController
    {
        // debetnote/dnupdateoverbudget
        /// <summary>
        /// DN delete attachment , Old API = "debetnote/dnupdateoverbudget"
        /// </summary>
        /// <returns></returns>
        [HttpPut("api/dn/overbudget", Name = "dn_refresh_update")]
        public async Task<IActionResult> DNUpdateOverBudget([FromQuery] int PromoId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                // get token from request header
                await __repoDNOverBudget.DNUpdateOverBudget(PromoId);
                return Ok(
                    new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.UpdateSuccess,
                        values = null
                    });
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        // debetnote/listrefreshoverbudget
        /// <summary>
        /// DN  Refreshlist, old API = debetnote/listrefreshoverbudget"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/overbudget", Name = "dn_refresh_list")]
        public async Task<IActionResult> GetDNOverBudgetList()
        {
            IActionResult result;
            try
            {
                var __val = await __repoDNOverBudget.GetDNOverBudgetList("",0,0,"0","0","0",true );
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