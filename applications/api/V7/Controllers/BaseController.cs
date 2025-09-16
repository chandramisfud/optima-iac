using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace V7.Controllers
{

    [ApiController]
    [LoggingIntercept]
    [Authorize]

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }
    }
}
