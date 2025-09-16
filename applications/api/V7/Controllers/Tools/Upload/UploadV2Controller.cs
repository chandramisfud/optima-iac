using Microsoft.AspNetCore.Mvc;
using NLog.Targets;
using NLog;
using Repositories.Entities.Models;
using V7.MessagingServices;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;
using System.Data;
using V7.Services;


namespace V7.Controllers.Tools
{
    public partial class ToolsController : BaseController
    {

        [HttpPost("api/tools/promorecon/status", Name = "set_promo_recon_status")]
        public async Task<IActionResult> SetPromoReconStatus(IFormFile formFile)
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
                DataTable dtTable = new("promo recon");
                try
                {
                    using var package = new ExcelPackage(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet sheet = package.Workbook.Worksheets[0];
                    int rowCount = sheet.Dimension.Rows;

                    dtTable.Columns.Add("keyid", typeof(string));

                    for (int row = 2; row <= rowCount; row++)
                    {
                        List<object> cells = new List<object>();
                        // ignored empty row
                        if (sheet.Cells[row, 1].Value != null)
                        {
                            cells.Add(sheet.Cells[row, 1].Value);
                        }
                        if (cells.Count > 0)
                            dtTable.Rows.Add(cells.ToArray());
                    }
                }
                catch (Exception)
                {

                    throw new Exception("Please check template entry");
                }

                string uploadActivity = "Mass Update Recon Status";
                var log = __uploadRepo.InsertUploadLog(uploadActivity, formFile.FileName, __res.ProfileID,
                    __res.UserEmail, MessageService.UploadLogSuccess);

                var res = __uploadRepo.UpdatePromoReconStatus(dtTable, __res.ProfileID, __res.UserEmail);

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

    }
}
