using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers
{
    /// <summary>
    /// DN Detail Reporting handler
    /// </summary>
    public partial class ReportController : BaseController
    {
        /// <summary>
        /// Get DN Detail Reporting with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/report/dndetailreporting", Name = "dndetailreporting_rpt_get_dndetailreporting_lp")]
        public async Task<IActionResult> GetDNDetailReportingLandingPage([FromQuery] DNDetailReportingListRequestParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNDetailReporting.GetDNDetailReportingLandingPage(param.Period!, param.CategoryId, param.EntityId, param.DistributorId, param.ChannelId, param.AccountId, __res.ProfileID,
                        param.Search!, param.SortColumn.ToString(), param.PageNumber, param.PageSize, param.SortDirection.ToString());
                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/report/dndetailreporting/entity", Name = "dndetailreporting_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityDNDetailReporting()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNDetailReporting.GetEntityList();
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
        [HttpGet("api/report/dndetailreporting/distributor", Name = "dndetailreporting_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributor([FromQuery] DNDetailReportingDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNDetailReporting.GetDistributorList(param.budgetId, param.entityId!);
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
        /// Get List Sub Account
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/report/dndetailreporting/subaccount", Name = "dndetailreporting_rpt_get_subaccount_list")]
        public async Task<IActionResult> GetSubAccount()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNDetailReporting.GetSubAccountList(__res.ProfileID);
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
        /// Get List Category
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/report/dndetailreporting/category", Name = "dndetailreporting_rpt_get_category_list")]
        public async Task<IActionResult> GetCategoryDropdownListforDNDetailReporting()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNDetailReporting.GetCategoryDropdownList();
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
