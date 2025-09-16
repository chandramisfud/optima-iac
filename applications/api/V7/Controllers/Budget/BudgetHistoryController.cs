using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities;
using Repositories.Entities.Models;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.Budget;
using V7.Services;

namespace V7.Controllers.Budget
{
    /// <summary>
    /// Document Completeness handler
    /// </summary>
    public partial class BudgetController : BaseController
    {
        /// <summary>
        /// Get Budget History Listing with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/history", Name = "budget_history_get_lp")]
        public async Task<IActionResult> GetBudgetHistoryLandingPage([FromQuery] budgetHistoryLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetHistory.GetBudgetHistoryLandingPage(param.year!, param.entity, param.distributor, param.budgetParent, param.channel, __res.ProfileID,
                        String.IsNullOrEmpty(param.Search) ? "" : param.Search, param.SortColumn.ToString(), param.SortDirection.ToString(), param.PageNumber, param.PageSize);
                    if (__val != null)
                    {
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
        /// Get Distributor
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/history/distributor", Name = "budget_history_distributor")]
        public async Task<IActionResult> GetBudgetHistoryDistributor([FromQuery] DistributorListParam param)
        {
            try
            {
                var __val = await _repoBudgetApproval.GetDistributorList(param.budgetId, param.entityId!);
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/history/entity", Name = "budget_history_entity")]
        public async Task<IActionResult> GetBudgetHistoryEntity()
        {
            try
            {

                //if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetHistory.GetAllEntity();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
    }
}
