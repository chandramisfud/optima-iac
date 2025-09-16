using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers
{
    /// <summary>
    /// Report DN Display handler
    /// </summary>
    public partial class ReportController : BaseController
    {
        /// <summary>
        /// Get DN Display with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/report/dndisplay", Name = "dndisplay_rpt_get_lp")]
        public async Task<IActionResult> GetDNDisplayLandingPage([FromQuery] DNDisplayRequestParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNDisplay.GetDNDisplayLandingPage(param.Period!, param.EntityId, param.DistributorId, param.ChannelId, param.AccountId, __res.ProfileID, param.IsDNManual!,
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
        /// Get Data DN by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/report/dndisplay/id", Name = "dndisplay_rpt_get_by_id")]
        public async Task<IActionResult> GetDNById([FromQuery] int id)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoDNDisplay.GetDNDisplayData(id);
                if (__val == null)
                {
                    return NotFound(new { code = 404, error = true, message = MessageService.DataNotFound });
                }
                else
                {
                    return Ok(new
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
        [HttpGet("api/report/dndisplay/entity", Name = "dndisplay_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityDNDisplay()
        {
            try
            {
                var __val = await __repoDNDisplay.GetEntityList();
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
        [HttpGet("api/report/dndisplay/distributor", Name = "dndisplay_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributorDNDisplay([FromQuery] DNDisplayDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDNDisplay.GetDistributorList(param.budgetId, param.entityId!);
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
        [HttpGet("api/report/dndisplay/sellingpoint", Name = "dndisplay_rpt_get_sellingpoint_list")]
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
                    var __val = await __repoDNDisplay.GetSellingPointList(__res.ProfileID);
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
        [HttpGet("api/report/dndisplay/taxlevel", Name = "dndisplay_rpt_get_taxlevel_list")]
        public async Task<IActionResult> GetTaxLevelDNDisplay()
        {
            try
            {
                var __val = await __repoDNDisplay.GetTaxLevelList();
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
        [HttpGet("api/report/dndisplay/print", Name = "dndisplay_rpt_get_print_data")]
        public async Task<IActionResult> GetDNDisplayPrint([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoDNDisplay.GetDNPrint(id);
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
