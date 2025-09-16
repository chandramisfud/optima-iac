using System.Data;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.DebitNote;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    public partial class DebitNoteController : BaseController
    {
        /// <summary>
        /// Upload DN, Old API = "api/debetnote/create_by_batch"
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/dn/upload", Name = "dn_upload")]
        public async Task<IActionResult> DNUpload(IFormFile formFile)
        {
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "unsupported extension" });
            }
            using var stream = new MemoryStream();
            await formFile.CopyToAsync(stream);
            try
            {
                using var package = new ExcelPackage(stream);
                var rowCount = 0;
                ExcelWorksheet dn = package.Workbook.Worksheets[0];
                rowCount = dn.Dimension.Rows;
                DataTable header = new("DebetNoteType");
                header.Columns.Add("Periode", typeof(string));
                header.Columns.Add("Entity", typeof(string));
                header.Columns.Add("Distributor", typeof(string));
                header.Columns.Add("SubAccount", typeof(string));
                header.Columns.Add("Activity", typeof(string));
                header.Columns.Add("PromoNumber", typeof(string));
                header.Columns.Add("InternalDocNumber", typeof(string));
                header.Columns.Add("TotalClaim", typeof(string));
                header.Columns.Add("DueDate", typeof(string));
                header.Columns.Add("FeeDesc", typeof(string));
                header.Columns.Add("FeeAmount", typeof(string));
                header.Columns.Add("DNType", typeof(string));
                header.Columns.Add("statusPPN", typeof(string));
                header.Columns.Add("PPNPct", typeof(string));
                header.Columns.Add("PPNAmt", typeof(string));
                header.Columns.Add("statusPPH", typeof(string));
                header.Columns.Add("PPHPct", typeof(string));
                header.Columns.Add("PPHAmt", typeof(string));
                header.Columns.Add("FPNumber", typeof(string));
                header.Columns.Add("FPDate", typeof(string));
                header.Columns.Add("TaxLevel", typeof(string));

                for (int row = 2; row <= rowCount; row++)
                {
                    header.Rows.Add(
                        dn.Cells[row, 1].Value.ToString()!.Trim(),
                        dn.Cells[row, 2].Value.ToString()!.Trim(),
                        dn.Cells[row, 3].Value.ToString()!.Trim(),
                        dn.Cells[row, 4].Value.ToString()!.Trim(),
                        dn.Cells[row, 5].Value ?? string.Empty,
                        dn.Cells[row, 6].Value ?? string.Empty,
                        dn.Cells[row, 7].Value ?? string.Empty,
                        dn.Cells[row, 8].Value ?? string.Empty,
                        dn.Cells[row, 9].Value ?? string.Empty,
                        dn.Cells[row, 10].Value ?? string.Empty,
                        dn.Cells[row, 11].Value ?? string.Empty,
                        dn.Cells[row, 12].Value ?? string.Empty,
                        dn.Cells[row, 13].Value ?? string.Empty,
                        dn.Cells[row, 14].Value ?? string.Empty,
                        dn.Cells[row, 15].Value ?? string.Empty,
                        dn.Cells[row, 16].Value ?? string.Empty,
                        dn.Cells[row, 17].Value ?? string.Empty,
                        dn.Cells[row, 18].Value ?? string.Empty,
                        dn.Cells[row, 19].Value ?? string.Empty,
                        dn.Cells[row, 20].Value ?? string.Empty,
                        dn.Cells[row, 21].Value ?? string.Empty
                    );
                }
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    DNUploadparam __bodytoken = new()
                    {
                        userId = __res.ProfileID,
                    };
                    DNUploadReturn result = await __repoDNUpload.DNUpload(header, __bodytoken.userId);

                    return Ok(new
                    {
                        error = false,
                        code = 200,
                        message = MessageService.UploadSuccess,
                        values = result.data,
                        RecordTotal = result.totalRecord
                    });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.UploadFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 404, message = __ex.Message });
            }

        }
        // debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}
        /// <summary>
        /// DN Filter for Upload, Old API = "debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/upload/filter", Name = "dn_upload_filter")]
        public async Task<IActionResult> DNUploadFilter([FromForm] DNFilterParam param)
        {
            if (!Path.GetExtension(param.formFile!.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "unsupported extension" });
            }
            using var stream = new MemoryStream();
            await param.formFile.CopyToAsync(stream);
            try
            {
                using var package = new ExcelPackage(stream);
                var rowCount = 0;
                ExcelWorksheet dn = package.Workbook.Worksheets[0];
                rowCount = dn.Dimension.Rows;
                DataTable header = new("TemplateDebetNote");
                header.Columns.Add("RefId", typeof(string));
                header.Columns.Add("PromoRefId", typeof(string));
                header.Columns.Add("Activity", typeof(string));
                header.Columns.Add("TotalClaim", typeof(string));
                header.Columns.Add("LastStatus", typeof(string));
                header.Columns.Add("LastUpdate", typeof(string));

                for (int row = 2; row <= rowCount; row++)
                {
                    header.Rows.Add(
                        dn.Cells[row, 1].Value.ToString()!.Trim(),
                        dn.Cells[row, 2].Value ?? string.Empty,
                        dn.Cells[row, 3].Value.ToString()!.Trim(),
                        dn.Cells[row, 4].Value.ToString()!.Trim(),
                        dn.Cells[row, 5].Value.ToString()!.Trim(),
                        dn.Cells[row, 6].Value.ToString()!.Trim()
                    );
                }
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNUpload.DNUploadFilter(__res.ProfileID, param.status!, param.entity, param.TaxLevel!, header);
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
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.UploadFailed });
                }

            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 404, message = __ex.Message });
            }
        }
    }
}
