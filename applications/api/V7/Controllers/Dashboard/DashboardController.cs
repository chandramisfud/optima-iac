using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using V7.Controllers;
using V7.MessagingServices;

namespace V7.Controllers.Dashboard
{
    public partial class DashboardController : BaseController
    {
        private readonly IConfiguration __config;
        private readonly IDashboardMainRepo __repoDashboardMain;
        private readonly IDashboardSummaryRepo __repoDashboardSummary;
        private readonly IDashboardPromoCalendarRepo __repoDashboardPromoCalendar;
        private readonly IDashboardApproverRepo __repoDashboardApprover;
        private readonly IDashboardCreatorRepo __repoDashboardCreator;

        private readonly string __TokenSecret;
        public DashboardController(
            IDashboardMainRepo repoDashboardMain,
            IDashboardSummaryRepo repoDashboardSummary,
            IDashboardPromoCalendarRepo repoDashboarPromoCalendar,
            IDashboardApproverRepo repoDashboardApprover,
            IDashboardCreatorRepo repoDashboardCreator,
            IConfiguration config)
        {
            __config = config;
            __repoDashboardMain = repoDashboardMain;
            __repoDashboardSummary = repoDashboardSummary;
            __repoDashboardPromoCalendar = repoDashboarPromoCalendar;
            __repoDashboardApprover = repoDashboardApprover;
            __repoDashboardCreator = repoDashboardCreator;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
        }
    }
}