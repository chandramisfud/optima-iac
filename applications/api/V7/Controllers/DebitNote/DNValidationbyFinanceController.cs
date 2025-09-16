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
        [HttpGet("api/dn/validation-byfinance/entity", Name = "dn_validation-byfinance_entity_list")]
        public async Task<IActionResult> GetEntityValidatebyFinance()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoValidationbyFinance.GetEntityList();
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
        [HttpGet("api/dn/validation-byfinance/distributor", Name = "dn_validation-byfinance_distributor_list")]
        public async Task<IActionResult> GetDistributorListDNValidationbyFinance([FromQuery] DNDistributorGlobalParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoValidationbyFinance.GetDistributorList(param.budgetId, param.entityId!);
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
        [HttpDelete("api/dn/validation-byfinance/promoattachment", Name = "dn_validation-byfinance_promo_attachment_remove")]
        public async Task<IActionResult> DeletePromoAttachmentForValidationbyFinance([FromQuery] int PromoId, string DocLink)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                await __repoValidationbyFinance.DeletePromoAttachmentForValidationbyFinance(PromoId, DocLink);
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
        [HttpGet("api/dn/validation-byfinance/id", Name = "dn_validation-byfinance_by_id")]
        public async Task<IActionResult> GetDNbyIdforValidationbyFinance([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoValidationbyFinance.GetDNbyIdforValidationbyFinance(id);
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
        [HttpGet("api/dn/validation-byfinance/promo/id", Name = "dn_validation-byfinance_fin_rpt_get_id")]
        public async Task<IActionResult> GetPromobyIdforDNValidationbyFinance([FromQuery] int id)
        {
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __valRCDC = await __repoValidationbyFinance.SelectPromoRCorDC(id);
                if (__valRCDC.RCorDC == 1)
                {
                    var __valRC = await __repoValidationbyFinance.GetPromobyIdforDNValidationbyFinanceRC(id);
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
                    var __valDC = await __repoValidationbyFinance.GetPromobyIdforDNValidationbyFinanceDC(id);
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
        [HttpPost("api/dn/validation-byfinance/changestatus/validate-byfinance-multiapproval", Name = "dn_validation-byfinance_changestatus")]
        public async Task<IActionResult> DNChangeStatusValidatebyFinanceMultiApproval([FromBody] DNChangeStatusParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
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

                    var __val = await __repoValidationbyFinance.DNChangeStatusValidatebyFinanceMultiApproval("validate_by_finance", __res.ProfileID!, __bodyToken.dnid);
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
        /// DN change status dn validation
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/validation-byfinance/validation", Name = "dn_validation-byfinance_change_status")]
        public async Task<IActionResult> DNValidationbyFinance([FromBody] DNValidationbyFinanceParam param)
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
                    DNValidationbyFinanceParam __bodyToken = new()
                    {
                        StatusCode = param.StatusCode,
                        userid = __res.ProfileID,
                        Notes = param.Notes
                    };
                    __bodyToken.StatusCode = param.StatusCode;
                    __bodyToken.TaxLevel = param.TaxLevel;

                    var __val = await __repoValidationbyFinance.DNValidationbyFinance(__bodyToken);
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
        /// Get DN received by danone
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/validation-byfinance", Name = "get_dn_validation-byfinance_change_status")]
        public async Task<IActionResult> GetDNValidationbyFinance([FromQuery] DNGetStatusValidateByFinanceParamwithTaxLevel param)
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
                    var __val = await __repoValidationbyFinance.GetDNValidationbyFinance("received_by_danone", __res.ProfileID!, param.entityid, param.distributorid, param.TaxLevel!);
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

        //api/debetnote/filter/validatebyfinance/{status}/{entity}/{userid}/{TaxLevel}
        /// <summary>
        /// DN Filter for Validation by Finance, Old API = "api/debetnote/filter/validatebyfinance/{status}/{entity}/{userid}/{TaxLevel}"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/validation-byfinance/filter", Name = "dn_validation_by_finance_filter")]
        public async Task<IActionResult> DNFilterValidateByFinance([FromForm] DNFilterParam param)
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
                    var __val = await __repoValidationbyFinance.DNFilterValidateByFinance(__res.ProfileID, param.status!, param.entity, param.TaxLevel!, header);
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
        //debetnote/dnvalidation/completeness
        /// <summary>
        /// DN Validation Completeness, Old API = "debetnote/dnvalidation/completeness"
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/dn/validation-byfinance/doc-completeness", Name = "dn_validation-byfinance_doc_completeness")]
        public async Task<IActionResult> DNValidationParalelCompleteness([FromBody] DNValidationCompletenessParam body)
        {
            IActionResult result;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                if (body.taxlevel == null)
                {
                    return UnprocessableEntity("Fill TaxLevel please");
                }
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    DNDocCompletenessforValidationbyFinance __DNDocCompletenessHeader = new();
                    {
                        __DNDocCompletenessHeader.DNId = body.DNDocCompletenessHeader!.DNId;
                        __DNDocCompletenessHeader.Original_Invoice_from_retailers = body.DNDocCompletenessHeader.Original_Invoice_from_retailers;
                        __DNDocCompletenessHeader.Tax_Invoice = body.DNDocCompletenessHeader.Tax_Invoice;
                        __DNDocCompletenessHeader.Promotion_Agreement_Letter = body.DNDocCompletenessHeader.Promotion_Agreement_Letter;
                        __DNDocCompletenessHeader.Trading_Term = body.DNDocCompletenessHeader.Trading_Term;
                        __DNDocCompletenessHeader.Sales_Data = body.DNDocCompletenessHeader.Sales_Data;
                        __DNDocCompletenessHeader.Copy_of_Mailer = body.DNDocCompletenessHeader.Copy_of_Mailer;
                        __DNDocCompletenessHeader.Copy_of_Photo_Doc = body.DNDocCompletenessHeader.Copy_of_Photo_Doc;
                        __DNDocCompletenessHeader.List_of_Transfer = body.DNDocCompletenessHeader.List_of_Transfer;
                    }
                    var __val = await __repoValidationbyFinance.DNValidationParalelCompleteness(
                                body.DNId,
                                body.status!,
                                body.notes!,
                                __res.ProfileID,
                                body.taxlevel,
                                body.entityId,
                                body.promoId,
                                body.isDNPromo,
                                body.wHTType!,
                                body.statusPPH!,
                                body.pphPct,
                                body.pphAmt,
                                __DNDocCompletenessHeader);
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = __val
                    });
                }
                else
                {
                    result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse
                    {
                        error = true,
                        code = 403,
                        message = MessageService.EmailTokenFailed
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
            return result;
        }

        /// <summary>
        /// DN Validation by Finance on Document Completeness only
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/dn/validation-byfinance/save-doc-completeness", Name = "dn_validation-byfinance_save_doc_completeness")]
        public async Task<IActionResult> DNFinanceValidationDocumentCompleteness([FromBody] DNFinValidationOnDocCompletenessParam body)
        {
            IActionResult result;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    DNDocCompletenessforValidationbyFinance __DNDocCompletenessHeader = new();
                    {
                        __DNDocCompletenessHeader.DNId = body.DNDocCompletenessHeader!.DNId;
                        __DNDocCompletenessHeader.Original_Invoice_from_retailers = body.DNDocCompletenessHeader.Original_Invoice_from_retailers;
                        __DNDocCompletenessHeader.Tax_Invoice = body.DNDocCompletenessHeader.Tax_Invoice;
                        __DNDocCompletenessHeader.Promotion_Agreement_Letter = body.DNDocCompletenessHeader.Promotion_Agreement_Letter;
                        __DNDocCompletenessHeader.Trading_Term = body.DNDocCompletenessHeader.Trading_Term;
                        __DNDocCompletenessHeader.Sales_Data = body.DNDocCompletenessHeader.Sales_Data;
                        __DNDocCompletenessHeader.Copy_of_Mailer = body.DNDocCompletenessHeader.Copy_of_Mailer;
                        __DNDocCompletenessHeader.Copy_of_Photo_Doc = body.DNDocCompletenessHeader.Copy_of_Photo_Doc;
                        __DNDocCompletenessHeader.List_of_Transfer = body.DNDocCompletenessHeader.List_of_Transfer;
                    }
                    var __val = await __repoValidationbyFinance.DNFinValidationDocCompleteness(
                                body.DNId, __res.ProfileID, __DNDocCompletenessHeader);
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = __val
                    });
                }
                else
                {
                    result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse
                    {
                        error = true,
                        code = 403,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (Exception __ex)
            {
                result = Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }
    }
}