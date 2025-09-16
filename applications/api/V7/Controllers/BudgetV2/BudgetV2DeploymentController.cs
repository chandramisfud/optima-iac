using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using V7.MessagingServices;
using V7.Model;
using V7.Services;

namespace V7.Controllers.Budget
{
    public partial class BudgetV2Controller : BaseController
    {
        /// <summary>
        /// Budget Deployment Landing
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        [HttpGet("api/budget/deployment/list", Name = "budgetv2_deployment_request_list")]
        public async Task<IActionResult> GetBudgetV2DeploymentRequest([FromQuery] BudgetV2DeployListParam param)
       
        {
            IActionResult result;
            try
            {

                DataTable dtChannel = Helper.ListIntToKeyId(param.channelId!);             
                DataTable dtBrand = Helper.ListIntToKeyId(param.groupBrand!);
                DataTable dtSubActivityType = Helper.ListIntToKeyId(param.subActivityTypeId!);
                var __val = await __repoBudgetApproval.GetBudgetDeploymentRequestList(param.period, dtChannel, dtBrand,
                    dtSubActivityType);
                if (__val != null)
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataFailed
                    });

                }

            }
            catch (Exception __ex)
            {
                result = Conflict(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }

        ///// <summary>
        ///// Set budget deployment promo status update, DRAFT to WAITING FOR APPROVAL
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
      
        //[HttpPost("api/budget/deployment/status", Name = "budgetv2_deployment_update")]
        //public async Task<IActionResult> SetBudgetV2DeploymentStatus([FromBody] BudgetV2PromoArrayIntType param)
        //{
        //    IActionResult result;
        //    try
        //    {

        //        string tokenHeader = Request.Headers["Authorization"]!;
        //        tokenHeader = tokenHeader.Replace("Bearer ", "");
        //        var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
        //        if (__res.ProfileID == null)
        //        {
        //            return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
        //        }
        //        DataTable dtPromo = Helper.ListIntToKeyId(param.promoid!);
        //        var __val = await __repoBudgetApproval.GetBudgetDeploymentUpdateStatus(__res.ProfileID, __res.UserEmail, param.b
        //            );

        //        if (__val != null)
        //        {
        //            result = Ok(new Model.BaseResponse
        //            {
        //                error = false,
        //                code = 200,
        //                values = __val,
        //                message = MessageService.UpdateSuccess
        //            });
        //        } else
        //        {
        //            result = Ok(new Model.BaseResponse
        //            {
        //                error = true,
        //                code = 200,
        //                values = __val,
        //                message = MessageService.UpdateFailed
        //            });
        //        }
               
        //    }
        //    catch (Exception __ex)
        //    {
        //        result = Conflict(new Model.BaseResponse
        //        {
        //            error = true,
        //            code = 500,
        //            message = __ex.Message
        //        });
        //    }
        //    return result;
        //}

        /// <summary>
        /// Set budget deployment request, returning batchid
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/budget/deployment/request", Name = "budgetv2_deployment_request")]
        public async Task<IActionResult> SetBudgetV2DeploymentRequest([FromBody] BudgetV2PromoArrayIntType param)
        {
            IActionResult result;
            try
            {

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID == null)
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
                string batchId = DateTime.Now.ToString("yyyyMMddHHmmss");
                DataTable dtPromo = Helper.ListIntToKeyId(param.promoid!);
                var __val = await __repoBudgetApproval.GetBudgetDeploymentRequest(dtPromo, __res.ProfileID, __res.UserEmail, batchId);

                if (__val != null)
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = new
                        {
                            batchId = batchId
                        },
                        message = MessageService.SaveSuccess
                    });
                }
                else
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = true,
                        code = 200,
                        values = __val,
                        message = MessageService.SaveFailed
                    });
                }

            }
            catch (Exception __ex)
            {
                result = Conflict(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }

        /// <summary>
        /// Set deployment by batch id, returnin promo and approval data email
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/budget/deployment/batchid", Name = "budgetv2_deployment_batchid")]
        public async Task<IActionResult> SetBudgetV2DeploymentBatchid([FromBody] BudgetV2DeployBatchParam param)
        {
            IActionResult result;
            try
            {

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID == null)
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

                var __val = await __repoBudgetApproval.GetBudgetDeploymentUpdateStatus( __res.ProfileID, __res.UserEmail, param.batchId);

                if (__val != null)
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.SaveSuccess
                    });
                }
                else
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = true,
                        code = 200,
                        values = __val,
                        message = MessageService.SaveFailed
                    });
                }

            }
            catch (Exception __ex)
            {
                result = Conflict(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }
        /// <summary>
        /// Budget deployment filter
        /// </summary>
        /// <returns></returns>

        [HttpGet("api/budget/deployment/filter", Name = "budgetv2_deployment_filter")]
        public async Task<IActionResult> BudgetV2DeploymentFilter()
        {
            IActionResult result;
            try
            {

                var __val = await __repoBudgetApproval.GetBudgetDeploymentRequestFilter();
                if (__val != null)
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    result = Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataFailed
                    });

                }

            }
            catch (Exception __ex)
            {
                result = Conflict(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }

       
    }
}
