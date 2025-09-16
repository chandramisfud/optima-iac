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
    /// Budget Handle Controller
    /// </summary>
    public partial class BudgetController : BaseController
    {
        /// <summary>
        /// Get Entity List
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/assignment/entity", Name = "budget_assignment_entity")]
        public async Task<IActionResult> GetBudgetAssignmentEntity()
        {
            try
            {
                var __val = await _repoBudgetMaster.GetAllEntity();
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
        /// Get distributor list
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/assignment/distributor", Name = "budget_assignment_distributor")]
        public async Task<IActionResult> GetBudgetAssignmentDistributor([FromQuery] DistributorListParam param)
        {
            try
            {
                var __val = await _repoBudgetMaster.GetDistributorList(param.budgetId, param.entityId!);
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
        /// Get Budget Assignment with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/assignment/", Name = "budget_assignment_LP")]
        public async Task<IActionResult> GetBudgetAssignmentWithPagination([FromQuery] BudgetAssignmentListParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                BaseLP respon = new();
                if (__res.ProfileID != null)
                {
                    var result = await _repoBudgetMaster.GetBudgetAssignmentLP(param.year!, param.entity, param.distributor, param.budgetparent, 0, __res.ProfileID);

                    if (result != null)
                    {
                        var filtered = result;
                        if (!String.IsNullOrEmpty(param.Search))
                        {
                            param.Search = param.Search.ToLower();
                            filtered = result.Where(x => x.RefId!.ToLower().Contains(param.Search) ||
                            x.BudgetAmount.ToString().Contains(param.Search)
                            ).ToList();
                        }
                        // ordered before paging
                        filtered = filtered.OrderBy(x => x.Id).ToList();

                        if (param.PageSize >= 0)
                        {
                            var pagedData = Helper.GetPagedData(filtered.Cast<object>().ToList(), param.PageNumber, param.PageSize);
                            respon.Data = pagedData;
                        }
                        else
                        {
                            respon.Data = filtered.Cast<object>().ToList();
                        }
                        respon.TotalCount = result.Count;
                        respon.FilteredCount = filtered.Count;
                        return Ok(new { error = false, statuscode = 200, values = respon, message = "Success" });
                    }
                    else
                    {
                        return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
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
        /// Get Budget Assignment Allocation
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/assignment/allocation", Name = "budget_assignment_allocation_source")]
        public async Task<IActionResult> GetBudgetAssignmentAllocation([FromQuery] BudgetAssignmentAllocationParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    BaseLP respon = new();

                    var result = await _repoBudgetMaster.GetBudgetAllocationByConditions(param.year!, param.entity,
                        param.distributor, 0, 0, __res.ProfileID.ToString());

                    if (result != null)
                    {
                        var filtered = result;
                        if (!String.IsNullOrEmpty(param.Search))
                        {
                            param.Search = param.Search.ToLower();
                            filtered = result.Where(x => x.refId!.ToLower().Contains(param.Search) ||
                            x.budgetAmount.ToString().Contains(param.Search) || x.distributor!.ToLower().Contains(param.Search)
                            || x.entity!.ToLower().Contains(param.Search) || x.longDesc!.ToLower().Contains(param.Search)
                            ).ToList();
                        }
                        // ordered before paging
                        filtered = filtered.OrderBy(x => x.id).ToList();

                        if (param.PageSize > 0) // use paging
                        {
                            var pagedData = Helper.GetPagedData(filtered.Cast<object>().ToList(), param.PageNumber, param.PageSize);
                            respon.Data = pagedData;
                        }
                        else
                        {
                            respon.Data = filtered.Cast<object>().ToList();
                        }
                        respon.TotalCount = result.Count;
                        respon.FilteredCount = filtered.Count;


                        return Ok(new BaseResponse { error = false, code = 200, values = respon, message = MessageService.GetDataSuccess });
                    }
                    else
                    {
                        return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                    }

                }

                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = __ex.Message });
            }
        }

        [HttpGet("api/budget/assignment/allocation/id", Name = "budget_assignment_allocation_by_id")]
        public async Task<IActionResult> GetBudgetAssignmentAllocationId(int id)
        {
            try
            {
                var __acc = await _repoBudgetMaster.GetByPrimaryId(id);
                if (__acc != null)
                    return Ok(new BaseResponse { error = false, code = 200, values = __acc });
                else
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
            }
            catch (System.Exception __ex)
            {
                return NotFound(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Save Budget Assignment
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/budget/assignment", Name = "budget_assignment_create")]
        public async Task<IActionResult> CreateBudgetAssignment([FromBody] BudgetAssignmentCreateParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    BudgetAssignmentStoreDto budgetAssignment = new()
                    {
                        AssignmentDetail = new List<BudgetAssignmentDetailCreate>()
                    };
                    foreach (var item in param.assignmentDetail!)
                    {
                        budgetAssignment.AssignmentDetail.Add(new BudgetAssignmentDetailCreate
                        {
                            RefId = item.refId,
                            AssignmentId = item.assignmentId,
                            BudgetAmount = item.budgetAmount,
                            BudgetSourceId = item.budgetSourceId,
                            OwnId = item.ownId,
                            Desc = item.desc,
                        });
                    }
                    budgetAssignment.AllocationId = param.allocationId;
                    budgetAssignment.BudgetAmount = param.budgetAmount;
                    budgetAssignment.FrownId = param.frownId;
                    budgetAssignment.BudgetId = param.budgetId;
                    budgetAssignment.UserId = __res.ProfileID;
                    ErrorMessageDto res = await _repoBudgetMaster.CreateBudgetAssignment(budgetAssignment);

                    return Ok(new BaseResponse
                    {
                        code = 200,
                        message = MessagingServices.MessageService.SaveSuccess,
                        values = res
                    });
                }
                else
                {
                    return NotFound(new BaseResponse
                    {
                        error = true,
                        code = 404,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Update Budget Assignment
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/budget/assignment", Name = "budget_assignment_update")]
        public async Task<IActionResult> UpateBudgetAssignment([FromBody] BudgetAssignmentUpdateParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    BudgetAssignmentUpdateDto budgetAssignment = new()
                    {
                        AssignmentDetail = new List<BudgetAssignmentDetailUpdateDto>()
                    };
                    foreach (var item in param.assignmentDetail!)
                    {
                        budgetAssignment.AssignmentDetail.Add(new BudgetAssignmentDetailUpdateDto
                        {
                            Id = item.id,
                            BudgetAmount = item.budgetAmount,
                            ownid = item.ownId,
                            Desc = item.desc,
                        });
                    }

                    budgetAssignment.UserId = __res.ProfileID;
                    budgetAssignment.AssignmentId = param.assignmentId;
                    ErrorMessageDto res = await _repoBudgetMaster.UpdateBudgetAssignment(budgetAssignment);

                    return Ok(new BaseResponse
                    {
                        code = 200,
                        message = MessagingServices.MessageService.SaveSuccess,
                        values = res
                    });
                }
                else
                {
                    return NotFound(new BaseResponse
                    {
                        error = true,
                        code = 404,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }
        /// <summary>
        /// Get data assignment by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("api/budget/assignment/id", Name = "budgetassignment_getbyId")]
        public async Task<IActionResult> GetAssignmentById(int Id)
        {
            try
            {
                var __res = await _repoBudgetMaster.GetAssignmentById(Id);
                if (__res == null)
                {
                    return NotFound(
                        new BaseResponse
                        {
                            error = true,
                            code = 404,
                            message = MessagingServices.MessageService.GetDataFailed
                        }
                    );
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __res,
                        message = MessagingServices.MessageService.GetDataSuccess
                    });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }
    }
}
