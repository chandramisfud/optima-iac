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
        /// Get all pic to dnmanual mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/pic-dnmanual", Name = "pic-dnmanual_list")]
        public async Task<IActionResult> GetPICDNManualLandingPage([FromQuery] PICDNManualListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoPICDNManual.GetPICDNManualLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
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
        /// Get all pic to dnmanual mapping data for download
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/pic-dnmanual/download", Name = "pic-dnmanual_download")]
        public async Task<IActionResult> GetPICDNManualDownload()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoPICDNManual.GetPICDNManualDownload();
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
        /// Get distributor data for pic to dnmanual mapping data dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/pic-dnmanual/userid", Name = "pic-dnmanual_userid_dropdown")]
        public async Task<IActionResult> GetUserIdPICDNManual()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoPICDNManual.GetUserIdPICDNManual();
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
        /// Get all channel data pic to dnmanual mapping data for dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/pic-dnmanual/channel", Name = "pic-dnmanual_channel_dropdown")]
        public async Task<IActionResult> GetChannelforPICDNManual()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoPICDNManual.GetChannelforPICDNManual();
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
        /// Get all subchannel data for pic to dnmanual mapping data dropdown
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/pic-dnmanual/subchannel", Name = "pic-dnmanual_subchannel_dropdown")]
        public async Task<IActionResult> GetSubChannelforPICDNManual([FromQuery] int ChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoPICDNManual.GetSubChannelforPICDNManual(ChannelId);
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
        /// Get all account data for pic to dnmanual mapping data dropdown
        /// </summary>
        /// <param name="SubChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/pic-dnmanual/account", Name = "pic-dnmanual_account_dropdown")]
        public async Task<IActionResult> GetAccountforPICDNManual([FromQuery] int SubChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoPICDNManual.GetAccountforPICDNManual(SubChannelId);
                if (__val != null)
                {
                    return Ok(new BaseResponse{ error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse{ error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse{ error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get all account data for pic to dnmanual mapping data dropdown
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/pic-dnmanual/subaccount", Name = "pic-dnmanual_subaccount_dropdown")]
        public async Task<IActionResult> GetSubAccountforPICDNManual([FromQuery] int AccountId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoPICDNManual.GetSubAccountforPICDNManual(AccountId);
                if (__val != null)
                {
                    return Ok(new  BaseResponse{ error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse{ error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// Create pic to dnmanual mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/mapping/pic-dnmanual", Name = "pic-dnmanual_store")]
        public async Task<IActionResult> CreatePICDNManual([FromBody] PICDNManualPostBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    PICDNManualCreate __bodytoken = new()
                    {
                        Pic1 = body.pic1,
                        Pic2 = body.pic2,
                        ChannelId = body.channelId,
                        SubChannelId = body.subChannelId,
                        AccountId = body.accountId,
                        SubAccountId = body.subAccountId,
                        CreatedBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoPICDNManual.CreatePICDNManual(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess, values = __val });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Delete pic to dnmanual mapping data 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/mapping/pic-dnmanual", Name = "pic-dnmanual_delete")]
        public async Task<IActionResult> DeletePICDNManual([FromBody] PICDNManualDeleteBody body)
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
                    PICDNManualDelete __bodytoken = new()
                    {
                        Id = body.id,
                        DeletedBy = __res.ProfileID,
                        DeleteBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoPICDNManual.DeletePICDNManual(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.DeleteSucceed, values = __val });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
    }
}