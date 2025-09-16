
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.FinanceReport;

namespace V7.Controllers.FinanceReport
{
    /// <summary>
    /// Promo approval aging Report handler
    /// </summary>
    public partial class FinanceReportController : BaseController
    {
        /// <summary>
        /// Get  Promo Approval Aging with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/promoapprovalaging", Name = "promoapprovalaging_fin_rpt_get_lp")]
        public async Task<IActionResult> GetFinPromoDisplayLandingPage([FromQuery] FinPromoApprovalAgingRequestParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinPromoApprovalAging.GetFinPromoApprovalAgingLandingPage(
                    param.Period!,
                    param.EntityId,
                    param.DistributorId,
                    param.BudgetParentId,
                    param.ChannelId,
                    param.profileId!,
                    param.Search!,
                    param.SortColumn.ToString(),
                    param.SortDirection.ToString(),
                    param.PageNumber,
                    param.PageSize);
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
        [HttpGet("api/finance-report/promoapprovalaging/entity", Name = "promoapprovalaging_fin_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityPromoApprovalAging()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinPromoApprovalAging.GetEntityList();
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

        /// <summary>
        /// Get List Distributor
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/promoapprovalaging/distributor", Name = "promoapprovalaging_fin_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributorPromoApprovalAging([FromQuery] FinPromoApprovalAgingDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoFinPromoApprovalAging.GetDistributorList(param.budgetId, param.entityId!);
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