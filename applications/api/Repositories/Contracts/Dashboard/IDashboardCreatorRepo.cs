using Repositories.Entities.Models.Dashboard;

namespace Repositories.Contracts
{
    public interface IDashboardCreatorRepo
    {
        // PromoPlan/landing-page/getData/ —> promoplanning/byconditions
        // getalltsm —> kpiscoring/dashboard
        Task<IList<KPIScoringDashboard_DashboardSummary>> GetKPIScoringDashboard(DateTime create_from, DateTime create_to, string userid, bool period_monitoring, DateTime date_monitoring);

        // getligasummary —> kpiscoring/league
        Task<KPIScoringLeagueDashboardSummary> GetKPIScoringLeagues(DateTime create_from, DateTime create_to, string ChannelDesc);

        // getligastanding  —> kpiscoring/standing
        Task<KPIScoringStandingDashboardSummary> GetKPIScoringStanding(DateTime create_from, DateTime create_to);

        // summarycreatorforcreatordashboard —> kpiscoring/dashboard
        // Task<IList<KPIScoringDashboard_DashboardSummary>> GetKPIScoringDashboard(DateTime create_from, DateTime create_to, string userid, bool period_monitoring, DateTime date_monitoring);

    }
}