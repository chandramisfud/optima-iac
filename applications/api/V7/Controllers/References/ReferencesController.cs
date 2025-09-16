using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Services;

namespace V7.Controllers.Budget
{
    /// <summary>
    /// Budget Handle Controller
    /// </summary>
    public partial class ReferencesController : BaseController
    {

        private readonly string __TokenSecret;
        private readonly IReferencesRepository __repo;

        private readonly IConfiguration __config;
        /// <summary>
        /// Controller Init
        /// </summary>
        /// <param name="config"></param>
        /// <param name="repo"></param>
        public ReferencesController(IConfiguration config, IReferencesRepository repo)
        {
            __config = config;           
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
            __repo = repo;
        }

        /// <summary>
        /// Get Distributor List by profile id (token)
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/references/distributor", Name = "get_references_distributor")]
        public async Task<IActionResult> GetReferencesDistributor()
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repo.GetDistributorByProfileId(__res.ProfileID);
                    if (__val != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            code = 200,
                            error = false,
                            message = MessageService.GetDataSuccess,
                            values = __val
                        });
                    }
                    else
                    {
                        return Conflict(
                            new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed }
                        );
                    }
                } else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
    }
}
