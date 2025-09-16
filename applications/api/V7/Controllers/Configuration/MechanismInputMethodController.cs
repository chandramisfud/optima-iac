using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repositories.Contracts;
using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using Repositories.Repos;
using System.ComponentModel.DataAnnotations;
using System.Data;
using V7.Controllers;
using V7.MessagingServices;
using V7.Model.FinanceReport;
using V7.Services;

namespace V7.Controllers.Configuration
{
    public partial class ConfigController : BaseController
    {
        /// <summary>
        /// Upload Data mechanisme input method
        /// </summary>
        /// <param name="reminderType"></param>
        /// <returns></returns>
        [HttpPost("api/config/mechanisminputmethod/upload", Name = "Set_Mechanism_Input_Method_upload")]
        public async Task<IActionResult> SetMechanisminputmethod(IFormFile formFile)
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
                using var stream = new MemoryStream();
                await formFile.CopyToAsync(stream);
                DataTable dtTable = new("nama table");
                try
                {
                    using var package = new ExcelPackage(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet sheet = package.Workbook.Worksheets[0];
                    int rowCount = sheet.Dimension.Rows;

                    dtTable.Columns.Add("categoryDesc", typeof(string));
                    dtTable.Columns.Add("subCategoryDesc", typeof(string));
                    dtTable.Columns.Add("activityDesc", typeof(string));
                    dtTable.Columns.Add("subActivityDesc", typeof(string));
                    dtTable.Columns.Add("inputMethod", typeof(string));

                    for (int row = 2; row <= rowCount; row++)
                    {
                        List<object> cells = new List<object>();
                        for (int col = 1; col <= dtTable.Columns.Count; col++)
                        {
                            // ignored empty row
                            if (sheet.Cells[row, 1].Value != null)
                            {
                                cells.Add(Convert.ToString(sheet.Cells[row, col].Value));
                            }
                        }
                        // ignored empty row
                        if (cells.Count > 0)
                            dtTable.Rows.Add(cells.ToArray());
                    }
                }
                catch (Exception)
                {

                    throw new Exception("Please check template entry");
                }

                var log = __ConfigRepo.UploadMechanismInput(dtTable, formFile.FileName, __res.ProfileID, __res.UserEmail);

                result = Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.SaveSuccess
                });

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get mechanisme input method
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/config/mechanisminputmethod", Name = "get_mechanism_input_method")]
        public async Task<IActionResult> GetMechanismInputMethodLP()
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);

                var __val = await __ConfigRepo.GetMechanismInput();
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
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
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