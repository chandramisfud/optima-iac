using System.Data;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.DebitNote;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    /// <summary>
    /// DebitNote Controller
    /// </summary>
    public partial class DebitNoteController : BaseController
    {
        // debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}
        /// <summary>
        /// DN Filter for Upload Faktur, Old API = "debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/upload-faktur/filter", Name = "dn_upload_faktur_filter")]
        public async Task<IActionResult> DNFilterUploadFaktur([FromForm] DNFilterParam param)
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
                    var __val = await __repoDNUploadFaktur.DNFilterUploadFaktur(__res.ProfileID, param.status!, param.entity, param.TaxLevel!, header);
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
                return Conflict(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        /// <summary>
        /// Upload DN Update FP, Old API = "debetnote/create_by_batch/updatefp/{userid}"
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/dn/upload-faktur", Name = "dn_upload_update_fp")]
        public async Task<IActionResult> DNUploadUpdateFP(IFormFile formFile)
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
                DataTable header = new("DebetNoteFPType");
                header.Columns.Add("Type", typeof(string));
                header.Columns.Add("RefId", typeof(string));
                header.Columns.Add("FPNumber", typeof(string));
                header.Columns.Add("FPDate", typeof(string));


                for (int row = 2; row <= rowCount; row++)
                {
                    header.Rows.Add(
                        dn.Cells[row, 1].Value.ToString()!.Trim(),
                        dn.Cells[row, 2].Value.ToString()!.Trim(),
                        dn.Cells[row, 3].Value ?? string.Empty,
                        dn.Cells[row, 4].Value.ToString()!.Trim()


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
                    IList<DNUpload> result = await __repoDNUploadFaktur.DNUploadUpdateFP(header, __bodytoken.userId);
                    return Ok(new
                    {
                        error = false,
                        code = 200,
                        message = MessageService.UploadSuccess,
                        values = result
                    });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.UploadFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
    }
}