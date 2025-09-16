using System.Data;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.Master;
using V7.Services;

namespace V7.Controllers.Master
{
    public partial class MasterController : BaseController
    {
        /// <summary>
        /// Get mechanism landing page data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/mechanism", Name = "mechanisme_lp")]
        public async Task<IActionResult> GetMechanismLandingPage([FromQuery] MechanismListRequest body)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoMechanism.GetMechanismLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
                    body.PageSize, body.PageNumber);
                return Ok(new BaseResponse
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get mechanism template download
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/mechanism/download", Name = "mechanisme_download")]
        public async Task<IActionResult> GetMechanismDownload()
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoMechanism.GetMechanismTemplate();
                return Ok(new BaseResponse
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, 
                    new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get product landing page data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/mechanism/product", Name = "master_mechanism_product_lp")]
        public async Task<IActionResult> GetProductLandingPageforMechanism([FromQuery] ProductListRequest body)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoMechanism.GetProductLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
                    body.PageSize, body.PageNumber);
                return Ok(new BaseResponse
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get subactivity landing page data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/mechanism/subactivity", Name = "master_mechanisme_subactivity_lp")]
        public async Task<IActionResult> GetSubActivityLandingPageforMechanism([FromQuery] SubActivityListRequest body)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoMechanism.GetSubActivityLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
                    body.PageSize, body.PageNumber);
                return Ok(new BaseResponse
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get subaccount landing page data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/mechanism/subaccount", Name = "master_mechanisme_subaccount_lp")]
        public async Task<IActionResult> GetSubAccountLandingPageforMechanism([FromQuery] SubAccountListRequest body)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoMechanism.GetSubAccountLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
                    body.PageSize, body.PageNumber);
                return Ok(new BaseResponse
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;

        }
        /// <summary>
        /// Create mechanism data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/master/mechanism", Name = "mechanisme_store")]
        public async Task<IActionResult> CreateMechanisme([FromBody] InsertMechanismeBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    InsertMechanismeBody __bodytoken = new()
                    {
                        EntityId = body.EntityId,
                        Entity = body.Entity,
                        SubCategoryId = body.SubCategoryId,
                        SubCategory = body.SubCategory,
                        ActivityId = body.ActivityId,
                        Activity = body.Activity,
                        SubActivityId = body.SubActivityId,
                        SubActivity = body.SubActivity,
                        ProductId = body.ProductId,
                        Product = body.Product,
                        Requirement = body.Requirement,
                        Discount = body.Discount,
                        Mechanism = body.Mechanism,
                        ChannelId = body.ChannelId,
                        Channel = body.Channel,
                        StartDate = body.StartDate,
                        EndDate = body.EndDate,
                        CreateBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoMechanism.CreateMechanisme(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Store data " + __val.Mechanism + " success", values = __val });
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
        /// <summary>
        /// Modified mechanism data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/master/mechanism", Name = "mechanisme_update")]
        public async Task<IActionResult> UpdateMechanisme([FromBody] UpdateMechanismeBody body)
        {
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
                    UpdateMechanismeBody __bodytoken = new()
                    {
                        Id = body.Id,
                        EntityId = body.EntityId,
                        Entity = body.Entity,
                        SubCategoryId = body.SubCategoryId,
                        SubCategory = body.SubCategory,
                        ActivityId = body.ActivityId,
                        Activity = body.Activity,
                        SubActivityId = body.SubActivityId,
                        SubActivity = body.SubActivity,
                        ProductId = body.ProductId,
                        Product = body.Product,
                        Requirement = body.Requirement,
                        Discount = body.Discount,
                        Mechanism = body.Mechanism,
                        ChannelId = body.ChannelId,
                        Channel = body.Channel,
                        StartDate = body.StartDate,
                        EndDate = body.EndDate,
                        ModifiedBy = __res.ProfileID,
                        ModifiedEmail = __res.UserEmail
                    };
                    var __val = await __repoMechanism.UpdateMechanisme(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Update data " + __val.Mechanism + " success", values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Delete mechanism data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/master/mechanism", Name = "mechanisme_delete")]
        public async Task<IActionResult> DeleteMechanisme([FromBody] DeleteMechanismeBody body)
        {
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
                    DeleteMechanismeBody __bodytoken = new()
                    {
                        Id = body.Id,
                        DeleteBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    await __repoMechanism.DeleteMechanisme(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.DeleteSucceed, values = null });
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
        /// <summary>
        /// Get mechanism data base on parameter 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/mechanism/param", Name = "mechanisme_listbyparam")]
        public async Task<IActionResult> GetMechanismeListsByParam([FromBody] GetMechanismByParam body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoMechanism.GetMechanismeListByParam(body);
                return Ok(new BaseResponse
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __exc)
            {
                return Ok(new BaseResponse { error = true, code = 404, message = __exc.Message });
            }
        }
        /// <summary>
        /// Get mechanism data base on mechanism Id
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/mechanism/id", Name = "mechanisme_listbyid")]
        public async Task<IActionResult> GetMechanismeListById([FromQuery] GetMechanismById body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoMechanism.GetMechanismeListById(body);
                return Ok(new BaseResponse
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return Ok(new BaseResponse { error = true, code = 404, message = __ex.Message });
            }
        }
        /// <summary>
        /// Upload mechanism data using excel 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/master/mechanism/upload", Name = "mechanism_upload")]
        public async Task<IActionResult> ImportMechanism(IFormFile formFile)
        {
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return UnprocessableEntity(new { error = true, code = 422, message = "unsupported extension" });
            }
            try
            {
                using var stream = new MemoryStream();
                await formFile.CopyToAsync(stream);
                DataTable header = new("MstMechanismType");
                try
                {
                    using var package = new ExcelPackage(stream);
                    //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var rowCount = 0;
                    ExcelWorksheet mechanism = package.Workbook.Worksheets[0];
                    rowCount = mechanism.Dimension.Rows;
                   
                    header.Columns.Add("Entity", typeof(string));
                    header.Columns.Add("SubCategory", typeof(string));
                    header.Columns.Add("Activity", typeof(string));
                    header.Columns.Add("SubActivity", typeof(string));
                    header.Columns.Add("SKU", typeof(string));
                    header.Columns.Add("Requirement", typeof(string));
                    header.Columns.Add("Discount", typeof(string));
                    header.Columns.Add("Mechanism", typeof(string));
                    header.Columns.Add("Channel", typeof(string));
                    header.Columns.Add("StartPromo", typeof(string));
                    header.Columns.Add("EndPromo", typeof(string));

                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (mechanism.Cells[row, 1].Value != null)
                        {
                            header.Rows.Add(
                            mechanism.Cells[row, 1].Value.ToString()!.Trim(),
                            mechanism.Cells[row, 2].Value.ToString()!.Trim(),
                            mechanism.Cells[row, 3].Value.ToString()!.Trim(),
                            mechanism.Cells[row, 4].Value ?? string.Empty,
                            mechanism.Cells[row, 5].Value ?? string.Empty,
                            mechanism.Cells[row, 6].Value ?? string.Empty,
                            mechanism.Cells[row, 7].Value ?? string.Empty,
                            mechanism.Cells[row, 8].Value ?? string.Empty,
                            mechanism.Cells[row, 9].Value ?? string.Empty,
                            mechanism.Cells[row, 10].Value ?? string.Empty,
                            mechanism.Cells[row, 11].Value ?? string.Empty
                        );
                        }

                    }
                }
                catch (Exception)
                {
                    throw new Exception("Please check template entry");
                }
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    MechanismParamUpload __bodytoken = new()
                    {
                        userid = __res.ProfileID,
                        useremail = __res.UserEmail
                    };
                    IList<ResponseImportDto> result = (IList<ResponseImportDto>)await __repoMechanism.ImportMechanism(header, __bodytoken.userid!, __bodytoken.useremail);

                    return Ok(new
                    {
                        error = false,
                        code = 200,
                        message = MessageService.UploadSuccess,
                        result
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new
                {
                    error = true,
                    code = 500,
                    message = ex.Message
                });
            }
        }
        /// <summary>
        /// Get attribute mechanism data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/mechanism/attribute", Name = "getattributebyparent")]
        public async Task<IActionResult> GetMechanismAttributeByParent([FromQuery] GetAttributeByParentBodyReq body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoMechanism.GetMechanismAttributeByParent(body);
                if (__val == null) return NoContent();
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                }); ;
            }

        }
        /// <summary>
        /// Get entity dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/mechanism/entity", Name = "master_mechanism_entity_dropdown")]
        public async Task<IActionResult> GetEntityForMechanisms()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMechanism.GetEntityForMechanisms();
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get channel dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/mechanism/channel", Name = "master_mechanism_channel_dropdown")]
        public async Task<IActionResult> GetChannelForMechanisms()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMechanism.GetChannelForMechanisms();
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
    }
}