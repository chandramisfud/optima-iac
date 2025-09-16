using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Master
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial class MasterController : BaseController
    {
        /// <summary>
        /// Get entity data base on entity Id
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/entity/id", Name = "master_entity_id")]
        public async Task<IActionResult> GetEntityById([FromQuery] EntityById body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoEntity.GetEntityById(body);
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
        /// Create entity data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/master/entity", Name = "entity_store")]
        public async Task<IActionResult> CreateEntity([FromBody] EntityCreate body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    EntityCreate __bodytoken = new()
                    {
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        CompanyName = body.CompanyName,
                        EntityAddress = body.EntityAddress,
                        EntityUp = body.EntityUp,
                        EntityNPWP = body.EntityNPWP,
                        DescForInvoice = body.DescForInvoice,
                        ShortDesc2 = body.ShortDesc2,
                        CreateBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoEntity.CreateEntity(__bodytoken);
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
        /// Modified entity data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/master/entity", Name = "master_Entity_update")]
        public async Task<IActionResult> UpdateEntity([FromBody] EntityUpdate body)
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
                    EntityUpdate __bodytoken = new()
                    {
                        Id = body.Id,
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        CompanyName = body.CompanyName,
                        ModifiedBy = __res.ProfileID,
                        ModifiedEmail = __res.UserEmail
                    };
                    var __val = await __repoEntity.UpdateEntity(__bodytoken);
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
        /// Delete Entity
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/master/entity", Name = "master_Entity_delete")]
        public async Task<IActionResult> DeleteEntity([FromBody] EntityDelete body)
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
                    EntityDelete __bodytoken = new()
                    {
                        Id = body.Id,
                        DeletedBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoEntity.DeleteEntity(__bodytoken);
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
        /// Get entity data for landing page
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/entity", Name = "master_entity_lp")]
        public async Task<IActionResult> GetEntityLandingPage([FromQuery] EntityListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoEntity.GetEntityLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
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
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }
    }
}