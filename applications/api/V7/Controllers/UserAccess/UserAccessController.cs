using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Models;
using V7.Controllers;

namespace WebAPI.Controllers
{
    /// <summary>
    /// User Access Controller
    /// </summary>
    public partial class UserAccessController : BaseController
    {
        private readonly IConfiguration __config;
        private readonly IUserGroupMenuRepository __repoUserGroup;
        private readonly string __TokenSecret;
        private readonly IUserRepository __userRepo;
        private readonly IUserLevelRepository __userLevelRepo;
        private readonly IUserProfileRepository __userProfileRepo;
        private readonly IUserAdminReportRepository __userAdminReport;

        public UserAccessController(IConfiguration config, IUserGroupMenuRepository repoUserGroup,
            IUserRepository userRepo, IUserLevelRepository userLevel, IUserProfileRepository userProfile, IUserAdminReportRepository userAdminReport)
        {
            __config = config;
            __repoUserGroup = repoUserGroup;
            __userRepo = userRepo;
            __userLevelRepo = userLevel;
            __userProfileRepo = userProfile;
            __userAdminReport = userAdminReport;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
        }

     
    }
}
