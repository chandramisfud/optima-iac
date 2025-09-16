using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.FinanceReport;
using V7.Model.Master;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers.FinanceReport
{
    /// <summary>
    /// Matrix Approval Listing handler
    /// </summary>
    public partial class FinanceReportController : BaseController
    {
        /// <summary>
        /// Get Matrix Approval Listing Report with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/matrixapprovallisting", Name = "matrixapprovallisting_finance_rpt_get_matrix_approval_lp")]
        public async Task<IActionResult> GetInvestmentReportLandingPage([FromQuery] FinMatrixApprovalListingRequestParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinMatrixApproval.GetMatrixApprovalLandingPage(param.Period!, param.CategoryId, param.EntityId, param.DistributorId,
                    param.Search!, param.SortColumn.ToString(), param.PageNumber, param.PageSize, param.SortDirection.ToString());
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
        /// Get Report Matrix approval history
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/matrixapprovallisting/history", Name = "get_finance_promoapprovallisting_history")]
        public async Task<IActionResult> GetFinanceMatrixPromoAprovallistingHistory([FromQuery] MatrixPromoApprovalHistoryListReq body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoFinMatrixApproval.GetMatrixPromoAprovalHistoryList(body.category, body.entity, body.distributor,
                    body.userid!, body.PageNumber, body.PageSize, body.txtSearch!, body.order!, body.sort!);
                if (__val == null)
                {
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
                else
                {
                    return Ok(new { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get List Distributor
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/matrixapprovallisting/distributor", Name = "matrixapprovallisting_finance_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributorMatrixApprovalListing([FromQuery] FinMatrixApprovalListingDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoFinMatrixApproval.GetDistributorList(param.budgetId, param.entityId!);
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
        /// Get List Category
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/matrixapprovallisting/category", Name = "matrixapprovallisting_fin_rpt_get_category_list")]
        public async Task<IActionResult> GetCategoryDropdownListforMatrixApprovalListing()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinMatrixApproval.GetCategoryDropdownList();
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
