using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using V7.MessagingServices;
using V7.Model;

namespace V7.Controllers.Budget
{
    public partial class BudgetController : BaseController
    {
        /// <summary>
        /// Get list region
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/allocation/region", Name = "budget_allocation_region")]
        public async Task<IActionResult> GetBudgetAllocationRegion()
        {
            try
            {
                var __val = await _repoBudgetMaster.GetAllRegion(); 
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get list channel
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/allocation/channel", Name = "budget_allocation_channel")]
        public async Task<IActionResult> GetBudgetAllocationChannel()
        {
            try
            {
               
                var __val = await _repoBudgetMaster.GetAllChannel();
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Subchannel by channelId
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/allocation/subchannel", Name = "budget_allocation_subchannel")]
        public async Task<IActionResult> GetBudgetAllocationSubChannel(int channelId)
        {
            try
            {
                var __val = await _repoBudgetMaster.GetAllSubChannel(channelId);
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get List Account by subChannelId
        /// </summary>
        /// <param name="subChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/allocation/account", Name = "budget_allocation_account")]
        public async Task<IActionResult> GetBudgetAllocationAccount(int subChannelId)
        {
            try
            {

                var __val = await _repoBudgetMaster.GetAllAccount(subChannelId);
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get list SUb account by accountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/allocation/subaccount", Name = "budget_allocation_subaccount")]
        public async Task<IActionResult> GetBudgetAllocationSubAccount(int accountId)
        {
            try
            {

                var __val = await _repoBudgetMaster.GetAllSubAccount(accountId);
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// List Active User
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/allocation/user", Name = "budget_allocation_user")]
        public async Task<IActionResult> GetBudgetAllocationUser()
        {
            try
            {

                var __val = await _repoBudgetMaster.GetAllActiveUser();
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// List Brand based on budget
        /// </summary>
        /// <param name="budgetId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/allocation/brand", Name = "budget_allocation_brand")]
        public async Task<IActionResult> GetBudgetAllocationBrand(int budgetId, [FromQuery] int[] parentId)
        {
            try
            {

                var __val = await _repoBudgetMaster.GetBrandAttributeByParent(budgetId, parentId);
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        [HttpGet("api/budget/allocation/sku", Name = "budget_allocation_sku")]
        public async Task<IActionResult> GetBudgetAllocationSKU(int brandId)
        {
            try
            {
                var __val = await _repoBudgetMaster.GetAllProductByBrandId(brandId);
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        [HttpGet("api/budget/allocation/subcategory", Name = "budget_allocation_subcategory")]
        public async Task<IActionResult> GetBudgetAllocationSubcategory(int budgetId, [FromQuery] int[] parentId)
        {
            try
            {
                var __val = await _repoBudgetMaster.GetSubCategoryAttributeByParent(budgetId, parentId);
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        [HttpGet("api/budget/allocation/activity", Name = "budget_allocation_activity")]
        public async Task<IActionResult> GetBudgetAllocationActivity(int budgetId, [FromQuery] int[] parentId)
        {
            try
            {
                var __val = await _repoBudgetMaster.GetActivityAttributeByParent(budgetId, parentId);
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        [HttpGet("api/budget/allocation/subactivity", Name = "budget_allocation_subactivity")]
        public async Task<IActionResult> GetBudgetAllocationSubActivity(int budgetId, [FromQuery] int[] parentId)
        {
            try
            {
                var __val = await _repoBudgetMaster.GetSubActivityAttributeByParent(budgetId, parentId);
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
    }
}
