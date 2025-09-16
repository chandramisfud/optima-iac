using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
//using Repositories.Entities.Models;
using System.Data;
using V7.MessagingServices;
using V7.Model;
using V7.Model.Tools;
using V7.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace V7.Controllers.Mapping
{
    public partial class MappingController : BaseController
    {
        /// <summary>
        /// Get all distributor WHT data LP
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-wht", Name = "map_distributor_wht_list")]
        public async Task<IActionResult> MapDistributorWHTLP([FromQuery] DistributorWHTLPRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDistributorWHT.GetDistributorWHTLP(body.Search!,
                    body.distributor, body.subActivity, body.subAccount, body.WHTType,
                    body.PageNumber, body.PageSize, body.sortColumn!, body.sortDirection.ToString());
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }

        /// <summary>
        /// GET deductable value by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-wht/id", Name = "get_distributor_wht_by_id")]
        public async Task<IActionResult> MapDistributorWHTById(int id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDistributorWHT.GetDistributorWHT(id);
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get all distributor activity wht mapping data for download
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-wht/download", Name = "mapdistributor_wht_download")]
        public async Task<IActionResult> MapDistributorWHTDownload([FromQuery] DistributorWHTLPRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDistributorWHT.GetDistributorWHTLP(body.Search!,
                   body.distributor, body.subActivity, body.subAccount, body.WHTType,
                   body.PageNumber, body.PageSize, body.sortColumn!, body.sortDirection.ToString());
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get all distributor WHT data for template
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-wht/template", Name = "mapdistributor_wht_template")]
        public async Task<IActionResult> MapDistributorWHTDownloadTemplate([FromQuery] DistributorWHTLPRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoDistributorWHT.GetDistributorWHTLP(body.Search!,
                   body.distributor, body.subActivity, body.subAccount, body.WHTType,
                   body.PageNumber, body.PageSize, body.sortColumn!, body.sortDirection.ToString());
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }

        /// <summary>
        /// update distributor wht map
        /// </summary>
        /// <returns></returns>
        [HttpPut("api/mapping/distributor-wht", Name = "mapdistributor_wht_update")]
        public async Task<IActionResult> MapDistributorWHTUpdate([FromBody] DistributorWHTUpdateParam body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDistributorWHT.UpdateDistributorWHT(body.id, body.WHTType,
                    __res.ProfileID, __res.UserEmail);
                    if (__val)
                    {
                        return Ok(new Model.BaseResponse { error = false, code = 200, message = MessageService.SaveSuccess });
                    }
                    else
                    {
                        return Ok(new BaseResponse { error = true, code = 404, message = MessageService.SaveFailed });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }

        /// <summary>
        /// delete distributor wht map by id
        /// </summary>
        /// <returns></returns>
        [HttpDelete("api/mapping/distributor-wht", Name = "mapdistributor_wht_delete")]
        public async Task<IActionResult> MapDistributorWHTDelete([FromBody] DistributorWHTDeleteParam body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDistributorWHT.DeleteDistributorWHT(body.id, __res.ProfileID, __res.UserEmail);
                    if (__val)
                    {
                        return Ok(new Model.BaseResponse { error = false, code = 200, message = MessageService.DeleteSucceed });
                    }
                    else
                    {
                        return Ok(new BaseResponse { error = true, code = 404, message = MessageService.DeleteFailed });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }


        /// <summary>
        /// Upload distributor WHTType mapping data
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/mapping/distributor-wht/upload", Name = "mapping_distributor_wht_upload")]
        public async Task<IActionResult> MapDistributorWHTUpload(IFormFile formFile)
        {

            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID == null)
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }


                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return UnprocessableEntity(new { error = true, code = 422, message = "un supported extension" });
                }
                using var stream = new MemoryStream();
                await formFile.CopyToAsync(stream);

                using var package = new ExcelPackage(stream);
                //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var rowCount = 0;


                ExcelWorksheet wstemplate = package.Workbook.Worksheets[0];

                rowCount = wstemplate.Dimension.Rows;
                DataTable dt = new("distributor_wht_type");
                dt.Columns.Add("Distributor", typeof(string));
                dt.Columns.Add("SubActivity", typeof(string));
                dt.Columns.Add("SubAccount", typeof(string));
                dt.Columns.Add("WHTType", typeof(string));

                for (int row = 2; row <= rowCount; row++)
                {
                    List<object> lsCol = new();
                    for (int col = 1; col < 5; col++)
                    {
                        // ignore empty col, cos mandatory
                        if (wstemplate.Cells[row, col].Value == null) break;
                        lsCol.Add(wstemplate.Cells[row, col].Value);
                    }
                    if (lsCol.Count == 4)
                    {
                        dt.Rows.Add(lsCol.ToArray());
                    }
                }

                List<object> lsErr = new List<object>();
                if (dt.Rows.Count > 0)
                {
                    lsErr = await __repoDistributorWHT.ImportDistributorWHT(dt, __res.ProfileID, __res.UserEmail);
                    dt.Rows.Clear();
                }

                return Ok(new
                {
                    error = false,
                    code = 200,
                    message = MessageService.UploadSuccess,
                    values = lsErr
                });


            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse
                {
                    error = true,
                    code = 409,
                    message = __ex.Message,                   
                });
            }
        }

        /// <summary>
        /// Create distributor wht maping
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/mapping/distributor-wht", Name = "mapdistributor_wht_create")]
        public async Task<IActionResult> MapDistributorWHTCreate([FromBody] DistributorWHTCreateParam body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDistributorWHT.CreateDistributorWHT(body.distributor, body.subActivity,
                    body.subAccount, body.WHTType, __res.ProfileID, __res.UserEmail);
                    if (__val)
                    {
                        return Ok(new Model.BaseResponse { error = false, code = 200, message = MessageService.SaveSuccess });
                    }
                    else
                    {
                        return Ok(new BaseResponse { error = true, code = 404, message = MessageService.SaveFailed });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get WHTType list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-wht/whttype", Name = "mapdistributor_whttype_list")]
        public async Task<IActionResult> MaspDistributorWHTTypeList()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string qry = "SELECT WHTType FROM tbmst_wht_type";
                var __val = await __repoDistributorWHT.RunQueryString(qry);
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get Distributor list - distinct
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-wht/distributor", Name = "mapdistributor_distributor_list")]
        public async Task<IActionResult> MapDistributorDistributorList()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __query = @"
                    SELECT LongDesc FROM tbmst_distributor
                    WHERE isnull(IsDeleted, 0)=0 ";
                var __val = await __repoDistributorWHT.RunQueryString(__query);
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get Sub Activity list - distinct
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-wht/subactivity", Name = "mapdistributor_subactivity_list")]
        public async Task<IActionResult> MapDistributorsubActivityList()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __query = @"
                    SELECT Distinct LongDesc FROM tbmst_subActivity
                    WHERE isnull(IsDeleted, 0)=0 ";
                var __val = await __repoDistributorWHT.RunQueryString(__query);
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get Sub Account list - distinct
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/mapping/distributor-wht/subaccount", Name = "mapdistributor_subaccount_list")]
        public async Task<IActionResult> MapDistributorsubAccountList()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __query = @"
                    SELECT Distinct LongDesc FROM tbmst_subAccount
                    WHERE isnull(IsDelete, 0)=0  ";
                var __val = await __repoDistributorWHT.RunQueryString(__query);
                if (__val != null)
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 400, message = __ex.Message });
            }
        }

    }
}