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
    /// Listing Promo Reconciliation Reporting handler
    /// </summary>
    /// 
    public partial class FinanceReportController : BaseController
    {
        /// <summary>
        /// Get Listing Promo Reconciliation with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromorecon", Name = "listingpromorecon_fnc_rpt_get_lp")]
        public async Task<IActionResult> GetListingPromoReconLandingPage([FromQuery] FinListingPromoReconRequestParam param)
        {
            IActionResult result;
            try
            {
                if (param.profileId != null)
                {
                    var __val = await __repoFinListingPromoRecon.GetListingPromoReconLandingPage(param.Period!, param.EntityId, param.DistributorId, param.BudgetParentId, param.ChannelId,
                        param.CreateFrom!, param.CreateTo!, param.StartFrom!, param.StartTo!, param.SubmissionParam, param.profileId.ToArray()!,
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
        [HttpGet("api/finance-report/listingpromorecon/entity", Name = "listingpromorecon_fnc_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityListingPromoReconReporting()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var __val = await __repoFinListingPromoRecon.GetEntityList();
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
        [HttpGet("api/finance-report/listingpromorecon/distributor", Name = "listingpromorecon_fnc_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributorListingPromoRecon([FromQuery] FinListingPromoReconDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoFinListingPromoRecon.GetDistributorList(param.budgetId, param.entityId!);
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
        /// Get List channel
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromorecon/channel", Name = "listingpromorecon_fnc_rpt_get_channel_list")]
        public async Task<IActionResult> GetChannelListingPromoRecon([FromQuery] ChannelSKPValidationParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                List<int> arrayParent = new();

                var __val = await __repoFinListingPromoRecon.GetChannelList(param.userid!, arrayParent.ToArray());
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
        /// Get user groups for promo reconciliation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/listingpromorecon/usergroups", Name = "listingpromorecon_fnc_rpt_get_usergroups_list")]
        public async Task<IActionResult> GetUserGroupsforFinPromoRecon([FromQuery] string[] id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoFinListingPromoRecon.GetUserGroupsforFinPromoRecon(id);
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
