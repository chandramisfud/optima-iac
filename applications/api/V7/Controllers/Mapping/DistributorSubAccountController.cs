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
        /// Get all distributor to subaccount mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-subaccount", Name = "mapdistributor_list")]
        public async Task<IActionResult> GetDistributorSubAccountLandingPage([FromQuery] DistributorSubAccountListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDistributorSubAccount.GetDistributorSubAccountLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
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
        /// Get all distributor to subaccount mapping data for download
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-subaccount/download", Name = "mapdistributor_download")]
        public async Task<IActionResult> GetDistributorSubAccountDownload()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDistributorSubAccount.GetDistributorSubAccountDownload();
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
        /// Get distributor data for distributor to subaccount mapping data dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-subaccount/distributor", Name = "mapdistributor_distributor_dropdown")]
        public async Task<IActionResult> GetDistributorforDistributorSubAccount()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDistributorSubAccount.GetDistributorforDistributorSubAccount();
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
        /// Get all channel data distributor to subaccount mapping data for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-subaccount/channel", Name = "mapdistributor_channel_dropdown")]
        public async Task<IActionResult> GetChannelforDistributorSubAccount()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDistributorSubAccount.GetChannelforDistributorSubAccount();
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
        /// Get all subchannel data for distributor to subaccount mapping data dropdown
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-subaccount/subchannel", Name = "mapdistributor_subchannel_dropdown")]
        public async Task<IActionResult> GetSubChannelforDistributorSubAccount([FromQuery] int ChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDistributorSubAccount.GetSubChannelforDistributorSubAccount(ChannelId);
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
        /// Get all account data for distributor to subaccount mapping data dropdown
        /// </summary>
        /// <param name="SubChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-subaccount/account", Name = "mapdistributor_account_dropdown")]
        public async Task<IActionResult> GetAccountforDistributorSubAccount([FromQuery] int SubChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDistributorSubAccount.GetAccountforDistributorSubAccount(SubChannelId);
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
        /// Get all account data for distributor to subaccount mapping data dropdown
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-subaccount/subaccount", Name = "mapdistributor_subaccount_dropdown")]
        public async Task<IActionResult> GetSubAccountforDistributorSubAccount([FromQuery] int AccountId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDistributorSubAccount.GetSubAccountforDistributorSubAccount(AccountId);
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
        /// Create distributor to subaccount mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/mapping/distributor-subaccount", Name = "mapdistributor_store")]
        public async Task<IActionResult> CreateDistributorSubAccount([FromBody] DIstributorSubAccountPostBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    DistributorSubAccountCreate __bodytoken = new()
                    {
                        DistributorId = body.distributorId,
                        ChannelId = body.channelId,
                        SubChannelId = body.subChannelId,
                        AccountId = body.accountId,
                        SubAccountId = body.subAccountId,
                        CreateBy = __res.ProfileID!,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoDistributorSubAccount.CreateDistributorSubAccount(__bodytoken);
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
        /// Delete distributor to subaccount mapping data 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/mapping/distributor-subaccount", Name = "mapdistributor_delete")]
        public async Task<IActionResult> DeleteDistributorSubAccount([FromBody] DIstributorSubAccountDeleteBody body)
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
                    DistributorSubAccountDelete __bodytoken = new()
                    {
                        Id = body.id,
                        DeleteBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoDistributorSubAccount.DeleteDistributorSubAccount(__bodytoken);
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