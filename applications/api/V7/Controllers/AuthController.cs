using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using V7.MessagingServices;
using V7.Model;
using V7.Services;

namespace V7.Controllers
{
    /// <summary>
    /// Controller that handle user authentication
    /// https://restfulapi.net/http-status-codes/
    /// </summary>
    public class AuthController : BaseController
    {
        private readonly IAuthUserRepository __repoUser;
        private readonly IConfiguration __config;
        private readonly IToolsFileRepository __repoFile;
        private readonly IFileService __serviceFile;
        private readonly IAuthMenuRepository __repoMenu;
        private readonly IToolsEmailRepository __repoEmail;
        private readonly string __TokenSecret;

        /// <summary>
        /// controller injected with other interface that will used inside
        /// </summary>
        /// <param name="repoUser"></param>
        /// <param name="fileRepo"></param>
        /// <param name="config"></param>
        /// <param name="serviceFile"></param>
        /// <param name="repoMenu"></param>
        /// <param name="repoEmail"></param>
        public AuthController(IAuthUserRepository repoUser
        , IToolsFileRepository fileRepo
        , IConfiguration config
        , IFileService serviceFile
        , IAuthMenuRepository repoMenu
        , IToolsEmailRepository repoEmail
        )
        {
            __repoUser = repoUser;
            __config = config;
            __repoFile = fileRepo;
            __serviceFile = serviceFile;
            __repoMenu = repoMenu;
            __repoEmail = repoEmail;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
        }

        /// <summary>
        /// succesfully login. will provide user profiles
        /// </summary>
        /// <param name="Auth"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/auth/login", Name = "authLogin")]
        public async Task<ActionResult> Login([FromBody] Login Auth)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            ActionResult result;
            try
            {
                Repositories.Entities.Dtos.User userresult = await __repoUser.DoLogin(Auth.Id!, Auth.Password!);
                if (userresult != null)
                {
                    if (userresult.errCode > 0 && userresult.errCode != 99)
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, new BaseResponse
                        {
                            error = true,
                            code = 401,
                            values = new
                            {
                                ErrCode = userresult.errCode,
                                ErrMessage = userresult.errMessage,
                                LoginFailedCount = userresult.loginFailedCount,
                                LoginFailedlastTime = userresult.loginFailedLastTime,
                                LoginFreezeTime = userresult.loginFreezeTime
                            },
                            message = MessageService.LoginFailed
                        }
                        );
                    }
                    // get profile by email
                    List<UserProfileCategory> usrProfile = await __repoUser.GetProfile(Auth.Id!);
                    // fill the category


                    AuthClaim userClaim = new()
                    {
                        ProfileID = usrProfile != null ? usrProfile[0].profileid! : "",
                        UserEmail = userresult.email!,
                        UserGroupID = usrProfile != null ? usrProfile[0].usergroupid! : "",
                        UserLevel = usrProfile != null ? usrProfile[0].userlevel.ToString() : "",
                        // added andrie Sept 27 2023 #33
                        ProfileCategories = usrProfile?[0].ProfileCategories!.ToList(),
                    };

                    int tokenAge = __repoUser.GetTokenAge();
                    var tokenString = TokenManager.GenerateToken(__TokenSecret, userClaim, tokenAge);
                    string hostUrl = Request.Scheme + "://" + Request.Host.Value + "/api/tools/file/view/";
                    result = Ok(new BaseResponse
                    {
                        code = 200,
                        values = new
                        {
                            ErrCode = userresult.errCode
                        ,
                            ErrMessage = userresult.errMessage
                        ,
                            Id = userresult.id
                        ,
                            UserName = userresult.username
                        ,
                            UserGroupId = userresult.usergroupid
                        ,
                            UserLevel = userresult.userlevel
                        ,
                            IsLogin = userresult.isLogin
                        ,
                            Cnt = userresult.cnt
                        ,
                            Password_Change = userresult.password_change
                        ,
                            GroupMenuPermission = userresult.groupmenupermission
                        ,
                            Email = userresult.email
                        ,
                            ContactInfo = userresult.contactinfo
                        ,
                            ProfilePictureUrl = hostUrl + userresult.pictureprofilefile
                        ,
                            UserNew = userresult.usernew
                        ,
                            LoginFailedCount = userresult.loginFailedCount
                        ,
                            LoginFailedlastTime = userresult.loginFailedLastTime
                        ,
                            LoginFreezeTime = userresult.loginFreezeTime
                        ,
                            Profile = usrProfile
                        ,
                            Token = tokenString
                        ,
                            TokenExpiredTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone).AddMinutes(tokenAge)
                        },
                        message = MessageService.LoginSuccess,
                        error = false
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new BaseResponse
                    {
                        error = true,
                        code = 401,
                        values = new
                        {
                            ErrCode = 0,
                            ErrMessage = "User Not Found"
                        },
                        message = MessageService.LoginFailed
                    }
                        );
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// construct url for viewing server files
        /// </summary>
        /// <returns></returns>
        private string GetViewUrl()
        {
            return Request.Scheme + "://" + Request.Host.Value + "/api/tools/file/view";
        }

        /// <summary>
        /// To reset user IsLogin data
        /// </summary>
        /// <returns></returns>
        [HttpPut("api/auth/resetislogin", Name = "auth_resetislogin")]
        public ActionResult ResetIsLogin()
        {
            ActionResult result;
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            try
            {
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    var user = __repoUser.ResetIsLogin(__res.UserEmail).Result;
                    result = Ok(new BaseResponse { error = false, code = 200, values = 0, message = MessageService.UpdateSuccess });
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
        /// Get user IsLogin data
        /// </summary>
        /// <remarks>
        /// This will return the number of IsLogin
        /// </remarks>
        /// <returns></returns>
        [HttpGet("api/auth/islogin", Name = "auth_islogin")]
        public ActionResult IsLogin()
        {
            ActionResult result;
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            try
            {
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    var user = __repoUser.GetUserByEmail(__res.UserEmail).Result;
                    result = Ok(new BaseResponse { error = false, code = 200, values = user.isLogin, message = MessageService.GetDataSuccess });
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
        /// update after user login
        /// </summary>
        /// <remarks>
        /// This will return the number of IsLogin
        /// </remarks>
        /// <returns></returns>
        [HttpPut("api/auth/islogin", Name = "auth_updateIslogin")]
        public ActionResult UpdateIsLogin()
        {
            ActionResult result;
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            try
            {
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    var islogin = __repoUser.UpdateIsLogin(__res.UserEmail).Result;
                    result = Ok(new BaseResponse { error = false, code = 200, values = islogin, message = MessageService.UpdateSuccess });
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
        /// Claim user data  based on token
        /// </summary>
        /// <remarks>return:  (useremail, profileid, usergroupid, userlevel)</remarks>
        /// <returns>Auth Claim</returns>
        [HttpPost("api/auth/claim", Name = "auth_claim")]
        public ActionResult AuthClaim()
        {
            ActionResult result;

            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            try
            {
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    result = Ok(new BaseResponse { error = false, code = 200, values = __res, message = MessageService.GetDataSuccess });
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, values = __res, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                //result = BadRequest(new BaseResponse { error = true, code = 400, message = __ex.Message }); 
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// replace old password with new
        /// </summary>
        /// <remarks>
        /// No Return Value
        /// </remarks>
        /// <param name="body">Body Param</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/auth/changepassword/", Name = "auth_changepassword")]
        public ActionResult ChangePassword([FromBody] Repositories.Entities.Dtos.UserPasswordChange body)
        {
            ActionResult result;
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var yangDiharapkan = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone);
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse { error = true, code = 400, message = "Param required" });
                }
                if (__repoUser.ChangePassword(body).Result)
                {
                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.ChangePasswordSuccess,
                        values = new
                        {
                            UtcTime = utcTime,
                            Zone = zone,
                            Harapan = yangDiharapkan
                        }
                    });
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.UserNotFound });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// replace old password with new
        /// </summary>
        /// <remarks>Validate proses: select data based on email</remarks>
        /// <param name="body">Body Param</param>
        /// <returns></returns>

        [HttpPost("api/auth/changeoldpassword/", Name = "auth_changeoldpassword")]
        public ActionResult ChangeOldPassword([FromBody] Repositories.Entities.Dtos.UserOldPasswordChange body)
        {
            ActionResult result;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse { error = true, code = 400, message = "Param required" });
                }
                string tokenHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var UserClaim = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__repoUser.ChangeOldPassword(body, UserClaim.UserEmail).Result)
                {
                    result = Ok(new BaseResponse { error = false, code = 200, message = MessageService.ChangePasswordSuccess });
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.UserPasswordNotCorrect });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Change user profile after login. get user email from token claim
        /// </summary>
        /// <remarks>return values
        ///     {
        ///                Token,
        ///                UserGroupID,
        ///                UserGroupName,
        ///                UserLevel,
        ///                userProfileId,
        ///                tokenExpiredTime,
        ///                categoryId
        ///     }
        /// 
        /// </remarks>
        /// <param name="profileID">Profile ID from login response</param>
        /// <returns></returns>        
        [HttpPost("api/auth/token/changeprofile/", Name = "auth_tokenchangeprofile")]
        public async Task<ActionResult> ChangeProfile([FromForm] string profileID)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            ActionResult result;
            try
            {
                if (!ModelState.IsValid)
                {
                    string errorMessage = "The request param is invalid.";
                    var invalidField = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage)).ToList();
                    errorMessage += String.Join(" ", invalidField);
                    return BadRequest(new BaseResponse { error = true, code = 400, message = errorMessage });
                }
                // Get user login info from token
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var UserClaim = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                List<UserProfileCategory> usrProfile = await __repoUser.GetProfile(UserClaim.UserEmail);
                var profiles = usrProfile.Where(x => x.profileid == profileID);
                if (profiles != null && profiles.Any())
                {
                    UserProfileCategory newProfileInfo = profiles.First();
                    // update claim based on new profile info
                    UserClaim.UserGroupID = newProfileInfo.usergroupid!;
                    UserClaim.UserLevel = newProfileInfo.userlevel.ToString();
                    UserClaim.ProfileID = profileID;
                    // added andrie Sept 27 2023 #33
                    UserClaim.ProfileCategories = newProfileInfo.ProfileCategories;

                    int tokenAge = __repoUser.GetTokenAge();
                    string newToken = TokenManager.GenerateToken(__TokenSecret, UserClaim, tokenAge);
                    ChangeProfileResponse profile = new()
                    {
                        Token = newToken,
                        TokenExpiredTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone).AddMinutes(tokenAge),
                        UserGroupID = newProfileInfo.usergroupid,
                        UserGroupName = newProfileInfo.usergroupname,
                        UserProfileId = newProfileInfo.profileid,
                        UserLevel = newProfileInfo.userlevel.ToString(),
                        // added andrie Sept 27 2023 #33
                        ProfileCategories = newProfileInfo.ProfileCategories!.ToList()
                    };

                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.SaveSuccess,
                        values = profile
                    });
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 200, message = MessageService.ProfileNotFound }); ;
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Upload files, generate filecode, update user pict profile. 
        /// </summary>
        /// <remarks>
        /// Return: New pict profile url
        /// </remarks>
        /// <param name="body">
        /// param is required
        /// </param>
        [HttpPost("api/auth/picture", Name = "authPictureStore")]
        public IActionResult Store([FromForm] Repositories.Entities.Dtos.PictureProfileBody body)
        {
            IActionResult res;
            try
            {
                // Get user login info from token
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var UserClaim = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (!ModelState.IsValid)
                {
                    res = BadRequest(new BaseResponse { error = true, code = 400, message = "Param required" });
                }
                else
                {
                    string dirPath = __config.GetSection("FileStorageDir").Value!;
                    string uniqCode = __repoFile.GetUniqCode();
                    if (__serviceFile.UploadFile(body.FormFile!, dirPath, uniqCode))
                    {
                        //save file info
                        var filecode = __repoFile.Insert(body.FormFile!.FileName, uniqCode).Result;
                        //update user profile picture file code
                        if (__repoUser.UpdateProfilePicture(UserClaim.UserEmail, uniqCode).Result)
                        {
                            res = Ok(new BaseResponse { error = false, code = 200, values = GetViewUrl() + "/" + filecode, message = MessageService.UpdateSuccess });
                        }
                        else
                        {
                            res = StatusCode(StatusCodes.Status304NotModified, new BaseResponse { error = true, code = 304, message = MessageService.UpdateFailed });
                        }
                    }
                    else
                    {
                        res = StatusCode(StatusCodes.Status304NotModified, new BaseResponse { error = true, code = 304, message = MessageService.UploadFailed });
                    }
                }
            }
            catch (Exception __ex)
            {
                res = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return res;
        }

        /// <summary>
        /// upload file, Update profile picture filename in table. Pict profile url still the same
        /// </summary>
        /// <remarks>
        /// Return: Pict profile url
        /// </remarks>        
        /// 
        /// <param name="body">body</param>
        /// <returns>pict profile url</returns>
        [HttpPut("api/auth/picture", Name = "authPictureUpdate")]
        public IActionResult Update([FromForm] Repositories.Entities.Dtos.PictureProfileBody body)
        {
            IActionResult res;
            try
            {
                if (!ModelState.IsValid)
                {
                    res = BadRequest(new BaseResponse { error = true, code = 400, message = "Param required" });
                }
                else
                {
                    // Get user login info from token
                    string tokenHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    var UserClaim = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                    string dirPath = __config.GetSection("FileStorageDir").Value!;
                    var usr = __repoUser.GetUserByEmail(UserClaim.UserEmail).Result;

                    if (usr != null)
                    {
                        if (__serviceFile.UploadFile(body.FormFile!, dirPath, usr.pictureprofilefile!))
                        {
                            //save file info
                            res = Ok(new BaseResponse
                            {
                                error = false,
                                code = 200,
                                values = GetViewUrl() + "/" + usr.pictureprofilefile,
                                message = MessageService.UpdateSuccess
                            });
                        }
                        else
                        {
                            res = Ok(new BaseResponse { error = true, code = 200, message = MessageService.UploadFailed });
                        }
                    }
                    else
                    {
                        res = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.UserNotFound });
                    }
                }
            }
            catch (Exception __ex)
            {
                res = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return res;
        }

        /// <summary>
        /// Get user menu based on userGroup and userLevel
        /// </summary>
        /// <remarks>userGroup and userLevel are provide by token claim
        /// Return: Menu
        /// </remarks>
        /// <returns></returns>
        [HttpGet("api/auth/menu", Name = "authMenuByUserGroup")]
        public async Task<IActionResult> GetMenuByUserGroupLevel()
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var UserClaim = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (UserClaim != null)
                {
                    var mainMenu = await __repoMenu.GetByGroup(UserClaim.UserGroupID, UserClaim.UserLevel);
                    result = Ok(new BaseResponse { error = false, code = 200, values = mainMenu, message = MessageService.GetDataSuccess });
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
        /// Send single Email with user email exist checking (no cc, bcc, attch)
        /// </summary>
        /// <Author>andrie, May 31 2023</Author>
        /// <param name="param"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/auth/sendemail/", Name = "sendemail_withEmailExist")]
        public async Task<IActionResult> SendEmail([FromForm] EmailForgotPassBody param)
        {
            try
            {
                // Cek user exist
                var user = await __repoUser.GetUserByEmail(param.email!);
                if (user != null)
                {
                    Repositories.Entities.Dtos.EmailBody body = new();
                    List<string> emails = new()
                    {
                        param.email!
                    };
                    body.email = emails.ToArray();
                    body.body = param.body;
                    body.subject = param.subject;
                    await __repoEmail.SendEmail(body);
                    return Ok(new { error = false, code = 200, Message = "Email sent. Please check your email" });
                }
                else
                {
                    return Ok(new { error = false, code = 404, Message = "Your email address is not registered" });
                }

            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Reset and send generated new password via email
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/auth/resetpassword/", Name = "sendemail_resetpassword")]
        public async Task<IActionResult> ResetPassword([FromForm] EmailResetBody param)
        {
            try
            {
                // Cek user exist
                var user = await __repoUser.GetUserById(param.userId);
                if (user != null)
                {
                    Repositories.Entities.Dtos.EmailBody body = new();
                    string newPass = Helper.generateStrongPassword(8);
                    // update pass 
                    UserPasswordChange pass = new()
                    {
                        password = newPass,
                        email = user.email
                    };
                    await __repoUser.ChangePassword(pass);

                    List<string> emails = new()
                    {
                        user.email!
                    };
                    body.email = emails.ToArray();
                    string mailContent = @"This email was sent automatically by Optima System in responses to reset password by Administrator." +
                        "<br><p><p>User ID : {0}<br>Password : {1}<br><p><p>Thank you,<br>Optima�System";
                    body.body = String.Format(mailContent, user.email, newPass);
                    body.subject = "Reset Password by Admin";
                    await __repoEmail.SendEmail(body);
                    return Ok(new { error = false, code = 200, Message = "Email sent. Please check�your�email" });
                }
                else
                {
                    return Ok(new { error = false, code = 404, Message = "Data Not Found" });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get user profile based on user Email login
        /// </summary>
        /// <remarks>userEmail are provide by token claim
        /// Return: Menu
        /// </remarks>
        /// <returns></returns>
        [HttpGet("api/auth/profile", Name = "authProfile")]
        public async Task<IActionResult> GetProfile()
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var UserClaim = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (UserClaim != null)
                {
                    // get profile by email
                    var usrProfile = await __repoUser.GetProfile(UserClaim.UserEmail);
                    //List<UserProfile> usrProfile = await __repoUser.GetProfile(UserClaim.UserEmail);

                    result = Ok(new BaseResponse { error = false, code = 200, values = usrProfile, message = MessageService.GetDataSuccess });
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
        /// Get user profile based on user Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/auth/user/email", Name = "get_email_from_tbsetuser")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            IActionResult result;
            try
            {
                var __val = await __repoUser.GetByEmail(email);
                if (__val != null)
                {
                    result = Ok(new BaseResponse { error = false, code = 200, values = __val, message = MessageService.GetDataSuccess });
                }
                else
                {
                    result = Ok(new BaseResponse { error = true, code = 204, message = "Your email address is not registered" });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Delete profile based on user Email
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpDelete("api/auth/user/email", Name = "DELETE_email_from_tbsetuser")]
        public async Task<IActionResult> DeleteUserEmail([FromBody] EmailDeleteBody body)
        {
            IActionResult result;
            try
            {
                var __resGetEmail = await __repoUser.GetByEmail(body.email!);
                if (__resGetEmail == null)
                {
                    result = Ok(new BaseResponse { code = 204, error = true, message = "Your email address is not registered" });
                }
                else
                {
                    await __repoUser.DeleteUserEmail(body.email);
                    result = Ok(new BaseResponse { code = 200, error = false, message = MessageService.DeleteSucceed });
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