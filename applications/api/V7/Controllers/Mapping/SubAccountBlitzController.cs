using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Esf;
using Repositories.Contracts;
using Repositories.Entities.Models;
using V7.Controllers;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Mapping
{
    public partial class MappingController : BaseController
    {
        /// <summary>
        /// Get all subaccount to blitz mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/subaccount-blitz", Name = "mapsubaccount-blitz_list")]
        public async Task<IActionResult> GetSubAccountBlitzLandingPage([FromQuery] SubAccountBlitzListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoSubAccountBlitz.GetSubAccountBlitzLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
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
                return Conflict(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get all subaccount to blitz mapping data for download
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/subaccount-blitz/download", Name = "mapsubaccount-blitz_download")]
        public async Task<IActionResult> GetSubAccountBlitzDownload()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoSubAccountBlitz.GetSubAccountBlitzDownload();
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
        /// Get all channel data subaccount to blitz mapping data for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/subaccount-blitz/channel", Name = "mapsubaccount-blitz_channel_dropdown")]
        public async Task<IActionResult> GetChannelforSubAccountBlitz()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoSubAccountBlitz.GetChannelforSubAccountBlitz();
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
        /// Get all subchannel data for subaccount to blitz mapping data dropdown
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/subaccount-blitz/subchannel", Name = "mapsubaccount-blitz_subchannel_dropdown")]
        public async Task<IActionResult> GetSubChannelforSubAccountBlitz([FromQuery] int ChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoSubAccountBlitz.GetSubChannelforSubAccountBlitz(ChannelId);
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
        /// Get all account data for subaccount to blitz mapping data dropdown
        /// </summary>
        /// <param name="SubChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/subaccount-blitz/account", Name = "mapsubaccount-blitz_account_dropdown")]
        public async Task<IActionResult> GetAccountforSubAccountBlitz([FromQuery] int SubChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoSubAccountBlitz.GetAccountforSubAccountBlitz(SubChannelId);
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
        /// Get all account data for subaccount to blitz mapping data dropdown
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/subaccount-blitz/subaccount", Name = "mapsubaccount-blitz_subaccount_dropdown")]
        public async Task<IActionResult> GetSubAccountforSubAccountBlitz([FromQuery] int AccountId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoSubAccountBlitz.GetSubAccountforSubAccountBlitz(AccountId);
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
        /// Create subaccount to blitz mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/mapping/subaccount-blitz", Name = "mapsubaccount-blitz_store")]
        public async Task<IActionResult> CreateSubAccountBlitz([FromBody] SubAccountBlitzCreate body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    SubAccountBlitzCreate __bodytoken = new()
                    {
                        SAPCode = body.SAPCode,
                        ChannelId = body.ChannelId,
                        SubChannelId = body.SubChannelId,
                        AccountId = body.AccountId,
                        SubAccountId = body.SubAccountId,
                        CreateBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoSubAccountBlitz.CreateSubAccountBlitz(__bodytoken);
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
        /// Delete subaccount to blitz mapping data 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/mapping/subaccount-blitz", Name = "mapsubaccount-blitz_delete")]
        public async Task<IActionResult> DeleteSubAccountBlitz([FromBody] SubAccountBlitzDelete body)
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
                    SubAccountBlitzDelete __bodytoken = new()
                    {
                        Id = body.Id,
                        DeleteBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoSubAccountBlitz.DeleteSubAccountBlitz(__bodytoken);
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
    }
}