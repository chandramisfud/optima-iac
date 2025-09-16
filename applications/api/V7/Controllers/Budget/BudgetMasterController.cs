using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model;
using V7.Model.Budget;
using V7.Model.UserAccess;
using V7.Services;

namespace V7.Controllers.Budget
{
    /// <summary>
    /// Budget Handle Controller
    /// </summary>
    //[AllowAnonymous]
    public partial class BudgetController : BaseController
    {

        /// <summary>
        /// Get Budget Master List with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        [HttpGet("api/budget/master", Name = "budget_master")]
        public async Task<IActionResult> GetBudgetWithPagination([FromQuery] budgetMasterLPParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    //if (!ModelState.IsValid) return Conflict(ModelState);
                    var __val = await _repoBudgetMaster.GetBudgetMasterLandingPage(param.year!, param.entity, param.distributor, __res.ProfileID,
                        String.IsNullOrEmpty(param.Search) ? "" : param.Search, param.SortColumn.ToString(), param.SortDirection.ToString(), param.PageNumber, param.PageSize);
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
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Entity for Budget Master Menu
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/master/entity", Name = "budget_master_entity")]
        public async Task<IActionResult> GetBudgetMasterEntity()
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
        /// Get  distributor for Budget Master Menu
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/master/distributor", Name = "budget_master_distributor")]
        public async Task<IActionResult> GetBudgetMasterDistributor([FromQuery] DistributorListParam param)
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
        /// Get category for Budget Master Menu
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/master/category", Name = "budget_master_category")]
        public async Task<IActionResult> GetBudgetMasterCategory()
        {
            try
            {
                var __val = await _repoBudgetMaster.GetCategoryList();
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
        /// Get Budget Master by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/budget/master/id", Name = "budget_master_by_ID")]
        public async Task<IActionResult> GetBudgetMasterByID(int id)
        {
            try
            {
                var __val = await _repoBudgetMaster.BudgetMasterById(id);
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
        /// Create Budget Master
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/budget/master", Name = "budgetmaster_store")]
        public async Task<IActionResult> CreateBudgetMaster([FromBody] BudgetMasterSaveParam param)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    if (!ModelState.IsValid) return Conflict(ModelState);

                    Repositories.Entities.Models.BudgetMasterSaveDto budgetMasterSaveDto = new()
                    {
                        BudgetAmount = param.BudgetAmount,
                        CategoryId = param.CategoryId,
                        DistributorId = param.DistributorId,
                        LongDesc = param.LongDesc,
                        Periode = param.Periode,
                        PrincipalId = param.EntityId,
                        OwnerId = __res.ProfileID,
                        CreateBy = __res.ProfileID,
                        CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    };

                    var __res1 = await _repoBudgetMaster.CreateBudgetMaster(budgetMasterSaveDto);
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
        /// Update Budget Master
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/budget/master", Name = "budgetmaster_update")]
        public async Task<IActionResult> UpdateBudgetMaster([FromBody] BudgetMasterUpdateParam param)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    Repositories.Entities.Models.BudgetMasterSaveDto budgetMasterSaveDto = new()
                    {
                        BudgetAmount = param.BudgetAmount,
                        CategoryId = param.CategoryId,
                        DistributorId = param.DistributorId,
                        LongDesc = param.LongDesc,
                        Periode = param.Periode,
                        PrincipalId = param.EntityId,
                        Id = param.Id,
                        ModifiedBy = __res.ProfileID,
                        ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    };
                    var __res1 = await _repoBudgetMaster.UpdateBudgetMaster(budgetMasterSaveDto);
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

        [HttpDelete("api/budget/master", Name = "budgetmaster_delete")]
        public async Task<IActionResult> DeleteBudgetMaster([FromBody] BudgetMasterDeleteParam param)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    Repositories.Entities.Models.BudgetMasterDeleteDto budgetMasterDeleteDto = new()
                    {
                        Id = param.Id,
                        DeleteBy = __res.ProfileID,
                        DeleteOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        IsDelete = true
                    };
                    await _repoBudgetMaster.DeleteBudgetMaster(budgetMasterDeleteDto);
                    return Ok(new { error = false, status_code = 200, message = "Delete Data Budget Master Successfull" });
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
