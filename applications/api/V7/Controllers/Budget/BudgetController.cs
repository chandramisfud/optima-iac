using Repositories.Contracts;

namespace V7.Controllers.Budget
{
    /// <summary>
    /// Budget Handle Controller
    /// </summary>
    public partial class BudgetController : BaseController
    {

        private readonly string __TokenSecret;
        private readonly IBudgetMasterRepository _repoBudgetMaster;
        private readonly IBudgetApprovalRepository _repoBudgetApproval;
        private readonly IBudgetHistoryRepository _repoBudgetHistory;
        private readonly IBudgetConversionRateRepo _repoBudgetSS;
        private readonly IUploadRepo _repoUpload;

        private readonly IConfiguration __config;
        /// <summary>
        /// Controller Init
        /// </summary>
        /// <param name="config"></param>
        /// <param name="repoBudgetMaster"></param>
        /// <param name="repoBudgetApproval"></param>
        /// <param name="repoBudgetHistory"></param>
        /// <param name="repoBudgetConversionRate"></param>
        /// <param name="repoUpload"></param>
        public BudgetController(IConfiguration config, 
            IBudgetMasterRepository repoBudgetMaster, 
            IBudgetApprovalRepository repoBudgetApproval, 
            IBudgetHistoryRepository repoBudgetHistory,
            IBudgetConversionRateRepo repoBudgetConversionRate, 
            IUploadRepo repoUpload)
        {
            __config = config;           
            _repoBudgetMaster = repoBudgetMaster;
            _repoBudgetApproval = repoBudgetApproval;
            _repoBudgetHistory = repoBudgetHistory;
            _repoBudgetSS = repoBudgetConversionRate;
            _repoUpload = repoUpload;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
        }

        
    }
}
