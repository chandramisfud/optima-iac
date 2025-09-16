using Repositories.Contracts;
using Repositories.Contracts.Promo;

namespace V7.Controllers.Promo
{
    /// <summary>
    /// Promo Controller
    /// </summary>
    public partial class PromoController : BaseController
    {

        private readonly string __TokenSecret;
        private readonly IPromoPlanningRepository _repoPromoPlanning;
        private readonly IPromoApprovalRepository _repoPromoApproval;
        private readonly IPromoCreationRepository _repoPromoCreation;
        private readonly IPromoSendbackRepository _repoPromoSendback;
        private readonly IPromoReconRepository _repoPromoRecon;
        private readonly IPromoWorkflowRepository _repoPromoWorkflow;
        private readonly IPromoSKPValidationRepository _repoPromoSKPValidation;
        private readonly IPromoDisplayRepository _repoPromoDisplay;
        private readonly IPromoClosureRepository _repoPromoClosure;
        private readonly IPromoCancelRepository _repoPromoCancel;
        private readonly IConfiguration __config;

        public PromoController(IConfiguration config, IPromoPlanningRepository repoPromoPlanning,
            IPromoApprovalRepository repoPromoApproval, IPromoCreationRepository repoPromoCreation,
            IPromoReconRepository repoPromoRecon, IPromoSendbackRepository repoPromoSendback,
            IPromoWorkflowRepository repoPromoWorkflow, IPromoSKPValidationRepository repoSKP,
            IPromoDisplayRepository repoPromoDisplay, IPromoClosureRepository repoPromoClosure,
            IPromoCancelRepository repoPromoCancel)
        {
            __config = config;
            _repoPromoPlanning = repoPromoPlanning;
            _repoPromoApproval = repoPromoApproval;
            _repoPromoCreation = repoPromoCreation;
            _repoPromoSendback = repoPromoSendback;
            _repoPromoRecon = repoPromoRecon;
            _repoPromoWorkflow = repoPromoWorkflow;
            _repoPromoSKPValidation = repoSKP;
            _repoPromoDisplay = repoPromoDisplay;
            _repoPromoClosure = repoPromoClosure;
            _repoPromoCancel = repoPromoCancel;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
        }


    }
}
