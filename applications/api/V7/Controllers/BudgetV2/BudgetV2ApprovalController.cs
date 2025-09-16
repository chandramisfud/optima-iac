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
        /// Budget Approval Landing
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        [HttpGet("api/budget/approvalrequest/list", Name = "budgetv2_approvalrequest_list")]
        public async Task<IActionResult> GetBudgetV2ApprovalRequest([FromQuery] BudgetV2ListParam param)
       
        {
            IActionResult result;
            try
            {

                DataTable dtChannel = Helper.ListIntToKeyId(param.channelId!);             
                DataTable dtBrand = Helper.ListIntToKeyId(param.groupBrand!);
                DataTable dtApprovalStatus = Helper.ListStringToId(param.budgetApprovalStatus!);
                DataTable dtMonth = Helper.ListIntToKeyId(param.month!);
                var selCat = 0;
                if (param.category != null && param.category.Length == 1)
                {
                    selCat = param.category[0];
                }
                var __val = await __repoBudgetApproval.GetBudgetApprovalRequestList(param.period, dtChannel, dtBrand, 
                    dtApprovalStatus, dtMonth, param.is5Bio, selCat,
                    param.PageNumber, param.PageSize, param.txtSearch!, param.sort!, param.order!);
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
 
        /// <summary>
        /// Budget Approval download report
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        
        [HttpGet("api/budget/approvalrequest/report", Name = "budgetv2_approvalrequest_report")]
        public async Task<IActionResult> GetBudgetV2ApprovalRequestReport([FromQuery] BudgetV2ReportParam param)

        {
            IActionResult result;
            try
            {

                DataTable dtChannel = Helper.ListIntToKeyId(param.channelId!);
                DataTable dtBrand = Helper.ListIntToKeyId(param.groupBrand!);
                DataTable dtApprovalStatus = Helper.ListStringToId(param.budgetApprovalStatus!);
                var __val = await __repoBudgetApproval.GetBudgetApprovalRequestReport(param.period, dtChannel, param.category,
                    dtBrand, dtApprovalStatus);
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

        /// <summary>
        /// Get budget approval request data + batchid for sending email
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
     
        [HttpGet("api/budget/approvalrequest/data", Name = "budgetv2_approvalrequest_dataemail")]
        public async Task<IActionResult> GetBudgetV2ApprovalRequestData([FromQuery] BudgetV2ReportParam param)

        {
            IActionResult result;
            try
            {

                DataTable dtChannel = Helper.ListIntToKeyId(param.channelId!);
                DataTable dtBrand = Helper.ListIntToKeyId(param.groupBrand!);
                DataTable dtApprovalStatus = Helper.ListStringToId (param.budgetApprovalStatus!); 
                DataTable dtMonth = Helper.ListIntToKeyId(param.month!);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID == null)
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
                var __val = await __repoBudgetApproval.GetBudgetApprovalRequestDataForEmail(param.period, param.category,
                    dtChannel, dtBrand,
                    dtApprovalStatus, dtMonth, param.is5Bio,  __res.ProfileID, __res.UserEmail);

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

        /// <summary>
        ///  Get budget approval request data for download (using batchid if available)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/approvalrequest/download", Name = "budgetv2_approvalrequest_download")]
        public async Task<IActionResult> GetBudgetV2ApprovalRequestDownload([FromQuery] BudgetApprovalV2DownloadParam param)

        {
            IActionResult result;
            try
            {

                DataTable dtChannel = Helper.ListIntToKeyId(param.channelId!);
                DataTable dtBrand = Helper.ListIntToKeyId(param.groupBrand!);
                DataTable dtApprovalStatus = Helper.ListStringToId(param.budgetApprovalStatus!);
                DataTable dtMonth = Helper.ListIntToKeyId(param.month!);
                var selCat = 0;
                if (param.category != null && param.category.Length == 1)
                {
                    selCat = param.category[0];
                }
                var __val = await __repoBudgetApproval.GetBudgetApprovalRequestDataForDownload(param.period, selCat,
                    dtChannel, dtBrand, dtApprovalStatus, dtMonth, param.is5Bio);

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

        /// <summary>
        /// Budget approval filter
        /// </summary>
        /// <returns></returns>

        [HttpGet("api/budget/approvalrequest/filter", Name = "budgetv2_approvalrequest_filter")]
        public async Task<IActionResult> BudgetV2ApprovalRequestFilter()
        {
            IActionResult result;
            try
            {

                var __val = await __repoBudgetApproval.GetBudgetApprovalRequestFilter();
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

        /// <summary>
        /// Get Budget deployemnt Log
        /// date: yyyyMMdd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/budget/deployment/log", Name = "budgetv2_deployment_log")]
        public IActionResult GetBudgetDeploymentLogs(string date)
        {
            string _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");

            try
            {
                if (!Directory.Exists(_logDirectory))
                    return NotFound(new { message = _logDirectory + " Not Exist" });

                // Format tanggal (YYYYMMDD)
                string searchPattern = $"logs_{date}_*.txt";

                // Ambil daftar file sesuai tanggal
                var logFiles = Directory.GetFiles(_logDirectory, searchPattern)
                                        .Select(Path.GetFileName)
                                        .ToList();

                if (logFiles.Count == 0)
                    return NotFound(new { message = "No logs found for this date" });

                // Base URL dari request (agar bisa diakses dari mana saja)
                var baseUrl = $"{Request.Scheme}://{Request.Host}/api/budget/deployment/dl/";

                // Buat HTML dengan daftar file yang bisa diklik
                string htmlContent = "<h2>Available Log Files</h2><ul>";
                foreach (var file in logFiles)
                {
                    string downloadLink = $"{baseUrl}{file}";
                    htmlContent += $"<li><a href='{downloadLink}' download>{file}</a></li>";
                }
                htmlContent += "</ul>";

                return new ContentResult { Content = htmlContent, ContentType = "text/html" };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("api/budget/deployment/dl/{filename}", Name = "budgetv2_deployment_donlot_log")]
        public IActionResult DownloadLogFile(string filename)
        {
            try
            {
                string _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                string filePath = Path.Combine(_logDirectory, filename);

                if (!System.IO.File.Exists(filePath))
                    return NotFound(new { message = "File not found" });

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "text/plain", filename);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
        /// <summary>
        /// Update after user click button approve via email (no auth)
        /// </summary>
        /// <param name="promo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/budget/approvalrequest/approve", Name = "budgetv2_approvalrequest_approve")]
        public async Task<IActionResult> SetBudgetV2ApprovalRequestApprove([FromBody] BudgetV2RequestApprove param) 
        {
            IActionResult result;
            try
            {                          
                
                var __val = await __repoBudgetApproval.SetBudgetApprovalRequestApprove(param.batchId, param.profileId, param.profileEmail);
                if (__val!= null)
                    {
                    __repoBudgetApproval.CekAndRunBudgetDeployment(param.batchId, param.profileId, param.profileEmail);
                    result = Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = "Budget successfuly approved" //MessageService.SaveSuccess
                        });
                    }
                    else
                    {
                        result = Ok(new Model.BaseResponse
                        {
                            error = false,
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
        /// Update after user click button reject via email (no auth)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/budget/approvalrequest/reject", Name = "budgetv2_approvalrequest_reject")]
        public async Task<IActionResult> SetBudgetV2ApprovalRequestReject([FromBody] BudgetV2RequestApprove param)
        {
            IActionResult result;
            try
            {

                var __val = await __repoBudgetApproval.SetBudgetApprovalRequestReject(param.batchId, param.profileId, param.profileEmail);
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
                        error = false,
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
        /// Get Budget Approval by batchid
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/approvalrequest/batchid", Name = "budgetv2_approval_batchid")]
        public async Task<IActionResult> GetBudgetApprovalBatchId([FromQuery]string batchId)
        {
            IActionResult result;
            try
            {
                var __val = await __repoBudgetApproval.GetBudgetApprovalByBatch(batchId);
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
