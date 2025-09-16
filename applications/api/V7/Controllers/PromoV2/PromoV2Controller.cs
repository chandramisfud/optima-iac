using Repositories.Contracts;
using Repositories.Contracts.Promo;

namespace V7.Controllers.Promo
{
    /// <summary>
    /// Promo Controller
    /// </summary>
    public partial class PromoV2Controller : BaseController
    {

        private readonly string __TokenSecret;
        private readonly IPromoCreationV2Repository _repoPromoCreation;
        private readonly IPromoReconV2Repository _repoPromoRecon;
        private readonly IConfiguration __config;
        private readonly IToolsEmailRepository __emailRepo;


        public PromoV2Controller(IConfiguration config, IPromoCreationV2Repository repoPromoCreation, 
            IPromoReconV2Repository repoPromoRecon, IToolsEmailRepository emailRepo)
        {
            __config = config;
            _repoPromoCreation = repoPromoCreation;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
            _repoPromoRecon = repoPromoRecon;
            __emailRepo = emailRepo;
        }


    }
}
