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
        /// Get sub channel data base on sub channel Id
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/subchannel/id", Name = "master_SubChannel_id")]
        public async Task<IActionResult> GetSubChannelById([FromQuery] SubChannelById body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoSubChannel.GetSubChannelById(body);
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
        /// Get channel dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/subchannel/channel", Name = "master_SubChannel_subchannel_dropdown")]
        public async Task<IActionResult> GetChannelLongDescforSubChannel()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoSubChannel.GetChannelLongDescforSubChannel();
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
        /// Create sub channel data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/master/subchannel", Name = "SubChannel_store")]
        public async Task<IActionResult> CreateSubChannel([FromBody] SubChannelCreate body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    SubChannelCreate __bodytoken = new()
                    {
                        ChannelId = body.ChannelId,
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        CreateBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoSubChannel.CreateSubChannel(__bodytoken);
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
        /// Modified sub channel data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/master/subchannel", Name = "master_SubChannel_update")]
        public async Task<IActionResult> UpdateSubChannel([FromBody] SubChannelUpdate body)
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
                    SubChannelUpdate __bodytoken = new()
                    {
                        Id = body.Id,
                        ChannelId = body.ChannelId,
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        ModifiedBy = __res.ProfileID,
                        ModifiedEmail = __res.UserEmail
                    };
                    var __val = await __repoSubChannel.UpdateSubChannel(__bodytoken);
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
        /// Delete sub channel data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/master/subchannel", Name = "master_SubChannel_delete")]
        public async Task<IActionResult> DeleteSubChannel([FromBody] SubChannelDelete body)
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
                    SubChannelDelete __bodytoken = new()
                    {
                        Id = body.Id,
                        DeleteBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoSubChannel.DeleteSubChannel(__bodytoken);
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
        /// Get sub channel landing page data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/subchannel", Name = "master_SubChannel_lp")]
        public async Task<IActionResult> GetSubChannelLandingPage([FromQuery] SubChannelListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoSubChannel.GetSubChannelLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
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