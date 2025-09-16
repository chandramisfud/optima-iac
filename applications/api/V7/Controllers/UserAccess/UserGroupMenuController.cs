using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using V7.Controllers;
using V7.MessagingServices;
using V7.Model.UserAccess;
using V7.Services;
//using WebAPI.Model;

namespace WebAPI.Controllers
{
    public partial class UserAccessController : BaseController
    {
        /// <summary>
        /// get data user group with paginate filter
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/usergroupmenu", Name = "useraccess_UserGroupMenu_lp")]
        public async Task<IActionResult> GetUserGroupMenuLandingPage([FromQuery] UserGroupMenuListRequestParam body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoUserGroup.GetUserGroupMenuLandingPage(body.Search!, body.SortColumn.ToString(),
                    body.SortDirection.ToString(), body.PageSize, body.PageNumber);
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

                    return NotFound(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// get user group by id
        /// </summary>
        /// <param name="usergroupid"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/usergroupmenu/id", Name = "useraccess_UserGroupMenu_id")]
        public async Task<IActionResult> GetUserGroupMenuById([FromQuery] string usergroupid)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    var __val = await __repoUserGroup.GetUserGroupMenuById(usergroupid);
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
                        return NotFound(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
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
        /// edit user group
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/useraccess/usergroupmenu", Name = "useraccess_UserGroupMenu_update")]
        public async Task<IActionResult> UpdateUserGroupMenu([FromBody] UserGroupMenuCreateBodyParam body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                // get token from request header
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    UserGroupMenuUpdate __bodytoken = new()
                    {
                        usergroupid = body.usergroupid!,
                        usergroupname = body.usergroupname,
                        groupmenupermission = body.groupmenupermission,
                        useredit = __res.ProfileID!,
                        ModifiedEmail = __res.UserEmail
                    };
                    var __val = await __repoUserGroup.UpdateUserGroupMenu(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Update data " + __val.usergroupid + " success", values = __val });
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
        /// deleting user group
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete("api/useraccess/usergroupmenu", Name = "useraccess_UserGroupMenu_delete")]
        public async Task<IActionResult> DeleteUserGroupMenu([FromBody] userGroupIdParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                // get token from request header
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {

                    bool __val = await __repoUserGroup.DeleteUserGroupMenu(param.usergroupid!, __res.UserEmail, __res.ProfileID!);
                    if (__val)
                    {
                        return Ok(new BaseResponse { code = 200, error = false, message = "Delete data " + param.usergroupid + " success" });
                    }
                    else
                    {
                        return Ok(new BaseResponse { code = 404, error = true, message = "Gagal Delete data " + param.usergroupid });
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
        /// Createing user group
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/useraccess/usergroupmenu", Name = "usergroupmenu_store")]
        public async Task<IActionResult> CreateUserGroupMenu([FromBody] UserGroupMenuCreateBody body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var __val_user = await __repoUserGroup.GetUserGroupMenuById(body.usergroupid!);
                if (__val_user == null)
                {
                    // get token from request header
                    string tokenHeader = Request.Headers["Authorization"]!;
                    tokenHeader = tokenHeader.Replace("Bearer ", "");
                    // get user data from token
                    var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                    if (__res.UserEmail != null)
                    {
                        UserGroupMenuCreate __bodytoken = new()
                        {
                            usergroupid = body.usergroupid!,
                            usergroupname = body.usergroupname!,
                            groupmenupermission = body.groupmenupermission,
                            userinput = __res.ProfileID!,
                            CreatedEmail = __res.UserEmail
                        };
                        var __val = await __repoUserGroup.CreateUserGroupMenu(__bodytoken);
                        return Ok(new BaseResponse { code = 200, error = false, message = "Store data " + __val.usergroupid + " success", values = __val });
                    }
                    else
                    {
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = "User Group ID is already exist" });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get All menu
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/usergroupmenu/menus", Name = "get_user_group_menu")]
        public async Task<IActionResult> GetUserGroupRightsMenu([FromQuery] userGroupIdParam param)
        {
            IActionResult result;
            try
            {
                var __val = await __repoUserGroup.GetMenuAccesByGroupId(param.usergroupid!);
                result = Ok(new BaseResponse { error = false, code = 200, values = __val, message = MessageService.GetDataSuccess });

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Save Configuration Menu by User Group ID
        /// </summary>
        /// <param name="userRightPostDto"></param>
        /// <returns></returns>
        [HttpPost("api/useraccess/usergroupmenu/menus", Name = "save_user_groupGroup_menu")]
        public async Task<IActionResult> SaveUserGroupRightsMenu([FromBody] UserRightPostDto userRightPostDto)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    var _res = await __repoUserGroup.InsertUserRights(__res.UserEmail, userRightPostDto);
                    if (_res)
                    {
                        result = Ok(new BaseResponse { error = false, code = 200, message = MessageService.SaveSuccess });
                    }
                    else
                    {
                        result = Ok(new BaseResponse { error = true, code = 404, message = MessageService.SaveFailed });

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
        /// Get List User Rights by User Group Id
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/usergroup/menus/userlevel", Name = "usergroup_permissionmenu_list_userlevel")]
        public async Task<IActionResult> GetListUserLevelByUserGroup([FromQuery] string userGroupId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoUserGroup.GetUserLevelByUserGroupId(userGroupId);
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

        /// <summary>
        /// Get Permission Menu
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/usergroup/menus/permissionmenu", Name = "usergroup_permissionmenu")]
        public async Task<IActionResult> GetLevelId([FromQuery] userLevelParam param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await __repoUserGroup.GetMenuByLevelId(param.userlevelid!);
                if (result == null)
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                }

                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    values = result,
                    message = MessageService.GetDataSuccess
                });

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message
                });
            }
        }
        /// <summary>
        /// Get Permission Access
        /// </summary>
        /// <param name="cRUDMenuDto"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/usergroupmenu/menus/permissionaccess", Name = "userrights_permissionaccess")]
        public async Task<IActionResult> GetLevelMenuId([FromQuery] V7.Model.UserAccess.CRUDMenuDto cRUDMenuDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await __repoUserGroup.GetLevelMenuById(cRUDMenuDto.usergroupid!, cRUDMenuDto.userlevel, cRUDMenuDto.menuid!);
                if (result == null)
                {
                    return Ok(new BaseResponse { error = true, code = 204, message = MessageService.DataNotFound });
                }
                return Ok(
                    new BaseResponse
                    {
                        code = 200,
                        error = false,
                        values = new
                        {
                            Flag = result.flag,
                            Crud = result.crud,
                            Approve = result.approve,
                            C = result.create_rec,
                            R = result.read_rec,
                            U = result.update_rec,
                            D = result.delete_rec
                        },
                        message = MessageService.GetDataSuccess
                    }
                );
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message
                });
            }
        }
        /// <summary>
        /// Save Permission Access
        /// </summary>
        /// <param name="userLevelAccess"></param>
        /// <returns></returns>
        [HttpPost("api/useraccess/usergroupmenu/menus/permissionaccess", Name = "userrights_savelevelmenu")]
        public async Task<IActionResult> SaveLevelMenu([FromBody] UserLevelAccess userLevelAccess)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await __repoUserGroup.SaveLevelMenu(userLevelAccess);
                return Ok(new BaseResponse { error = false, code = 200, message = MessageService.SaveSuccess });
            }

            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }
    }
}