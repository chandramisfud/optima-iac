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
    /// Report DN Display handler
    /// </summary>
    public partial class FinanceReportController : BaseController
    {
        /// <summary>
        /// Get DN Display with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/dndisplay", Name = "dndisplay_fin_rpt_get_lp")]
        public async Task<IActionResult> GetDNDisplayLandingPage([FromQuery] FinDNDisplayRequestParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinDNDisplayReport.GetDNDisplayLandingPage(param.Period!, param.EntityId, param.DistributorId, param.ChannelId, param.AccountId, param.profileId!, param.IsDNManual!,
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
        /// Get Data DN by ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/dndisplay/id", Name = "dndisplay_fin_rpt_get_by_id")]
        public async Task<IActionResult> GetDNById([FromQuery] int id)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinDNDisplayReport.GetDNDisplayData(id);
                if (__val == null)
                {
                    return NotFound(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/dndisplay/entity", Name = "dndisplay__fin_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityDNDisplay()
        {
            try
            {
                var __val = await __repoFinDNDisplayReport.GetEntityList();
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
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
        [HttpGet("api/finance-report/dndisplay/distributor", Name = "dndisplay__fin_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributorDNDisplay([FromQuery] FinDNDisplayDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var __val = await __repoFinDNDisplayReport.GetDistributorList(param.budgetId, param.entityId!);
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get List Selling Point
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/dndisplay/sellingpoint", Name = "dndisplay_fin_rpt_get_sellingpoint_list")]
        public async Task<IActionResult> GetSellingPointDNDisplay()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoFinDNDisplayReport.GetSellingPointList(__res.ProfileID);
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
        /// Get List Tax Level
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/dndisplay/taxlevel", Name = "dndisplay_fin_rpt_get_taxlevel_list")]
        public async Task<IActionResult> GetTaxLevelDNDisplay()
        {
            try
            {
                var __val = await __repoFinDNDisplayReport.GetTaxLevelList();
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Data DN for Print by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/finance_report/dndisplay/print", Name = "dndisplay_fin_rpt_get_print_data")]
        public async Task<IActionResult> GetDNDisplayPrint([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinDNDisplayReport.GetDNPrint(id);
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
    }
}
