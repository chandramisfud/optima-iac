using Repositories.Contracts;
using V7.Services;

namespace V7.Controllers.Tools
{
    public partial class ToolsController : BaseController
    {
        private readonly IFileService __fileService;
        private readonly IToolsFileRepository __fileRepo;
        private readonly IConfiguration __config;
        private readonly IToolsEmailRepository __emailRepo;
        private readonly IToolsBlitzRepository __blitzRepo;
        private readonly IUploadRepo __uploadRepo;
        private readonly string __TokenSecret;
        private readonly IXMLPaymentRepository __xmlPaymentRepo;
        private readonly ISchedulerRepo __schedulerRepo;

        public ToolsController (
            IFileService fileService
        , IToolsFileRepository fileRepo
        , IConfiguration config
        , IToolsEmailRepository emailRepo
        , IToolsBlitzRepository blitzRepo
        , IUploadRepo uploadRepo
        , IXMLPaymentRepository xmlPaymentRepo
        , ISchedulerRepo schedulerRepo
        )
        {
            __config = config;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
            __fileService = fileService;
            __fileRepo = fileRepo;
            __emailRepo = emailRepo;
            __blitzRepo = blitzRepo;
            __uploadRepo = uploadRepo;
            __xmlPaymentRepo = xmlPaymentRepo;
            __schedulerRepo = schedulerRepo;
        }
    }
}