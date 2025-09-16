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
        //debetnote/invoicelist
        /// <summary>
        /// Get DN Invoice List 
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/create-invoice/invoice", Name = "dn_create-invoice_invoice_list")]
        public async Task<IActionResult> GetInvoiceList([FromQuery] DNGetInvoiceLPParam param)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                var __val = await __repoCreateInvoice.GetInvoiceList(param.CreateDate, param.Entity, param.Distributor, __res.ProfileID,
                    param.SortColumn, param.SortDirection,param.PageSize, param.PageNumber, param.Search);

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
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });

            }
        }
        //debetnote/getInvoiceById
        /// <summary>
        /// Get DN Invoice by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/create-invoice/invoice/id", Name = "dn_create-invoice_by_id")]
        public async Task<IActionResult> GetInvoiceById([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoCreateInvoice.GetInvoiceById(id);
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
                    return NotFound(new { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, error = true, message = __ex.Message });
            }
        }
        //debetnote/invoicetaxlevel
        /// <summary>
        /// Get DN Invoice TaxLevel by status "ready to invoice"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/create-invoice/invoice-taxlevel", Name = "get_dn_create-invoice_taxlevel")]
        public async Task<IActionResult> GetDNByStatusforInvoiceTaxLevel([FromQuery] DNGetStatusParamwithTaxLevel param)
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
                    var __val = await __repoCreateInvoice.GetDNByStatusforInvoiceTaxLevel("ready_to_invoice", __res.ProfileID!, param.entityid, param.distributorid, param.TaxLevel!, param.dnPeriod!, param.categoryId);
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
        //debetnote/invoice/store
        /// <summary>
        /// Create DN Invoice
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/create-invoice", Name = "dn_create-invoices")]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceParam param)
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
                    CreateInvoiceParam __bodyToken = new()
                    {
                        DNId = new List<DNIdReadytoInvoiceArray>()
                    };

                    foreach (var item in param.DNId!)
                    {
                        __bodyToken.DNId.Add(new DNIdReadytoInvoiceArray
                        {
                            DNId = item.DNId
                        });
                    }
                    var __val = await __repoCreateInvoice.CreateInvoice(
                        param.DistributorId,
                        param.EntityId,
                        param.DPPAmount,
                        param.PPNpct,
                        param.InvoiceAmount,
                        param.Desc!,
                        __res.ProfileID!,
                        __bodyToken.DNId,
                        param.TaxLevel!,
                        param.dnPeriod!,
                        param.categoryId
                    );
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
        // debetnote/invoice/update
        /// <summary>
        /// Update DN Invoice
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/dn/create-invoice", Name = "dn_update-invoices")]
        public async Task<IActionResult> UpdateInvoice([FromBody] UpdateInvoiceParam param)
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
                    UpdateInvoiceParam __bodyToken = new()
                    {
                        DNId = new List<DNIdReadytoInvoiceArray>()
                    };

                    foreach (var item in param.DNId!)
                    {
                        __bodyToken.DNId.Add(new DNIdReadytoInvoiceArray
                        {
                            DNId = item.DNId
                        });
                    }
                    var __val = await __repoCreateInvoice.UpdateInvoice(
                        param.InvoiceId,
                        param.DistributorId,
                        param.EntityId,
                        param.DPPAmount,
                        param.PPNpct,
                        param.InvoiceAmount,
                        param.Desc!,
                        __res.ProfileID!,
                        __bodyToken.DNId,
                        param.TaxLevel!,
                        param.dnPeriod!,
                        param.categoryId
                        );
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
        //debetnote/reject
        /// <summary>
        /// DN Invoice Notification by Danone Reject
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/create-invoice/reject", Name = "dn_create-invoice_reject")]
        public async Task<IActionResult> DNRejectCreateInvoice([FromBody] DNRejectParam param)
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
                    var __val = await __repoCreateInvoice.DNRejectCreateInvoice(param.dnid, param.reason!, __res.ProfileID);
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
        //promoattachment/delete
        /// <summary>
        /// Remove promo attachment
        /// </summary>
        /// <returns></returns>
        [HttpDelete("api/dn/create-invoice/promoattachment", Name = "dn_create-invoice_promo_attachment_remove")]
        public async Task<IActionResult> DeletePromoAttachmentDNCreateInvoice([FromBody] int PromoId, string DocLink)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                await __repoCreateInvoice.DeletePromoAttachmentDNCreateInvoice(PromoId, DocLink);
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
        //debetnote/getbyId/
        /// <summary>
        /// Get DN by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/id/create-invoice", Name = "get_dn_create-invoice_by_id")]
        public async Task<IActionResult> GetDNGetbyIdforCreateInvoice([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoCreateInvoice.GetDNGetbyIdforCreateInvoice(id);
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
        //Select entity
        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/create-invoice/entity", Name = "dn_create-invoice_entity_list")]
        public async Task<IActionResult> GetEntityListDNCreateInvoice()
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    DNAttributebyUserParam __bodytoken = new()
                    {
                        userid = __res.ProfileID
                    };
                    var __val = await __repoCreateInvoice.GetAttributeByUser(__bodytoken.userid);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val.Entity,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message
                });

            }
        }
        //debetnote/ready_to_invoice
        /// <summary>
        /// Get DN status Ready to invoice 
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/create-invoice/validate-bydanone", Name = "dn_create-invoice-validate-bydanone")]
        public async Task<IActionResult> GetDNStatusReadytoInvoice([FromQuery] DNGetStatusParam param)
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
                    var __val = await __repoCreateInvoice.GetDNStatusReadytoInvoice("validate_by_danone", __res.ProfileID!, param.entityid, param.distributorid);
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
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }
        //debetnote/printinvoice
        /// <summary>
        /// Get DN Invoice Print by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/create-invoice/print-invoice/id", Name = "dn_invoice-print_by_id")]
        public async Task<IActionResult> GetPrintInvoicebyId([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoCreateInvoice.GetPrintInvoicebyId(id);
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
                    return NotFound(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        //select taxlevel
        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/create-invoice/taxlevel", Name = "dn_create-invoice_taxlevel_list")]
        public async Task<IActionResult> GetTaxLevelList()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoCreateInvoice.GetTaxLevelList();
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
        // debetnote/ready_to_invoice [POST]
        /// <summary>
        /// DN change status to "Ready to Invoice"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/create-invoice/changestatus/ready-to-invoice", Name = "dn_changestatus_ready-to-invoice_changestatus")]
        public async Task<IActionResult> DNChangeStatusReadytoInvoice([FromBody] DNChangeStatusReadytoInvoice param)
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
                    DNChangeStatusReadytoInvoice __bodyToken = new()
                    {
                        DNId = new List<DNIdReadytoInvoiceArray>(),
                        UserId = __res.ProfileID,
                        status = "ready_to_invoice"
                    };

                    foreach (var item in param.DNId!)
                    {
                        __bodyToken.DNId.Add(new DNIdReadytoInvoiceArray
                        {
                            DNId = item.DNId
                        });
                    }

                    var __val = await __repoCreateInvoice.DNChangeStatusReadytoInvoice(__bodyToken);
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
        /// DN Filter for Validation by Finance, Old API = "api/debetnote/filter/validatebyfinance/{status}/{entity}/{userid}/{TaxLevel}"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/dn/create-invoice/filter", Name = "dn_create_invoice_filter")]
        public async Task<IActionResult> DNFilterforCreatedInvoice([FromForm] DNInvoiceFilterParam param)
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
                    var __val = await __repoCreateInvoice.DNFilterforCreatedInvoice(
                        __res.ProfileID,
                        param.status!,
                        param.entity,
                        param.TaxLevel!,
                        header,
                        param.invoiceId,
                        param.dnPeriod!,
                        param.categoryId
                        );
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
        /// <summary>
        /// Get distributor dropdown data base on entity id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpGet("api/dn/create-invoice/distributor", Name = "dn_create_invoice_byentityId")]
        public async Task<IActionResult> GetDistributorforCreateInvoice([FromQuery] int entityId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoCreateInvoice.GetDistributorforCreateInvoice(entityId);
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get Data User Profile by Id for DN Create Invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/dn/create-invoice/userprofile/id", Name = "dn_create_invoice_get_userprofile_byprimarykey")]
        public IActionResult GetByIdforDNCreateInvoice(string id)
        {
            try
            {
                var __res = __repoCreateInvoice.GetById(id);

                if (__res == null)
                {
                    return UnprocessableEntity(
                        new BaseResponse
                        {
                            error = true,
                            code = 204,
                            message = MessageService.DataNotFound
                        }
                    );
                }
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    values = __res.Result,
                    message = MessageService.GetDataSuccess
                });
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get List Category
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/create-invoice/category", Name = "dn_create_invoice_get_category_list")]
        public async Task<IActionResult> GetCategoryDropdownListforDNCreateInvoice()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoCreateInvoice.GetCategoryDropdownList();
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
    }
}
