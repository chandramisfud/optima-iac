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
        /// Get Budget SS volume with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ssvolume/list", Name = "budget_ssvolume_list_LP")]
        public async Task<IActionResult> GetBudgetssvolumeLP([FromQuery] budgetVolumeLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.GetBudgetSSVolumeLP(param.period!, param.channel!, param.subChannel!,
                        param.account!, param.subAccount!,param.region!, param.groupBrand!, param.txtSearch!, param.order!, param.sort!,
                        param.PageNumber, param.PageSize, __res.ProfileID);
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
        /// Get data for template include region
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ssvolume", Name = "budget_ssvolume_list_template")]
        public async Task<IActionResult> GetBudgetssvolume([FromQuery] budgetVolumeLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.GetBudgetSSVolumeTemplate(param.period!, param.channel!, param.subChannel!,
                        param.account!, param.subAccount!, param.region!, param.groupBrand!, param.txtSearch!, param.order!, param.sort!,
                        param.PageNumber, param.PageSize, __res.ProfileID);
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
        /// upload budget SS Volume file
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>

        [HttpPost("api/budget/ssvolume/upload", Name = "budget_ssvolume_upload")]
        public async Task<IActionResult> GetBudgetssvolumeUpload(IFormFile formFile)
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
                    DataTable header = new("ssvolumeType");
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
                        header.Columns.Add("account", typeof(string));
                        header.Columns.Add("subaccount", typeof(string));
                        header.Columns.Add("region", typeof(string));
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


                        for (int row = 3; row <= rowCount; row++)
                        {
                            List<object> cells = new List<object>();
                            for (int col = 1; col <= header.Columns.Count; col++)
                            {
                                if (mechanism.Cells[row, 1].Value != null)
                                {
                                    cells.Add(Convert.ToString(mechanism.Cells[row, col].Value));
                                } else
                                {
                                    break;
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
                    

                    var __val = await _repoBudgetSS.SetBudgetVolumeUpload(header, __res.ProfileID, __res.UserEmail);
                    if (__val != null)
                    {
                        string uploadActivity = "Budget Volume Upload";
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
        /// get active region list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/ssvolume/regionlist", Name = "budget_ssvolume_regionList")]
        public async Task<IActionResult> GetSSvolumeRegionList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetRegionList();
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
        /// get active channel list (follow mapping profile-channel)
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/ssvolume/channellist", Name = "budget_ssvolume_channnelList")]
        public async Task<IActionResult> GetssvolumeChannnelList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.GetUserProfileChannelList(__res.ProfileID);
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
                } else
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
        /// get active sub channel list (0=ALL)
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ssvolume/subchannellist", Name = "budget_ssvolume_subchannnelList")]
        public async Task<IActionResult> GetSSvolumeSubChannnelList([FromQuery]int[] channelId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetSubChannelList(channelId);
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
        /// Get active group brand list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/ssvolume/groupbrandlist", Name = "budget_ssvolume_groupbrandlist")]
        public async Task<IActionResult> GetssvolumeGrpBrandList()
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

        /// <summary>
        /// Get active account list by subChannel (0=ALL)
        /// </summary>
        /// <param name="subChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ssvolume/accountlist", Name = "budget_ssvolume_accountlist")]
        public async Task<IActionResult> GetSSvolumeAccountList([FromQuery]int[] subChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetAccountList(subChannelId);
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
        /// Get active sub account list by account (0=ALL)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ssvolume/subaccountlist", Name = "budget_ssvolume_subaccountlist")]
        public async Task<IActionResult> GetSSvolumeSubAccountList([FromQuery]int[] accountId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetSubAccountList(accountId);
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
