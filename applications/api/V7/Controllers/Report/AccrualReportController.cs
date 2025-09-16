using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Models;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers
{
    /// <summary>
    /// Accrual Report handler
    /// </summary>
    public partial class ReportController : BaseController
    {
        /// <summary>
        /// Get Accrual Report with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/report/accrual", Name = "accrual_rpt_get_accrual_report_lp")]
        public async Task<IActionResult> GetAccrualReportLandingPage([FromQuery] AccrualReportListRequestParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoAccrualReport.GetAccrualReportLandingPage(param.Download, param.Period!, param.EntityId,
                    param.DistributorId, param.BudgetParentId, param.ChannelId,param.GrpBrandId, __res.ProfileID, param.ClosingDate!,
                    param.Search!, param.SortColumn.ToString(), param.PageNumber, param.PageSize, param.SortDirection.ToString());
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
        [HttpGet("api/report/accrual/entity", Name = "accrual_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityAccrual()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoAccrualReport.GetEntityList();
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
        /// Get grp brand by entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/report/accrual/brand", Name = "accrual_rpt_get_brand_list")]
        public async Task<IActionResult> GetEntityAccrual(int entityId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoAccrualReport.GetGroupBrandList(entityId);
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