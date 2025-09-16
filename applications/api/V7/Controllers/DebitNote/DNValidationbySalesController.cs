using System.Data;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;
using V7.MessagingServices;
using V7.Model.DebitNote;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    public partial class DebitNoteController : BaseController
    {
        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/validation-bysales/entity", Name = "dn_validation_bysales_entity_list")]
        public async Task<IActionResult> GetEntityValidatebySales()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoValidationbySales.GetEntityList();
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
        /// Get List Distributor
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/dn/validation-bysales/distributor", Name = "validation-bysales_distributor_list")]
        public async Task<IActionResult> GetDistributorListDNValidationbySales([FromQuery] DNDistributorGlobalParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoValidationbySales.GetDistributorList(param.budgetId, param.entityId!);
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
        /// Remove promo attachment
        /// </summary>
        /// <returns></returns>
        [HttpDelete("api/dn/validation-bysales/promoattachment", Name = "dn_validation_bysales_promo_attachment_remove")]
        public async Task<IActionResult> DeletePromoAttachmentDNValidatebySales([FromQuery] int PromoId, string DocLink)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                await __repoValidationbySales.DeletePromoAttachmentDNValidatebySales(PromoId, DocLink);
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
        /// Get DN by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/validation-bysales/id", Name = "dn_validation-bysales_by_id")]
        public async Task<IActionResult> GetDNbyIdforValidationbySales([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoValidationbySales.GetDNbyIdforValidationbySales(id);
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
        /// <summary>
        /// Get Promo by  Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/validation-bysales/promo/id", Name = "dn_validation-bysales_fin_rpt_get_id")]
        public async Task<IActionResult> GetPromobyIdforDNValidationbySales([FromQuery] int id)
        {
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __valRCDC = await __repoValidationbySales.SelectPromoRCorDC(id);
                if (__valRCDC.RCorDC == 1)
                {
                    var __valRC = await __repoValidationbySales.GetPromobyIdforDNValidationbySalesRC(id);
                    if (__valRC != null)
                    {
                        return Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            message = MessageService.GetDataSuccess,
                            values = __valRC
                        });
                    }
                    else
                    {
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                    }
                }
                else if (__valRCDC.RCorDC == 0)
                {
                    var __valDC = await __repoValidationbySales.GetPromobyIdforDNValidationbySalesDC(id);
                    if (__valDC != null)
                    {
                        return Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            message = MessageService.GetDataSuccess,
                            values = __valDC
                        });
                    }
                    else
                    {
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                    }
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = EmptyList
                    });
                }
            }
            catch (Exception)
            {
                return Ok(new BaseResponse { error = true, code = 204, message = MessageService.GetDataSuccess, values = EmptyList});
            }
        }
        /// <summary>
        /// DN change status validate by finance multi approval
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/validation-bysales/changestatus/validate-bysales-multiapproval", Name = "dn_validation-bysales_changestatus")]
        public async Task<IActionResult> DNChangeStatusSalesMultiApproval([FromBody] DNChangeStatusParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    DNChangeStatusParam __bodyToken = new()
                    {
                        dnid = new List<DNId>()
                    };
                    foreach (var item in param.dnid!)
                    {
                        __bodyToken.dnid.Add(new DNId
                        {
                            dnid = item.dnid
                        });
                    }

                    var __val = await __repoValidationbySales.DNChangeStatusSalesMultiApproval("validate_by_sales", __res.ProfileID!, __bodyToken.dnid);
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
        /// Get DN validate by sales
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/validation-bysales", Name = "get_dn_validation_bysales_change_status")]
        public async Task<IActionResult> GetDNByStatusForValidateBySales([FromQuery] DNGetStatusParamforValidatebySales param)
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
                    var __val = await __repoValidationbySales.GetDNByStatusForValidateBySales("validate_by_sales", __res.ProfileID!, param.entityid, param.distributorid, param.TaxLevel!, param.period!);
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
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                }
            }
            catch (System.Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// DN validation by sales approval
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/validation-bysales/approval", Name = "dn_validation-bysales_approval")]
        public async Task<IActionResult> DNValidatebySalesApproval([FromBody] DNValidationbySalesParam param)
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

                    var __val = await __repoValidationbySales.DNValidatebySalesApproval(param.DNId, param.status!, param.notes!, __res.ProfileID);
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
        // debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}
        /// <summary>
        /// DN Filter for Validation by Sales, Old API = "api/debetnote/filter/validatebysales/{status}/{entity}/{userid}/{TaxLevel}"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/validation-bysales/filter", Name = "dn_validation-bysales__filter")]
        public async Task<IActionResult> DNFilterValidationbySales([FromForm] DNFilterParam param)
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
                    var __val = await __repoValidationbySales.DNFilterValidationbySales(__res.ProfileID, param.status!, param.entity, param.TaxLevel!, header);
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