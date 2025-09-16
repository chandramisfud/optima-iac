using Repositories.Entities.Models.Dashboard;
using Repositories.Entities.Report;

namespace Repositories.Contracts
{
    public interface IDashboardApproverRepo
    {
        // getligastanding ——> kpiscoring/standing
        Task<KPIScoringStandingDashboardSummary> GetKPIScoringStanding(DateTime create_from, DateTime create_to);

        // getmstchannel  —> GetAllChannel
        Task<IList<ListingPromoReportingChannelList>> GetChannelList(string userid, int[] arrayParent);

        // getligaapprover —> kpiscoring/approver
        Task<KPIScoringApproverDashboardSummary> GetKPIScoringApprover(string userid, DateTime promostart, DateTime promoend);

        // summaryapproverandleagueforapproverdashboard —> kpiscoring/approver
    }
}