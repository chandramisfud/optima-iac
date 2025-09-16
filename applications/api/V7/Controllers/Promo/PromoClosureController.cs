using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;
using V7.MessagingServices;
using V7.Model.Promo;
using V7.Services;

namespace V7.Controllers.Promo
{
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// List ALL promo to close with pagination
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="distributor"></param>
        /// <param name="channel"></param>
        /// <param name="start_from"></param>
        /// <param name="start_to"></param>
        /// <param name="remaining_budget"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="filter"></param>
        /// <param name="txtsearch"></param>
        /// <returns></returns>
        [HttpGet("api/promo/closure", Name = "promo_closure_LP")]
        public async Task<IActionResult> GetPromoClosureLP(int entity, int distributor, int channel,
            DateTime start_from, DateTime start_to, enBudgetRemaining remaining_budget, 
            int start, int length, string filter, string txtsearch)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __acc = await _repoPromoClosure.GetPromoClosureLP(entity, distributor, channel, start_from, start_to,
                    (int)remaining_budget, __res.ProfileID, start, length, filter, txtsearch);
                    if (__acc != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __acc,
                            message = MessagingServices.MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = true,
                            code = 404,
                            message = MessagingServices.MessageService.GetDataFailed
                        });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Import promo closure from excel (E1-2023 Item 17) 
        /// </summary>
        /// <author>andRie. May 9 2023 #885</author>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/promo/closure/import", Name = "promo_closure_import")]
        public async Task<IActionResult> PromoClosureImport(IFormFile formFile)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                    {
                        return UnprocessableEntity(new { status_code = "422", message = "un supported extension" });
                    }
                    using var stream = new MemoryStream();
                    await formFile.CopyToAsync(stream);

                    using var package = new ExcelPackage(stream);
                    var rowCount = 0;
                    ExcelWorksheet dn = package.Workbook.Worksheets[0];
                    rowCount = dn.Dimension.Rows;
                    DataTable header = new("CharAttributeType");
                    header.Columns.Add("Promo ID", typeof(string));

                    for (int row = 2; row <= rowCount; row++)
                    {
                        header.Rows.Add(
                            dn.Cells[row, 1].Value.ToString()!.Trim()
                        );
                    }

                    var result = await _repoPromoClosure.ImportPromoClosure(header, __res.ProfileID, __res.UserEmail!);

                    return Ok(new
                    {
                        error = false,
                        StatusCode = 200,
                        message = "Upload Success",
                        values = result
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

            }
            catch (Exception __ex)
            {
                return Ok(new { error = true, StatusCode = 500, message = __ex.Message });
            }
        }

        [HttpPost("api/promo/closure/close", Name = "promo_closure_closing")]
        public async Task<IActionResult> PromoClosure([FromBody] promoClosureParam promo)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var result = await _repoPromoClosure.ClosingPromo(promo.promoId!.ToArray(), __res.ProfileID, __res.UserEmail);
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = result,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
        [HttpPost("api/promo/closure/reopen", Name = "promo_closure_reopening")]
        public async Task<IActionResult> ClosePromoOpen([FromBody] promoClosureParam promo)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    await _repoPromoClosure.ReOpenPromo(promo.promoId!.ToArray(), __res.ProfileID, __res.UserEmail);
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                    //    values = result,
                        message = "Reopen Success"
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }


        [HttpGet("api/promo/closure/id", Name = "promo_closure_byId")]
        public async Task<IActionResult> GetPromoClosureById(int id)
        {

            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __acc = await _repoPromoDisplay.GetPromoDisplayById(id);
                if (__acc != null)
                {
                    return Ok(new Model.BaseResponse { error = false, code = 200, values = __acc, message = MessageService.GetDataSuccess });
                }
                else
                {
                    return Ok(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Ok(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

    }
}
