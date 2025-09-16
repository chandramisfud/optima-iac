using Repositories.Contracts;
using V7.Controllers;

namespace V7.Controllers.FinanceReport
{
    /// <summary>
    /// Report Controller
    /// </summary>
    public partial class FinanceReportController : BaseController
    {
        private readonly IConfiguration __config;
        private readonly string __TokenSecret;
        private readonly IFinAccrualReportRepo __repoFinAccrualReport;
        private readonly IFinDNDetailReportingRepo __repoFinDNDetailReporting;
        private readonly IFinDNDisplayRepo __repoFinDNDisplayReport;
        private readonly IFinDocumentCompletenessRepo __repoFinDocCompleteness;
        private readonly IFinInvestmentReportRepo __repoFinInvestmentReport;
        private readonly IFinListingDNRepo __repoFinListingDN;
        private readonly IFinListingPromoReconRepo __repoFinListingPromoRecon;
        private readonly IFinListingPromoReportingRepo __repoFinListingPromoReport;
        private readonly IFinMatrixApprovalListingRepo __repoFinMatrixApproval;
        private readonly IFinPromoHistoricalMovementRepo __repoFinPromoHistoricalMovement;
        private readonly IFinPromoPlanningReportingRepo __repoFinPromoPlanningReport;
        private readonly IFinSKPValidationRepo __repoFinSKPValidation;
        private readonly IFinSummaryAgingApprovalRepo __repoFinSummaryAging;
        private readonly IFinPromoDisplayRepo __repoFinPromoDisplay;
        private readonly IFinPromoApprovalAgingRepo __repoFinPromoApprovalAging;
        private readonly IFinPromoSubmissionReportRepo __repoFinPromoSubmission;
        private readonly IFinPromoApprovalReminderRepository __repoFinPromoApprovalReminder;


        public FinanceReportController(IConfiguration config,
            IFinAccrualReportRepo repoFinAccrualReport,
            IFinDNDetailReportingRepo repoFinDNDetailReporting,
            IFinDNDisplayRepo repoFinDNDisplayReport,
            IFinDocumentCompletenessRepo repoFinDocCompleteness,
            IFinInvestmentReportRepo repoFinInvestmentReport,
            IFinListingDNRepo repoFinListingDN,
            IFinListingPromoReconRepo repoFinListingPromoRecon,
            IFinListingPromoReportingRepo repoFinListingPromoReport,
            IFinMatrixApprovalListingRepo repoFinMatrixApproval,
            IFinPromoHistoricalMovementRepo repoFinPromoHistoricalMovement,
            IFinPromoPlanningReportingRepo repoFinPromoPlanningReport,
            IFinSKPValidationRepo repoFinSKPValidation,
            IFinSummaryAgingApprovalRepo repoFinSummaryAging,
            IFinPromoDisplayRepo repoFinPromoDisplay,
            IFinPromoApprovalAgingRepo repoPromoApprovalAging,
            IFinPromoSubmissionReportRepo repoFinPromoSubmission,
            IFinPromoApprovalReminderRepository repoFinPromoApprovalReminder
        )
        {
            __config = config;
            __repoFinAccrualReport = repoFinAccrualReport;
            __repoFinDNDetailReporting = repoFinDNDetailReporting;
            __repoFinDNDisplayReport = repoFinDNDisplayReport;
            __repoFinDocCompleteness = repoFinDocCompleteness;
            __repoFinInvestmentReport = repoFinInvestmentReport;
            __repoFinListingDN = repoFinListingDN;
            __repoFinListingPromoRecon = repoFinListingPromoRecon;
            __repoFinListingPromoReport = repoFinListingPromoReport;
            __repoFinMatrixApproval = repoFinMatrixApproval;
            __repoFinPromoHistoricalMovement = repoFinPromoHistoricalMovement;
            __repoFinPromoPlanningReport = repoFinPromoPlanningReport;
            __repoFinSKPValidation = repoFinSKPValidation;
            __repoFinSummaryAging = repoFinSummaryAging;
            __repoFinPromoDisplay = repoFinPromoDisplay;
            __repoFinPromoApprovalAging = repoPromoApprovalAging;
            __repoFinPromoSubmission = repoFinPromoSubmission;
            __repoFinPromoApprovalReminder = repoFinPromoApprovalReminder;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;

        }
    }
}
