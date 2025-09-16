using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.FinanceReport;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers.FinanceReport
{
    /// <summary>
    /// Listing Promo Reporting handler
    /// </summary>
    /// 
    public partial class FinanceReportController : BaseController
    {
        /// <summary>
        /// Get Listing Promo Reporting with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromoreporting", Name = "listingpromoreporting_fin_rpt_get_lp")]
        public async Task<IActionResult> GetListingPromoReportingLandingPage([FromQuery] FinListingPromoReportingRequestParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinListingPromoReport.GetListingPromoReportingLandingPage(param.Period!, param.EntityId, param.DistributorId, param.BudgetParentId, param.ChannelId, param.profileId!,
                    param.CreateFrom!, param.CreateTo!, param.StartFrom!, param.StartTo!, param.SubmissionParam,
                    param.Search!, param.SortColumn.ToString(), param.CategoryId, param.SortDirection.ToString(), param.PageNumber, param.PageSize);
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
        /// Get Listing Promo Reporting by mechanism with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromoreportingbymechanism", Name = "listingpromoreportingmecha_fin_rpt_get_lp")]
        public async Task<IActionResult> GetListingPromoReportingMechanismLP([FromQuery] FinListingPromoReportingRequestParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinListingPromoReport.GetListingPromoReportingByMechaLP(param.Period!, param.EntityId, 
                    param.DistributorId, param.BudgetParentId, param.ChannelId, param.profileId!,
                    param.CreateFrom!, param.CreateTo!, param.StartFrom!, param.StartTo!, param.SubmissionParam,
                    param.Search!, param.SortColumn.ToString(), param.CategoryId, param.SortDirection.ToString(), 
                    param.PageNumber, param.PageSize);
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
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Listing Promo Reporting Post Recon
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromoreportingpostrecon", Name = "listingpromoreportingpostrecon_fin_rpt_get_lp")]
        public async Task<IActionResult> GetListingPromoReportingPostReconLP([FromQuery] FinListingPromoReportingRequestParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinListingPromoReport.GetListingPromoReportingPostRecon(param.Period!, param.EntityId,
                    param.DistributorId, param.BudgetParentId, param.ChannelId, param.profileId!,
                    param.CreateFrom!, param.CreateTo!, param.StartFrom!, param.StartTo!, param.SubmissionParam,
                    param.Search!, param.SortColumn.ToString(), param.CategoryId, param.SortDirection.ToString(),
                    param.PageNumber, param.PageSize);
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
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
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
        [HttpGet("api/finance-report/listingpromoreporting/entity", Name = "listingpromoreporting_fin_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityListingPromoReporting()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinListingPromoReport.GetEntityList();
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
        [HttpGet("api/finance-report/listingpromoreporting/distributor", Name = "listingpromoreporting_fin_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributorListingPromoReporting([FromQuery] FinListingPromoReportingDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoFinListingPromoReport.GetDistributorList(param.budgetId, param.entityId!);
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
        /// Get List Channel
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromoreporting/channel", Name = "listingpromoreporting_fin_rpt_get_channel_list")]
        public async Task<IActionResult> GetChannelListingPromoReporting([FromQuery] ChannelListingPromoReporting param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                List<int> arrayParent = new();
                var __val = await __repoFinListingPromoReport.GetChannelList(param.userid!, arrayParent.ToArray());
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
        /// Get list investment notification for promo reporting
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromoreporting/investmentnotif", Name = "listingpromoreporting_rpt_investmentnotif")]
        public async Task<IActionResult> CekInvestmentNotifListingPromoReporting([FromQuery] InvestmentNotifBodyFinReport param)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinListingPromoReport.CekInvestmentNotif(param);
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
        [HttpGet("api/finance-report/listingpromoreporting/category", Name = "listingpromoreporting_fin_rpt_get_category_list")]
        public async Task<IActionResult> GetCategoryDropdownList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinListingPromoReport.GetCategoryDropdownList();
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
