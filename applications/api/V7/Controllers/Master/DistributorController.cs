using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using V7.Controllers;
using V7.Services;
using V7.MessagingServices;

namespace V7.Controllers.Master
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial class MasterController : BaseController
    {
        /// <summary>
        /// Create new distributor
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/master/distributor", Name = "Distributor_store")]
        public async Task<IActionResult> CreateDistributor([FromBody] DistributorCreate body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    DistributorCreate __bodytoken = new()
                    {
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        CompanyName = body.CompanyName,
                        Address = body.Address,
                        NPWP = body.NPWP,
                        Phone = body.Phone,
                        Fax = body.Fax,
                        NoRekening = body.NoRekening,
                        BankName = body.BankName,
                        BankCabang = body.BankCabang,
                        ClaimManager = body.ClaimManager,
                        SAPCode = body.SAPCode,
                        SAPCodex = body.SAPCodex,
                        CreateBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoDistributor.CreateDistributor(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Store data " + __val.RefId + " success", values = __val });
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
        /// Distributor LP with pagination
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/distributor", Name = "master_Distributor_lp")]
        public async Task<IActionResult> GetDistributorLandingPage([FromQuery] DistributorListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDistributor.GetDistributorLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(), 
                    body.PageSize, body.PageNumber);
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
                return Conflict(new  BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Get Distributor by ID
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/distributor/id", Name = "master_Distributor_id")]
        public async Task<IActionResult> GetDistributorById([FromQuery] DistributorById body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDistributor.GetDistributorById(body);
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

        /// <summary>
        /// Edit distributor
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/master/distributor", Name = "master_Distributor_update")]
        public async Task<IActionResult> UpdateDistributor([FromBody] DistributorUpdate body)
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
                    DistributorUpdate __bodytoken = new()
                    {
                        Id = body.Id,
                        ShortDesc = body.ShortDesc,
                        LongDesc = body.LongDesc,
                        CompanyName = body.CompanyName,
                        Address = body.Address,
                        NPWP = body.NPWP,
                        Phone = body.Phone,
                        Fax = body.Fax,
                        NoRekening = body.NoRekening,
                        BankName = body.BankName,
                        BankCabang = body.BankCabang,
                        ClaimManager = body.ClaimManager,
                        SAPCode = body.SAPCode,
                        SAPCodex = body.SAPCodex,
                        ModifiedBy = __res.ProfileID,
                        ModifiedEmail = __res.UserEmail
                    };
                    var __val = await __repoDistributor.UpdateDistributor(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Update data " + __val.RefId + " success", values = __val });
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
        /// Delete Distributor
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/master/distributor", Name = "master_Distributor_delete")]
        public async Task<IActionResult> DeleteDistributor([FromBody] DistributorDelete body)
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
                    DistributorDelete __bodytoken = new()
                    {
                        Id = body.Id,
                        DeletedBy = __res.ProfileID,
                        DeletedEmail = __res.UserEmail
                    };
                    var __val = await __repoDistributor.DeleteDistributor(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Update data " + __val.RefId + " success", values = __val });
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
       


    }
}
