using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using V7.Controllers;
using V7.MessagingServices;
using V7.Model.UserAccess;
using V7.Services;

namespace WebAPI.Controllers
{
    /// <summary>
    /// User Access Controller
    /// </summary>
    public partial class UserAccessController : BaseController
    {

        /// <summary>
        /// Get data user with paginate filter 
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/useraccess/users", Name = "get_users")]
        public async Task<IActionResult> GetUsers([FromQuery] usersLPParam param)
        {
            IActionResult result;
            try
            {
                var __val = await __userRepo.GetUserWithPaging(param.Search!, param.Status.ToString(),
                    param.SortColumn.ToString(), param.SortDirection.ToString().ToUpper(),
                    param.PageSize, param.PageNumber);
                if (__val != null)
                {
                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                }

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Delete - make user inactive
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete("api/useraccess/users", Name = "delete_users")]
        public async Task<IActionResult> DeleteUsers([FromBody] userIdParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);

                if (!String.IsNullOrEmpty(__res.ProfileID))
                {
                    var __val = await __userRepo.UserProfileDeleteStatus(param.id!, __res.ProfileID, 3);
                    if (__val)
                    {
                        result= Ok(new BaseResponse { code = 200, error = false, message = "Delete data " + param.id + " success" });
                    }
                    else
                    {
                        result= Ok(new BaseResponse { code = 404, error = true, message = MessageService.DeleteFailed });
                    }
                }
                else
                {
                    result= NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/users/id", Name = "get_usersByID")]
        public async Task<IActionResult> GetUserByID([FromQuery] string userid)
        {
            try
            {
                var __val = await __userRepo.GetUserById(userid);
                if (__val != null)
                {
                    return Ok(new BaseResponse { code = 200, error = false, values = __val, message = MessageService.GetDataSuccess });
                }
                else
                {
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// Activate user
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/useraccess/users/activate", Name = "activate_users")]
        public async Task<IActionResult> ActivateUsers([FromBody] userIdParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);

                if (!String.IsNullOrEmpty(__res.ProfileID))
                {
                    var __val = await __userRepo.UserProfileDeleteStatus(param.id!, __res.ProfileID, 4);
                    if (__val)
                    {
                        return Ok(new BaseResponse { code = 200, error = false, message = "Activate " + param.id + " success" });
                    }
                    else
                    {
                        return Ok(new BaseResponse { code = 404, error = true, message = "Failed Activate data " });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Create New User
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/useraccess/users", Name = "create_users")]
        public async Task<IActionResult> CreateUsers([FromBody] UserProfileInsertParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);
                if (__res.UserEmail != null)
                {
                    UserProfileInsert userProfile = new()
                    {
                        contactinfo = param.contactinfo,
                        email = param.email,
                        id = param.id,
                        // default passsword
                        password = "Optima.2025",
                        userid = __res.ProfileID,
                        profile = param.profile!,
                        username = param.username
                    };

                    await __userRepo.UserProfileInsert(userProfile);
                    return Ok(new { statuscode = "200", Message = MessageService.SaveSuccess });
                } else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/useraccess/users", Name = "edit_users")]
        public async Task<IActionResult> UpdateUsers([FromBody] UserProfileInsertParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);
                if (__res.UserEmail != null)
                {
                    UserProfileInsert userProfile = new()
                    {
                        contactinfo = param.contactinfo,
                        email = param.email,
                        id = param.id,
                        // default passsword
                        password = "Optima.2023",
                        userid = __res.ProfileID,
                        profile = param.profile!,
                        username = param.username
                    };
                    await __userRepo.UserProfileInsert(userProfile);
                    return Ok(new  BaseResponse { code = 200, message = MessageService.SaveSuccess });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return  StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get List User Profile
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/useraccess/users/userprofile", Name = "users_getlist_userprofile")]
        public async Task<IActionResult> GetListUserProfile()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __userRepo.GetListUserProfile();
                if (__val != null)
                {
                    return Ok(new
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new { code = 500, error = true, message = __ex.Message });
            }
        }
    }
}
