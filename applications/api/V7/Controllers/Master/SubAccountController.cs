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
        /// Get sub account data base on sub account Id
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/subaccount/id", Name = "master_SubAccount_id")]
        public async Task<IActionResult> GetSubAccountById([FromQuery] SubAccountById body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoSubAccount.GetSubAccountById(body);
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
        /// Get channel dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/subaccount/channel", Name = "master_SubAccount_channel_dropdown")]
        public async Task<IActionResult> GetChannelforSubAccount()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoSubAccount.GetChannelforSubAccount();
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
        /// Get sub channel dropdown data
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/master/subaccount/subchannel", Name = "master_SubAccount_subchannel_dropdown")]
        public async Task<IActionResult> GetSubChannelforSubAccount([FromQuery] int ChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoSubAccount.GetSubChannelforSubAccount(ChannelId);
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
        /// Get account dropdown data
        /// </summary>
        /// <param name="SubChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/master/subaccount/account", Name = "master_SubAccount_account_dropdown")]
        public async Task<IActionResult> GetAccountforSubAccount([FromQuery] int SubChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoSubAccount.GetAccountforSubAccount(SubChannelId);
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
        /// Create sub account data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/master/subaccount", Name = "SubAccount_store")]
        public async Task<IActionResult> CreateSubAccount([FromBody] SubAccountCreate body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    SubAccountCreate __bodytoken = new()
                    {
                        AccountId = body.AccountId,
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        CreateBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoSubAccount.CreateSubAccount(__bodytoken);
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
        /// Modified sub account data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/master/subaccount", Name = "master_SubAccount_update")]
        public async Task<IActionResult> UpdateSubAccount([FromBody] SubAccountUpdate body)
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
                    SubAccountUpdate __bodytoken = new()
                    {
                        Id = body.Id,
                        AccountId = body.AccountId,
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        ModifiedBy = __res.ProfileID,
                        ModifiedEmail = __res.UserEmail
                    };
                    var __val = await __repoSubAccount.UpdateSubAccount(__bodytoken);
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
        /// Delete sub account data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/master/subaccount", Name = "master_SubAccount_delete")]
        public async Task<IActionResult> DeleteSubAccount([FromBody] SubAccountDelete body)
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
                    SubAccountDelete __bodytoken = new()
                    {
                        Id = body.Id,
                        DeleteBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoSubAccount.DeleteSubAccount(__bodytoken);
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
        /// Get sub account landing page data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/subaccount", Name = "master_SubAccount_lp")]
        public async Task<IActionResult> GetSubAccountLandingPage([FromQuery] SubAccountListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoSubAccount.GetSubAccountLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
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