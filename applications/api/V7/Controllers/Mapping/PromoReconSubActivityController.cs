using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.Mapping;
using V7.Services;

namespace V7.Controllers.Mapping
{
    public partial class MappingController : BaseController
    {
        /// <summary>
        /// Get all promorecon-subactivity mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/promorecon-subactivity", Name = "promorecon-subactivity_lp")]
        public async Task<IActionResult> GetPromoReconSubActivityLandingPage([FromQuery] PromoReconSubActivityListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoPromoReconSubActivity.GetPromoReconSubActivityLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
                    body.PageSize, body.PageNumber);
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// Create promorecon-subactivity mapping data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/mapping/promorecon-subactivity", Name = "promorecon-subactivity_store")]
        public async Task<IActionResult> CreateDistributorSubAccount([FromBody] PromoReconSubActivityCreateParam param)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    PromoReconSubActivityCreate __bodytoken = new()
                    {
                        SubActivityId = param.SubActivityId,
                        AllowEdit = param.AllowEdit,
                        CreateBy = __res.ProfileID!,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoPromoReconSubActivity.CreatePromoReconSubActivity(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess, values = __val });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Update promorecon-subactivity mapping data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/mapping/promorecon-subactivity", Name = "promorecon-subactivity_update")]
        public async Task<IActionResult> UpdatePromoReconSubActivity([FromBody] PromoReconSubActivityCreateParam param)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    PromoReconSubActivityUpdate __bodytoken = new()
                    {
                        SubActivityId = param.SubActivityId,
                        AllowEdit = param.AllowEdit,
                        ModifiedBy = __res.ProfileID!,
                        ModifiedEmail = __res.UserEmail
                    };
                    var __val = await __repoPromoReconSubActivity.UpdatePromoReconSubActivity(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess, values = __val });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get all promorecon-subactivity mapping data for download
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/promorecon-subactivity/download", Name = "promorecon-subactivity_download")]
        public async Task<IActionResult> GetPromoReconSubActivityDownload()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoPromoReconSubActivity.GetPromoReconSubActivityDownload();
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Delete promorecon-subactivity mapping data 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/mapping/promorecon-subactivity", Name = "promorecon-subactivity_delete")]
        public async Task<IActionResult> DeletePromoReconSubActivity([FromBody] PromoReconSubActivityDeleteParam body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                // get token from request header
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    PromoReconSubActivityDelete __bodytoken = new()
                    {
                        SubActivityId = body.SubActivityId,
                        DeleteBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoPromoReconSubActivity.DeletePromoReconSubActivity(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.DeleteSucceed, values = __val });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get promorecon-subactivity by subactivity Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/promorecon-subactivity/subactivityid", Name = "promorecon-subactivity_by_subactivity_id")]
        public async Task<IActionResult> GetPromoReconSubActivitybySubActivityId([FromQuery] int Id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoPromoReconSubActivity.GetPromoReconSubActivitybySubActivityId(Id);
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get all subactivity mapping data for dropdown
        /// </summary>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/promorecon-subactivity/subactivity", Name = "promorecon-subactivity_subactivity_dropdown")]
        public async Task<IActionResult> GetSubActivityDropdown(int ActivityId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoPromoReconSubActivity.GetSubActivityDropdown(ActivityId);
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// Import Excell Mapping promo Recon Sub Activity
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/mapping/promorecon-subactivity/import", Name = "importpromorecon_getsubactivity")]
        public async Task<IActionResult> PromoReconSubactivity(IFormFile formFile)
        {
            try
            {
                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return UnprocessableEntity(new { status_code = "422", message = "un supported extension" });
                }
                using var stream = new MemoryStream();
                await formFile.CopyToAsync(stream);
                try
                {
                    using var package = new ExcelPackage(stream);
                    var rowCount = 0;
                    ExcelWorksheet dn = package.Workbook.Worksheets[0];
                    rowCount = dn.Dimension.Rows;
                    DataTable header = new("PromoReconPeriodSubActivityType");
                    header.Columns.Add("SubActivity", typeof(string));
                    header.Columns.Add("Category", typeof(string));
                    header.Columns.Add("SubCategory", typeof(string));
                    header.Columns.Add("Activity", typeof(string));
                    header.Columns.Add("SubActivityType", typeof(string));
                    header.Columns.Add("LongDesc", typeof(string));
                    header.Columns.Add("Action", typeof(string));
                    for (int row = 2; row <= rowCount; row++)
                    {
                        header.Rows.Add(
                            dn.Cells[row, 1].Value.ToString()!.Trim(),
                            dn.Cells[row, 2].Value.ToString()!.Trim(),
                            dn.Cells[row, 3].Value.ToString()!.Trim(),
                            dn.Cells[row, 4].Value.ToString()!.Trim(),
                            dn.Cells[row, 5].Value.ToString()!.Trim(),
                            dn.Cells[row, 6].Value.ToString()!.Trim(),
                            dn.Cells[row, 7].Value.ToString()!.Trim()
                        );
                    }
                    string tokenHeader = Request.Headers["Authorization"]!;
                    tokenHeader = tokenHeader.Replace("Bearer ", "");
                    var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                    if (__res.UserEmail != null)
                    {
                        PromoReconSubActivityImportParam __bodyToken = new()
                        {
                            userid = __res.ProfileID,
                            useremail = __res.UserEmail
                        };
                        var __val = await __repoPromoReconSubActivity.ImportPromoReconSubactivity(header, __bodyToken.userid, __bodyToken.useremail);
                        return Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            message = MessageService.UploadSuccess,
                            values = __val
                        });
                    }
                    else
                    {
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                    }
                }
                catch (SqlException __ex)
                {
                    return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }


        }
        /// <summary>
        /// Get activity dropdown data base on sub category id
        /// </summary>
        /// <param name="SubCategoryId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/promorecon-subactivity/activity", Name = "mapping_promorecon-subactivity_activity_dropdown")]
        public async Task<IActionResult> ActivityforSubActivityPromoRecon([FromQuery] int SubCategoryId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoPromoReconSubActivity.ActivityforSubActivityPromoRecon(SubCategoryId);
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get subcategory for dropdown data base on category id 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/promorecon-subactivity/subcategory", Name = "promorecon-subactivity_subcategory_dropdown")]
        public async Task<IActionResult> SubCategoryforSubActivity([FromQuery] int CategoryId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoPromoReconSubActivity.SubCategoryforSubActivity(CategoryId);
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Get category dropdown data  
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/promorecon-subactivity/category", Name = "promorecon-subactivity_category_dropdown")]
        public async Task<IActionResult> CategoryforSubActivityPromoRecon()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoPromoReconSubActivity.CategoryforSubActivityPromoRecon();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get SubActivity data for template
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/promorecon-subactivity/template", Name = "promorecon-subactivity_category_template")]
        public async Task<IActionResult> GetSubActivityTemplate()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoPromoReconSubActivity.GetSubActivityTemplate();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
    }
}
