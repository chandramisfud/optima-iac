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
    /// User Profile handler
    /// </summary>
    public partial class UserAccessController : BaseController
    {
        /// <summary>
        /// Get User Profile with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/userprofile", Name = "get_userprofileLP")]
        public async Task<IActionResult> GetUserProfile([FromQuery] userProfileLPParam param)
        {
            IActionResult result;
            try
            {
                var __val = await __userProfileRepo.GetUserProfileWithPaging(param.Search!,
                    param.usergroupid!, param.userlevel, param.Status.ToString(),
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
        /// Get Data User Profile by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/userprofile/id", Name = "get_userprofile_byprimarykey")]
        public IActionResult GetById(string id)
        {
            try
            {
                var __res = __userProfileRepo.GetById(id);
                if (__res == null)
                {
                    return UnprocessableEntity(
                        new BaseResponse
                        {
                            error = true,
                            code = 204,
                            message = MessageService.DataNotFound
                        }
                    );
                }
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    values = __res.Result,
                    message = MessageService.GetDataSuccess
                });

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get List User Group
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/useraccess/userprofile/usergroup", Name = "userprofile_list_usergroup")]
        public async Task<IActionResult> GetListUserGroup()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __userProfileRepo.GetUserGroupMenuList();
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
        /// Get list category
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/useraccess/userprofile/category", Name = "userprofile_list_category")]
        public async Task<IActionResult> GetListProfileCategory()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __userProfileRepo.GetProfileListCategory();
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
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Ok(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get List User Rights by User Group ID
        /// </summary>
        /// <param name="UserGroupId"></param>
        /// <returns></returns>
        [HttpGet("api/useraccess/userprofile/userrights", Name = "userprofile_list_userrights")]
        public async Task<IActionResult> GetListUserRights([FromQuery] string UserGroupId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __userProfileRepo.GettUserRightsList(UserGroupId);
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
        /// Create User Profile
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("api/useraccess/userprofile", Name = "userprofile_store")]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserAccessProfileInsertParam user)
        {
            IActionResult? result;
            try
            {
                var user_profile = __userProfileRepo.GetById(user.id!);

                if (user_profile.Result == null)
                {

                    string tokenHeader = Request.Headers["Authorization"]!;
                    tokenHeader = tokenHeader.Replace("Bearer ", "");
                    var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);
                    if (__res.UserEmail != null)
                    {
                        Repositories.Entities.Dtos.UserProfileInsertDto userProfile = new()
                        {
                            id = user.id,
                            username = user.username,
                            email = user.email,
                            password = "Optima.2023",
                            department = user.department,
                            jobtitle = user.jobtitle,
                            usergroupid = user.usergroupid,
                            userlevel = user.userlevel,
                            distributorlist = new List<UserProfileDistributorlist>(),
                            categoryId = new List<int>(),
                            userProfile = __res.ProfileID,
                            userEmail = __res.UserEmail,
                            channelId = new List<int>()
                        };
                        foreach (var item in user.distributorlist!)
                        {
                            userProfile.distributorlist.Add(new UserProfileDistributorlist
                            {
                                userId = item.userId,
                                distributorId = item.distributorId
                            });
                        }
                        foreach (var item in user.CategoryId)
                        {
                            userProfile.categoryId.Add(item);
                        }
                       
                            userProfile.channelId.AddRange(user.channelId);
                        
                        await __userProfileRepo.CreateUser(userProfile);
                        result = Ok(new BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess });
                    }
                    else
                    {
                        result = Ok(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                    }
                }
                else
                {
                    result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = "Profile ID is already exist" });
                }

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("api/useraccess/userprofile", Name = "userprofile_update")]
        public async Task<IActionResult?> UpdateUserProfile([FromBody] UserAccessProfileInsertParam user)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);
                if (!String.IsNullOrEmpty(__res.ProfileID))
                {
                    Repositories.Entities.Dtos.UserProfileInsertDto userProfile = new()
                    {
                        id = user.id,
                        username = user.username,
                        email = user.email,
                        password = "Optima.2023",
                        department = user.department,
                        jobtitle = user.jobtitle,
                        usergroupid = user.usergroupid,
                        userlevel = user.userlevel,
                        distributorlist = new List<UserProfileDistributorlist>(),
                        categoryId = new List<int>(),
                        userProfile = __res.ProfileID,
                        userEmail = __res.UserEmail,
                        channelId = new List<int>()
                    };

                    foreach (var item in user.distributorlist!)
                    {
                        userProfile.distributorlist.Add(new UserProfileDistributorlist { userId = item.userId, distributorId = item.distributorId });
                    }

                    foreach (var item in user.CategoryId)
                    {
                        userProfile.categoryId.Add(item);
                    }
                    foreach (var item in user.channelId)
                    {
                        if (item>0)
                            userProfile.channelId.Add(item);
                    }
                    var __res2 = await __userProfileRepo.UpdateUser(userProfile);

                    if (__res2)
                    {
                        return Ok(new BaseResponse { error = false, code = 200, message = MessageService.UpdateSuccess });
                    }
                    else
                    {
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        ///  Delete userprofile - make user profile inactive
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete("api/useraccess/userprofile", Name = "userprofile_delete")]
        public async Task<IActionResult> DeleteUserProfile([FromBody] userIdParam param)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);

                if (!String.IsNullOrEmpty(__res.ProfileID))
                {
                    UserUpdateDto userUpdateDto = new()
                    {
                        id = param.id,
                        deletedby = __res.ProfileID,
                        deletedon = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        isdeleted = 1
                    };
                    var __resp = await __userProfileRepo.DeleteUser(userUpdateDto);
                    if (__resp)
                    {
                        result = Ok(new BaseResponse { error = false, code = 200, message = MessageService.DeleteSucceed });
                    }
                    else
                    {
                        result = Ok(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
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
        ///  Activate userprofile 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/useraccess/userprofile/activate", Name = "userprofile_activate")]
        public async Task<IActionResult> ActivateUserProfile([FromBody] userIdParam param)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);

                if (!String.IsNullOrEmpty(__res.ProfileID))
                {
                    UserUpdateDto userUpdateDto = new()
                    {
                        id = param.id,
                        deletedby = __res.ProfileID,
                        deletedon = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        isdeleted = 0
                    };
                    var __resp = await __userProfileRepo.DeleteUser(userUpdateDto);
                    if (__resp)
                    {
                        result = Ok(new BaseResponse { error = false, code = 200, message = MessageService.DeleteSucceed });
                    }
                    else
                    {
                        result = Ok(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
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
        /// Get List Distributor
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/useraccess/userprofile/distributor", Name = "userprofile_list_distributor")]
        public async Task<IActionResult> GetProfileDistributor()
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);

                if (!String.IsNullOrEmpty(__res.ProfileID))
                {

                    List<DistributorSelect> __val = await __userProfileRepo.GetUserDistributor(__res.ProfileID);
                    if (__val.Count > 0)
                    {
                        result = Ok(new BaseResponse { code = 200, error = false, values = __val, message = MessageService.GetDataSuccess });
                    }
                    else
                    {
                        result = Ok(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                    }
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// get active channel list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/useraccess/userprofile/channellist", Name = "useraccess_profile_channnelList")]
        public async Task<IActionResult> GetProfileChannnelList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __userProfileRepo.GetChannelList();
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
