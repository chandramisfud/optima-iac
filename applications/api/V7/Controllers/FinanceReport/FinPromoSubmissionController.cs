using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.FinanceReport;
using V7.Services;

namespace V7.Controllers.FinanceReport
{
    /// <summary>
    /// Promo Submission handler
    /// </summary>
    /// 
    public partial class FinanceReportController : BaseController
    {
        /// <summary>
        /// Get Promo Planning Reporting with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/promosubmission", Name = "promosubmission_fnc_rpt_get_lp")]
        public async Task<IActionResult> GetFinPromoSubmissionLandingPage([FromQuery] FinPromoSubmissionRequestParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinPromoSubmission.GetFinPromoSubmissionLandingPage(
                    param.Period!,
                    param.EntityId,
                    param.DistributorId,
                    param.ChannelId,
                    param.profileId!,
                    param.Search!,
                    param.SortColumn.ToString(),
                    param.SortDirection.ToString(),
                    param.PageNumber,
                    param.PageSize
                    );
                if (__val != null)
                {
                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/promosubmission/entity", Name = "promosubmission_fnc_rpt_get_entity_list")]
        public async Task<IActionResult> GetEntityPromoSubmission()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinPromoSubmission.GetEntityList();
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get List Distributor
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/promosubmission/distributor", Name = "promosubmission_fnc_rpt_get_distributor_list")]
        public async Task<IActionResult> GetDistributorPromoSubmission([FromQuery] FinPromoSubmissionDistributorListParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoFinPromoSubmission.GetDistributorList(param.budgetId, param.entityId!);
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get List Channel
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/promosubmission/channel", Name = "promosubmission_fnc_rpt_get_channel_list")]
        public async Task<IActionResult> GetChanneLPromoSubmission([FromQuery] ChannelPromoSubmissionParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                List<int> arrayParent = new();
                var __val = await __repoFinPromoSubmission.GetChannelList(param.userid!, arrayParent.ToArray());
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });

            }
        }
        [HttpPost("api/finance-report/promosubmission/exception/upload", Name = "promosubmission_submissionEx_upload")]
        public async Task<IActionResult> PromoSubmissionEx(IFormFile formfile)
        {
            try
            {
                if (!Path.GetExtension(formfile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return UnprocessableEntity(new { status_code = "422", message = "unsupported extension" });
                }
                using var stream = new MemoryStream();
                await formfile.CopyToAsync(stream);

                using var package = new ExcelPackage(stream);
                var rowCount = 0;
                ExcelWorksheet __excel = package.Workbook.Worksheets[0];
                rowCount = __excel.Dimension.Rows;
                DataTable header = new("PromoSubmissionException");
                header.Columns.Add("Promo Number", typeof(string));
                header.Columns.Add("Reason", typeof(string));

                for (int row = 2; row <= rowCount; row++)
                {
                    header.Rows.Add(
                        __excel.Cells[row, 1].Value.ToString()!.Trim(),
                        __excel.Cells[row, 2].Value.ToString()!.Trim()
                    );
                }
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    PromoSubmissionExceptionUploadParam __bodytoken = new()
                    {
                        idx = __res.ProfileID!
                    };

                    await __repoFinPromoSubmission.PromoSubmissionExceptionUpload(header, __bodytoken.idx!);
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.UploadSuccess });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        [HttpGet("api/finance-report/promosubmission/exception", Name = "promosubmission_submissionEx_list")]
        public async Task<IActionResult> GetPromoSubmissonExceptionList([FromQuery] string idx)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinPromoSubmission.GetPromoSubmissonExceptionList(idx);
                if (__val != null)
                {
                    return Ok(new
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new { code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Clear promo submission exception 
        /// </summary>
        /// <returns></returns>
        [HttpDelete("api/finance-report/promosubmission/exception/clear", Name = "promosubmission_submissionEx_clear")]
        public async Task<IActionResult> PromoSubmissionExceptionClear([FromQuery] string idx)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                await __repoFinPromoSubmission.PromoSubmissionExceptionClear(idx);
                return Ok(new BaseResponse
                {
                    code = 200,
                    error = false,
                    message = MessageService.DeleteSucceed
                });
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get Data Config Late Promo Creation
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/promosubmission/latepromo", Name = "promosubmission_config_latepromocreation")]
        public async Task<IActionResult> GetFinLatePromoSubmission()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinPromoSubmission.GetFinLatePromoSubmission();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Get Data Config Late Promo Creation
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/promosubmission/email", Name = "promosubmission_fin_report")]
        public async Task<IActionResult> PromoSubmissionEmail([FromQuery] PromoSubmissionEmailParam param)
        {
            try
            {

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoFinPromoSubmission.PromoSubmissionEmail(param.period!, param.entity, param.distributor, __res.ProfileID);
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
                        return Ok(new BaseResponse { code = 204, error = true, message = MessageService.DataNotFound });
                    }
                }
                else
                {
                    return Conflict(new BaseResponse { code = 407, error = true, message = MessageService.EmailTokenFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get Promo Submission Download 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/finance-report/promosubmission/download", Name = "promosubmission_dl_fnc_rpt_get_lp")]
        public async Task<IActionResult> GetFinPromoSubmissionDownload([FromQuery] FinPromoSubmissionRequestParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __repoFinPromoSubmission.GetFinPromoSubmissionLandingPage(
                    param.Period!,
                    param.EntityId,
                    param.DistributorId,
                    param.ChannelId,
                    param.profileId!,
                    param.Search!,
                    param.SortColumn.ToString(),
                    param.SortDirection.ToString(),
                    param.PageNumber,
                    param.PageSize
                    );
                if (__val != null)
                {
                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get User data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/promosubmission/userlist", Name = "promosubmission_userlist_fin_report")]
        public async Task<IActionResult> GetUserList([FromQuery] string usergroupid, int userlevel, int isdeleted)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinPromoSubmission.GetUserList(usergroupid, userlevel, isdeleted);
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get User Group List
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/finance-report/promosubmission/usergroup", Name = "promosubmission_usergroup_fin_report")]
        public async Task<IActionResult> GetUserGroupList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoFinPromoSubmission.GetUserGroupList();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

    }
}