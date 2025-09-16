using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DNReceivedAndApproved;
using V7.MessagingServices;
using V7.Services;
using V7.Model.DebitNote;
using Org.BouncyCastle.Crypto.Digests;
using OfficeOpenXml;
using System.Data;

namespace V7.Controllers.DebitNote
{
    public partial class DebitNoteController : BaseController
    {
        /// <summary>
        /// Remove promo attachment
        /// </summary>
        /// <param name="PromoId"></param>
        /// <param name="DocLink"></param>
        /// <returns></returns>
        [HttpDelete("api/dn/received-approved-byho/promoattachment", Name = "dn_received-approved_promo_attachment_remove")]
        public async Task<IActionResult> DeletePromoAttachmentDNReceivedandApproved([FromQuery] int PromoId, string DocLink)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                await __repoReceivedAndApprovedHO.DeletePromoAttachmentDNReceivedandApproved(PromoId, DocLink);
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
        /// DN change status distributor multi approval
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/received-approved-byho/changestatus/distributor-multiapproval", Name = "fromdistributor-to-ho-distributor-multiapproval")]
        public async Task<IActionResult> DNChangeStatusDistributorMultiApproval([FromBody] DNChangeSingleStatusParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {

                    await __repoReceivedAndApprovedHO.DNChangeStatusDistributorMultiApproval("validate_by_dist_ho",
                        __res.ProfileID, __res.UserEmail, param.dnId);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess });
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
        /// Get DN by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/received-approved-byho/id", Name = "debetnote_received-approved_by_id")]
        public async Task<IActionResult> GetDNbyIdReceivedAndApproval([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoReceivedAndApprovedHO.GetDNbyId(id);
                if (__val == null)
                {
                    return Ok(new BaseResponse { code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        // debetnote/dnapproval
        /// <summary>
        /// DN change status distributor multi approval, Old API = "debetnote/dnapproval"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/received-approved-byho/multi-approval", Name = "received-approved-multi-approval")]
        public async Task<IActionResult> DNMultiApprovalParalel([FromBody] DNReceivedAndApproveMultiParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoReceivedAndApprovedHO.DNMultiApprovalParalel(param.DNId, param.status!, param.notes!, __res.ProfileID);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess, values = __val });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get DN validate by distributor ho, Old API = "debetnote/validate_by_dist_ho"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/received-approved-byho/validate-bydistributor-ho", Name = "received-approved-validate-bydistributor-ho")]
        public async Task<IActionResult> GetDNValidateByDistributorHO()
        {
            IActionResult result;
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
                    var __resDist = await __repoDNPromoDisplay.GetDistributorId(__res.ProfileID!);
                    if (__resDist != null)
                    {
                        var __val = await __repoReceivedAndApprovedHO.GetDNValidateByDistributorHO("send_to_dist_ho", __res.ProfileID!, 0, __resDist.DistributorId);
                        result = Ok(
                            new BaseResponse
                            {
                                error = false,
                                code = 200,
                                message = MessageService.GetDataSuccess,
                                values = __val
                            });
                    }
                    else
                    {
                        return Ok(new BaseResponse { error = false, code = 200, message = MessageService.DataNotFound, values = __resDist });
                    }
                }
                else
                {
                    result = Ok(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound, values = null });
                }
            }
            catch (System.Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }

        // debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}
        /// <summary>
        /// DN Filter for Validation by HO, Old API = "debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/received-approved-byho/filter", Name = "dn_received-approved-byho_filter")]
        public async Task<IActionResult> DNFilterReceivedandApprovedbyHO([FromForm] DNFilterParam param)
        {
            if (!Path.GetExtension(param.formFile!.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "unsupported extension" });
            }
            using var stream = new MemoryStream();
            await param.formFile.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            try
            {
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
                    var __val = await __repoReceivedAndApprovedHO.DNFilterReceivedandApprovedbyHO(__res.ProfileID, param.status!, param.entity, param.TaxLevel!, header);
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
    }
}