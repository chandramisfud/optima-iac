using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.DebitNote;
using V7.Model.FinanceReport;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    public partial class DebitNoteController : BaseController
    {
        /// <summary>
        /// Get Listing Promo Reporting with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/dn/listing-promo-distributor", Name = "dn_listing_promo_distributor_landing_page")]
        public async Task<IActionResult> GetDNListingPromoDistributorLandingPage([FromQuery] DNListingPromoDistributorRequestParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNListingPromoDistributor.GetListingPromoReportingLandingPage(param.Period!, param.CategoryId, param.EntityId, param.DistributorId, param.BudgetParentId, param.ChannelId, "admin",
                    param.CreateFrom!, param.CreateTo!, param.StartFrom!, param.StartTo!, param.SubmissionParam,
                    param.Search!, param.SortColumn.ToString(), param.SortDirection.ToString(), param.PageNumber, param.PageSize);
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
        /// Get List Entity for DN Listing promo distributor
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/listing-promo-distributor/entity", Name = "dn_listingpromodistributor_entity")]
        public async Task<IActionResult> GetEntityListDNListingPromoDistributor()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoDNListingPromoDistributor.GetEntityList();
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
        /// Get List Distributor for DN listing promo distributor 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/dn/listing-promo-distributor/distributor", Name = "dn_listing_promo_distributor")]
        public async Task<IActionResult> GetDistributorListDNListingPromoDistributor([FromQuery] FinListingPromoReportingDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDNListingPromoDistributor.GetDistributorList(param.budgetId, param.entityId!);
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
        /// Get List Channel for DN Listing promo Distributor
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/listing-promo-distributor/channel", Name = "dn_listing_promo_distributor_channel")]
        public async Task<IActionResult> GetChannelListDNPromoListingDistributor()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                List<int> arrayParent = new();
                var __val = await __repoDNListingPromoDistributor.GetChannelList(__res.ProfileID!, arrayParent.ToArray());
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
        /// Get Data User Profile by Id for DN Listing Promo Distributor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/listing-promo-distributor/userprofile/id", Name = "dn_get_userprofile_byprimarykey")]
        public IActionResult GetUserProfileById(string id)
        {
            try
            {
                var __res = __repoDNListingPromoDistributor.GetById(id);
                if (__res == null)
                {
                    return UnprocessableEntity(
                        new BaseResponse
                        {
                            error = true,
                            code = 204,
                            message = MessageService.DataNotFound
                        }
                    );
                }
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    values = __res.Result,
                    message = MessageService.GetDataSuccess
                });
            }
            catch (System.Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get List Category
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/listing-promo-distributor/category", Name = "listing-promo-distributor_dn_get_category_list")]
        public async Task<IActionResult> GetCategoryDropdownList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNListingPromoDistributor.GetCategoryDropdownList();
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


