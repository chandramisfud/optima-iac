using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Models;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.FinanceReport;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers.FinanceReport
{
    /// <summary>
    /// Accrual Report handler
    /// </summary>
    public partial class FinanceReportController : BaseController
    {
        /// <summary>
        /// Get Accrual Finance Report with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/accrual", Name = "accrual_rpt_get_accrual_finance_report_lp")]
        public async Task<IActionResult> GetAccrualReportLandingPage([FromQuery] FinAccrualReportListRequestParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinAccrualReport.GetAccrualReportLandingPage(param.Download, param.Period!, param.EntityId,
                param.DistributorId, param.BudgetParentId, param.ChannelId, param.GrpBrandId, param.profileId!, param.ClosingDate!,
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
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }

        /// <summary>
        /// Get grp brand by entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/accrual/brand", Name = "accrual_finance_rpt_get_brand_list")]
        public async Task<IActionResult> GetEntityAccrual(int entityId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinAccrualReport.GetGroupBrandList(entityId);
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
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/accrual/entity", Name = "accrual_finance_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityAccrual()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinAccrualReport.GetEntityList();
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
        /// Get list promo accrual report header by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/accrual/header/id", Name = "accrual_finance_rpt_get_byId")]
        public async Task<IActionResult> GetPromoAccrualReportById([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinAccrualReport.GetPromoAccrualReportById(id);
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
        /// Get list promo accrual report header
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/accrual/header", Name = "accrual_finance_rpt_get_header")]
        public async Task<IActionResult> GetPromoAccrualReportHeader([FromQuery] AccrualReportHeaderBody param)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinAccrualReport.GetPromoAccrualReportHeader(param);
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
        /// Get list investment notification for accrual report
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/accrual/investmentnotif", Name = "accrual_finance_rpt_investmentnotif")]
        public async Task<IActionResult> CekInvestmentNotif([FromQuery] InvestmentNotifBodyFinReport param)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinAccrualReport.CekInvestmentNotif(param);
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