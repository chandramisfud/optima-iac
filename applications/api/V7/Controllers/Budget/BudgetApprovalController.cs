using Microsoft.AspNetCore.Mvc;
using Repositories.Entities;
using Repositories.Entities.Models;
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
        /// Get Budget Approval Listing with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/approval", Name = "budget_approval_get_lp")]
        public async Task<IActionResult> GetBudgetApprovalLandingPage([FromQuery] budgetApprovalLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetApproval.GetBudgetApprovalLandingPage(param.year!, param.entity, param.distributor, param.budgetParent, param.channel, __res.ProfileID);
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
        [HttpGet("api/budget/approval/distributor", Name = "budget_approval_distributor")]
        public async Task<IActionResult> GetBudgetApprovalDistributor([FromQuery] DistributorListParam param)
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
        [HttpGet("api/budget/approval/entity", Name = "budget_approval_entity")]
        public async Task<IActionResult> GetBudgetApprovalEntity()
        {
            try
            {

                //if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetApproval.GetAllEntity();
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
        /// Get Channel
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/approval/channel", Name = "budget_approval_channel")]
        public async Task<IActionResult> GetBudgetApprovalChannel()
        {
            try
            {

                //if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetApproval.GetAllChannel();
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
        /// Approve Budget by ID
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/budget/approval/approve", Name = "budget_approval_approve")]
        public async Task<IActionResult> BudgetApprovalApprove([FromBody] BudgetApprovalApproveParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    if (!ModelState.IsValid) return Conflict(ModelState);

                    BudgetApprovalApproveDto budgetApprovalApproveDto = new()
                    {
                        budgetId = param.budgetId,
                        statsuApproval = "AP2",
                        notes = "",
                        profileId = __res.ProfileID

                    };

                    var __res1 = await _repoBudgetApproval.BudgetApproval(budgetApprovalApproveDto);
                    if (__res1)
                    {
                        return Ok(new BaseResponse { error = false, code = 200, message = MessageService.SaveSuccess });
                    }
                    else
                    {
                        return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.SaveFailed });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

            }
            catch (System.Exception __ex)
            {
                return NotFound(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Approve Budget by ID
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/budget/approval/unapprove", Name = "budget_approval_unapprove")]
        public async Task<IActionResult> BudgetApprovalUnapprove([FromBody] BudgetApprovalUnapproveParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    if (!ModelState.IsValid) return Conflict(ModelState);

                    BudgetApprovalApproveDto budgetApprovalApproveDto = new()
                    {
                        budgetId = param.budgetId,
                        statsuApproval = "AP1",
                        notes = param.notes,
                        profileId = __res.ProfileID

                    };

                    var __res1 = await _repoBudgetApproval.BudgetApproval(budgetApprovalApproveDto);
                    if (__res1)
                    {
                        return Ok(new BaseResponse { error = false, code = 200, message = MessageService.SaveSuccess });
                    }
                    else
                    {
                        return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.SaveFailed });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return NotFound(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
    }
}
