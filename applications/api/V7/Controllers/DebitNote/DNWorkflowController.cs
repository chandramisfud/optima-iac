using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using Microsoft.AspNetCore.Authorization;

namespace V7.Controllers.DebitNote
{
    public partial class DebitNoteController : BaseController
    {
        // debetnote/print/
        /// <summary>
        /// DN Print", Old API = "debetnote/print/"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/workflow/print", Name = "dn_workflow_print")]
        public async Task<IActionResult> DNPrintforDNWorkflow([FromQuery] int id)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNWorkflow.DNPrintforDNWorkflow(id);
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new BaseResponse { code = 204, error = true, message = MessageService.DataNotFound, values = __val });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        // debetnote/getbyId
        /// <summary>
        /// Get DN by Id for DN Workflow, Old API = "debetnote/getbyId"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/workflow/id", Name = "dn_getbyid_for_dn_workflow")]
        public async Task<IActionResult> GetDNbyIdforDNWorkflow([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                var __val = await __repoDNWorkflow.GetDNbyIdforDNWorkflow(id);
                if (__val == null)
                {
                    return Ok(new BaseResponse { code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        // debetnoteid/workflow
        /// <summary>
        /// Get DN Workflow, Old API = "debetnoteid/workflow"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/workflow", Name = "dn_workflow")]
        public async Task<IActionResult> GetDNWorkflow([FromQuery] string RefId)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNWorkflow.GetDNWorkflow(RefId);
                if (
                    __val.statusresult!.Count == 0
                    && __val.debetnoteresult!.Count == 0
                    && __val.sellingpointresult!.Count == 0
                    && __val.fileattactresult!.Count == 0
                    && __val.doccompletenessresult!.Count == 0
                )
                {
                    return Ok(new BaseResponse { code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        // debetnoteid/workflow_change
        /// <summary>
        /// Get DN Workflow Change, Old API = "debetnoteid/workflow_change"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/workflow-change", Name = "get_dn_workflow_change")]
        public async Task<IActionResult> GetDNWorkflowChange([FromQuery] string RefId)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNWorkflow.GetDNWorkflowChange(RefId);
                if (__val != null && __val.Count > 0)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new BaseResponse{ code = 204, error = true, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        // debetnoteid/workflow_history
        /// <summary>
        /// Get DN Workflow History, Old API = debetnoteid/workflow_history"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/workflow-history", Name = "get_dn_workflow_history")]
        public async Task<IActionResult> GetDNWorkflowHistory([FromQuery] string RefId)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNWorkflow.GetDNWorkflowHistory(RefId);
                if (__val != null && __val.Count > 0)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new BaseResponse{ code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        // promo/workflow
        /// <summary>
        /// Get Promo Workflow for DN Workflow, Old API = "promo/workflow"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/workflow/promo-workflow", Name = "get_promo_workflow_for_dn_workflow")]
        public async Task<IActionResult> GetPromoWorkflowforDNWorkflow([FromQuery] string RefId)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNWorkflow.GetPromoWorkflowforDNWorkflow(RefId);
                if (
                    __val.Promo!.Count == 0
                    && __val.Region!.Count == 0
                    && __val.Channel!.Count == 0
                    && __val.SubChannel!.Count == 0
                    && __val.Account!.Count == 0
                    && __val.SubAccount!.Count == 0
                    && __val.Brand!.Count == 0
                    && __val.Sku!.Count == 0
                    && __val.Activity!.Count == 0
                    && __val.SubActivity!.Count == 0
                    && __val.FileAttach!.Count == 0
                    && __val.StatusApproval!.Count == 0
                    && __val.Investment!.Count == 0
                    && __val.StatusPromo!.Count == 0
                    && __val.Mechanism!.Count == 0
                )
                {
                    return Ok(new BaseResponse{ code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }

    }
}