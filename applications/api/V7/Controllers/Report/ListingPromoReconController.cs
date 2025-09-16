using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers
{
    /// <summary>
    /// Listing Promo Reconciliation Reporting handler
    /// </summary>
    /// 
    public partial class ReportController : BaseController
    {
        /// <summary>
        /// Get Listing Promo Reconciliation with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/report/listingpromorecon", Name = "listingpromorecon_rpt_get_lp")]
        public async Task<IActionResult> GetListingPromoReconLandingPage([FromQuery] ListingPromoReconRequestParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    List<string> profileId = new()
                    {
                        __res.ProfileID
                    };
                    var __val = await __repoListingPromoReconRepo.GetListingPromoReconLandingPage(param.Period!, param.EntityId, param.DistributorId, param.BudgetParentId, param.ChannelId,
                        param.CreateFrom!, param.CreateTo!, param.StartFrom!, param.StartTo!, param.SubmissionParam, profileId.ToArray(),
                        param.Search!, param.SortColumn.ToString(), param.SortDirection.ToString(), param.PageNumber, param.PageSize);
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
        [HttpGet("api/report/listingpromorecon/entity", Name = "listingpromorecon_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityListingPromoReconReporting()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoListingPromoReconRepo.GetEntityList();
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
        [HttpGet("api/report/listingpromorecon/distributor", Name = "listingpromorecon_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributorListingPromoRecon([FromQuery] ListingPromoReconDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoListingPromoReconRepo.GetDistributorList(param.budgetId, param.entityId!);
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
