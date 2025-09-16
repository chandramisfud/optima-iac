using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using Repositories.Entities.Models.Dashboard;

namespace Repositories.Contracts
{
    public interface IDashboardPromoCalendarRepo
    {
        // dashboard/calendar
        Task<IList<DashboardCalendarDto>> GetDashboarPromodCalendar(string userId, int promoPlanId, string activity_desc);

        // select Entity
        Task<IList<EntityForMechanism>> GetEntityForDashboardPromoCalendar();

        // select Channel
        Task<IEnumerable<DashboardMasterbyAccess>> GetDashboardMasterChannelbyAccesses(string userId);

        // select Account
        Task<IEnumerable<DashboardMasterbyAccess>> GetDashboardMasterAccountbyAccesses(string userid, int channelid);

        // subcategory/getbyuseraccess                
        Task<IEnumerable<SubCategoryforDashboardPromoCalendar>> GetSubCategoryforDashboardPromoCalendars(string userid);

        //get all category
        Task<IList<SubCategorytoCategory>> GetCategoryListforDashboardPromoCalendar();

    }
}