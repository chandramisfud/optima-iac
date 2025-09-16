using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
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
        /// Get user group rights with pagination filter
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/usergrouprights", Name = "get_user_groupRightsLP")]
        public async Task<IActionResult> GetUserGroupRights([FromQuery] usersLevelLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);
                if (__res.UserEmail != null)
                {
                    var __val = await __userLevelRepo.GetUserLevelWithPaging(param.Search!, param.usergroupid!,
                        param.SortColumn.ToString(), param.SortDirection.ToString(), param.PageSize, param.PageNumber);
                    result = Ok(new BaseResponse { error = false, code = 200, values = __val, message = MessageService.GetDataSuccess });
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// get user group by userlevel id
        /// </summary>
        /// <param name="userlevel"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/usergrouprights/id", Name = "get_user_groupRightsByID")]
        public async Task<IActionResult> GetUserGroupRightsByID(int userlevel)
        {
            IActionResult result;
            try
            {

                var __val = await __userLevelRepo.GetUserLevelById(userlevel);
                if (__val != null)
                {
                    result = Ok(new BaseResponse { error = false, code = 200, values = __val, message = MessageService.GetDataSuccess });
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Delete user group rights
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete("api/useraccess/usergrouprights", Name = "delete_user_groupRights")]
        public async Task<IActionResult> DeleteUserGroupRights([FromBody] userLevelIdParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);
                if (__res.UserEmail != null)
                {
                    var __val = await __userLevelRepo.DeleteUserLevel(param.userlevel);
                    if (__val)
                    {
                        result = Ok(new BaseResponse { error = false, code = 200, values = __val, message = MessageService.DeleteSucceed });
                    }
                    else
                    {
                        result = Ok(new BaseResponse { error = true, code = 404, message = MessageService.DeleteFailed });
                    }
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Create new user group
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/useraccess/usergrouprights", Name = "create_user_groupRights")]
        public async Task<IActionResult> createUserGroup(userLevelCreateParam param)
        {
            IActionResult result;
            try
            {
                var __val_user = await __userLevelRepo.GetUserLevelById(param.userlevel);
                if (__val_user == null)
                {
                    string tokenHeader = Request.Headers["Authorization"]!;
                    tokenHeader = tokenHeader.Replace("Bearer ", "");
                    var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);
                    if (__res.UserEmail != null)
                    {
                        userLevelCreate userLevel = new()
                        {
                            levelname = param.levelname!,
                            usergroupid = param.usergroupid!,

                            userlevel = param.userlevel,
                            byUserEmail = __res.UserEmail,
                            byUserName = __res.ProfileID!
                        };
                        var __val = await __userLevelRepo.CreateUserLevel(userLevel, false);
                        result = Ok(new BaseResponse { error = false, code = 200, values = __val, message = MessageService.GetDataSuccess });
                    }
                    else
                    {
                        result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                    }
                }
                else
                {
                    result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = "User Group Rights ID is already exist" });
                }

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// update user group
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/useraccess/usergrouprights", Name = "update_user_groupRights")]
        public async Task<IActionResult> UpdateUserGroup(userLevelCreateParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);
                if (__res.UserEmail != null)
                {
                    userLevelCreate userLevel = new()
                    {
                        levelname = param.levelname!,
                        usergroupid = param.usergroupid!,
                        userlevel = param.userlevel,
                        byUserEmail = __res.UserEmail,
                        byUserName = __res.ProfileID!
                    };
                    var __val = await __userLevelRepo.CreateUserLevel(userLevel, true);
                    result = Ok(new BaseResponse { error = false, code = 200, values = __val, message = MessageService.GetDataSuccess });
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
    }
}
