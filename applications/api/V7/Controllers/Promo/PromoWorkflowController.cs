using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models.DN;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model;

namespace V7.Controllers.Promo
{
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// Get Promo Workflow by Promo Id
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/workflow", Name = "promo_workflow")]
        public async Task<IActionResult> GetPromoWorkflow(string refId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _repoPromoWorkflow.GetPromoWorkflow(refId.Trim());

                if (result != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = result,
                        message = MessagingServices.MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get Promo Workflow by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/workflow/id", Name = "promo_workflow_byid")]
        public async Task<IActionResult> GetPromoWorkflowById(int id)
        {
            try
            {
                var result = await _repoPromoWorkflow.GetPromoWorkflowById(id);
                if (result != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = result,
                        message = MessagingServices.MessageService.GetDataSuccess
                    });

                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get DN list for promo workflow
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/workflow/dn", Name = "promo_workflow_dn")]
        public async Task<IActionResult> GetPromoWorkflowDN(string refId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _repoPromoWorkflow.GetPromoWorkflowDN(refId.Trim());

                if (result != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = result,
                        message = MessagingServices.MessageService.GetDataSuccess
                    });

                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get promo changes
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/workflow/changes", Name = "promo_workflow_changes")]
        public async Task<IActionResult> GetPromoWorkflowChanges(string refId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _repoPromoWorkflow.GetPromoWorkFlowChanges(refId.Trim());

                if (result != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = result,
                        message = MessagingServices.MessageService.GetDataSuccess
                    });

                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// get promo history
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/workflow/history", Name = "promo_workflow_history")]
        public async Task<IActionResult> GetPromoWorkflowHistory(string refId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _repoPromoWorkflow.GetPromoWorkFlowHistory(refId.Trim());

                if (result != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = result,
                        message = MessagingServices.MessageService.GetDataSuccess
                    });

                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// get promo timeline
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/workflow/timeline", Name = "promo_workflow_timeline")]
        public async Task<IActionResult> GetPromoWorkflowTimeline(string refId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _repoPromoWorkflow.GetPromoWorkflowTimeline(refId.Trim());

                if (result != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = result,
                        message = MessagingServices.MessageService.GetDataSuccess
                    });

                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
    }
}
