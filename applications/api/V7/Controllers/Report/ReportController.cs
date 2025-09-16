using Repositories.Contracts;
using V7.Controllers;

namespace V7.Controllers
{
    /// <summary>
    /// Report Controller
    /// </summary>
    public partial class ReportController : BaseController
    {
        private readonly IConfiguration __config;
        private readonly string __TokenSecret;
        private readonly IInvestmentReportRepo __repoInvestmentReport;
        private readonly IAccrualReportRepo __repoAccrualReport;
        private readonly IMatrixApprovalListingRepo __repoMatrixApprovalListing;
        private readonly IPromoHistoricalMovementRepo __repoPromoHistoricalMovement;
        private readonly ISummaryAgingApprovalRepo __repoSummaryAgingApproval;
        private readonly IListingDNRepo __repoListingDN;
        private readonly IDNDetailReportingRepo __repoDNDetailReporting;
        private readonly IDNDisplayRepo __repoDNDisplay;
        private readonly IListingPromoReconRepo __repoListingPromoReconRepo;
        private readonly IPromoPlanningReportingRepo __repoPromoPlanningReporting;
        private readonly IListingPromoReportingRepo __repoListingPromoReportingRepo;
        private readonly ISKPValidationRepo __repoSKPValidation;
        private readonly IDocumentCompletenessRepo __repoDocumentCompleteness;

        public ReportController(IConfiguration config, IInvestmentReportRepo repoInvestmentReport, IAccrualReportRepo repoAccrualReport, IMatrixApprovalListingRepo repoMatrixApprovalListing,
            IPromoHistoricalMovementRepo repoPromoHistoricalMovement, ISummaryAgingApprovalRepo repoSummaryAgingApproval, IListingDNRepo repoListingDN, IDNDetailReportingRepo repoDNDetailReporting,
            IDNDisplayRepo repoDNDisplay, IListingPromoReconRepo repoListingPromoReconRepo, IPromoPlanningReportingRepo repoPromoPlanningReporting, IListingPromoReportingRepo repoListingPromoReportingRepo,
            ISKPValidationRepo repoSKPValidation, IDocumentCompletenessRepo repoDocumentCompleteness)
        {
            __config = config;
            __repoInvestmentReport = repoInvestmentReport;
            __repoAccrualReport = repoAccrualReport;
            __repoMatrixApprovalListing = repoMatrixApprovalListing;
            __repoPromoHistoricalMovement = repoPromoHistoricalMovement;
            __repoSummaryAgingApproval = repoSummaryAgingApproval;
            __repoListingDN = repoListingDN;
            __repoDNDetailReporting = repoDNDetailReporting;
            __repoListingPromoReconRepo = repoListingPromoReconRepo;
            __repoPromoPlanningReporting = repoPromoPlanningReporting;
            __repoListingPromoReportingRepo = repoListingPromoReportingRepo;
            __repoSKPValidation = repoSKPValidation;
            __repoDocumentCompleteness = repoDocumentCompleteness;
            __repoDNDisplay = repoDNDisplay;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
        }
    }
}
