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
        /// Get all sku to blitz mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/sku-blitz", Name = "mapsku-blitz_list")]
        public async Task<IActionResult> GetSKUBlitzLandingPage([FromQuery] SKUBlitzListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoSKUBlitz.GetSKUBlitzLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
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
        /// Get all sku to blitz mapping data for download
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/sku-blitz/download", Name = "mapsku-blitz_download")]
        public async Task<IActionResult> GetSKUBlitzDownload()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoSKUBlitz.GetSKUBlitzDownload();
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
        /// Get entity data for sku to blitz mapping data dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/sku-blitz/entity", Name = "mapsku-blitz_distributor_dropdown")]
        public async Task<IActionResult> GetEntityforSKUBlitz()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoSKUBlitz.GetEntityforSKUBlitz();
                if (__val != null)
                {
                    return Ok(new { error = false, code = 200, Message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Conflict(new { error = true, code = 404, Message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new { error = true, code = 400, Message = __ex.Message });
            }
        }
        /// <summary>
        /// Get all brand data for sku to blitz mapping data dropdown
        /// </summary>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/sku-blitz/brand", Name = "mapsku-blitz_brand_dropdown")]
        public async Task<IActionResult> GetBrandforSKUBlitz([FromQuery] int EntityId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoSKUBlitz.GetBrandforSKUBlitz(EntityId);
                if (__val != null)
                {
                    return Ok(new BaseResponse{ error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
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
        /// Get all sku data for sku to blitz mapping data dropdown
        /// </summary>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/sku-blitz/sku", Name = "mapsku-blitz_account_dropdown")]
        public async Task<IActionResult> GetSKUforSKUBlitz([FromQuery] int BrandId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoSKUBlitz.GetSKUforSKUBlitz(BrandId);
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
        /// Create sku to blitz mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/mapping/sku-blitz", Name = "mapsku-blitz_store")]
        public async Task<IActionResult> CreateSKUBlitz([FromBody] SKUBlitzPostBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    SKUBlitzCreate __bodytoken = new()
                    {
                        SKUId = body.SKUId,
                        SAPCode = body.SAPCode,
                        CreateBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoSKUBlitz.CreateSKUBlitz(__bodytoken);
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
        /// Delete sku to blitz mapping data 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/mapping/sku-blitz", Name = "mapsku-blitz_delete")]
        public async Task<IActionResult> DeleteSKUBlitz([FromBody] SKUBlitzDeleteBody body)
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
                    SKUBlitzDelete __bodytoken = new()
                    {
                        Id = body.id,
                        DeleteBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoSKUBlitz.DeleteSKUBlitz(__bodytoken);
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