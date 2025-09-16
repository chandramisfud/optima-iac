using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.Mapping;
using V7.Services;

namespace V7.Controllers.Mapping
{
    public partial class MappingController : BaseController
    {
        /// <summary>
        /// Get all taxlevel data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/taxlevel", Name = "maptaxlevel_lp")]
        public async Task<IActionResult> GetTaxLevelLandingPage([FromQuery] TaxLevelListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoTaxLevel.GetTaxLevelLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
                    body.PageSize, body.PageNumber);
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }
        /// <summary>
        /// Create taxlevel mapping data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/mapping/taxlevel", Name = "maptaxlevel_store")]
        public async Task<IActionResult> CreateDistributorSubAccount([FromBody] TaxLevelCreateParam param)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    TaxLevelCreate __bodytoken = new()
                    {
                        MaterialNumber = param.MaterialNumber,
                        Description = param.Description,
                        WHT_Type = param.WHT_Type,
                        WHT_Code = param.WHT_Code,
                        Purpose = param.Purpose,
                        EntityId = param.EntityId,
                        Entity = param.Entity,
                        PPNPct = param.PPNPct,
                        PPHPct = param.PPHPct,
                        CreateBy = __res.ProfileID!,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoTaxLevel.CreateTaxLevel(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess, values = __val });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get all taxlevel mapping data for download
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/taxlevel/download", Name = "maptaxlevel_download")]
        public async Task<IActionResult> GetTaxLevelDownload()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoTaxLevel.GetTaxLevelDownload();
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }

        /// <summary>
        /// Delete taxlevel mapping data 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/mapping/taxlevel", Name = "maptaxlevel_delete")]
        public async Task<IActionResult> DeleteTaxLevel([FromBody] TaxLevelDeleteParam body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                // get token from request header
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    TaxLevelDelete __bodytoken = new()
                    {
                        Id = body.Id,
                        UserId = __res.ProfileID,
                        UserLogin = __res.UserEmail
                    };
                    var __val = await __repoTaxLevel.DeleteTaxLevel(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.DeleteSucceed, values = __val });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get entity data for taxlevel mapping data dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/taxlevel/entity", Name = "taxlevel_entity_dropdown")]
        public async Task<IActionResult> GetEntityforTaxLevel()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoTaxLevel.GetEntityforTaxLevel();
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }
    }
}