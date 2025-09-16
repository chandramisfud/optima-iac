using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Master
{
    public partial class MasterController : BaseController
    {
        /// <summary>
        /// Get region data base on region Id
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/region/id", Name = "master_Region_id")]
        public async Task<IActionResult> GetRegionById([FromQuery] RegionById body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoRegion.GetRegionById(body);
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Create region data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/master/region", Name = "Region_store")]
        public async Task<IActionResult> CreateRegion([FromBody] RegionCreate body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    RegionCreate __bodytoken = new()
                    {
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        CreateBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoRegion.CreateRegion(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Store data " + __val.RefId + " success", values = __val });
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
        /// Modified region data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/master/region", Name = "master_Region_update")]
        public async Task<IActionResult> UpdateRegion([FromBody] RegionUpdate body)
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
                    RegionUpdate __bodytoken = new()
                    {
                        Id = body.Id,
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        ModifiedBy = __res.ProfileID,
                        ModifiedEmail = __res.UserEmail
                    };
                    var __val = await __repoRegion.UpdateRegion(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Update data " + __val.RefId + " success", values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Delete region data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/master/region", Name = "master_Region_delete")]
        public async Task<IActionResult> DeleteRegion([FromBody] RegionDelete body)
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
                    RegionDelete __bodytoken = new()
                    {
                        Id = body.Id,
                        DeleteBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoRegion.DeleteRegion(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Update data " + __val.RefId + " success", values = __val });
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
        /// Get region landing page data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/region", Name = "master_Region_lp")]
        public async Task<IActionResult> GetRegionLandingPage([FromQuery] RegionListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoRegion.GetRegionLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
                    body.PageSize, body.PageNumber);
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
    }
}