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
        /// Get Budget SS Conversion Listing with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ssconversionrate/list", Name = "budget_ssconversionrate_list")]
        public async Task<IActionResult> GetBudgetSSConversionrateLP([FromQuery] budgetConversionRateLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.GetBudgetConversionRateLP(param.period!, param.channel!, 
                        param.subChannel, param.groupBrand!, 
                        param.txtSearch!, param.order!, param.sort!, param.PageNumber, param.PageSize, __res.ProfileID );
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
        
        [HttpPost("api/budget/ssconversionrate/upload", Name = "budget_ssconversionrate_upload")]
        public async Task<IActionResult> GetBudgetSSConversionrateUpload(IFormFile formFile)
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
                    DataTable header = new("SSConversionRateType");
                    try
                    {
                        using var package = new ExcelPackage(stream);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        var rowCount = 0;
                        ExcelWorksheet mechanism = package.Workbook.Worksheets[0];
                        rowCount = mechanism.Dimension.Rows;
                        
                        header.Columns.Add("period", typeof(string));
                        header.Columns.Add("channel", typeof(string));
                        header.Columns.Add("subchannel", typeof(string));
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
                    

                    var __val = await _repoBudgetSS.SetBudgetConversionRateUpload(header, __res.ProfileID, __res.UserEmail);
                    if (__val != null)
                    {
                        string uploadActivity = "Budget Coversion Rate Upload";
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
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
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
        /// get active channel list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/ssconversionrate/channellist", Name = "budget_ssconversionrate_channnelList")]
        public async Task<IActionResult> GetSSConversionrateChannnelList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetChannelList();
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
        /// Get Sub Channel List
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ssconversionrate/subchannellist", Name = "budget_ssconversionrate_subchannnelList")]
        public async Task<IActionResult> GetSSConversionrateSubChannnelList([FromQuery] int[] channel)
        {
            try
            {
                var __val = await _repoBudgetSS.GetConversionRateSubChannelList(channel);
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
        /// Get active group brand list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/ssconversionrate/groupbrandlist", Name = "budget_ssconversionrate_groupbrandlist")]
        public async Task<IActionResult> GetSSConversionrateGrpBrandList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetGroupBrandList();
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
