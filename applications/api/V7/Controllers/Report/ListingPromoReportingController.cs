using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers
{
    /// <summary>
    /// Listing Promo Reporting handler
    /// </summary>
    /// 
    public partial class ReportController : BaseController
    {
        /// <summary>
        /// Get Listing Promo Reporting with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/report/listingpromoreporting", Name = "listingpromoreporting_rpt_get_lp")]
        public async Task<IActionResult> GetListingPromoReportingLandingPage([FromQuery] ListingPromoReportingRequestParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoListingPromoReportingRepo.GetListingPromoReportingLandingPage(param.Period!, param.EntityId, param.DistributorId, param.BudgetParentId, param.ChannelId, __res.ProfileID,
                        param.CreateFrom!, param.CreateTo!, param.StartFrom!, param.StartTo!, param.SubmissionParam,
                        param.Search!, param.SortColumn.ToString(), param.CategoryId, param.SortDirection.ToString(), param.PageNumber, param.PageSize);
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
        [HttpGet("api/report/listingpromoreportingbymechanism", Name = "listingpromoreportingbymechanism_rpt_get_lp")]
        public async Task<IActionResult> GetListingPromoReportingbyMechanismLP([FromQuery] ListingPromoReportingRequestParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoListingPromoReportingRepo.GetListingPromoReportingByMechanismLP(param.Period!, param.EntityId, param.DistributorId, param.BudgetParentId, param.ChannelId, __res.ProfileID,
                        param.CreateFrom!, param.CreateTo!, param.StartFrom!, param.StartTo!, param.SubmissionParam,
                        param.Search!, param.SortColumn.ToString(), param.CategoryId, param.SortDirection.ToString(), param.PageNumber, param.PageSize);
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
        [HttpGet("api/report/listingpromoreporting/entity", Name = "listingpromoreporting_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityListingPromoReporting()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoListingPromoReportingRepo.GetEntityList();
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
        [HttpGet("api/report/listingpromoreporting/distributor", Name = "listingpromoreporting_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributorListingPromoReporting([FromQuery] PromoPlanningReportingDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoListingPromoReportingRepo.GetDistributorList(param.budgetId, param.entityId!);
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
        [HttpGet("api/report/listingpromoreporting/channel", Name = "listingpromoreporting_rpt_get_channel_list")]
        public async Task<IActionResult> GetChannelListingPromoReporting()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                List<int> arrayParent = new();

                if (__res.ProfileID != null)
                {
                    var __val = await __repoListingPromoReportingRepo.GetChannelList(__res.ProfileID, arrayParent.ToArray());
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
        [HttpGet("api/report/listingpromoreporting/category", Name = "listingpromoreporting_rpt_get_category_list")]
        public async Task<IActionResult> GetCategoryDropdownList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoListingPromoReportingRepo.GetCategoryDropdownList();
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
