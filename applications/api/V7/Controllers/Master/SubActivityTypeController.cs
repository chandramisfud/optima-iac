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
        /// Get sub activity type data base on sub activity type Id
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/subactivitytype/id", Name = "master_SubActivityType_id")]
        public async Task<IActionResult> GetSubActivityTypeById([FromQuery] SubActivityTypeById body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoSubActivityType.GetSubActivityTypeById(body);
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Create sub activity type data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/master/subactivitytype", Name = "SubActivityType_store")]
        public async Task<IActionResult> CreateSubActivityType([FromBody] SubActivityTypeCreate body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    SubActivityTypeCreate __bodytoken = new()
                    {
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        CreateBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoSubActivityType.CreateSubActivityType(__bodytoken);
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
        /// Modified sub activity type data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/master/subactivitytype", Name = "master_SubActivityType_update")]
        public async Task<IActionResult> UpdateSubActivityType([FromBody] SubActivityTypeUpdate body)
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
                    SubActivityTypeUpdate __bodytoken = new()
                    {
                        Id = body.Id,
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        ModifiedBy = __res.ProfileID,
                        ModifiedEmail = __res.UserEmail
                    };
                    var __val = await __repoSubActivityType.UpdateSubActivityType(__bodytoken);
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
        /// Delete sub activity type data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/master/subactivitytype", Name = "master_SubActivityType_delete")]
        public async Task<IActionResult> DeleteSubActivityType([FromBody] SubActivityTypeDelete body)
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
                    SubActivityTypeDelete __bodytoken = new()
                    {
                        Id = body.Id,
                        DeleteBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoSubActivityType.DeleteSubActivityType(__bodytoken);
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
        /// Get sub activity type landing page
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/subactivitytype", Name = "master_SubActivityType_lp")]
        public async Task<IActionResult> GetSubActivityTypeLandingPage([FromQuery] SubActivityTypeListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoSubActivityType.GetSubActivityTypeLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }
    }
}