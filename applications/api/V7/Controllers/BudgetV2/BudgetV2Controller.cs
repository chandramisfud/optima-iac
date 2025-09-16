using Repositories.Contracts;


namespace V7.Controllers.Budget
{
    /// <summary>
    /// Promo Controller
    /// </summary>
    public partial class BudgetV2Controller : BaseController
    {

        private readonly string __TokenSecret;
        private readonly IBudgetV2Repository __repoBudgetApproval;
        private readonly IConfiguration __config;

        public BudgetV2Controller(IConfiguration config, IBudgetV2Repository repoBudgetApproval)
        {
            __config = config;
            __repoBudgetApproval = repoBudgetApproval;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
        }


    }
}
