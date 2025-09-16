using Repositories.Contracts;

namespace V7.Controllers.Mapping
{
    public partial class MappingController : BaseController
    {
        private readonly IConfiguration __config;
        private readonly string __TokenSecret;
        private readonly IDistributorSubAccountRepo __repoDistributorSubAccount;
        private readonly IPICDNManualRepo __repoPICDNManual;
        private readonly ISKUBlitzRepo __repoSKUBlitz;
        private readonly ISubAccountBlitzRepo __repoSubAccountBlitz;
        private readonly IUserSubAccountRepo __repoUserSubAccount;
        private readonly ITaxLevelRepo __repoTaxLevel;
        private readonly IPromoReconSubActivityRepo __repoPromoReconSubActivity;
        private readonly IDistributorWHTRepo __repoDistributorWHT;

        public MappingController(
        IConfiguration config,
        IDistributorSubAccountRepo repoDistributorSubAccount,
        IPICDNManualRepo repoPICDNManual,
        ISKUBlitzRepo repoBlitzSKU,
        ISubAccountBlitzRepo repoSubAccountBlitz,
        IUserSubAccountRepo repoUserSubAccount,
        ITaxLevelRepo repoTaxLevel,
        IPromoReconSubActivityRepo repoPromoReconSubActivity,
        IDistributorWHTRepo repoDistributorWHT
        )
        {
            __config = config;
            __repoDistributorSubAccount = repoDistributorSubAccount;
            __repoPICDNManual = repoPICDNManual;
            __repoSKUBlitz = repoBlitzSKU;
            __repoSubAccountBlitz = repoSubAccountBlitz;
            __repoUserSubAccount = repoUserSubAccount;
            __repoTaxLevel = repoTaxLevel;
            __repoPromoReconSubActivity = repoPromoReconSubActivity;
            __repoDistributorWHT = repoDistributorWHT;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
        }

    }

}







