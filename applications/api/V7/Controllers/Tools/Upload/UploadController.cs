using System.Data;
using System.Data.SqlClient;
using Entities.Tools;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using OfficeOpenXml;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.Promo;
using V7.Model.Tools;
using V7.Services;

namespace V7.Controllers.Tools
{


    public partial class ToolsController : BaseController
    {
        /// <summary>
        /// Get upload log and status by activity
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        [HttpGet("api/tools/upload/log", Name = "getuploadlog")]
        public async Task<IActionResult> getUploadLog(string activity)
        {
            try
            {
                var _res = await __uploadRepo.GetUploadLog(activity);

                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = _res
                });
            }
            catch (Exception __ex)
            {

                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }

        }

        /// <summary>
        /// Insert upload log
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/log", Name = "insertuploadlog")]
        public async Task<IActionResult> insertUploadLog([FromBody] UploadLogParam body)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var token = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                var _res = await __uploadRepo.InsertUploadLog(body.activity, body.filename, 
                    token.ProfileID, token.UserEmail, body.status);

                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.SaveSuccess,
                    values = _res
                });
            }
            catch (Exception __ex)
            {

                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }

        }
        /// <summary>
        /// Upload budget region data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/budgetregion", Name = "budgetregionimport")]
        public async Task<IActionResult> ImportBudgetRegion(IFormFile formFile)
        {
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "unsupported extension" });
            }
            DataTable budgetregion = new("BudgetRegionType");
            budgetregion.Columns.Add("Allocation", typeof(string));
            budgetregion.Columns.Add("Derivative", typeof(string));
            budgetregion.Columns.Add("1", typeof(string));
            budgetregion.Columns.Add("2", typeof(string));
            budgetregion.Columns.Add("3", typeof(string));
            budgetregion.Columns.Add("4", typeof(string));
            budgetregion.Columns.Add("5", typeof(string));
            budgetregion.Columns.Add("6", typeof(string));
            budgetregion.Columns.Add("7", typeof(string));
            budgetregion.Columns.Add("8", typeof(string));
            budgetregion.Columns.Add("9", typeof(string));
            budgetregion.Columns.Add("10", typeof(string));
            budgetregion.Columns.Add("11", typeof(string));
            budgetregion.Columns.Add("12", typeof(string));
            budgetregion.Columns.Add("13", typeof(string));
            budgetregion.Columns.Add("14", typeof(string));
            budgetregion.Columns.Add("15", typeof(string));
            budgetregion.Columns.Add("16", typeof(string));
            budgetregion.Columns.Add("17", typeof(string));
            budgetregion.Columns.Add("18", typeof(string));
            budgetregion.Columns.Add("19", typeof(string));
            budgetregion.Columns.Add("20", typeof(string));

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var rowCount = 0;
            ExcelWorksheet wsbudgetuser = package.Workbook.Worksheets[8]; // ALLOCATION REGION
            rowCount = wsbudgetuser.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                budgetregion.Rows.Add(
                    wsbudgetuser.Cells[row, 1].Value.ToString()!.Trim(),
                    wsbudgetuser.Cells[row, 2].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 3].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 4].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 5].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 6].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 7].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 8].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 9].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 10].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 11].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 12].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 13].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 14].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 15].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 16].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 17].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 18].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 19].Value ?? string.Empty,
                    wsbudgetuser.Cells[row, 20].Value ?? string.Empty
                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportBudgetRegion(budgetregion, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload budget user data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/budgetuser", Name = "budgetuserimport")]
        public async Task<IActionResult> ImportBudgetUser(IFormFile formFile)
        {
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }
            DataTable budgetuser = new("BudgetUserAllocationType");
            budgetuser.Columns.Add("Allocation", typeof(string));
            budgetuser.Columns.Add("1", typeof(string));
            budgetuser.Columns.Add("2", typeof(string));
            budgetuser.Columns.Add("3", typeof(string));
            budgetuser.Columns.Add("4", typeof(string));
            budgetuser.Columns.Add("5", typeof(string));
            budgetuser.Columns.Add("6", typeof(string));
            budgetuser.Columns.Add("7", typeof(string));
            budgetuser.Columns.Add("8", typeof(string));
            budgetuser.Columns.Add("9", typeof(string));
            budgetuser.Columns.Add("10", typeof(string));
            budgetuser.Columns.Add("11", typeof(string));
            budgetuser.Columns.Add("12", typeof(string));
            budgetuser.Columns.Add("13", typeof(string));
            budgetuser.Columns.Add("14", typeof(string));
            budgetuser.Columns.Add("15", typeof(string));
            budgetuser.Columns.Add("16", typeof(string));
            budgetuser.Columns.Add("17", typeof(string));
            budgetuser.Columns.Add("18", typeof(string));
            budgetuser.Columns.Add("19", typeof(string));
            budgetuser.Columns.Add("20", typeof(string));
            budgetuser.Columns.Add("21", typeof(string));
            budgetuser.Columns.Add("22", typeof(string));

            budgetuser.Columns.Add("23", typeof(string));
            budgetuser.Columns.Add("24", typeof(string));
            budgetuser.Columns.Add("25", typeof(string));
            budgetuser.Columns.Add("26", typeof(string));
            budgetuser.Columns.Add("27", typeof(string));
            budgetuser.Columns.Add("28", typeof(string));
            budgetuser.Columns.Add("29", typeof(string));
            budgetuser.Columns.Add("30", typeof(string));

            budgetuser.Columns.Add("31", typeof(string));
            budgetuser.Columns.Add("32", typeof(string));
            budgetuser.Columns.Add("33", typeof(string));
            budgetuser.Columns.Add("34", typeof(string));
            budgetuser.Columns.Add("35", typeof(string));
            budgetuser.Columns.Add("36", typeof(string));
            budgetuser.Columns.Add("37", typeof(string));
            budgetuser.Columns.Add("38", typeof(string));
            budgetuser.Columns.Add("39", typeof(string));
            budgetuser.Columns.Add("40", typeof(string));

            budgetuser.Columns.Add("41", typeof(string));
            budgetuser.Columns.Add("42", typeof(string));
            budgetuser.Columns.Add("43", typeof(string));
            budgetuser.Columns.Add("44", typeof(string));
            budgetuser.Columns.Add("45", typeof(string));
            budgetuser.Columns.Add("46", typeof(string));
            budgetuser.Columns.Add("47", typeof(string));
            budgetuser.Columns.Add("48", typeof(string));
            budgetuser.Columns.Add("49", typeof(string));
            budgetuser.Columns.Add("50", typeof(string));

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var rowCount = 0;
            ExcelWorksheet wsbudgetuserderivative = package.Workbook.Worksheets[11]; // ALLOCATION USER
            rowCount = wsbudgetuserderivative.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                budgetuser.Rows.Add(
                    wsbudgetuserderivative.Cells[row, 1].Value.ToString()!.Trim(),
                    wsbudgetuserderivative.Cells[row, 2].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 3].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 4].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 5].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 6].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 7].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 8].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 9].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 10].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 11].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 12].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 13].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 14].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 15].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 16].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 17].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 18].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 19].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 20].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 21].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 22].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 23].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 24].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 25].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 26].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 27].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 28].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 29].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 30].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 31].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 32].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 33].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 34].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 35].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 36].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 37].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 38].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 39].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 40].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 41].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 42].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 43].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 44].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 45].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 46].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 47].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 48].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 49].Value ?? string.Empty,
                    wsbudgetuserderivative.Cells[row, 50].Value ?? string.Empty
                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportBudgetUser(budgetuser, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }


        /// <summary>
        /// Upload matrix approval data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/matrixapproval", Name = "matriximport")]
        public async Task<IActionResult> ImporMatrixApproval(IFormFile formFile)
        {
            try
            {
                if (formFile != null && formFile.Length > 0)
                {
                    if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
                    }
                    using var stream = new MemoryStream();
                    await formFile.CopyToAsync(stream);
                    using var package = new ExcelPackage(stream);
                    //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    DataTable matrix = new("PromoApproverType");
                    var rowCount = 0;
                    ExcelWorksheet wsmatrix = package.Workbook.Worksheets[0];
                    if (wsmatrix.Dimension == null)
                    {
                        return Ok(
                            new BaseResponse
                            {
                                error = true,
                                code = 404,
                                message = "Excel template is empty"
                            });
                    }
                    rowCount = wsmatrix.Dimension.Rows;
                    for (int i = wsmatrix.Dimension.Start.Row; i <= wsmatrix.Dimension.End.Row; i++)
                    {
                        bool isRowEmpty = true;
                        //loop all columns in a row
                        for (int j = wsmatrix.Dimension.Start.Column; j <= wsmatrix.Dimension.End.Column; j++)
                        {
                            if (wsmatrix.Cells[i, j].Value != null)
                            {
                                isRowEmpty = false;
                                break;
                            }
                        }
                        if (isRowEmpty)
                        {
                            wsmatrix.DeleteRow(i);
                        }
                    }
                    //matrix.Columns.Add("Periode", typeof(string));
                    matrix.Columns.Add("PrincipalId", typeof(string));
                    matrix.Columns.Add("DistributorId", typeof(string));
                    matrix.Columns.Add("Category", typeof(string));
                    matrix.Columns.Add("SubActivityType", typeof(string));
                    matrix.Columns.Add("ChannelId", typeof(string));
                    matrix.Columns.Add("SubChannelId", typeof(string));
                    matrix.Columns.Add("Initiator", typeof(string));
                    matrix.Columns.Add("MinInvestment", typeof(string));
                    matrix.Columns.Add("MaxInvestment", typeof(string));
                    matrix.Columns.Add("Approver1", typeof(string));
                    matrix.Columns.Add("Approver2", typeof(string));
                    matrix.Columns.Add("Approver3", typeof(string));
                    matrix.Columns.Add("Approver4", typeof(string));
                    matrix.Columns.Add("Approver5", typeof(string));
                    for (int row = 2; row <= rowCount; row++)
                    {
                        matrix.Rows.Add(
                            wsmatrix.Cells[row, 1].Value.ToString()!.Trim(),
                            wsmatrix.Cells[row, 2].Value ?? string.Empty,
                            wsmatrix.Cells[row, 3].Value ?? string.Empty,
                            wsmatrix.Cells[row, 4].Value ?? string.Empty,
                            wsmatrix.Cells[row, 5].Value ?? string.Empty,
                            wsmatrix.Cells[row, 6].Value ?? string.Empty,
                            wsmatrix.Cells[row, 7].Value ?? string.Empty,
                            wsmatrix.Cells[row, 8].Value,
                            wsmatrix.Cells[row, 9].Value ?? string.Empty,
                            wsmatrix.Cells[row, 10].Value ?? string.Empty,
                            wsmatrix.Cells[row, 11].Value ?? string.Empty,
                            wsmatrix.Cells[row, 12].Value ?? string.Empty,
                            wsmatrix.Cells[row, 13].Value ?? string.Empty,
                            wsmatrix.Cells[row, 14].Value ?? string.Empty
                         //   wsmatrix.Cells[row, 15].Value ?? string.Empty
                        );
                    }

                    string tokenHeader = Request.Headers["Authorization"]!;
                    tokenHeader = tokenHeader.Replace("Bearer ", "");
                    // get user data from token
                    var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                    if (__res.UserEmail != null)
                    {
                        var _res = await __uploadRepo.ImportMatrix(__res.ProfileID!, __res.UserEmail,  matrix);
                        return Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = _res,
                            message = MessageService.UploadSuccess,
                        });
                    }
                    else
                    {
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                    }
                }
                else
                {
                    return Conflict(new BaseResponse
                    {
                        error = true,
                        code = 500,
                        message = "Excel Template is not found",
                    });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Get list of unfinished process id
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpGet("api/tools/upload/matrixapprovalprocess", Name = "getMatrixAprovvalpromoprocess")]
        //public async Task<IActionResult> getMatrixAprovvalPromoProcess()
        //{
        //    try
        //    {
        //       var _res = await __uploadRepo.GetMatrixApprovalProcessId();

        //        return Ok(new BaseResponse
        //        {
        //            error = false,
        //            code = 200,
        //            values = _res
        //        });
        //    }
        //    catch (Exception __ex) {

        //        return Conflict(new BaseResponse
        //        {
        //            error = true,
        //            code = 500,
        //            message = __ex.Message
        //        });
        //    }
           
        //}

        /// <summary>
        /// Update promo  by matrix id and return list of affected promo
        /// </summary>
        /// <param name="matrixId"></param>
        /// <returns></returns>
        [HttpGet("api/tools/upload/matrixapprovalpromobymatrix", Name = "getMatrixAprovvalpromoprocessbymatrix")]
        public async Task<IActionResult> getMatrixApprovalPromoProcessByProcessMatrix(int matrixId)
        {
            try
            {
                var _res = await __uploadRepo.GetMatrixApprovalPromoByMatrix(matrixId);

                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = _res
                });
            }
            catch (Exception __ex)
            {

                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }

        }

        /// <summary>
        /// Set matrix approval per process done
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/matrixapprovalprocessdone", Name = "setmatrixapprovalprocessdone")]
        public async Task<IActionResult> setMatrixapprovalProcessDone(int processId)
        {
            try
            {
                var _res = await __uploadRepo.SetMatrixApprovalFinishedByProcess(processId);

                if (_res > 0)
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessagingServices.MessageService.SaveSuccess

                    });
                } else
                {
                    return Ok(new BaseResponse
                    {
                        error = true,
                        code = 200,
                        message = MessagingServices.MessageService.SaveFailed
                    });
                }
            }
            catch (Exception __ex)
            {

                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }

        }
        /// <summary>
        /// Get matrix list by process id
        /// </summary>
        /// <param name="matrixId"></param>
        /// <returns></returns>
        [HttpGet("api/tools/upload/matrixapprovalbyprocess", Name = "getMatrixApprovalbyprocess")]
        public async Task<IActionResult> getMatrixApprovalByProcess(int processId)
        {
            try
            {
                var _res = await __uploadRepo.GetMatrixApprovalByProcess(processId);

                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = _res
                });
            }
            catch (Exception __ex)
            {

                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }

        }
        /// <summary>
        /// Upload matrix budget approval data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/matrixbudget", Name = "matrixbudgetimport")]
        public async Task<IActionResult> ImporMatrixApprovalBudget(IFormFile formFile)
        {
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }
            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);

            var rowCount = 0;


            ExcelWorksheet wsmatrix = package.Workbook.Worksheets[0];
            rowCount = wsmatrix.Dimension.Rows;
            DataTable matrix = new("BudgetApproverType");

            matrix.Columns.Add("Periode", typeof(string));
            matrix.Columns.Add("PrincipalId", typeof(string));
            matrix.Columns.Add("DistributorId", typeof(string));
            matrix.Columns.Add("SubActivityType", typeof(string));
            matrix.Columns.Add("ChannelId", typeof(string));
            matrix.Columns.Add("Initiator", typeof(string));
            matrix.Columns.Add("MinInvestment", typeof(string));
            matrix.Columns.Add("MaxInvestment", typeof(string));
            matrix.Columns.Add("1", typeof(string));
            matrix.Columns.Add("2", typeof(string));
            matrix.Columns.Add("3", typeof(string));
            matrix.Columns.Add("4", typeof(string));
            matrix.Columns.Add("5", typeof(string));


            for (int row = 2; row <= rowCount; row++)
            {
                matrix.Rows.Add(
                    wsmatrix.Cells[row, 1].Value.ToString()!.Trim(),
                    wsmatrix.Cells[row, 2].Value ?? string.Empty,
                    wsmatrix.Cells[row, 3].Value ?? string.Empty,
                    wsmatrix.Cells[row, 4].Value ?? string.Empty,
                    wsmatrix.Cells[row, 5].Value ?? string.Empty,
                    wsmatrix.Cells[row, 6].Value,
                    wsmatrix.Cells[row, 7].Value,
                    wsmatrix.Cells[row, 8].Value ?? string.Empty,
                    wsmatrix.Cells[row, 9].Value ?? string.Empty,
                    wsmatrix.Cells[row, 10].Value ?? string.Empty,
                    wsmatrix.Cells[row, 11].Value ?? string.Empty,
                    wsmatrix.Cells[row, 12].Value ?? string.Empty,
                    wsmatrix.Cells[row, 13].Value ?? string.Empty

                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportMatrixBudget(__bodytoken.userid!, matrix);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload channel data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/channel", Name = "masterchannelimport")]
        public async Task<IActionResult> ImportMasterChannel(IFormFile formFile)
        {

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var rowCount = 0;
            ExcelWorksheet wschannel = package.Workbook.Worksheets[2];
            rowCount = wschannel.Dimension.Rows;
            DataTable channel = new("ChannelType");
            channel.Columns.Add("Channel", typeof(string));
            channel.Columns.Add("SubChannel", typeof(string));
            channel.Columns.Add("Account", typeof(string));
            channel.Columns.Add("SubAccount", typeof(string));
            for (int row = 2; row <= rowCount; row++)
            {
                channel.Rows.Add(
                    wschannel.Cells[row, 1].Value.ToString()!.Trim(),
                    wschannel.Cells[row, 2].Value.ToString()!.Trim(),
                    wschannel.Cells[row, 3].Value.ToString()!.Trim(),
                    wschannel.Cells[row, 4].Value.ToString()!.Trim()
                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportMasterChannel(channel, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload budget DC data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/budgetdc", Name = "budgetimportdc")]
        public async Task<IActionResult> ImportBudgetDC(IFormFile formFile)
        {
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

            if (formFile != null && formFile.Length > 0)
            {
                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
                }

                // get active distributor
                IList<string> lsDistributor = await __uploadRepo.GetActiveDistributor();

                // get group brand list
                IList<string> lsGrpBrand = await __uploadRepo.GetGroupBrand();
                // get subaccounttype list
                IList<string> lsSubAccountType = await __uploadRepo.GetSubActivityTypeDC();

                List<ImportBudgetResponse> errList = new();

                using var stream = new MemoryStream();
                await formFile.CopyToAsync(stream);
                try
                {
                    using var package = new ExcelPackage(stream);
                    //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    DataTable alloc = new("DCBudgetUploadType");
                    var rowCount = 0;
                    ExcelWorksheet wsalloc = package.Workbook.Worksheets[0];
                    if (wsalloc.Dimension == null)
                    {
                        return Ok(
                            new BaseResponse
                            {
                                error = true,
                                code = 404,
                                message = "Excel template is empty"
                            });
                    }
                    rowCount = wsalloc.Dimension.Rows;
                    alloc.Columns.Add("Budget Year", typeof(string));
                    alloc.Columns.Add("Category", typeof(string));
                    alloc.Columns.Add("Distributor", typeof(string));
                    alloc.Columns.Add("Brand", typeof(string));
                    alloc.Columns.Add("Sub Activity", typeof(string));
                    alloc.Columns.Add("Budget Amount", typeof(decimal));
                    alloc.Columns.Add("Budget Approval", typeof(string));
                    alloc.Columns.Add("User Access 1", typeof(string));
                    alloc.Columns.Add("User Access 2", typeof(string));
                    alloc.Columns.Add("User Access 3", typeof(string));
                    alloc.Columns.Add("User Access 4", typeof(string));
                    alloc.Columns.Add("User Access 5", typeof(string));
                    alloc.Columns.Add("User Access 6", typeof(string));
                    alloc.Columns.Add("User Access 7", typeof(string));
                    alloc.Columns.Add("User Access 8", typeof(string));
                    alloc.Columns.Add("User Access 9", typeof(string));
                    alloc.Columns.Add("User Access 10", typeof(string));
                    alloc.Columns.Add("User Access 11", typeof(string));
                    alloc.Columns.Add("User Access 12", typeof(string));
                    alloc.Columns.Add("User Access 13", typeof(string));
                    alloc.Columns.Add("User Access 14", typeof(string));
                    alloc.Columns.Add("User Access 15", typeof(string));
                    alloc.Columns.Add("User Access 16", typeof(string));
                    alloc.Columns.Add("User Access 17", typeof(string));
                    alloc.Columns.Add("User Access 18", typeof(string));
                    alloc.Columns.Add("User Access 19", typeof(string));
                    alloc.Columns.Add("User Access 20", typeof(string));
                    alloc.Columns.Add("User Access 21", typeof(string));
                    alloc.Columns.Add("User Access 22", typeof(string));
                    alloc.Columns.Add("User Access 23", typeof(string));
                    alloc.Columns.Add("User Access 24", typeof(string));
                    alloc.Columns.Add("User Access 25", typeof(string));
                    alloc.Columns.Add("User Access 26", typeof(string));
                    alloc.Columns.Add("User Access 27", typeof(string));
                    alloc.Columns.Add("User Access 28", typeof(string));
                    alloc.Columns.Add("User Access 29", typeof(string));
                    alloc.Columns.Add("User Access 30", typeof(string));
                    alloc.Columns.Add("User Access 31", typeof(string));
                    alloc.Columns.Add("User Access 32", typeof(string));
                    alloc.Columns.Add("User Access 33", typeof(string));
                    alloc.Columns.Add("User Access 34", typeof(string));
                    alloc.Columns.Add("User Access 35", typeof(string));
                    alloc.Columns.Add("ProfileId", typeof(string));
                    for (int row = 2; row <= rowCount; row++)
                    {
                        // if col 1 (year) empty, indicate empty row, dont process 
                        if (wsalloc.Cells[row, 1].Value != null)
                        {
                            List<object> lsCol = new();
                            for (int col = 1; col < 43; col++)
                            {
                                lsCol.Add(wsalloc.Cells[row, col].Value == null ? string.Empty : wsalloc.Cells[row, col].Value.ToString()!);
                            }
                            string err = "";
                            if (wsalloc.Cells[row, 2].Value.ToString()!.Trim() != "Distributor Cost")
                            {
                                err += " Category " + wsalloc.Cells[row, 2].Value + " not valid.";
                            }
                            if (!lsDistributor.Contains(wsalloc.Cells[row, 3].Value.ToString()!.Trim(), StringComparer.OrdinalIgnoreCase))
                            {
                                err += " Distributor " + wsalloc.Cells[row, 3].Value + " not available.";
                            }
                            // cek if brand available
                            if (!lsGrpBrand.Contains(wsalloc.Cells[row, 4].Value.ToString()!.Trim(), StringComparer.OrdinalIgnoreCase))
                            {
                                err += " Group brand " + wsalloc.Cells[row, 4].Value + " not available.";
                            }
                            if (!lsSubAccountType.Contains(wsalloc.Cells[row, 5].Value.ToString()!.Trim(), StringComparer.OrdinalIgnoreCase))
                            {
                                err += " Sub activity type " + wsalloc.Cells[row, 5].Value + " not available.";
                            }
                            if (err != "")
                                errList.Add(new ImportBudgetResponse
                                {
                                    budget = "Row-" + row,
                                    status = err
                                });

                            lsCol.Add(__res.ProfileID);
                            alloc.Rows.Add(lsCol.ToArray());
                        }
                    }

                    if (errList.Count > 0)
                    {
                        throw new Exception("Template entry error");
                    }

                    List<UploadBudgetDCModel> __dtList = new();
                    __dtList = (from DataRow dr in alloc.Rows
                                select new UploadBudgetDCModel()
                                {
                                    Periode = dr["Budget Year"].ToString(),
                                    Category = dr["Category"].ToString(),
                                    Distributor = dr["Distributor"].ToString(),
                                    Brand = dr["Brand"].ToString(),
                                    SubActivity = dr["Sub Activity"].ToString(),
                                    BudgetAmount = Convert.ToDecimal(dr["Budget Amount"]),
                                    BudgetApproval = dr["Budget Approval"].ToString(),
                                    UserAccess1 = dr["User Access 1"].ToString(),
                                    UserAccess2 = dr["User Access 2"].ToString(),
                                    UserAccess3 = dr["User Access 3"].ToString(),
                                    UserAccess4 = dr["User Access 4"].ToString(),
                                    UserAccess5 = dr["User Access 5"].ToString(),
                                    UserAccess6 = dr["User Access 6"].ToString(),
                                    UserAccess7 = dr["User Access 7"].ToString(),
                                    UserAccess8 = dr["User Access 8"].ToString(),
                                    UserAccess9 = dr["User Access 9"].ToString(),
                                    UserAccess10 = dr["User Access 10"].ToString(),
                                    UserAccess11 = dr["User Access 11"].ToString(),
                                    UserAccess12 = dr["User Access 12"].ToString(),
                                    UserAccess13 = dr["User Access 13"].ToString(),
                                    UserAccess14 = dr["User Access 14"].ToString(),
                                    UserAccess15 = dr["User Access 15"].ToString(),
                                    UserAccess16 = dr["User Access 16"].ToString(),
                                    UserAccess17 = dr["User Access 17"].ToString(),
                                    UserAccess18 = dr["User Access 18"].ToString(),
                                    UserAccess19 = dr["User Access 19"].ToString(),
                                    UserAccess20 = dr["User Access 20"].ToString(),
                                    UserAccess21 = dr["User Access 21"].ToString(),
                                    UserAccess22 = dr["User Access 22"].ToString(),
                                    UserAccess23 = dr["User Access 23"].ToString(),
                                    UserAccess24 = dr["User Access 24"].ToString(),
                                    UserAccess25 = dr["User Access 25"].ToString(),
                                    UserAccess26 = dr["User Access 26"].ToString(),
                                    UserAccess27 = dr["User Access 27"].ToString(),
                                    UserAccess28 = dr["User Access 28"].ToString(),
                                    UserAccess29 = dr["User Access 29"].ToString(),
                                    UserAccess30 = dr["User Access 30"].ToString(),
                                    UserAccess31 = dr["User Access 31"].ToString(),
                                    UserAccess32 = dr["User Access 32"].ToString(),
                                    UserAccess33 = dr["User Access 33"].ToString(),
                                    UserAccess34 = dr["User Access 34"].ToString(),
                                    UserAccess35 = dr["User Access 35"].ToString(),
                                    ProfileId = dr["ProfileId"].ToString()
                                }).ToList();

                    var __val = await __uploadRepo.ImportBudgetDC(alloc);
                    var __valTemp = await __uploadRepo.GetDCTableTemp();
                    if (__valTemp.Any())
                    {
                        return Ok(new BaseResponseUpload
                        {
                            error = false,
                            code = 200,
                            message = MessageService.UploadSuccess,
                            //                           values = errList,
                            xlsRowValues = __dtList,
                            tableTempValues = __valTemp
                        });
                    }
                    else
                    {
                        return Conflict(new BaseResponseUpload
                        {
                            error = true,
                            code = 409,
                            message = "Data cannot uploaded to table temporary",
                            values = __val,
                            xlsRowValues = __dtList,
                            tableTempValues = __valTemp
                        });
                    }
                }
                catch (Exception __ex)
                {
                    return Conflict(new BaseResponse
                    {
                        error = true,
                        code = 409,
                        message = __ex.Message,
                        values = errList
                    });
                }
            }
            else
            {
                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = "Excel Template is not found",
                });
            }
        }

        /// <summary>
        /// Upload budget data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/budget", Name = "budgetimport")]
        public async Task<IActionResult> ImportBudget(IFormFile formFile)
        {

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }
            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);

            var rowCount = 0;
            ExcelWorksheet wsalloc = package.Workbook.Worksheets[7];
            rowCount = wsalloc.Dimension.Rows;
            try
            {
                DataTable alloc = new("AllocationType");
                alloc.Columns.Add("Periode", typeof(string));
                alloc.Columns.Add("BudgetDesc", typeof(string));
                alloc.Columns.Add("Owner", typeof(string));
                alloc.Columns.Add("Distributor", typeof(string));
                alloc.Columns.Add("Principal", typeof(string));
                alloc.Columns.Add("Category", typeof(string));
                alloc.Columns.Add("SubCategory", typeof(string));
                alloc.Columns.Add("Activity", typeof(string));
                alloc.Columns.Add("SubActivity", typeof(string));
                alloc.Columns.Add("BudgetAmount", typeof(string));

                for (int row = 2; row <= rowCount; row++)
                {
                    alloc.Rows.Add(
                        wsalloc.Cells[row, 1].Value.ToString()!.Trim(),
                        wsalloc.Cells[row, 2].Value.ToString()!.Trim(),
                        wsalloc.Cells[row, 3].Value.ToString()!.Trim(),
                        wsalloc.Cells[row, 4].Value.ToString()!.Trim(),
                        wsalloc.Cells[row, 5].Value.ToString()!.Trim(),
                        wsalloc.Cells[row, 6].Value.ToString()!.Trim(),
                        wsalloc.Cells[row, 7].Value.ToString()!.Trim(),
                        wsalloc.Cells[row, 8].Value ?? string.Empty,
                        wsalloc.Cells[row, 9].Value ?? string.Empty,
                        wsalloc.Cells[row, 10].Value.ToString()!.Trim()

                    );
                }

                ExcelWorksheet wsder = package.Workbook.Worksheets[8];
                rowCount = wsder.Dimension.Rows;
                DataTable der = new("DerivativeType");
                der.Columns.Add("BudgetParent", typeof(string));
                der.Columns.Add("TotalAssignmentAmount", typeof(string));
                der.Columns.Add("AssignTo", typeof(string));
                der.Columns.Add("AssignDesc", typeof(string));
                der.Columns.Add("AssignAmount", typeof(string));
                der.Columns.Add("Category", typeof(string));
                der.Columns.Add("SubCategory", typeof(string));
                der.Columns.Add("Activity", typeof(string));
                der.Columns.Add("SubActivity", typeof(string));
                der.Columns.Add("Approval", typeof(string));


                for (int row = 2; row <= rowCount; row++)
                {
                    der.Rows.Add(
                        wsder!.Cells[row, 1].Value.ToString()!.Trim(),
                        wsder.Cells[row, 2].Value.ToString()!.Trim(),
                        wsder.Cells[row, 3].Value.ToString()!.Trim(),
                        wsder.Cells[row, 4].Value.ToString()!.Trim(),
                        wsder.Cells[row, 5].Value.ToString()!.Trim(),
                        wsder.Cells[row, 6].Value.ToString()!.Trim(),
                        wsder?.Cells[row, 7]?.Value?.ToString()?.Trim(),
                        wsder?.Cells[row, 8]?.Value?.ToString()?.Trim(),
                        wsder?.Cells[row, 9]?.Value?.ToString()?.Trim(),
                        wsder?.Cells[row, 10]?.Value?.ToString()?.Trim()
                    );
                }

                //ATTRIBUTE ACCOUNT
                rowCount = 0;


                ExcelWorksheet wsaccount = package.Workbook.Worksheets[10];
                rowCount = wsaccount.Dimension.Rows;
                DataTable account = new("AccountType");

                account.Columns.Add("Allocation", typeof(string));
                account.Columns.Add("Derivative", typeof(string));
                account.Columns.Add("1", typeof(string));
                account.Columns.Add("2", typeof(string));
                account.Columns.Add("3", typeof(string));
                account.Columns.Add("4", typeof(string));
                account.Columns.Add("5", typeof(string));
                account.Columns.Add("6", typeof(string));
                account.Columns.Add("7", typeof(string));
                account.Columns.Add("8", typeof(string));
                account.Columns.Add("9", typeof(string));
                account.Columns.Add("10", typeof(string));
                account.Columns.Add("11", typeof(string));
                account.Columns.Add("12", typeof(string));
                account.Columns.Add("13", typeof(string));
                account.Columns.Add("14", typeof(string));
                account.Columns.Add("15", typeof(string));
                account.Columns.Add("16", typeof(string));
                account.Columns.Add("17", typeof(string));
                account.Columns.Add("18", typeof(string));
                account.Columns.Add("19", typeof(string));
                account.Columns.Add("20", typeof(string));
                account.Columns.Add("21", typeof(string));
                account.Columns.Add("22", typeof(string));

                account.Columns.Add("23", typeof(string));
                account.Columns.Add("24", typeof(string));
                account.Columns.Add("25", typeof(string));
                account.Columns.Add("26", typeof(string));
                account.Columns.Add("27", typeof(string));
                account.Columns.Add("28", typeof(string));
                account.Columns.Add("29", typeof(string));
                account.Columns.Add("30", typeof(string));

                account.Columns.Add("31", typeof(string));
                account.Columns.Add("32", typeof(string));
                account.Columns.Add("33", typeof(string));
                account.Columns.Add("34", typeof(string));
                account.Columns.Add("35", typeof(string));
                account.Columns.Add("36", typeof(string));
                account.Columns.Add("37", typeof(string));
                account.Columns.Add("38", typeof(string));
                account.Columns.Add("39", typeof(string));
                account.Columns.Add("40", typeof(string));

                account.Columns.Add("41", typeof(string));
                account.Columns.Add("42", typeof(string));
                account.Columns.Add("43", typeof(string));
                account.Columns.Add("44", typeof(string));
                account.Columns.Add("45", typeof(string));
                account.Columns.Add("46", typeof(string));
                account.Columns.Add("47", typeof(string));
                account.Columns.Add("48", typeof(string));
                account.Columns.Add("49", typeof(string));
                account.Columns.Add("50", typeof(string));

                account.Columns.Add("51", typeof(string));
                account.Columns.Add("52", typeof(string));
                account.Columns.Add("53", typeof(string));
                account.Columns.Add("54", typeof(string));
                account.Columns.Add("55", typeof(string));
                account.Columns.Add("56", typeof(string));
                account.Columns.Add("57", typeof(string));
                account.Columns.Add("58", typeof(string));
                account.Columns.Add("59", typeof(string));
                account.Columns.Add("60", typeof(string));

                account.Columns.Add("61", typeof(string));
                account.Columns.Add("62", typeof(string));
                account.Columns.Add("63", typeof(string));
                account.Columns.Add("64", typeof(string));
                account.Columns.Add("65", typeof(string));
                account.Columns.Add("66", typeof(string));
                account.Columns.Add("67", typeof(string));
                account.Columns.Add("68", typeof(string));
                account.Columns.Add("69", typeof(string));
                account.Columns.Add("70", typeof(string));

                for (int row = 2; row <= rowCount; row++)
                {
                    account.Rows.Add(
                        wsaccount.Cells[row, 1].Value.ToString()!.Trim(),
                        wsaccount.Cells[row, 2].Value ?? string.Empty,
                        wsaccount.Cells[row, 3].Value ?? string.Empty,
                        wsaccount.Cells[row, 4].Value ?? string.Empty,
                        wsaccount.Cells[row, 5].Value ?? string.Empty,
                        wsaccount.Cells[row, 6].Value ?? string.Empty,
                        wsaccount.Cells[row, 7].Value ?? string.Empty,
                        wsaccount.Cells[row, 8].Value ?? string.Empty,
                        wsaccount.Cells[row, 9].Value ?? string.Empty,
                        wsaccount.Cells[row, 10].Value ?? string.Empty,
                        wsaccount.Cells[row, 11].Value ?? string.Empty,
                        wsaccount.Cells[row, 12].Value ?? string.Empty,
                        wsaccount.Cells[row, 13].Value ?? string.Empty,
                        wsaccount.Cells[row, 14].Value ?? string.Empty,
                        wsaccount.Cells[row, 15].Value ?? string.Empty,
                        wsaccount.Cells[row, 16].Value ?? string.Empty,
                        wsaccount.Cells[row, 17].Value ?? string.Empty,
                        wsaccount.Cells[row, 18].Value ?? string.Empty,
                        wsaccount.Cells[row, 19].Value ?? string.Empty,
                        wsaccount.Cells[row, 20].Value ?? string.Empty,
                        wsaccount.Cells[row, 21].Value ?? string.Empty,
                        wsaccount.Cells[row, 22].Value ?? string.Empty,
                        wsaccount.Cells[row, 23].Value ?? string.Empty,
                        wsaccount.Cells[row, 24].Value ?? string.Empty,
                        wsaccount.Cells[row, 25].Value ?? string.Empty,
                        wsaccount.Cells[row, 26].Value ?? string.Empty,
                        wsaccount.Cells[row, 27].Value ?? string.Empty,
                        wsaccount.Cells[row, 28].Value ?? string.Empty,
                        wsaccount.Cells[row, 29].Value ?? string.Empty,
                        wsaccount.Cells[row, 30].Value ?? string.Empty,
                        wsaccount.Cells[row, 31].Value ?? string.Empty,
                        wsaccount.Cells[row, 32].Value ?? string.Empty,
                        wsaccount.Cells[row, 33].Value ?? string.Empty,
                        wsaccount.Cells[row, 34].Value ?? string.Empty,
                        wsaccount.Cells[row, 35].Value ?? string.Empty,
                        wsaccount.Cells[row, 36].Value ?? string.Empty,
                        wsaccount.Cells[row, 37].Value ?? string.Empty,
                        wsaccount.Cells[row, 38].Value ?? string.Empty,
                        wsaccount.Cells[row, 39].Value ?? string.Empty,
                        wsaccount.Cells[row, 40].Value ?? string.Empty,
                        wsaccount.Cells[row, 41].Value ?? string.Empty,
                        wsaccount.Cells[row, 42].Value ?? string.Empty,
                        wsaccount.Cells[row, 43].Value ?? string.Empty,
                        wsaccount.Cells[row, 44].Value ?? string.Empty,
                        wsaccount.Cells[row, 45].Value ?? string.Empty,
                        wsaccount.Cells[row, 46].Value ?? string.Empty,
                        wsaccount.Cells[row, 47].Value ?? string.Empty,
                        wsaccount.Cells[row, 48].Value ?? string.Empty,
                        wsaccount.Cells[row, 49].Value ?? string.Empty,
                        wsaccount.Cells[row, 50].Value ?? string.Empty,
                        wsaccount.Cells[row, 51].Value ?? string.Empty,
                        wsaccount.Cells[row, 52].Value ?? string.Empty,
                        wsaccount.Cells[row, 53].Value ?? string.Empty,
                        wsaccount.Cells[row, 54].Value ?? string.Empty,
                        wsaccount.Cells[row, 55].Value ?? string.Empty,
                        wsaccount.Cells[row, 56].Value ?? string.Empty,
                        wsaccount.Cells[row, 57].Value ?? string.Empty,
                        wsaccount.Cells[row, 58].Value ?? string.Empty,
                        wsaccount.Cells[row, 59].Value ?? string.Empty,
                        wsaccount.Cells[row, 60].Value ?? string.Empty,
                        wsaccount.Cells[row, 61].Value ?? string.Empty,
                        wsaccount.Cells[row, 62].Value ?? string.Empty,
                        wsaccount.Cells[row, 63].Value ?? string.Empty,
                        wsaccount.Cells[row, 64].Value ?? string.Empty,
                        wsaccount.Cells[row, 65].Value ?? string.Empty,
                        wsaccount.Cells[row, 66].Value ?? string.Empty,
                        wsaccount.Cells[row, 67].Value ?? string.Empty,
                        wsaccount.Cells[row, 68].Value ?? string.Empty,
                        wsaccount.Cells[row, 69].Value ?? string.Empty,
                        wsaccount.Cells[row, 70].Value ?? string.Empty,
                        wsaccount.Cells[row, 70].Value ?? string.Empty

                    );
                }
                //END ACCOUNT

                //USER

                DataTable budgetuser = new("BudgetUserAllocationType");
                budgetuser.Columns.Add("Allocation", typeof(string));
                budgetuser.Columns.Add("1", typeof(string));
                budgetuser.Columns.Add("2", typeof(string));
                budgetuser.Columns.Add("3", typeof(string));
                budgetuser.Columns.Add("4", typeof(string));
                budgetuser.Columns.Add("5", typeof(string));
                budgetuser.Columns.Add("6", typeof(string));
                budgetuser.Columns.Add("7", typeof(string));
                budgetuser.Columns.Add("8", typeof(string));
                budgetuser.Columns.Add("9", typeof(string));
                budgetuser.Columns.Add("10", typeof(string));
                budgetuser.Columns.Add("11", typeof(string));
                budgetuser.Columns.Add("12", typeof(string));
                budgetuser.Columns.Add("13", typeof(string));
                budgetuser.Columns.Add("14", typeof(string));
                budgetuser.Columns.Add("15", typeof(string));
                budgetuser.Columns.Add("16", typeof(string));
                budgetuser.Columns.Add("17", typeof(string));
                budgetuser.Columns.Add("18", typeof(string));
                budgetuser.Columns.Add("19", typeof(string));
                budgetuser.Columns.Add("20", typeof(string));
                budgetuser.Columns.Add("21", typeof(string));
                budgetuser.Columns.Add("22", typeof(string));

                budgetuser.Columns.Add("23", typeof(string));
                budgetuser.Columns.Add("24", typeof(string));
                budgetuser.Columns.Add("25", typeof(string));
                budgetuser.Columns.Add("26", typeof(string));
                budgetuser.Columns.Add("27", typeof(string));
                budgetuser.Columns.Add("28", typeof(string));
                budgetuser.Columns.Add("29", typeof(string));
                budgetuser.Columns.Add("30", typeof(string));

                budgetuser.Columns.Add("31", typeof(string));
                budgetuser.Columns.Add("32", typeof(string));
                budgetuser.Columns.Add("33", typeof(string));
                budgetuser.Columns.Add("34", typeof(string));
                budgetuser.Columns.Add("35", typeof(string));
                budgetuser.Columns.Add("36", typeof(string));
                budgetuser.Columns.Add("37", typeof(string));
                budgetuser.Columns.Add("38", typeof(string));
                budgetuser.Columns.Add("39", typeof(string));
                budgetuser.Columns.Add("40", typeof(string));

                budgetuser.Columns.Add("41", typeof(string));
                budgetuser.Columns.Add("42", typeof(string));
                budgetuser.Columns.Add("43", typeof(string));
                budgetuser.Columns.Add("44", typeof(string));
                budgetuser.Columns.Add("45", typeof(string));
                budgetuser.Columns.Add("46", typeof(string));
                budgetuser.Columns.Add("47", typeof(string));
                budgetuser.Columns.Add("48", typeof(string));
                budgetuser.Columns.Add("49", typeof(string));
                budgetuser.Columns.Add("50", typeof(string));


                rowCount = 0;
                ExcelWorksheet wsbudgetuser = package.Workbook.Worksheets[12]; // ALLOCATION USER
                rowCount = wsbudgetuser.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    budgetuser.Rows.Add(
                        wsbudgetuser.Cells[row, 1].Value.ToString()!.Trim(),
                        wsbudgetuser.Cells[row, 2].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 3].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 4].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 5].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 6].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 7].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 8].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 9].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 10].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 11].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 12].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 13].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 14].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 15].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 16].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 17].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 18].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 19].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 20].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 21].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 22].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 23].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 24].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 25].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 26].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 27].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 28].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 29].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 30].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 31].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 32].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 33].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 34].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 35].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 36].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 37].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 38].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 39].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 40].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 41].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 42].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 43].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 44].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 45].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 46].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 47].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 48].Value ?? string.Empty,
                        wsbudgetuser.Cells[row, 49].Value ?? string.Empty

                    );
                }


                DataTable derivativeuser = new("BudgetUserAllocationType");
                derivativeuser.Columns.Add("Derivative", typeof(string));
                derivativeuser.Columns.Add("1", typeof(string));
                derivativeuser.Columns.Add("2", typeof(string));
                derivativeuser.Columns.Add("3", typeof(string));
                derivativeuser.Columns.Add("4", typeof(string));
                derivativeuser.Columns.Add("5", typeof(string));
                derivativeuser.Columns.Add("6", typeof(string));
                derivativeuser.Columns.Add("7", typeof(string));
                derivativeuser.Columns.Add("8", typeof(string));
                derivativeuser.Columns.Add("9", typeof(string));
                derivativeuser.Columns.Add("10", typeof(string));
                derivativeuser.Columns.Add("11", typeof(string));
                derivativeuser.Columns.Add("12", typeof(string));
                derivativeuser.Columns.Add("13", typeof(string));
                derivativeuser.Columns.Add("14", typeof(string));
                derivativeuser.Columns.Add("15", typeof(string));
                derivativeuser.Columns.Add("16", typeof(string));
                derivativeuser.Columns.Add("17", typeof(string));
                derivativeuser.Columns.Add("18", typeof(string));
                derivativeuser.Columns.Add("19", typeof(string));
                derivativeuser.Columns.Add("20", typeof(string));
                derivativeuser.Columns.Add("21", typeof(string));
                derivativeuser.Columns.Add("22", typeof(string));

                derivativeuser.Columns.Add("23", typeof(string));
                derivativeuser.Columns.Add("24", typeof(string));
                derivativeuser.Columns.Add("25", typeof(string));
                derivativeuser.Columns.Add("26", typeof(string));
                derivativeuser.Columns.Add("27", typeof(string));
                derivativeuser.Columns.Add("28", typeof(string));
                derivativeuser.Columns.Add("29", typeof(string));
                derivativeuser.Columns.Add("30", typeof(string));

                derivativeuser.Columns.Add("31", typeof(string));
                derivativeuser.Columns.Add("32", typeof(string));
                derivativeuser.Columns.Add("33", typeof(string));
                derivativeuser.Columns.Add("34", typeof(string));
                derivativeuser.Columns.Add("35", typeof(string));
                derivativeuser.Columns.Add("36", typeof(string));
                derivativeuser.Columns.Add("37", typeof(string));
                derivativeuser.Columns.Add("38", typeof(string));
                derivativeuser.Columns.Add("39", typeof(string));
                derivativeuser.Columns.Add("40", typeof(string));

                derivativeuser.Columns.Add("41", typeof(string));
                derivativeuser.Columns.Add("42", typeof(string));
                derivativeuser.Columns.Add("43", typeof(string));
                derivativeuser.Columns.Add("44", typeof(string));
                derivativeuser.Columns.Add("45", typeof(string));
                derivativeuser.Columns.Add("46", typeof(string));
                derivativeuser.Columns.Add("47", typeof(string));
                derivativeuser.Columns.Add("48", typeof(string));
                derivativeuser.Columns.Add("49", typeof(string));
                derivativeuser.Columns.Add("50", typeof(string));


                rowCount = 0;
                ExcelWorksheet wsderivativebudgetuser = package.Workbook.Worksheets[13]; // ALLOCATION USER
                rowCount = wsderivativebudgetuser.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    derivativeuser.Rows.Add(
                    wsderivativebudgetuser.Cells[row, 1].Value.ToString()!.Trim(),
                    wsderivativebudgetuser.Cells[row, 2].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 3].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 4].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 5].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 6].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 7].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 8].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 9].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 10].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 11].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 12].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 13].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 14].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 15].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 16].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 17].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 18].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 19].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 20].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 21].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 22].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 23].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 24].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 25].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 26].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 27].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 28].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 29].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 30].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 31].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 32].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 33].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 34].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 35].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 36].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 37].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 38].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 39].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 40].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 41].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 42].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 43].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 44].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 45].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 46].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 47].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 48].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 49].Value ?? string.Empty,
                    wsderivativebudgetuser.Cells[row, 50].Value ?? string.Empty
                );
                }

                //END USER

                //REGION
                DataTable budgetregion = new("BudgetRegionType");
                budgetregion.Columns.Add("Allocation", typeof(string));
                budgetregion.Columns.Add("Derivative", typeof(string));
                budgetregion.Columns.Add("1", typeof(string));
                budgetregion.Columns.Add("2", typeof(string));
                budgetregion.Columns.Add("3", typeof(string));
                budgetregion.Columns.Add("4", typeof(string));
                budgetregion.Columns.Add("5", typeof(string));
                budgetregion.Columns.Add("6", typeof(string));
                budgetregion.Columns.Add("7", typeof(string));
                budgetregion.Columns.Add("8", typeof(string));
                budgetregion.Columns.Add("9", typeof(string));
                budgetregion.Columns.Add("10", typeof(string));
                budgetregion.Columns.Add("11", typeof(string));
                budgetregion.Columns.Add("12", typeof(string));
                budgetregion.Columns.Add("13", typeof(string));
                budgetregion.Columns.Add("14", typeof(string));
                budgetregion.Columns.Add("15", typeof(string));
                budgetregion.Columns.Add("16", typeof(string));
                budgetregion.Columns.Add("17", typeof(string));
                budgetregion.Columns.Add("18", typeof(string));
                budgetregion.Columns.Add("19", typeof(string));
                budgetregion.Columns.Add("20", typeof(string));


                rowCount = 0;
                ExcelWorksheet wsbudgetregion = package.Workbook.Worksheets[9]; // ALLOCATION REGION
                rowCount = wsbudgetregion.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    budgetregion.Rows.Add(
                    wsbudgetregion.Cells[row, 1].Value.ToString()!.Trim(),
                    wsbudgetregion.Cells[row, 2].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 3].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 4].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 5].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 6].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 7].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 8].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 9].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 10].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 11].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 12].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 13].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 14].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 15].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 16].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 17].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 18].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 19].Value ?? string.Empty,
                    wsbudgetregion.Cells[row, 20].Value ?? string.Empty
                );
                }
                //END REGION

                //BRAND
                DataTable budgetbrand = new("ImportBrandType");
                budgetbrand.Columns.Add("Allocation", typeof(string));
                budgetbrand.Columns.Add("Derivative", typeof(string));
                budgetbrand.Columns.Add("1", typeof(string));
                budgetbrand.Columns.Add("2", typeof(string));
                budgetbrand.Columns.Add("3", typeof(string));
                budgetbrand.Columns.Add("4", typeof(string));
                budgetbrand.Columns.Add("5", typeof(string));
                budgetbrand.Columns.Add("6", typeof(string));
                budgetbrand.Columns.Add("7", typeof(string));
                budgetbrand.Columns.Add("8", typeof(string));
                budgetbrand.Columns.Add("9", typeof(string));
                budgetbrand.Columns.Add("10", typeof(string));
                budgetbrand.Columns.Add("11", typeof(string));
                budgetbrand.Columns.Add("12", typeof(string));
                budgetbrand.Columns.Add("13", typeof(string));
                budgetbrand.Columns.Add("14", typeof(string));
                budgetbrand.Columns.Add("15", typeof(string));
                budgetbrand.Columns.Add("16", typeof(string));
                budgetbrand.Columns.Add("17", typeof(string));
                budgetbrand.Columns.Add("18", typeof(string));
                budgetbrand.Columns.Add("19", typeof(string));
                budgetbrand.Columns.Add("20", typeof(string));
                budgetbrand.Columns.Add("21", typeof(string));
                budgetbrand.Columns.Add("22", typeof(string));
                budgetbrand.Columns.Add("23", typeof(string));
                budgetbrand.Columns.Add("24", typeof(string));
                budgetbrand.Columns.Add("25", typeof(string));
                budgetbrand.Columns.Add("26", typeof(string));
                budgetbrand.Columns.Add("27", typeof(string));
                budgetbrand.Columns.Add("28", typeof(string));
                budgetbrand.Columns.Add("29", typeof(string));
                budgetbrand.Columns.Add("30", typeof(string));

                rowCount = 0;
                ExcelWorksheet wsbudgetbrand = package.Workbook.Worksheets[11]; // ALLOCATION brand
                rowCount = wsbudgetbrand.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    budgetbrand.Rows.Add(
                        wsbudgetbrand.Cells[row, 1].Value.ToString()!.Trim(),
                        wsbudgetbrand.Cells[row, 2].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 3].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 4].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 5].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 6].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 7].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 8].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 9].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 10].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 11].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 12].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 13].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 14].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 15].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 16].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 17].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 18].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 19].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 20].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 21].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 22].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 23].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 24].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 25].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 26].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 27].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 28].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 29].Value ?? string.Empty,
                        wsbudgetbrand.Cells[row, 30].Value ?? string.Empty

                    );
                }
                //END BRAND
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    ToolsUploadParam __bodytoken = new()
                    {
                        userid = __res.ProfileID,
                        useremail = __res.UserEmail
                    };
                    var __val = await __uploadRepo.ImportBudgetWithAttribute(alloc, der, account, budgetuser, derivativeuser, budgetregion, budgetbrand, __bodytoken.userid!);
                    return Ok(new
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
            catch (NullReferenceException __ex)
            {
                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 406,
                    message = "Template error",
                    values = __ex.Message
                });
            }
            catch (SqlException __ex)
            {
                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            catch (ArgumentException __ex)
            {
                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            catch (FormatException __ex)
            {
                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }
        /// <summary>
        /// Upload budget adjustment data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/budgetadjustment", Name = "budgetimportadjustment")]
        public async Task<IActionResult> ImportBudgetAdjustment(IFormFile formFile)
        {
            if (formFile != null && formFile.Length > 0)
            {
                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
                }
                using var stream = new MemoryStream();
                await formFile.CopyToAsync(stream);
                try
                {
                    using var package = new ExcelPackage(stream);
                    var rowCount = 0;
                    ExcelWorksheet wsder = package.Workbook.Worksheets[0];
                    rowCount = wsder.Dimension.Rows;
                    DataTable der = new("DerivativeType");
                    der.Columns.Add("BudgetParent", typeof(string));
                    der.Columns.Add("TotalAssignmentAmount", typeof(decimal));
                    der.Columns.Add("AssignTo", typeof(string));
                    der.Columns.Add("AssignDesc", typeof(string));
                    der.Columns.Add("AssignAmount", typeof(decimal));
                    der.Columns.Add("Category", typeof(string));
                    der.Columns.Add("SubCategory", typeof(string));
                    der.Columns.Add("Activity", typeof(string));
                    der.Columns.Add("SubActivity", typeof(string));
                    der.Columns.Add("Approval", typeof(string));


                    for (int row = 2; row <= rowCount; row++)
                    {
                        // modified, AND Des 5 2022
                        der.Rows.Add(
                            wsder.Cells[row, 1].Value.ToString()!.Trim(),
                            wsder.Cells[row, 2].Value,
                            wsder.Cells[row, 3].Value ?? string.Empty,
                            wsder.Cells[row, 4].Value.ToString()!.Trim(),
                            wsder.Cells[row, 5].Value,
                            wsder.Cells[row, 6].Value.ToString()!.Trim(),
                            wsder.Cells[row, 7].Value.ToString()!.Trim(),
                            wsder.Cells[row, 8].Value ?? string.Empty,
                            wsder.Cells[row, 9].Value ?? string.Empty,
                            wsder.Cells[row, 10].Value.ToString()!.Trim()

                        // Convert.ToString(wsder.Cells[row, 2].Value)!.Trim(),
                        // Convert.ToString(wsder.Cells[row, 3].Value)!.Trim(),
                        // Convert.ToString(wsder.Cells[row, 4].Value)!.Trim(),
                        // Convert.ToString(wsder.Cells[row, 5].Value)!.Trim(),
                        // Convert.ToString(wsder.Cells[row, 6].Value)!.Trim(),
                        // Convert.ToString(wsder.Cells[row, 7].Value)!.Trim(),
                        // Convert.ToString(wsder.Cells[row, 8].Value)!.Trim(),
                        // Convert.ToString(wsder.Cells[row, 9].Value)!.Trim(),
                        // Convert.ToString(wsder.Cells[row, 10].Value)!.Trim()
                        );
                    }
                    string tokenHeader = Request.Headers["Authorization"]!;
                    tokenHeader = tokenHeader.Replace("Bearer ", "");
                    // get user data from token
                    var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                    if (__res.UserEmail != null)
                    {
                        ToolsUploadParam __bodytoken = new()
                        {
                            userid = __res.ProfileID,
                            useremail = __res.UserEmail
                        };
                        var __val = await __uploadRepo.ImportBudgetAdjustment(der, __bodytoken.userid!);
                        if (__val.Any())
                            return Conflict(new BaseResponse
                            {
                                error = true,
                                code = 409,
                                message = MessageService.UploadFailed,
                                values = __val
                            });
                        else
                        {
                            return Ok(new BaseResponse
                            {
                                error = false,
                                code = 200,
                                message = MessageService.UploadSuccess,
                                values = __val
                            }
                            );
                        }
                    }
                    else
                    {
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                    }
                }
                catch (Exception __ex)
                {
                    return Conflict(new BaseResponse
                    {
                        error = true,
                        code = 409,
                        message = __ex.Message

                    });
                }
            }
            else
            {
                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = "Excel Template is not found",
                });
            }
        }
        /// <summary>
        /// Upload budget attribute data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/budgetattribute", Name = "budgetattributeimport")]
        public async Task<IActionResult> ImportBudgetAttribute(IFormFile formFile)
        {
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }
            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var rowCount = 0;
            ExcelWorksheet wsaccount = package.Workbook.Worksheets[11];
            rowCount = wsaccount.Dimension.Rows;
            DataTable account = new("AccountType");

            account.Columns.Add("Allocation", typeof(string));
            account.Columns.Add("Derivative", typeof(string));
            account.Columns.Add("1", typeof(string));
            account.Columns.Add("2", typeof(string));
            account.Columns.Add("3", typeof(string));
            account.Columns.Add("4", typeof(string));
            account.Columns.Add("5", typeof(string));
            account.Columns.Add("6", typeof(string));
            account.Columns.Add("7", typeof(string));
            account.Columns.Add("8", typeof(string));
            account.Columns.Add("9", typeof(string));
            account.Columns.Add("10", typeof(string));
            account.Columns.Add("11", typeof(string));
            account.Columns.Add("12", typeof(string));
            account.Columns.Add("13", typeof(string));
            account.Columns.Add("14", typeof(string));
            account.Columns.Add("15", typeof(string));
            account.Columns.Add("16", typeof(string));
            account.Columns.Add("17", typeof(string));
            account.Columns.Add("18", typeof(string));
            account.Columns.Add("19", typeof(string));
            account.Columns.Add("20", typeof(string));
            account.Columns.Add("21", typeof(string));
            account.Columns.Add("22", typeof(string));

            account.Columns.Add("23", typeof(string));
            account.Columns.Add("24", typeof(string));
            account.Columns.Add("25", typeof(string));
            account.Columns.Add("26", typeof(string));
            account.Columns.Add("27", typeof(string));
            account.Columns.Add("28", typeof(string));
            account.Columns.Add("29", typeof(string));
            account.Columns.Add("30", typeof(string));

            account.Columns.Add("31", typeof(string));
            account.Columns.Add("32", typeof(string));
            account.Columns.Add("33", typeof(string));
            account.Columns.Add("34", typeof(string));
            account.Columns.Add("35", typeof(string));
            account.Columns.Add("36", typeof(string));
            account.Columns.Add("37", typeof(string));
            account.Columns.Add("38", typeof(string));
            account.Columns.Add("39", typeof(string));
            account.Columns.Add("40", typeof(string));

            account.Columns.Add("41", typeof(string));
            account.Columns.Add("42", typeof(string));
            account.Columns.Add("43", typeof(string));
            account.Columns.Add("44", typeof(string));
            account.Columns.Add("45", typeof(string));
            account.Columns.Add("46", typeof(string));
            account.Columns.Add("47", typeof(string));
            account.Columns.Add("48", typeof(string));
            account.Columns.Add("49", typeof(string));
            account.Columns.Add("50", typeof(string));

            account.Columns.Add("51", typeof(string));
            account.Columns.Add("52", typeof(string));
            account.Columns.Add("53", typeof(string));
            account.Columns.Add("54", typeof(string));
            account.Columns.Add("55", typeof(string));
            account.Columns.Add("56", typeof(string));
            account.Columns.Add("57", typeof(string));
            account.Columns.Add("58", typeof(string));
            account.Columns.Add("59", typeof(string));
            account.Columns.Add("60", typeof(string));

            account.Columns.Add("61", typeof(string));
            account.Columns.Add("62", typeof(string));
            account.Columns.Add("63", typeof(string));
            account.Columns.Add("64", typeof(string));
            account.Columns.Add("65", typeof(string));
            account.Columns.Add("66", typeof(string));
            account.Columns.Add("67", typeof(string));
            account.Columns.Add("68", typeof(string));
            account.Columns.Add("69", typeof(string));
            account.Columns.Add("70", typeof(string));

            for (int row = 2; row <= rowCount; row++)
            {
                account.Rows.Add(
                    wsaccount.Cells[row, 1].Value.ToString()!.Trim(),
                    wsaccount.Cells[row, 2].Value ?? string.Empty,
                    wsaccount.Cells[row, 3].Value ?? string.Empty,
                    wsaccount.Cells[row, 4].Value ?? string.Empty,
                    wsaccount.Cells[row, 5].Value ?? string.Empty,
                    wsaccount.Cells[row, 6].Value ?? string.Empty,
                    wsaccount.Cells[row, 7].Value ?? string.Empty,
                    wsaccount.Cells[row, 8].Value ?? string.Empty,
                    wsaccount.Cells[row, 9].Value ?? string.Empty,
                    wsaccount.Cells[row, 10].Value ?? string.Empty,
                    wsaccount.Cells[row, 11].Value ?? string.Empty,
                    wsaccount.Cells[row, 12].Value ?? string.Empty,
                    wsaccount.Cells[row, 13].Value ?? string.Empty,
                    wsaccount.Cells[row, 14].Value ?? string.Empty,
                    wsaccount.Cells[row, 15].Value ?? string.Empty,
                    wsaccount.Cells[row, 16].Value ?? string.Empty,
                    wsaccount.Cells[row, 17].Value ?? string.Empty,
                    wsaccount.Cells[row, 18].Value ?? string.Empty,
                    wsaccount.Cells[row, 19].Value ?? string.Empty,
                    wsaccount.Cells[row, 20].Value ?? string.Empty,
                    wsaccount.Cells[row, 21].Value ?? string.Empty,
                    wsaccount.Cells[row, 22].Value ?? string.Empty,
                    wsaccount.Cells[row, 23].Value ?? string.Empty,
                    wsaccount.Cells[row, 24].Value ?? string.Empty,
                    wsaccount.Cells[row, 25].Value ?? string.Empty,
                    wsaccount.Cells[row, 26].Value ?? string.Empty,
                    wsaccount.Cells[row, 27].Value ?? string.Empty,
                    wsaccount.Cells[row, 28].Value ?? string.Empty,
                    wsaccount.Cells[row, 29].Value ?? string.Empty,
                    wsaccount.Cells[row, 30].Value ?? string.Empty,
                    wsaccount.Cells[row, 31].Value ?? string.Empty,
                    wsaccount.Cells[row, 32].Value ?? string.Empty,
                    wsaccount.Cells[row, 33].Value ?? string.Empty,
                    wsaccount.Cells[row, 34].Value ?? string.Empty,
                    wsaccount.Cells[row, 35].Value ?? string.Empty,
                    wsaccount.Cells[row, 36].Value ?? string.Empty,
                    wsaccount.Cells[row, 37].Value ?? string.Empty,
                    wsaccount.Cells[row, 38].Value ?? string.Empty,
                    wsaccount.Cells[row, 39].Value ?? string.Empty,
                    wsaccount.Cells[row, 40].Value ?? string.Empty,
                    wsaccount.Cells[row, 41].Value ?? string.Empty,
                    wsaccount.Cells[row, 42].Value ?? string.Empty,
                    wsaccount.Cells[row, 43].Value ?? string.Empty,
                    wsaccount.Cells[row, 44].Value ?? string.Empty,
                    wsaccount.Cells[row, 45].Value ?? string.Empty,
                    wsaccount.Cells[row, 46].Value ?? string.Empty,
                    wsaccount.Cells[row, 47].Value ?? string.Empty,
                    wsaccount.Cells[row, 48].Value ?? string.Empty,
                    wsaccount.Cells[row, 49].Value ?? string.Empty,
                    wsaccount.Cells[row, 50].Value ?? string.Empty,
                    wsaccount.Cells[row, 51].Value ?? string.Empty,
                    wsaccount.Cells[row, 52].Value ?? string.Empty,
                    wsaccount.Cells[row, 53].Value ?? string.Empty,
                    wsaccount.Cells[row, 54].Value ?? string.Empty,
                    wsaccount.Cells[row, 55].Value ?? string.Empty,
                    wsaccount.Cells[row, 56].Value ?? string.Empty,
                    wsaccount.Cells[row, 57].Value ?? string.Empty,
                    wsaccount.Cells[row, 58].Value ?? string.Empty,
                    wsaccount.Cells[row, 59].Value ?? string.Empty,
                    wsaccount.Cells[row, 60].Value ?? string.Empty,
                    wsaccount.Cells[row, 61].Value ?? string.Empty,
                    wsaccount.Cells[row, 62].Value ?? string.Empty,
                    wsaccount.Cells[row, 63].Value ?? string.Empty,
                    wsaccount.Cells[row, 64].Value ?? string.Empty,
                    wsaccount.Cells[row, 65].Value ?? string.Empty,
                    wsaccount.Cells[row, 66].Value ?? string.Empty,
                    wsaccount.Cells[row, 67].Value ?? string.Empty,
                    wsaccount.Cells[row, 68].Value ?? string.Empty,
                    wsaccount.Cells[row, 69].Value ?? string.Empty,
                    wsaccount.Cells[row, 70].Value ?? string.Empty,
                    wsaccount.Cells[row, 70].Value ?? string.Empty

                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                var __val = await __uploadRepo.ImportBudgetAttribute(account, __bodytoken.userid!);
                return Ok(new
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
        /// <summary>
        /// Upload budget brand data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/budgetbrand", Name = "budgetbrandimport")]
        public async Task<IActionResult> ImportBudgetBrand(IFormFile formFile)
        {
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }


            DataTable budgetbrand = new("ImportBrandType");
            budgetbrand.Columns.Add("Allocation", typeof(string));
            budgetbrand.Columns.Add("Derivative", typeof(string));
            budgetbrand.Columns.Add("1", typeof(string));
            budgetbrand.Columns.Add("2", typeof(string));
            budgetbrand.Columns.Add("3", typeof(string));
            budgetbrand.Columns.Add("4", typeof(string));
            budgetbrand.Columns.Add("5", typeof(string));
            budgetbrand.Columns.Add("6", typeof(string));
            budgetbrand.Columns.Add("7", typeof(string));
            budgetbrand.Columns.Add("8", typeof(string));
            budgetbrand.Columns.Add("9", typeof(string));
            budgetbrand.Columns.Add("10", typeof(string));
            budgetbrand.Columns.Add("11", typeof(string));
            budgetbrand.Columns.Add("12", typeof(string));
            budgetbrand.Columns.Add("13", typeof(string));
            budgetbrand.Columns.Add("14", typeof(string));
            budgetbrand.Columns.Add("15", typeof(string));
            budgetbrand.Columns.Add("16", typeof(string));
            budgetbrand.Columns.Add("17", typeof(string));
            budgetbrand.Columns.Add("18", typeof(string));
            budgetbrand.Columns.Add("19", typeof(string));
            budgetbrand.Columns.Add("20", typeof(string));
            budgetbrand.Columns.Add("21", typeof(string));
            budgetbrand.Columns.Add("22", typeof(string));

            budgetbrand.Columns.Add("23", typeof(string));
            budgetbrand.Columns.Add("24", typeof(string));
            budgetbrand.Columns.Add("25", typeof(string));
            budgetbrand.Columns.Add("26", typeof(string));
            budgetbrand.Columns.Add("27", typeof(string));
            budgetbrand.Columns.Add("28", typeof(string));
            budgetbrand.Columns.Add("29", typeof(string));
            budgetbrand.Columns.Add("30", typeof(string));



            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var rowCount = 0;
            ExcelWorksheet wsbudgetbrand = package.Workbook.Worksheets[10]; // ALLOCATION brand
            rowCount = wsbudgetbrand.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                budgetbrand.Rows.Add(
                    wsbudgetbrand.Cells[row, 1].Value.ToString()!.Trim(),
                    wsbudgetbrand.Cells[row, 2].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 3].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 4].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 5].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 6].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 7].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 8].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 9].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 10].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 11].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 12].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 13].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 14].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 15].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 16].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 17].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 18].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 19].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 20].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 21].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 22].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 23].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 24].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 25].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 26].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 27].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 28].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 29].Value ?? string.Empty,
                    wsbudgetbrand.Cells[row, 30].Value ?? string.Empty

                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportBudgetBrand(budgetbrand, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload master data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/master", Name = "masterimport")]
        public async Task<IActionResult> ImportMaster(IFormFile formFile)
        {

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var rowCount = 0;


            ExcelWorksheet wsbrand = package.Workbook.Worksheets[0];
            rowCount = wsbrand.Dimension.Rows;
            DataTable brand = new("BrandType");
            brand.Columns.Add("Brand", typeof(string));
            brand.Columns.Add("SKU", typeof(string));
            brand.Columns.Add("Principal", typeof(string));
            for (int row = 2; row <= rowCount; row++)
            {
                brand.Rows.Add(
                    wsbrand.Cells[row, 1].Value.ToString()!.Trim(),
                    wsbrand.Cells[row, 2].Value.ToString()!.Trim(),
                    wsbrand.Cells[row, 3].Value.ToString()!.Trim()
                );
            }

            ExcelWorksheet wschannel = package.Workbook.Worksheets[1];
            rowCount = wschannel.Dimension.Rows;
            DataTable channel = new("ChannelType");
            channel.Columns.Add("Channel", typeof(string));
            channel.Columns.Add("SubChannel", typeof(string));
            channel.Columns.Add("Account", typeof(string));
            channel.Columns.Add("SubAccount", typeof(string));
            for (int row = 2; row <= rowCount; row++)
            {
                channel.Rows.Add(
                    wschannel.Cells[row, 1].Value.ToString()!.Trim(),
                    wschannel.Cells[row, 2].Value.ToString()!.Trim(),
                    wschannel.Cells[row, 3].Value.ToString()!.Trim(),
                    wschannel.Cells[row, 4].Value.ToString()!.Trim()
                );
            }

            ExcelWorksheet wssubact = package.Workbook.Worksheets[2];
            rowCount = wssubact.Dimension.Rows;
            DataTable subact = new("SubActivityType");
            subact.Columns.Add("SubActivityType", typeof(string));
            for (int row = 2; row <= rowCount; row++)
            {
                subact.Rows.Add(
                    wssubact.Cells[row, 1].Value.ToString()!.Trim()
                );
            }


            ExcelWorksheet wsctg = package.Workbook.Worksheets[3];
            rowCount = wsctg.Dimension.Rows;
            DataTable ctg = new("ActivityType");
            ctg.Columns.Add("Category", typeof(string));
            ctg.Columns.Add("SubCategory", typeof(string));
            ctg.Columns.Add("Activity", typeof(string));
            ctg.Columns.Add("SubActivity", typeof(string));
            ctg.Columns.Add("SubActivityType", typeof(string));

            for (int row = 2; row <= rowCount; row++)
            {
                ctg.Rows.Add(
                    wsctg.Cells[row, 1].Value.ToString()!.Trim(),
                    wsctg.Cells[row, 2].Value.ToString()!.Trim(),
                    wsctg.Cells[row, 3].Value.ToString()!.Trim(),
                    wsctg.Cells[row, 4].Value.ToString()!.Trim(),
                    wsctg.Cells[row, 5].Value.ToString()!.Trim()
                );
            }

            ExcelWorksheet wsdis = package.Workbook.Worksheets[4];
            rowCount = wsdis.Dimension.Rows;
            DataTable dis = new("DistributorType");
            dis.Columns.Add("LongDesc", typeof(string));
            dis.Columns.Add("ShortDesc", typeof(string));
            dis.Columns.Add("Company", typeof(string));
            dis.Columns.Add("Address", typeof(string));
            dis.Columns.Add("NPWP", typeof(string));


            for (int row = 2; row <= rowCount; row++)
            {
                dis.Rows.Add(
                    wsdis.Cells[row, 1].Value.ToString()!.Trim(),
                    wsdis.Cells[row, 2].Value.ToString()!.Trim(),
                    wsdis.Cells[row, 3].Value.ToString()!.Trim(),
                    wsdis.Cells[row, 4].Value.ToString()!.Trim(),
                    wsdis.Cells[row, 5].Value.ToString()!.Trim()
                );
            }

            ExcelWorksheet wspcp = package.Workbook.Worksheets[5];
            rowCount = wspcp.Dimension.Rows;
            DataTable pcp = new("PrincipalType");
            pcp.Columns.Add("LongDesc", typeof(string));
            pcp.Columns.Add("ShortDesc", typeof(string));

            for (int row = 2; row <= rowCount; row++)
            {
                pcp.Rows.Add(
                    wspcp.Cells[row, 1].Value.ToString()!.Trim(),
                    wspcp.Cells[row, 2].Value.ToString()!.Trim()

                );
            }

            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportMaster(brand, channel, subact, ctg, dis, pcp, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload brand data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/brand", Name = "masterbrandimport")]
        public async Task<IActionResult> ImportMasterBrand(IFormFile formFile)
        {
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var rowCount = 0;


            ExcelWorksheet wsbrand = package.Workbook.Worksheets[1];
            rowCount = wsbrand.Dimension.Rows;
            DataTable brand = new("BrandType");
            brand.Columns.Add("SeqNo", typeof(string));
            brand.Columns.Add("Brand", typeof(string));
            brand.Columns.Add("SKU", typeof(string));
            brand.Columns.Add("Principal", typeof(string));


            for (int row = 2; row <= rowCount; row++)
            {
                brand.Rows.Add(
                    wsbrand.Cells[row, 1].Value.ToString()!.Trim(),
                    wsbrand.Cells[row, 2].Value.ToString()!.Trim(),
                    wsbrand.Cells[row, 3].Value.ToString()!.Trim(),
                    wsbrand.Cells[row, 4].Value.ToString()!.Trim()
                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportMasterBrand(brand, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload subactivity data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/subactivitytype", Name = "subactivitytype_upload")]
        public async Task<IActionResult> ImportMasterSubActivity(IFormFile formFile)
        {

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var rowCount = 0;
            ExcelWorksheet wssubact = package.Workbook.Worksheets[3];
            rowCount = wssubact.Dimension.Rows;
            DataTable subact = new("SubActivityType");
            subact.Columns.Add("SubActivityType", typeof(string));
            for (int row = 2; row <= rowCount; row++)
            {
                subact.Rows.Add(
                    wssubact.Cells[row, 1].Value.ToString()!.Trim()
                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportSubactivityType(subact, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload activity data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/activity", Name = "masteractivityimport")]
        public async Task<IActionResult> ImportMasterActivity(IFormFile formFile)
        {

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var rowCount = 0;
            ExcelWorksheet wsctg = package.Workbook.Worksheets[4];
            rowCount = wsctg.Dimension.Rows;
            DataTable ctg = new("ActivityType");
            ctg.Columns.Add("Category", typeof(string));
            ctg.Columns.Add("CategoryShortDesc", typeof(string));
            ctg.Columns.Add("SubCategory", typeof(string));
            ctg.Columns.Add("Activity", typeof(string));
            ctg.Columns.Add("SubActivity", typeof(string));
            ctg.Columns.Add("SubActivityType", typeof(string));

            for (int row = 2; row <= rowCount; row++)
            {
                ctg.Rows.Add(
                    wsctg.Cells[row, 1].Value.ToString()!.Trim(),
                    wsctg.Cells[row, 2].Value.ToString()!.Trim(),
                    wsctg.Cells[row, 3].Value.ToString()!.Trim(),
                    wsctg.Cells[row, 4].Value.ToString()!.Trim(),
                    wsctg.Cells[row, 5].Value.ToString()!.Trim(),
                    wsctg.Cells[row, 6].Value.ToString()!.Trim()
                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportCategory(ctg, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload distributor data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/distributor", Name = "masterdistributorimport")]
        public async Task<IActionResult> ImportMasterDistributor(IFormFile formFile)
        {

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var rowCount = 0;
            ExcelWorksheet wsdis = package.Workbook.Worksheets[5];
            rowCount = wsdis.Dimension.Rows;
            DataTable dis = new("DistributorType");
            dis.Columns.Add("LongDesc", typeof(string));
            dis.Columns.Add("ShortDesc", typeof(string));
            dis.Columns.Add("Company", typeof(string));
            dis.Columns.Add("Address", typeof(string));
            dis.Columns.Add("NPWP", typeof(string));


            for (int row = 2; row <= rowCount; row++)
            {
                dis.Rows.Add(
                    wsdis.Cells[row, 1].Value.ToString()!.Trim(),
                    wsdis.Cells[row, 2].Value.ToString()!.Trim(),
                    wsdis.Cells[row, 3].Value.ToString()!.Trim(),
                    wsdis.Cells[row, 4].Value.ToString()!.Trim(),
                    wsdis.Cells[row, 5].Value.ToString()!.Trim()
                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportDistributor(dis, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload principal data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/principal", Name = "masterprincipalimport")]
        public async Task<IActionResult> ImportMasterPrincipal(IFormFile formFile)
        {

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var rowCount = 0;
            ExcelWorksheet wspcp = package.Workbook.Worksheets[6];
            rowCount = wspcp.Dimension.Rows;
            DataTable pcp = new("PrincipalType");
            pcp.Columns.Add("LongDesc", typeof(string));
            pcp.Columns.Add("ShortDesc", typeof(string));

            for (int row = 2; row <= rowCount; row++)
            {
                pcp.Rows.Add(
                    wspcp.Cells[row, 1].Value.ToString()!.Trim(),
                    wspcp.Cells[row, 2].Value.ToString()!.Trim()

                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportPrincipal(pcp, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload region approval data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/region", Name = "regionimport")]
        public async Task<IActionResult> Import(IFormFile formFile)
        {

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            ExcelWorksheet wsreg = package.Workbook.Worksheets[0];
            var rowCount = wsreg.Dimension.Rows;
            DataTable reg = new("RegionType");
            reg.Columns.Add("Region", typeof(string));


            for (int row = 2; row <= rowCount; row++)
            {
                reg.Rows.Add(
                    wsreg.Cells[row, 1].Value.ToString()!.Trim()
                );
            }

            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportRegion(reg, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload selling point data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/sellingpoint", Name = "sellpoimport")]
        public async Task<IActionResult> SellpoImport(IFormFile formFile)
        {

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
            }

            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            ExcelWorksheet wsslp = package.Workbook.Worksheets[0];
            var rowCount = wsslp.Dimension.Rows;
            DataTable sellpo = new("SellingPointType");
            sellpo.Columns.Add("SellingPoint", typeof(string));
            sellpo.Columns.Add("SellingPointDesc", typeof(string));
            sellpo.Columns.Add("Region", typeof(string));
            sellpo.Columns.Add("Status", typeof(string));
            sellpo.Columns.Add("ProfitCenter", typeof(string));
            sellpo.Columns.Add("AreaCode", typeof(string));
            sellpo.Columns.Add("Area", typeof(string));


            for (int row = 2; row <= rowCount; row++)
            {
                sellpo.Rows.Add(
                    wsslp.Cells[row, 1].Value.ToString()!.Trim(),
                    wsslp.Cells[row, 2].Value.ToString()!.Trim(),
                    wsslp.Cells[row, 3].Value.ToString()!.Trim(),
                    wsslp.Cells[row, 4].Value.ToString()!.Trim(),
                    wsslp.Cells[row, 5].Value.ToString()!.Trim(),
                    wsslp.Cells[row, 6].Value.ToString()!.Trim(),
                    wsslp.Cells[row, 7].Value.ToString()!.Trim()

                );
            }
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            // get user data from token
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                ToolsUploadParam __bodytoken = new()
                {
                    userid = __res.ProfileID,
                    useremail = __res.UserEmail
                };
                await __uploadRepo.ImportSellingpoint(sellpo, __bodytoken.userid!);
                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                });
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }
        /// <summary>
        /// Upload promo attachment save
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/tools/promoattachment", Name = "promo_attachmentstore_upload")]
        public async Task<IActionResult> CreatePromoAttachment([FromBody] SavePromoAttachmentParam body)
        {
            // IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    SavePromoAttachmentParam __bodytoken = new()
                    {
                        PromoId = body.PromoId,
                        DocLink = body.DocLink,
                        FileName = body.FileName,
                        CreateBy = __res.ProfileID
                    };
                    await __uploadRepo.CreatePromoAttachment(__bodytoken);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = null,
                        message = MessageService.SaveSuccess
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = MessageService.SaveFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Remove promo attachment save
        /// </summary>
        /// <returns></returns>
        [HttpDelete("api/tools/promoattachment", Name = "promo_attachment_delete")]
        public async Task<IActionResult> RemovePromoAttachment([FromQuery] int PromoId, string DocLink)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                await __uploadRepo.RemovePromoAttachment(PromoId, DocLink);
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    values = null,
                    message = MessageService.DeleteSucceed
                });
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get Promo by RefId
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/tools/search/promo/refid", Name = "get_promo_bu_refid")]
        public async Task<IActionResult> SearchPromoByRefId([FromQuery] string refId)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __uploadRepo.SearchPromoByRefId(refId);
                if (__val != null)
                {
                    result = Ok(new
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    result = NotFound(new { code = 404, error = true, Message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, error = true, Message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get Budget hirarki 
        /// </summary>
        /// <param name="period"></param>
        /// <param name="entityId"></param>
        /// <param name="budgetName"></param>
        /// <returns></returns>
        [HttpGet("api/tools/budgetadjustment/hierarchy", Name = "get_budgethierarchy-foradjust")]
        public async Task<IActionResult> GetBudgetHierarchyforAdjust([FromQuery] string period, int entityId, string budgetName)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __uploadRepo.GetBudgetHierarchyforAdjust(period, entityId, budgetName);
                if (__val != null)
                {
                    result = Ok(new
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    result = NotFound(new { code = 404, error = true, Message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, error = true, Message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get budget allocation for adjust 
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/tools/budgetadjustment/allocation", Name = "get_budgetallocation-foradjust")]
        public async Task<IActionResult> GetBudgetAllocationforAdjust([FromQuery] string period, int entityId)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __uploadRepo.GetBudgetAllocationforAdjust(period, entityId);
                if (__val != null)
                {
                    result = Ok(new
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    result = NotFound(new { code = 404, error = true, Message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, error = true, Message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/tools/budgetadjustment/entity", Name = "budgetadjustment_entity")]
        public async Task<IActionResult> GetEntityBudgetAdjustment()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __uploadRepo.GetEntityList();
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
        /// Get List Notes Proom Attachments
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/tools/promo/attachment", Name = "tools_promo_attachment")]
        public async Task<IActionResult> GetPromoNoteListAttachment([FromQuery] UploadModelPromoAttachment param)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                var __val = await __uploadRepo.GetPromoNoteListAttachment(param.periode!, param.entity, __res.ProfileID!);
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
        /// Get promo attribut for sending email approver
        /// </summary>
        /// <param name="promoId"></param>
        /// <returns></returns>
        [HttpPost("api/tools/upload/promoattachment", Name = "tools_upload_promoattachment")]
        public async Task<IActionResult> UploadPromoAttachment([FromBody] UploadPromoAttachmentParam body)
        {
            // IActionResult result;

            if (!ModelState.IsValid) return Conflict(ModelState);
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                try
                {
                    var __val = await __uploadRepo.ImportPromoUploadAttachment(body.promoId, __res.ProfileID, __res.UserEmail);
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

                    return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
                }
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }

        }

        /// <summary>
        /// upload excel with promo attachment info and get promo with SKP note in return
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns>List of promo with SKP note</returns>
        [HttpPost("api/tools/upload/promoattachmentget", Name = "get_tools_upload_promoattachment")]
        public async Task<IActionResult> GetUploadPromoAttachment(IFormFile formFile)
        {
            // IActionResult result;

            if (!ModelState.IsValid) return Conflict(ModelState);
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");
            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {

                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
                }
                using var stream = new MemoryStream();
                await formFile.CopyToAsync(stream);

                using var package = new ExcelPackage(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var rowCount = 0;
                ExcelWorksheet wsalloc = package.Workbook.Worksheets[0];
                rowCount = wsalloc.Dimension.Rows;

                // read xls into datatable
                DataTable alloc = new("AllocationType");
                try
                {

                    alloc.Columns.Add("PromoNumber", typeof(string));
                    alloc.Columns.Add("Filename1", typeof(string));
                    alloc.Columns.Add("Filename2", typeof(string));
                    alloc.Columns.Add("Filename3", typeof(string));
                    alloc.Columns.Add("Filename4", typeof(string));
                    alloc.Columns.Add("Filename5", typeof(string));
                    alloc.Columns.Add("Filename6", typeof(string));
                    alloc.Columns.Add("Filename7", typeof(string));

                    int colCount = alloc.Columns.Count;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        List<object> cells = new List<object>();
                        for (int col = 1; col <= colCount; col++)
                        {
                            // ignored empty row
                            if (wsalloc.Cells[row, 1].Value != null)
                            {
                                cells.Add(Convert.ToString(wsalloc.Cells[row, col].Value));
                            }
                        }
                        if (cells.Count > 0)
                            alloc.Rows.Add(cells.ToArray());
                    }
                }
                catch (Exception __ex)
                {
                    return Conflict(new BaseResponse
                    {
                        error = true,
                        code = 500,
                        message = "Template Error",
                    });
                }
                try
                {
                    var __val = await __uploadRepo.GetImportPromoUploadAttachment(alloc, __res.ProfileID);
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

                    return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
                }
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }

        }
    }
}