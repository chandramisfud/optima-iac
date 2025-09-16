using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repositories.Entities;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using Repositories.Repos;
using System.Data;
using System.IO;
using V7.MessagingServices;
using V7.Model.Budget;
using V7.Services;

namespace V7.Controllers.Budget
{
    /// <summary>
    /// Document Completeness handler
    /// </summary>
    public partial class BudgetController : BaseController
    {
        /// <summary>
        /// Get Budget PS Value Listing with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/psinput/list", Name = "budget_PSInput_list")]
        public async Task<IActionResult> GetBudgetPSInputLP([FromQuery] budgetPSInputParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.GetBudgetPSInputLP(param.period!, param.distributor!, param.groupBrand!, 
                        param.txtSearch!, param.order!, param.sort!, param.PageNumber, param.PageSize);
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
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                    }
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
        /// upload budget ss conversion rate file
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        
        [HttpPost("api/budget/psinput/upload", Name = "budget_psInput_upload")]
        public async Task<IActionResult> GetBudgetPSInputUpload(IFormFile formFile)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    using var stream = new MemoryStream();
                    await formFile.CopyToAsync(stream);
                    DataTable header = new("PSValueType");
                    try
                    {
                        using var package = new ExcelPackage(stream);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        var rowCount = 0;
                        ExcelWorksheet mechanism = package.Workbook.Worksheets[0];
                        rowCount = mechanism.Dimension.Rows;
                        
                        header.Columns.Add("period", typeof(string));
                        header.Columns.Add("distributor", typeof(string));
                        header.Columns.Add("groupbrand", typeof(string));
                        header.Columns.Add("m1", typeof(string));
                        header.Columns.Add("m2", typeof(string));
                        header.Columns.Add("m3", typeof(string));
                        header.Columns.Add("m4", typeof(string));
                        header.Columns.Add("m5", typeof(string));
                        header.Columns.Add("m6", typeof(string));
                        header.Columns.Add("m7", typeof(string));
                        header.Columns.Add("m8", typeof(string));
                        header.Columns.Add("m9", typeof(string));
                        header.Columns.Add("m10", typeof(string));
                        header.Columns.Add("m11", typeof(string));
                        header.Columns.Add("m12", typeof(string));

                        
                        for (int row = 2; row <= rowCount; row++)
                        {
                            List<object> cells = new List<object>();
                            for (int col = 1; col <= header.Columns.Count; col++)
                            {
                                if (mechanism.Cells[row, 1].Value != null)
                                {
                                    cells.Add(Convert.ToString(mechanism.Cells[row, col].Value));
                                }
                            }
                            if (cells.Count > 0)
                                header.Rows.Add(cells.ToArray());
                        }
                    }
                    catch (Exception)
                    {
                      
                        throw new Exception("Please check template entry");
                    }
                    

                    var __val = await _repoBudgetSS.SetBudgetPSInputUpload(header, __res.ProfileID, __res.UserEmail);
                    if (__val != null)
                    {
                        string uploadActivity = "Budget PS Input Upload";
                        var log =_repoUpload.InsertUploadLog(uploadActivity, formFile.FileName, __res.ProfileID, 
                            __res.UserEmail, MessageService.UploadLogSuccess);
                        
                        result = Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.SaveSuccess
                        });
                    }
                    else
                    {
                        return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.SaveFailed });
                    }
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
        /// get all filter list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/psinput/filter", Name = "budget_psinput_filterlist")]
        public async Task<IActionResult> GetPSInputFilterList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetBudgetPSInputFilter();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Budget PS Input template with ref
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/psinput", Name = "budget_psinput_template")]
        public async Task<IActionResult> GetBudgetPSInputTemplate([FromQuery] budgetPSInputParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.GetBudgetPSInputTemplate(param.period!, param.distributor!, param.groupBrand!,
                        param.txtSearch!, param.order!, param.sort!, param.PageNumber, param.PageSize);
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
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                    }
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

    }
}
