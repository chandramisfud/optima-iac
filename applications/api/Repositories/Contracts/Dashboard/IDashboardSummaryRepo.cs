using Repositories.Entities.Models.Dashboard;

namespace Repositories.Contracts
{
    public interface IDashboardSummaryRepo
    {
        // kpiscoring/approver
        Task<KPIScoringApproverDashboardSummary> GetKPIScoringApprover(DateTime promostart, DateTime promoend);

        // kpiscoring/league
        Task<KPIScoringLeagueDashboardSummary> GetKPIScoringLeagues(DateTime create_from, DateTime create_to, string ChannelDesc);

        // kpiscoring/standing
        Task<KPIScoringStandingDashboardSummary> GetKPIScoringStanding(DateTime create_from, DateTime create_to);

        // kpiscoring/dashboard
        Task<IList<KPIScoringDashboard_DashboardSummary>> GetKPIScoringDashboard(DateTime create_from, DateTime create_to, bool period_monitoring, DateTime date_monitoring);

        // kpiscoreboard/detail/all
        Task<IList<KPIScoringDetailDashboardSummary>> GetKPIScoringDetail(DateTime create_from, DateTime create_to, bool period_monitoring, DateTime date_monitoring);

        // kpiscoring/summaries
        Task<IList<KPIScoringSummaryDashboardSummary>> GetKPIScoringSummaries(DateTime create_from, DateTime create_to);

        // kpiscoring/all
        Task<IList<KPIScoringDashboardSummary>> GetKPIScoring(DateTime create_from, DateTime create_to);

    }
}


// summaryapproverandleague —> kpiscoring/approver
// summaryleaguesummary —> kpiscoring/league
// summarycreator —> kpiscoring/dashboard
// summaryleaguestanding —> kpiscoring/standing
// excel_detail —> kpiscoreboard/detail/all
// excel_export —> kpiscoring/summaries