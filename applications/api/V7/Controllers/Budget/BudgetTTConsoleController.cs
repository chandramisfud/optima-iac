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
    /// Budget TT Control handler
    /// </summary>
    public partial class BudgetController : BaseController
    {
        /// <summary>
        /// Budget TT Console creation
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/budget/ttconsole", Name = "budget_ttconsole_create")]
        public async Task<IActionResult> SetBudgetTTConsoleCreate([FromBody] ttConsoleCreateParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.SetBudgetTTConsoleCreate(param.period!,  param.category, param.subCategory,
                        param.channel!, param.subChannel, param.account, param.subAccount, param.distributor, param.distributorShortDesc,
                        param.subActivityType, param.activity, param.subActivity, param.groupBrand!, param.budgetName, param.tt, 
                        __res.ProfileID, __res.UserEmail);
                    if (__val != null)
                    {
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
        /// Budget TT Console update
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/budget/ttconsole", Name = "budget_ttconsole_update")]
        public async Task<IActionResult> SetBudgetTTConsoleUpdate([FromBody] ttConsoleUpdateParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid)  return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.SetBudgetTTConsoleUpdate(param.id, param.period!, param.category, param.subCategory,
                        param.channel!, param.subChannel, param.account, param.subAccount, param.distributor, param.distributorShortDesc,
                        param.subActivityType, param.activity, param.subActivity, param.groupBrand!, param.budgetName, param.tt,
                        __res.ProfileID, __res.UserEmail);
                    if (__val != null)
                    {
                        result = Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.UpdateSuccess
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
        /// Get budget ttconsole by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole/id", Name = "get_budget_ttconsole_byid")]
        public async Task<IActionResult> getBudgetTTConsoleById([FromQuery] int id)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.GetBudgetTTConsoleByid(id);
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
        /// Get Budget TT Console Listing with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole/list", Name = "budget_ttconsole_list")]
        public async Task<IActionResult> GetBudgetTTConsoleLP([FromQuery] ttConsoleLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.GetBudgetTTConsoleLP(param.period!, __res.ProfileID, param.category,
                        param.subCategory, param.subActivityType, param.activity,param.subActivity,
                        param.channel!, param.subChannel, param.account, param.subAccount, param.distributor, param.groupBrand!, 
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
        /// Get Budget TT Console History
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole/history", Name = "budget_ttconsole_history")]
        public async Task<IActionResult> GetBudgetTTConsoleHistory([FromQuery] ttConsoleLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.GetBudgetTTConsoleHistory(param.period!, __res.ProfileID, param.category,
                        param.subCategory, param.subActivityType, param.activity, param.subActivity,
                        param.channel!, param.subChannel, param.account, param.subAccount, param.distributor, param.groupBrand!,
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
        /// Get Budget TT Console template with ref
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole", Name = "budget_ttconsole_template")]
        public async Task<IActionResult> GetBudgetTTConsoleTemplate([FromQuery] ttConsoleLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoBudgetSS.GetBudgetTTConsoleTemplate(param.period!, __res.ProfileID, param.category,
                        param.subCategory, param.subActivityType, param.activity, param.subActivity,
                        param.channel!, param.subChannel, param.account, param.subAccount, param.distributor, param.groupBrand!,
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
        /// upload budget TT Console RC file
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>

        [HttpPost("api/budget/ttconsole/uploadrc", Name = "budget_ttconsolerc_upload")]
        public async Task<IActionResult> GetBudgetTTConsoleUploadRC(IFormFile formFile)
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
                    DataTable header = new("SSTTTypeRC");
                    try
                    {
                        using var package = new ExcelPackage(stream);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        var rowCount = 0;
                        ExcelWorksheet mechanism = package.Workbook.Worksheets[0];
                        rowCount = mechanism.Dimension.Rows;
                        
                        header.Columns.Add("period", typeof(string));
                        header.Columns.Add("category", typeof(string));
                        header.Columns.Add("subcategory", typeof(string));
                        header.Columns.Add("channel", typeof(string));
                        header.Columns.Add("subchannel", typeof(string));
                        header.Columns.Add("account", typeof(string));
                        header.Columns.Add("subaccount", typeof(string));
                        header.Columns.Add("distributor", typeof(string));
                        header.Columns.Add("distributorshortdesc", typeof(string));
                        header.Columns.Add("groupbrand", typeof(string));
                        header.Columns.Add("subactivitytype", typeof(string));
                        header.Columns.Add("subactivity", typeof(string));
                        header.Columns.Add("activity", typeof(string));
                        header.Columns.Add("tt", typeof(string));
                        header.Columns.Add("budgetname", typeof(string));

                        
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
                    

                    var __val = await _repoBudgetSS.SetBudgetTTConsoleUploadRC(header, __res.ProfileID, __res.UserEmail);
                    if (__val != null)
                    {
                        string uploadActivity = "Budget TT Console RC Upload";
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
        /// upload budget TT Console DC file
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>

        [HttpPost("api/budget/ttconsole/uploaddc", Name = "budget_ttconsoledc_upload")]
        public async Task<IActionResult> GetBudgetTTConsoleUploadDC(IFormFile formFile)
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
                    DataTable header = new("SSTTTypeDC");
                    try
                    {
                        using var package = new ExcelPackage(stream);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        var rowCount = 0;
                        ExcelWorksheet mechanism = package.Workbook.Worksheets[0];
                        rowCount = mechanism.Dimension.Rows;

                        header.Columns.Add("period", typeof(string));
                        header.Columns.Add("category", typeof(string));
                        header.Columns.Add("subcategory", typeof(string));
                        header.Columns.Add("channel", typeof(string));
                        header.Columns.Add("distributor", typeof(string));
                        header.Columns.Add("distributorshortdesc", typeof(string));
                        header.Columns.Add("groupbrand", typeof(string));
                        header.Columns.Add("subactivitytype", typeof(string));
                        header.Columns.Add("subactivity", typeof(string));
                        header.Columns.Add("activity", typeof(string));
                        header.Columns.Add("tt", typeof(string));
                        header.Columns.Add("budgetname", typeof(string));


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


                    var __val = await _repoBudgetSS.SetBudgetTTConsoleUploadDC(header, __res.ProfileID, __res.UserEmail);
                    if (__val != null)
                    {
                        string uploadActivity = "Budget TT Console DC Upload";
                        var log = _repoUpload.InsertUploadLog(uploadActivity, formFile.FileName, __res.ProfileID,
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
        /// get active channel list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole/channellist", Name = "budget_ttconsole_channnelList")]
        public async Task<IActionResult> GetttconsoleChannnelList()
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
        /// get active sub channel list (0=ALL)
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole/subchannellist", Name = "budget_ttconsole_subchannnelList")]
        public async Task<IActionResult> GetttconsoleSubChannnelList([FromQuery] int[] channelId)
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
        [HttpGet("api/budget/ttconsole/groupbrandlist", Name = "budget_ttconsole_groupbrandlist")]
        public async Task<IActionResult> GetttconsoleGrpBrandList()
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
        [HttpGet("api/budget/ttconsole/accountlist", Name = "budget_ttconsole_accountlist")]
        public async Task<IActionResult> GetttconsoleAccountList([FromQuery] int[] subChannelId)
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
        [HttpGet("api/budget/ttconsole/subaccountlist", Name = "budget_ttconsole_subaccountlist")]
        public async Task<IActionResult> GetttconsoleSubAccountList([FromQuery] int[] accountId)
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

        /// <summary>
        /// Get active distributor list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole/distributorlist", Name = "budget_ttconsole_distributorlist")]
        public async Task<IActionResult> GetttconsoleDistributorList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetDistributorList();
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
        /// Get sub activity type  list
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole/subactivitytypelist", Name = "budget_ttconsole_subActivityTypelist")]
        public async Task<IActionResult> GetttconsolesubActivityTypeList([FromQuery] int[] categoryId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetSubActivityTypeList(categoryId);
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
        /// Get active activity list
        /// </summary>
        /// <param name="subActivityTypeId"></param>
        /// <param name="categoryId"></param>
        /// <param name="subCategoryId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole/activitylist", Name = "budget_ttconsole_Activitylist")]
        public async Task<IActionResult> GetttconsoleActivityList([FromQuery] int[] subActivityTypeId, [FromQuery] int[] categoryId, [FromQuery] int[] subCategoryId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetActivityList(subActivityTypeId, categoryId, subCategoryId);
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
        /// Get active subactivity list
        /// </summary>
        /// <param name="subActivityTypeId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole/subactivitylist", Name = "budget_ttconsole_subActivitylist")]
        public async Task<IActionResult> GetttconsoleSubActivityList([FromQuery] int[] subActivityTypeId, [FromQuery] int[] subCategoryId, [FromQuery] int[] activityId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetSubActivityList(subCategoryId, activityId, subActivityTypeId);
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
        /// Get active category list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole/categorylist", Name = "budget_ttconsole_categorylist")]
        public async Task<IActionResult> GetttconsoleCategoryList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetCategoryList();
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
        /// Get Active Sub Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="subActivityTypeId"></param>
        /// <returns></returns>
        [HttpGet("api/budget/ttconsole/subcategorylist", Name = "budget_ttconsole_subcategorylist")]
        public async Task<IActionResult> GetttconsoleSubCategoryList([FromQuery] int[] categoryId, [FromQuery] int[] subActivityTypeId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetSS.GetSubCategoryList(categoryId, subActivityTypeId);
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
