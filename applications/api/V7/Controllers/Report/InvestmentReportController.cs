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
    /// Investment Report handler
    /// </summary>
    public partial class ReportController : BaseController
    {
        /// <summary>
        /// Get Investment Report with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/report/investment", Name = "investment_rpt_get_investment_report_lp")]
        public async Task<IActionResult> GetInvestmentReportLandingPage([FromQuery] InvestmentReportListRequestParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoInvestmentReport.GetInvestmentReportLandingPage(param.Search!, param.SortColumn.ToString(), param.Period!, param.EntityId,
                    param.DistributorId, param.BudgetParentId, param.ChannelId, __res.ProfileID, param.PageNumber, param.PageSize, param.SortDirection.ToString());
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
        /// Get List Budget Allocation
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/report/investment/budgetallocation", Name = "investment_rpt_get_budget_allocation_list")]
        public async Task<IActionResult> GetBudgetAllocation([FromQuery] BudgetAllocationListRequestParam param)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoInvestmentReport.GetBudgetAllocationList(param.year!, param.entityId, param.distributorId, param.budgetParentId, param.channelId, __res.ProfileID);
                    {
                        return Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            message = MessageService.GetDataSuccess,
                            values = __val
                        });
                    }
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
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/report/investment/entity", Name = "investment_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntity()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoInvestmentReport.GetEntityList();
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
        [HttpGet("api/report/investment/distributor", Name = "investment_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributor([FromQuery] DistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoInvestmentReport.GetDistributorList(param.budgetId, param.entityId!);
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
