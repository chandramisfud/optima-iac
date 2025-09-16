using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Models;
using V7.Controllers;
using V7.MessagingServices;
using V7.Model.UserAccess;

namespace WebAPI.Controllers
{
    public partial class UserAccessController : BaseController
    {
        /// <summary>
        /// Get List Admin Report With Paginate
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/useradminreport", Name = "useraccess_useradminreport_lp")]
        public async Task<IActionResult> GetUserAdminReportLandingPage([FromQuery] UserAdminReportListRequestParam query)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __userAdminReport.GetUserAdminReportLandingPage(query.Search!, query.PageSize, query.PageNumber);
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
                    return NotFound(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
    }
}
