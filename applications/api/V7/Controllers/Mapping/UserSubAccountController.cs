using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Models;
using V7.Controllers;
using V7.MessagingServices;
using V7.Model.Mapping;
using V7.Services;

namespace V7.Controllers.Mapping
{
    public partial class MappingController : BaseController
    {
        /// <summary>
        /// Get all user to subaccount mapping data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/user-subaccount", Name = "mapuser-subaccount_list")]
        public async Task<IActionResult> GetUserSubAccountLandingPage([FromQuery] UserSubAccountListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoUserSubAccount.GetUserSubAccountLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
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
        /// Get all user to subaccount mapping data for download
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/user-subaccount/download", Name = "mapuser-subaccount_download")]
        public async Task<IActionResult> GetUserSubAccountDownload()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoUserSubAccount.GetUserSubAccountDownload();
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
        /// Get all channel data user to subaccount mapping data for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/user-subaccount/channel", Name = "mapuser-subaccount_channel_dropdown")]
        public async Task<IActionResult> GetChannelforUserSubAccount()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoUserSubAccount.GetChannelforUserSubAccount();
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
        /// Get all userid data user to subaccount mapping data for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/user-subaccount/userid", Name = "mapuser-subaccount_userid_dropdown")]
        public async Task<IActionResult> GetUserIdUserSubAccount()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoUserSubAccount.GetUserIdUserSubAccount();
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
        /// Get all subchannel data for user to subaccount mapping data dropdown
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/user-subaccount/subchannel", Name = "mapuser-subaccount_subchannel_dropdown")]
        public async Task<IActionResult> GetSubChannelforUserSubAccount([FromQuery] int ChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoUserSubAccount.GetSubChannelforUserSubAccount(ChannelId);
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
        /// Get all account data for user to subaccount mapping data dropdown
        /// </summary>
        /// <param name="SubChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/user-subaccount/account", Name = "mapuser-subaccount_account_dropdown")]
        public async Task<IActionResult> GetAccountforUserSubAccount([FromQuery] int SubChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoUserSubAccount.GetAccountforUserSubAccount(SubChannelId);
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
        /// Get all account data for user to subaccount mapping data dropdown
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/user-subaccount/subaccount", Name = "mapuser-subaccount_subaccount_dropdown")]
        public async Task<IActionResult> GetSubAccountforUserSubAccount([FromQuery] int AccountId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoUserSubAccount.GetSubAccountforUserSubAccount(AccountId);
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
        /// Create user to subaccount mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/mapping/user-subaccount", Name = "mapuser-subaccount_store")]
        public async Task<IActionResult> CreateUserSubAccount([FromBody] UserSubAccountPostBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    UserSubAccountCreate __bodytoken = new()
                    {
                        UserId = body.profileId,
                        SubAccountId = body.subAccountId,
                        CreateBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoUserSubAccount.CreateUserSubAccount(__bodytoken);
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
        /// Delete user to subaccount mapping data 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/mapping/user-subaccount", Name = "mapuser-subaccount_delete")]
        public async Task<IActionResult> DeleteUserSubAccount([FromBody] UserSubAccountDeleteBody body)
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
                    UserSubAccountDelete __bodytoken = new()
                    {
                        Id = body.id,
                        DeletedBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoUserSubAccount.DeleteUserSubAccount(__bodytoken);
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